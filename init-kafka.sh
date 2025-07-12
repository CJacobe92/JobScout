#!/bin/bash

if [ ! -f "/tmp/kraft-combined-logs/meta.properties" ]; then
  echo "Formatting Kafka storage..."
  CLUSTER_ID=$(kafka-storage random-uuid)
  kafka-storage format --cluster-id "$CLUSTER_ID" -c /etc/kafka/kafka.properties
else
  echo "Kafka storage already formatted."
fi

exec /etc/confluent/docker/run
