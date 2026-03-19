# DMCorp.Framework.K8s

Интеграция сервисов **.NET** с **Kubernetes**: работа с API кластера и аутентификация **JWT** для токенов **Service Account** (issuer/audience из переменных окружения кластера, ключи через **JWKS**).

## Назначение

- **Клиент** — `K8sClientHelper` и связанные обёртки для вызовов Kubernetes API.
- **Окружение** — `K8sEnvironmentVariablesHelper` для стандартных путей и параметров JWT в Pod.
- **Безопасность** — `K8sTokenValidator`, `K8sJwksProvider`, настройка `JwtBearer` через `JwtBearerOptionsHelper`.
- **HTTP-обработчики** — обратный канал для метаданных OIDC/JWT (`K8sJWTBackChannelHandler`) и заголовки для service account (`K8sBasicServiceAccountAuthHeaderHandler`).

Зависит от **DMCorp.Framework.Basics** (базовая валидация токенов и контракты безопасности).

## Зависимости

- `KubernetesClient`
- `Microsoft.AspNetCore.Authentication.JwtBearer`

Целевая платформа: **net9.0**.