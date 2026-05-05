namespace ProductService.Kafka;

using Confluent.Kafka;
using System.Text.Json;
using ProductService.Services;
 
public class KafkaConsumer : BackgroundService
    {
        // lấy các cái server cần thiết để kết nối Kafka Consumer
        private readonly IServiceScopeFactory _scopeFactory;

        public KafkaConsumer(
            IServiceScopeFactory scopeFactory,
            ILogger<KafkaConsumer> logger)
        {
            _scopeFactory = scopeFactory;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // cần chạy Kafka Consumer trong một Task riêng biệt để không block thread chính của ứng dụng
            // ConsumeAsync lắng nghe topic order_created_
            return Task.Run(() => ConsumeAsync("order-created-topic", stoppingToken), stoppingToken);
        }

        private async Task ConsumeAsync(string topic, CancellationToken stoppingToken)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "product-service-group",
                AutoOffsetReset = AutoOffsetReset.Earliest,

                EnableAutoCommit = false // ❗ BẮT BUỘC
            };

            using var consumer = new ConsumerBuilder<string, string>(config).Build();
            consumer.Subscribe(topic);


            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    var consumeResult = consumer.Consume(stoppingToken);
                    if (consumeResult?.Message == null) continue;

                    try
                    {
                        var orderDetails = JsonSerializer.Deserialize<List<ProductOrderDto>>(
                            consumeResult.Message.Value
                        );

                        if (orderDetails == null || !orderDetails.Any())
                        {
                            consumer.Commit(consumeResult);
                            continue;
                        }

                        using var scope = _scopeFactory.CreateScope();
                        var productService = scope.ServiceProvider
                            .GetRequiredService<IProductService>();

                        foreach (var item in orderDetails)
                        {


                            var result = await productService
                                .UpdateStockAsync(item.ProductId, item.Quantity);

                            if (result.StatusCode != 200)
                            {
                                throw new Exception(result.Message);
                            }
                        }

                        // ✅ COMMIT SAU KHI XỬ LÝ THÀNH CÔNG
                        consumer.Commit(consumeResult);
                    }
                    catch (Exception ex)
                    {
                        // ❌ KHÔNG COMMIT → Kafka sẽ retry
                    }
                }
            }
            finally
            {
                consumer.Close();
            }
        }
    }

    // DTO
    class ProductOrderDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }

