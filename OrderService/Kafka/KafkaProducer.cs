using Confluent.Kafka;

namespace OrderService.Kafka;

public interface IKafkaProducer
{
    Task ProducerAsyns(string topic, Message<string,string> message);
}

public class KafkaProducer : IKafkaProducer
{
    private readonly IProducer<string, string> _producer;
    public KafkaProducer()
    {
        var config = new ProducerConfig
        {
            BootstrapServers = "localhost:9092" // dc kafka chạy ở localhost, cổng mặc định là 9092
        };
        _producer = new ProducerBuilder<string, string>(config).Build();
    }

    public Task ProducerAsyns(string topic, Message<string, string> message)
    {
        return _producer.ProduceAsync(topic, message);
    }
}