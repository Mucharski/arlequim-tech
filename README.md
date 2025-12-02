# ArlequimTech API

Sistema de e-commerce com arquitetura de microsserviços desenvolvido em .NET 10, utilizando princípios do DDD, Clean Architecture e Notification Pattern.

## Bibliotecas utilizadas

```FluentValidation```
```EntityFramework```
```Newtonsoft.Json```
```RabbitMQ.Client```
```xUnit```
```NSubstitute```


## Como Executar o Projeto

Para executar o projeto, rode o seguinte comando na raiz do projeto:

```bash
docker-compose up
```

Após o container "postgres" subir, rode o seguinte commando na raiz do projeto (necessário apenas na primeira vez que rodar o projeto):
```bash
dotnet ef database update -p ArlequimTech.Core
```
---

## Guia Passo a Passo

Este guia descreve os passos necessários para utilizar a API de autenticação.

---

### Passo 1: Criar Usuário

**Endpoint:** `POST /api/auth/createUser`

**Descrição:** Cria um novo usuário no sistema. Este endpoint é público e não requer autenticação.

#### Payload da Requisição

```json
{
  "name": "João Silva",
  "email": "joao.silva@email.com",
  "password": "senha123",
  "role": 1
}
```

#### Campos do Payload

| Campo | Tipo | Obrigatório | Descrição                      |
|-------|------|-------------|--------------------------------|
| name | string | Sim | Nome do usuário        |
| email | string | Sim | E-mail do usuário        |
| password | string | Sim | Senha com no mínimo 6 caracteres |
| role | integer | Sim | Role do usuário (ver tabela abaixo) |

#### Enum UserRole

| Valor | Nome | Descrição                                       |
|-------|------|-------------------------------------------------|
| 0 | None | Role inválida |
| 1 | User | Usuário comum                                   |
| 2 | Admin | Administrador do sistema                        |

#### Exemplo de Requisição (cURL)

```bash
curl -X POST http://localhost:5000/api/auth/createUser \
  -H "Content-Type: application/json" \
  -d '{
    "name": "João Silva",
    "email": "joao.silva@email.com",
    "password": "senha123",
    "role": 1
  }'
```

#### Resposta de Sucesso (200 OK)

```json
{
  "success": true,
  "message": "Usuário criado com sucesso!",
  "data": {}
}
```
### Respostas de Erro

**Erros de validação (400 Bad Request):**
```json
{
  "success": false,
  "message": "Erro ao criar usuário",
  "errors": [
    "O nome é obrigatório.",
    "E-mail inválido.",
    "A senha deve ter no mínimo 6 caracteres."
  ]
}
```

---

### Passo 2: Login

**Endpoint:** `POST /api/auth/login`

**Descrição:** Realiza o login do usuário e retorna um token JWT para autenticação nas demais requisições. Este endpoint é público e não requer autenticação.

#### Payload da Requisição

```json
{
  "email": "joao.silva@email.com",
  "password": "senha123"
}
```

#### Campos do Payload

| Campo | Tipo | Obrigatório | Descrição |
|-------|------|-------------|-----------|
| email | string | Sim | E-mail do usuário cadastrado |
| password | string | Sim | Senha do usuário |

#### Exemplo de Requisição (cURL)

```bash
curl -X POST http://localhost:5000/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "joao.silva@email.com",
    "password": "senha123"
  }'
```

#### Resposta de Sucesso (200 OK)

```json
{
  "success": true,
  "message": "Login realizado com sucesso!",
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "expiresAt": "2025-11-30T15:45:00",
    "userName": "João Silva",
    "userEmail": "joao.silva@email.com",
    "role": "User"
  }
}
```

#### Campos da Resposta

| Campo | Tipo | Descrição                         |
|-------|------|-----------------------------------|
| token | string | Token JWT para autenticação       |
| expiresAt | DateTime | Data e hora de expiração do token |
| userName | string | Nome do usuário logado            |
| userEmail | string | E-mail do usuário logado          |
| role | string | Role do usuário (User ou Admin)   |

#### Respostas de Erro

**Erros de validação (400 Bad Request):**
```json
{
  "success": false,
  "message": "Erro ao realizar login",
  "errors": [
    "E-mail inválido.",
    "A senha é obrigatória."
  ]
}
```

---

## Utilizando o Token JWT

Após realizar o login, utilize o token retornado no header `Authorization` das requisições que requerem autenticação:

```bash
curl -X GET http://localhost:8085/api/product \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
```

---

## API de Produtos (Product.API)

**Base URL:** `http://localhost:8085`

### Criar Produto

**Endpoint:** `POST /api/product`

**Autenticação:** Requer token JWT com role `Admin`

#### Payload da Requisição

```json
{
  "name": "Camiseta Básica",
  "description": "Camiseta 100% algodão",
  "price": 49.90,
  "quantityInStock": 100
}
```

#### Campos do Payload

| Campo | Tipo | Obrigatório | Descrição |
|-------|------|-------------|-----------|
| name | string | Sim | Nome do produto |
| description | string | Sim | Descrição do produto |
| price | decimal | Sim | Preço do produto (deve ser maior que zero) |
| quantityInStock | integer | Sim | Quantidade inicial em estoque (não pode ser negativa) |

#### Exemplo de Requisição (cURL)

```bash
curl -X POST http://localhost:8085/api/product \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer {token}" \
  -d '{
    "name": "Camiseta Básica",
    "description": "Camiseta 100% algodão",
    "price": 49.90,
    "quantityInStock": 100
  }'
```

#### Resposta de Sucesso (200 OK)

```json
{
  "Success": true,
  "Message": "Produto criado com sucesso!",
  "Data": {
    "Id": "550e8400-e29b-41d4-a716-446655440000",
    "Name": "Camiseta Básica",
    "Description": "Camiseta 100% algodão",
    "Price": 49.90,
    "QuantityInStock": 100,
    "CreatedAt": "2025-12-01T10:00:00Z"
  }
}
```

---

### Listar Produtos

**Endpoint:** `GET /api/product`

**Autenticação:** Requer token JWT com role `Admin`

#### Exemplo de Requisição (cURL)

```bash
curl -X GET http://localhost:8085/api/product \
  -H "Authorization: Bearer {token}"
```

#### Resposta de Sucesso (200 OK)

```json
{
  "Success": true,
  "Message": "Produtos listados com sucesso!",
  "Data": [
    {
      "Id": "550e8400-e29b-41d4-a716-446655440000",
      "Name": "Camiseta Básica",
      "Description": "Camiseta 100% algodão",
      "Price": 49.90,
      "QuantityInStock": 100,
      "CreatedAt": "2025-12-01T10:00:00Z"
    }
  ]
}
```

---

### Consultar Produto por Nome

**Endpoint:** `GET /api/product/consult?name={nome}`

**Autenticação:** Requer token JWT com role `Admin` ou `User`

#### Parâmetros da Query

| Parâmetro | Tipo | Obrigatório | Descrição |
|-----------|------|-------------|-----------|
| name | string | Sim | Nome ou parte do nome do produto para busca |

#### Exemplo de Requisição (cURL)

```bash
curl -X GET "http://localhost:8085/api/product/consult?name=Camiseta" \
  -H "Authorization: Bearer {token}"
```

#### Resposta de Sucesso (200 OK)

```json
{
  "Success": true,
  "Message": "Produto encontrado!",
  "Data": {
    "Id": "550e8400-e29b-41d4-a716-446655440000",
    "Name": "Camiseta Básica",
    "Description": "Camiseta 100% algodão",
    "Price": 49.90,
    "QuantityInStock": 100,
    "CreatedAt": "2025-12-01T10:00:00Z"
  }
}
```

#### Resposta de Erro - Produto não encontrado (400 Bad Request)

```json
{
  "Success": false,
  "Message": "Produto não encontrado."
}
```

---

### Atualizar Produto

**Endpoint:** `PUT /api/product`

**Autenticação:** Requer token JWT com role `Admin`

#### Payload da Requisição

```json
{
  "id": "550e8400-e29b-41d4-a716-446655440000",
  "name": "Camiseta Premium",
  "description": "Camiseta 100% algodão premium",
  "price": 79.90
}
```

#### Campos do Payload

| Campo | Tipo | Obrigatório | Descrição |
|-------|------|-------------|-----------|
| id | guid | Sim | ID do produto a ser atualizado |
| name | string | Não | Novo nome do produto |
| description | string | Não | Nova descrição do produto |
| price | decimal | Não | Novo preço do produto |

> **Nota:** Apenas os campos informados serão atualizados. Campos omitidos mantêm o valor original.

#### Exemplo de Requisição (cURL)

```bash
curl -X PUT http://localhost:8085/api/product \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer {token}" \
  -d '{
    "id": "550e8400-e29b-41d4-a716-446655440000",
    "name": "Camiseta Premium",
    "price": 79.90
  }'
```

#### Resposta de Sucesso (200 OK)

```json
{
  "Success": true,
  "Message": "Produto atualizado com sucesso!",
  "Data": {
    "Id": "550e8400-e29b-41d4-a716-446655440000",
    "Name": "Camiseta Premium",
    "Description": "Camiseta 100% algodão",
    "Price": 79.90,
    "QuantityInStock": 100,
    "CreatedAt": "2025-12-01T10:00:00Z",
    "UpdatedAt": "2025-12-01T11:00:00Z"
  }
}
```

---

### Deletar Produto

**Endpoint:** `DELETE /api/product?id={id}`

**Autenticação:** Requer token JWT com role `Admin`

#### Parâmetros da Query

| Parâmetro | Tipo | Obrigatório | Descrição |
|-----------|------|-------------|-----------|
| id | guid | Sim | ID do produto a ser deletado |

#### Exemplo de Requisição (cURL)

```bash
curl -X DELETE "http://localhost:8085/api/product?id=550e8400-e29b-41d4-a716-446655440000" \
  -H "Authorization: Bearer {token}"
```

#### Resposta de Sucesso (200 OK)

```json
{
  "Success": true,
  "Message": "Produto deletado com sucesso!"
}
```

#### Resposta de Erro - Produto não encontrado (400 Bad Request)

```json
{
  "Success": false,
  "Message": "Produto não encontrado."
}
```

---

## API de Estoque (Stock.API)

**Base URL:** `http://localhost:8090`

### Adicionar Estoque

**Endpoint:** `POST /api/stock`

**Autenticação:** Requer token JWT com role `Admin`

**Descrição:** Adiciona uma entrada de estoque para um produto existente. O estoque do produto é atualizado automaticamente via mensageria (RabbitMQ).

#### Payload da Requisição

```json
{
  "productId": "550e8400-e29b-41d4-a716-446655440000",
  "quantity": 50,
  "invoiceNumber": "NF-2025-001234"
}
```

#### Campos do Payload

| Campo | Tipo | Obrigatório | Descrição |
|-------|------|-------------|-----------|
| productId | guid | Sim | ID do produto para adicionar estoque |
| quantity | integer | Sim | Quantidade a ser adicionada (deve ser maior que zero) |
| invoiceNumber | string | Sim | Número da nota fiscal de entrada |

#### Exemplo de Requisição (cURL)

```bash
curl -X POST http://localhost:8090/api/stock \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer {token}" \
  -d '{
    "productId": "550e8400-e29b-41d4-a716-446655440000",
    "quantity": 50,
    "invoiceNumber": "NF-2025-001234"
  }'
```

#### Resposta de Sucesso (200 OK)

```json
{
  "Success": true,
  "Message": "Entrada de estoque registrada com sucesso!",
  "Data": {
    "Id": "660e8400-e29b-41d4-a716-446655440001",
    "ProductId": "550e8400-e29b-41d4-a716-446655440000",
    "Quantity": 50,
    "InvoiceNumber": "NF-2025-001234",
    "CreatedAt": "2025-12-01T12:00:00Z"
  }
}
```

#### Respostas de Erro

**Erros de validação (400 Bad Request):**
```json
{
  "Success": false,
  "Message": "Erro ao registrar entrada de estoque",
  "Errors": [
    "O ID do produto é obrigatório.",
    "A quantidade deve ser maior que zero.",
    "O número da nota fiscal é obrigatório."
  ]
}
```

---

## API de Pedidos (Order.API)

**Base URL:** `http://localhost:8095`

### Criar Pedido

**Endpoint:** `POST /api/order`

**Autenticação:** Requer token JWT com role `Admin` ou `User`

**Descrição:** Cria um novo pedido com os produtos selecionados. O sistema valida a disponibilidade de estoque e, ao confirmar, dá baixa automática no estoque dos produtos.

#### Payload da Requisição

```json
{
  "customerDocument": "12345678901",
  "sellerName": "João Silva",
  "items": [
    {
      "productId": "550e8400-e29b-41d4-a716-446655440000",
      "quantity": 2
    },
    {
      "productId": "550e8400-e29b-41d4-a716-446655440001",
      "quantity": 1
    }
  ]
}
```

#### Campos do Payload

| Campo | Tipo | Obrigatório | Descrição |
|-------|------|-------------|-----------|
| customerDocument | string | Sim | Documento do cliente (CPF/CNPJ, máx. 15 caracteres) |
| sellerName | string | Sim | Nome do vendedor (máx. 300 caracteres) |
| items | array | Sim | Lista de itens do pedido (mínimo 1 item) |

#### Campos do Item

| Campo | Tipo | Obrigatório | Descrição |
|-------|------|-------------|-----------|
| productId | guid | Sim | ID do produto |
| quantity | integer | Sim | Quantidade desejada (deve ser maior que zero) |

#### Exemplo de Requisição (cURL)

```bash
curl -X POST http://localhost:8095/api/order \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer {token}" \
  -d '{
    "customerDocument": "12345678901",
    "sellerName": "João Silva",
    "items": [
      {
        "productId": "550e8400-e29b-41d4-a716-446655440000",
        "quantity": 2
      }
    ]
  }'
```

#### Resposta de Sucesso (200 OK)

```json
{
  "Success": true,
  "Message": "Pedido criado com sucesso!",
}
```

---

## Portas dos Serviços

| Serviço | Porta | Descrição |
|---------|-------|-----------|
| Auth.API | 8080    | Autenticação e gerenciamento de usuários |
| Product.API | 8085  | CRUD de produtos |
| Stock.API | 8090  | Entrada de estoque |
| Order.API | 8095  | Emissão de pedidos |

---

## Permissões por Role

| Endpoint | Admin | User |
|----------|-------|------|
| POST /api/auth/createUser | ✓ (público) | ✓ (público) |
| POST /api/auth/login | ✓ (público) | ✓ (público) |
| POST /api/product | ✓ | ✗ |
| GET /api/product | ✓ | ✗ |
| GET /api/product/consult | ✓ | ✓ |
| PUT /api/product | ✓ | ✗ |
| DELETE /api/product | ✓ | ✗ |
| POST /api/stock | ✓ | ✗ |
| POST /api/order | ✓ | ✓ |
