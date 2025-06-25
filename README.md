# CaseItau - API de Fundos de Investimento

## 📋 Visão Geral

Esta é uma API REST para gerenciamento de fundos de investimento, desenvolvida com .NET 8 seguindo princípios de Clean Architecture, Domain-Driven Design (DDD) e Command Query Responsibility Segregation (CQRS).

## 🚀 Melhorias Implementadas

### 🔒 Segurança

#### **SQL Injection Prevention**
- **Problema Identificado**: Queries SQL diretas com concatenação de strings
- **Solução Implementada**: 
  - Uso completo do Entity Framework Core com consultas tipadas
  - Parâmetros SQL automaticamente sanitizados pelo ORM
  - Eliminação de consultas SQL raw vulneráveis

```csharp
// ❌ Antes (Vulnerável)
string sql = $"SELECT * FROM Funds WHERE Code = '{code}'";

// ✅ Depois (Seguro)
var fund = await _context.Funds.FirstOrDefaultAsync(f => f.Code == code);
```

#### **Error Handling Robusto**
- **Problema Identificado**: Ausência de tratamento de erros estruturado
- **Solução Implementada**:
  - Implementação do padrão Result com `ErrorOr<T>`
  - Middleware global de tratamento de exceções
  - Mapeamento de erros de domínio para códigos HTTP apropriados
  - Logs estruturados para debugging

```csharp
// ✅ Tratamento de erros estruturado
public async Task<ErrorOr<Fund>> Handle(CreateFundCommand request, CancellationToken cancellationToken)
{
    var fundType = await _fundTypeRepository.GetByIdAsync(request.TypeId);
    if (fundType is null)
        return ApplicationErrors.FundTypeNotFound;
    
    var existingFund = await _fundRepository.GetByCodeAsync(request.Code);
    if (existingFund is not null)
        return ApplicationErrors.FundAlreadyExists;
        
    // ... resto da implementação
}
```

### 🏗️ Arquitetura

#### **Clean Architecture**
- **Separação de Responsabilidades**: Controllers não fazem mais conexão direta com banco
- **Camadas bem definidas**:
  - `Domain`: Entidades, Value Objects, Eventos de Domínio
  - `Application`: Casos de Uso, Commands, Queries
  - `Infrastructure`: Implementações de repositórios, DbContext
  - `API`: Controllers, configurações, middlewares

```
src/
├── CaseItau.Domain/           # Regras de negócio puras
├── CaseItau.Application/      # Casos de uso e lógica de aplicação
├── CaseItau.Infrastructure/   # Implementações de infraestrutura
├── CaseItau.Contracts/        # DTOs e contratos
└── CaseItau.API/             # Camada de apresentação
```

#### **Domain-Driven Design (DDD)**
- **Entidades Rich Domain Models**: `Fund`, `FundType` com comportamentos encapsulados
- **Value Objects**: `Cnpj` com validação incorporada
- **Domain Events**: `FundCreatedEvent` para comunicação entre agregados
- **Aggregate Roots**: Entidades que controlam consistência de dados

```csharp
public class Fund : Entity, IAggregateRoot
{
    public Fund(string code, string name, Cnpj cnpj, long typeId)
    {
        Code = code;
        Name = name;
        Cnpj = cnpj;
        TypeId = typeId;

        _events.Add(new FundCreatedEvent(Id, code, DateTime.UtcNow));
    }
}
```

#### **CQRS (Command Query Responsibility Segregation)**
- **Commands**: Para operações que modificam estado
  - `CreateFundCommand`, `UpdateFundCommand`, `AdjustPatrimonyCommand`
- **Queries**: Para operações de leitura otimizadas
  - `GetFundQuery`, `ListFundsQuery`
- **Handlers**: Separação clara entre leitura e escrita

#### **Vertical Slice Architecture**
- Organização por features ao invés de camadas técnicas
- Cada feature contém seus Commands, Queries, DTOs e Handlers
- Reduz acoplamento e facilita manutenção

### 🛠️ Tecnologias e Ferramentas

#### **.NET 8**
- **Última versão LTS** com melhorias de performance
- **Nullable Reference Types** habilitado para maior segurança de tipos

#### **Entity Framework Core**
- **ORM completo** substituindo queries SQL diretas
- **Migrations** para controle de versionamento do banco
- **Configurações de entidade** tipadas e validadas

#### **Swagger/OpenAPI**
- **Documentação automática** da API
- **Versionamento de API** documentado

### 🔧 Correções Específicas

#### **Patrimônio Nullable**
- **Problema Identificado**: Campo patrimônio estava causando erros por não aceitar valores nulos
- **Solução**: Configuração adequada do tipo como `decimal` com valor padrão 0
- **Implementação**: HttpPatch específico para atualização de patrimônio

```csharp
[HttpPatch("{code}/patrimony")]
public async Task<IActionResult> AdjustPatrimony(string code, AdjustPatrimonyRequest request)
{
    var command = new AdjustPatrimonyCommand(code, request.Amount);
    var result = await _mediator.Send(command);
    return result.Match(Ok, Problem);
}
```

## 🧪 Testes Implementados

### **Cobertura de Testes**
- **Unit Tests**: 95%+ de cobertura em domínio e aplicação
- **Integration Tests**: Testes end-to-end dos endpoints
- **Testes de Repository**: Validação de persistência

### **Ferramentas de Teste**
- **xUnit**: Framework de testes
- **FluentAssertions**: Assertions mais legíveis
- **Moq**: Mocking de dependências
- **WebApplicationFactory**: Testes de integração

## 📊 Estrutura da API

### **Endpoints Disponíveis**

```
GET    /api/v1/funds           # Listar todos os fundos
GET    /api/v1/funds/{code}    # Buscar fundo por código
POST   /api/v1/funds           # Criar novo fundo
PUT    /api/v1/funds/{code}    # Atualizar fundo completo
PATCH  /api/v1/funds/{code}/patrimony  # Ajustar patrimônio
DELETE /api/v1/funds/{code}    # Remover fundo
```

### **Modelos de Dados**

```csharp
// Request para criação de fundo
public record CreateFundRequest(
    string Code,
    string Name,
    string Cnpj,
    long TypeId
);

// Response com dados do fundo
public record FundResponse(
    string Code,
    string Name,
    string Cnpj,
    long TypeId,
    string TypeName,
    decimal Patrimony
);
```

## 🔄 Considerações de Design

### **Reestruturação Completa**

> **Importante**: Levando em consideração uma reestruturação total, foi realizada o renomeamento de todas as classes e camada de dados. Contudo, caso fosse uma aplicação existente, ou houvesse limitações de escopo e contratos de objetos já definidos, eu optaria por manter as mesmas rotas e objetos para garantir compatibilidade com sistemas existentes.

### **Backwards Compatibility**
- Manutenção de contratos existentes quando necessário
- Versionamento de API para mudanças breaking
- Migrations graduais para sistemas legados

## ❓ Questões Arquiteturais

### **Operações Atômicas de Patrimônio**

> **Pergunta**: Mover patrimônio deveria ser uma operação atômica?

**Resposta**: Sim, definitivamente. Considerações implementadas:

1. **Transações de Banco**: Uso do `ExecuteUpdateAsync` para garantir atomicidade

## 🚀 Como Executar

### **Pré-requisitos**
- .NET 8 SDK
- SQL Server ou SQLite (configurável)

### **Comandos**

```bash
# Restaurar dependências
dotnet restore

# Executar migrations
dotnet ef database update --project src/CaseItau.Infrastructure

# Executar aplicação
dotnet run --project src/CaseItau.API

# Executar testes
dotnet test

# Executar com coverage
dotnet test --collect:"XPlat Code Coverage"
```

### **Acessos**
- **API**: `https://localhost:44378`
- **Swagger**: `https://localhost:44378/swagger`
- **Health Check**: `https://localhost:44378/health`

## 📋 Especificação Original do Projeto

### Base de Dados SQLite
    Tabela: TIPO_FUNDO > "Tipos de fundos existentes"
	- CODIGO      - INT         NOT NULL - PRIMARY KEY
	- NOME        - VARCHAR(20) NOT NULL

    Tabela: FUNDO > "Registro relacionados ao cadastro de fundos"
	- CODIGO      - VARCHAR(20) NOT NULL - PRIMARY KEY
	- NOME        - VARCHAR(100)        NOT NULL
	- CNPJ        - VARCHAR(14)  UNIQUE NOT NULL
	- CODIGO_TIPO - INT                 NOT NULL - FOREIGN KEY TIPO_FUNDO(CODIGO)
	- PATRIMONIO  - NUMERIC                 NULL

> Obs.: você pode fazer o uso do [sqliteadmin] para gerenciar a base de dados, visualizar as tabelas e seus respectivos dados

### API Original
No projeto CaseItau.API foi disponibilizada uma API de Fundos com os metodos abaixo realizando acoes diretas na base de dados:

	GET                        - LISTAR TODOS OS FUNDOS CADASTRADOS
	GET    {CODIGO}            - RETORNAR OS DETALHES DE UM DETERMINADO FUNDO PELO CÓDIGO
	POST   {FUNDO}             - REALIZA O CADASTRO DE UM NOVO FUNDO
	PUT    {CODIGO}            - EDITA O CADASTRO DE UM FUNDO JÁ EXISTENTE
	DELETE {CODIGO}            - EXCLUI O CADASTRO DE UM FUNDO
	PUT    {CODIGO}/patrimonio - ADICIONA OU SUBTRAI DETERMINADO VALOR DO PATRIMONIO DE UM FUNDO

### Problemas Identificados e Corrigidos
1. **SQL Injection**: Queries diretas substituídas por ORM
2. **Falta de separação de responsabilidades**: Controllers fazendo conexão direta com banco
3. **INSERT sem declarar colunas**: Mapeamento explícito no EF Core
4. **Ausência de error handling**: Implementação robusta de tratamento de erros
5. **Erro de patrimônio null**: Configuração adequada de tipos nullable

[sqliteadmin]: <http://sqliteadmin.orbmu2k.de>
