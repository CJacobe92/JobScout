using System;

namespace Infrastructure.Kafka;

public static class KafkaTopics
{
    public const string TenantCreated = "tenant-created";
    public const string TenantCreatedDLQ = "tenant-created-dlq";
    public const string TenantsFetched = "tenants-fetched";
}
