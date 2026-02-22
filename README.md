#  .NET Observability API

> A production-style ASP.NET Core service demonstrating real-world observability patterns using OpenTelemetry, Prometheus, Grafana, and Loki.

This project is not a “hello world” metrics demo.  
It is intentionally designed to reflect how backend services are instrumented and operated in production environments.

---

## Purpose

Modern backend systems must be:

- Observable
- Operable
- Debuggable under failure
- Safe to evolve

This repository demonstrates:

- Metrics instrumentation via OpenTelemetry
- Prometheus scraping configuration
- Grafana dashboards with PromQL
- Structured logging using Serilog
- Correlation ID propagation
- Centralized logging via Loki
- Local infrastructure orchestration using Docker Compose

---

## Architecture Overview

```
              ┌───────────────────────────┐
              │  ASP.NET Core API         │
              │                           │
              │  - OpenTelemetry Metrics  │
              │  - Structured Logging     │
              │  - Correlation IDs        │
              └─────────────┬─────────────┘
                            │
          ┌─────────────────┼─────────────────┐
          │                 │                 │
          ▼                 ▼                 ▼
     Prometheus         Loki              Console
   (metrics scrape)   (log storage)       (dev logs)
          │
          ▼
       Grafana
  (Dashboards & Explore)
```

All supporting infrastructure runs locally via Docker.

---

## Observability Philosophy

This project demonstrates:

- **Metrics for system behavior**
- **Logs for detailed diagnostics**
- **Correlation IDs for traceability**
- **Instrumentation as part of architecture**

Key principle:

> Observability is a design decision — not a library you bolt on later.

---

## Technology Stack

| Layer | Technology |
|-------|------------|
| API | ASP.NET Core |
| Metrics | OpenTelemetry (.NET) |
| Metrics Store | Prometheus |
| Visualization | Grafana |
| Logging | Serilog |
| Log Aggregation | Loki |
| Container Orchestration | Docker Compose |

---

## Running the Stack

### Start Infrastructure

```bash
docker compose up -d
```

This launches:

- Prometheus → http://localhost:9090
- Grafana → http://localhost:3000
- Loki → internal logging backend

---

### Run the API

```bash
dotnet run --project src/DotNetObservability.Api
```

Metrics endpoint:

```
http://localhost:3000/metrics
```

Prometheus scrapes:

```
http://host.docker.internal:3000/metrics
```

---

## Metrics Exposed

### Process Metrics

- `process_cpu_seconds_total`
- `process_resident_memory_bytes`

### HTTP Metrics

- `http_server_request_duration_seconds_count`
- `http_server_request_duration_seconds_bucket`
- `http_server_request_duration_seconds_sum`

### Runtime Metrics

- GC metrics
- Thread pool metrics
- Allocation metrics

---

## Example System Dashboard Queries

### CPU Usage (rate over 1m)

```promql
rate(process_cpu_seconds_total[1m])
```

### Memory Usage

```promql
process_resident_memory_bytes
```

### HTTP Throughput

```promql
rate(http_server_request_duration_seconds_count[1m])
```

### P95 Latency

```promql
histogram_quantile(
  0.95,
  rate(http_server_request_duration_seconds_bucket[5m])
)
```

---

## Structured Logging

Logging includes:

- Correlation ID enrichment
- Environment metadata
- Process and thread IDs
- JSON structured output
- Loki integration

Example enrichment:

```csharp
.Enrich.WithEnvironmentName()
.Enrich.WithProcessId()
.Enrich.WithThreadId()
```

---

## Correlation ID Middleware

Each request:

- Reads `X-Correlation-Id` header (if provided)
- Generates one if missing
- Injects into Serilog log context
- Returns it in the response headers

This enables request traceability across distributed systems.

---

##  Docker Services

| Service | Purpose |
|----------|----------|
| prometheus | Scrapes metrics |
| grafana | Dashboard visualization |
| loki | Centralized log storage |

Prometheus configuration:

```yaml
scrape_configs:
  - job_name: "dotnet-observability-api"
    static_configs:
      - targets: ["host.docker.internal:3000"]
```

---

## What This Repository Demonstrates

- Production-grade instrumentation
- Docker networking awareness
- PromQL fluency
- Structured logging best practices
- Operational maturity in backend systems
- Observability-first architecture

This is designed as a portfolio-quality example of backend engineering maturity.

---

##  Potential Enhancements

- Distributed tracing (OTLP exporter)
- Alerting rules (error rate / latency thresholds)
- Load testing with dashboard validation
- Health check monitoring
- CI validation for instrumentation coverage

---


