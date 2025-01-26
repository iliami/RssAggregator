# RssAggregator

Приложение для сбора [RSS](https://en.wikipedia.org/wiki/RSS).

---

## Запуск

Необходимо иметь `docker (docker compose)`.

```
git clone https://github.com/iliami/RssAggregator.git
cd ./RssAggregator/
docker compose -f ./docker/docker-compose.yml up -d
```

Итого:
- Сервис идентефикации: http://localhost:5002/swagger
- Основной сервис: http://localhost:5183/swagger
- Логи: http://localhost:3000/a/grafana-lokiexplore-app/explore
- RabbitMQ: http://localhost:15672/

Для добавления RSS-канала необходимо у пользователя изменить роль в базе данных сервиса идентефикации на `admin`.

---

## Стек технологий

Приложение: С#, ASP.NET Core Web Api, FluentValidation, EntityFrameworkCore, PostgreSQL, RabbitMQ, Grafana Loki (via Serilog & OTLP), Docker.

Тесты: xUnit, NSubstitute, FluenAssertions
