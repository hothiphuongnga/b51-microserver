# event/ message:
    - những sự kiện/ thông điệp có thay đổi trên hệ thống (order cần update tồn kho)

# Producer
    là service gửi message vào kafka 

# Consumer
    là service nhận message từ kafka

# topic
    nhóm các message có cùng chủ đề (ví dụ: update-stock) 
## partition
    là phần nhỏ hơn của topic, giúp phân phối message trên nhiều broker để tăng hiệu suất và khả năng mở rộng (ví dụ: topic update-stock có 3 partition)

# broker
    là server quản lý các topic và message (ví dụ: localhost:9092)

# Cluster
    là tập hợp nhiều broker để đảm bảo tính sẵn sàng và khả năng mở rộng (ví dụ: cluster gồm 3 broker)