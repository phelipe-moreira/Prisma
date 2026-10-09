# Prisma API

API REST da plataforma Prisma, criada para conectar pessoas, causas e organizações não governamentais. A aplicação oferece autenticação, gerenciamento de usuários, ONGs, causas, publicações, comentários, curtidas, posts salvos, seguidores e upload de mídias.

## Visão geral

O projeto é uma API ASP.NET Core construída sobre .NET 10, com arquitetura em camadas e separação entre regras de negócio, domínio, infraestrutura e exposição HTTP.

### Principais recursos

- Cadastro, login, logout e renovação de tokens com JWT + refresh token.
- Gerenciamento do perfil e da senha do usuário.
- CRUD de causas, ONGs, posts e comentários.
- Relacionamento entre usuários e ONGs, incluindo seguir e deixar de seguir.
- Curtidas e salvamento de posts.
- Upload direto de mídias para o Cloudflare R2 por URL pré-assinada.
- Persistência PostgreSQL com Entity Framework Core e migrations.
- Documentação interativa da API via Swagger em ambiente de desenvolvimento.
- Testes automatizados com NUnit e FluentAssertions.

## Stack

| Categoria | Tecnologia |
| --- | --- |
| Runtime | .NET 10 / ASP.NET Core |
| Linguagem | C# |
| Banco de dados | PostgreSQL |
| ORM | Entity Framework Core 10 + Npgsql |
| Autenticação | JWT Bearer + refresh tokens |
| Armazenamento | Cloudflare R2 compatível com S3 |
| Documentação | OpenAPI + Swagger UI |
| Testes | NUnit, FluentAssertions, coverlet |

## Arquitetura

```text
Prisma/
├── Prisma.Api/            # Controllers, configuração HTTP e composição da aplicação
├── Prisma.Application/    # Casos de uso, serviços, DTOs, interfaces e erros
├── Prisma.Domain/         # Entidades, enums, resultados e abstrações do domínio
├── Prisma.Infrastructure/ # EF Core, PostgreSQL, repositórios, JWT e Cloudflare R2
└── Prisma.Tests/          # Testes unitários e de comportamento
```

O fluxo principal segue a direção `Api → Application → Domain`, enquanto `Infrastructure` implementa as abstrações definidas no domínio e na aplicação.

## Pré-requisitos

- [.NET SDK 10](https://dotnet.microsoft.com/download/dotnet/10.0)
- PostgreSQL 14 ou superior
- `dotnet-ef` — o repositório fixa a versão no arquivo `dotnet-tools.json`

Para restaurar as ferramentas locais:

```bash
dotnet tool restore
```

## Configuração local

A aplicação exige uma connection string PostgreSQL e as credenciais do Cloudflare R2. Recomenda-se usar User Secrets durante o desenvolvimento, evitando armazenar segredos em arquivos versionados.

Na pasta da API, inicialize os secrets:

```bash
cd Prisma/Prisma.Api
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=prisma;Username=postgres;Password=sua-senha"
dotnet user-secrets set "CloudflareR2:AccountId" "seu-account-id"
dotnet user-secrets set "CloudflareR2:AccessKeyId" "sua-access-key"
dotnet user-secrets set "CloudflareR2:SecretAccessKey" "sua-secret-key"
dotnet user-secrets set "CloudflareR2:BucketName" "seu-bucket"
dotnet user-secrets set "CloudflareR2:PublicUrl" "https://seu-dominio-publico" # opcional
```

O bloco `Jwt` já possui valores de desenvolvimento em `appsettings.json`. Para qualquer ambiente compartilhado ou de produção, substitua principalmente `Jwt:SecretKey` por um segredo forte e externo:

```bash
dotnet user-secrets set "Jwt:SecretKey" "uma-chave-secreta-longa-e-aleatoria"
```

Também é possível configurar os mesmos valores por variáveis de ambiente usando a convenção `__`, por exemplo `ConnectionStrings__DefaultConnection` e `CloudflareR2__BucketName`.

## Banco de dados e migrations

As migrations ficam em `Prisma/Prisma.Infrastructure/Migrations`. Com o PostgreSQL configurado, aplique o schema usando:

```bash
dotnet ef database update --project Prisma/Prisma.Infrastructure --startup-project Prisma/Prisma.Api
```

Para criar uma nova migration:

```bash
dotnet ef migrations add NomeDaMigration --project Prisma/Prisma.Infrastructure --startup-project Prisma/Prisma.Api
```

## Executando a aplicação

Na raiz do repositório:

```bash
dotnet run --project Prisma/Prisma.Api --launch-profile https
```

URLs locais configuradas:

- HTTP: `http://localhost:5166`
- HTTPS: `https://localhost:7075`
- Swagger UI: `https://localhost:7075/swagger`

O Swagger é habilitado apenas quando `ASPNETCORE_ENVIRONMENT=Development`.

## Testes

Executar toda a suíte:

```bash
dotnet test Prisma.slnx
```

Executar com cobertura coletada pelo Coverlet:

```bash
dotnet test Prisma.slnx --collect:"XPlat Code Coverage"
```

Estado verificado na revisão deste README: **14 testes aprovados, 0 falhas**.

## Endpoints principais

Todos os identificadores `{id}`, `{postId}`, `{mediaId}`, `{ngoId}` e `{parentId}` são GUIDs. Endpoints marcados como autenticados exigem `Authorization: Bearer <token>`.

| Grupo | Rotas |
| --- | --- |
| Autenticação | `POST /api/auth/register`, `/login`, `/logout`, `/refresh` |
| Usuário autenticado | `GET /api/users/me`, `PUT /api/users/me`, `PUT /api/users/me/password`, `DELETE /api/users/me` |
| Causas | `GET/POST /api/cause`, `GET/PUT/DELETE /api/cause/{id}` |
| ONGs | `GET/POST /api/ngo`, `GET/PUT/DELETE /api/ngo/{id}` |
| Posts | `GET/POST /api/post`, `GET/PUT/DELETE /api/post/{id}` |
| Comentários | `GET/POST /api/comment`, `GET/PUT/DELETE /api/comment/{id}`, `GET /api/comment/parent/{parentId}` |
| Interações | `POST/DELETE /api/post/{postId}/like`, `POST/DELETE /api/post/{postId}/save` |
| Seguidores de ONG | `POST /api/user-ngo-follow`, `GET /api/user-ngo-follow/me`, `GET /api/user-ngo-follow/ngo/{ngoId}`, `DELETE /api/user-ngo-follow/{ngoId}` |
| Mídia | `POST /api/post/{postId}/media/upload-url`, `POST /api/post/{postId}/media/{mediaId}/complete`, `GET/DELETE /api/post/media/{mediaId}` |

As respostas de sucesso e erro são padronizadas por `ApiResponse`, `Result` e `ErrorResponse`. Os erros de domínio são convertidos pela API em códigos HTTP como 400, 401, 403, 404, 409 e 500, conforme o tipo do erro.

## Convenções de desenvolvimento

- Mantenha regras de negócio em `Prisma.Application` e entidades em `Prisma.Domain`.
- Use interfaces do domínio/aplicação para dependências externas e implementações na infraestrutura.
- Não versione credenciais, connection strings, tokens ou User Secrets.
- Ao alterar o modelo persistido, crie uma migration correspondente.
- Execute `dotnet build Prisma.slnx` e `dotnet test Prisma.slnx` antes de abrir um pull request.
