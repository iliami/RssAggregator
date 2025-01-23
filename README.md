# RssAggregator

Приложение для сбора [RSS](https://en.wikipedia.org/wiki/RSS).

---

## Запуск

Необходимо иметь `dotnet 8 (SDK, runtime), docker (docker compose)`.

```
git clone https://github.com/iliami/RssAggregator.git
cd ./RssAggregator/
docker compose -f ./docker/docker-compose.dev.yml up -d
dotnet run --project ./src/Iliami.Identity.Presentation/ -lp http
```

Далее нужно зайти и зарегистрировать пользователя: http://localhost:5002/swagger/
После этого запускаем основное приложение:

```
dotnet run --project ./src/RssAggregator.Presentation/ -lp http
```

Итого:
- Сервис идентефикации: http://localhost:5002/
- Основной сервис: http://localhost:5183/
- Логи: http://localhost:3000/a/grafana-lokiexplore-app/explore
- RabbitMQ: http://localhost:15672/

Для добавления RSS-канала необходимо у пользователя изменить роль в базе данных сервиса идентефикации на `admin`.

---

## Стек технологий

Приложение: С#, ASP.NET Core Web Api, FluentValidation, EntityFrameworkCore, PostgreSQL, RabbitMQ, Grafana Loki (via Serilog & OTLP), Docker.

Тесты: xUnit, NSubstitute, FluenAssertions
