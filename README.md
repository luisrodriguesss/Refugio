# REFÚGIO — API REST .NET

**Global Solution 2026/2 — FIAP**

**Tema:** CitizenTech — Tecnologia com o cidadão no centro da inovação

**Disciplina:** Advanced Business Development with .NET

## Sobre o projeto

O REFÚGIO é uma solução tecnológica voltada a facilitar a identificação de pontos de alívio térmico em ambientes urbanos.

A proposta busca auxiliar cidadãos, especialmente idosos, trabalhadores externos e pessoas expostas ao calor intenso, a localizar espaços cadastrados que ofereçam recursos como água, sombra, assentos e acessibilidade.

Esta API REST representa o módulo acadêmico desenvolvido em .NET, responsável pelo gerenciamento de pontos de alívio térmico e seus recursos.

## Objetivo

Desenvolver uma API REST utilizando ASP.NET Core 8, Entity Framework Core e Oracle, demonstrando operações CRUD, validação de dados, persistência e relacionamento entre entidades.

## Tecnologias utilizadas

- C# e ASP.NET Core 8
- Entity Framework Core 8
- Oracle Database
- Oracle.EntityFrameworkCore
- EF Core Migrations
- REST API com Controllers
- Git e GitHub
- Visual Studio Code

## Arquitetura

O sistema possui duas entidades principais.

### Ponto

Representa um local cadastrado para alívio térmico.

**Atributos:** IdPonto, CodigoPublico, Nome, Bairro, HorarioTexto e Status.

### Recurso

Representa um recurso disponível em determinado ponto.

**Atributos:** IdRecurso, IdPonto, Tipo e Disponivel.

### Relacionamento 1:N

Um ponto pode possuir vários recursos, mas cada recurso pertence a somente um ponto.

```text
NET_PONTO (1)
    |
    |--- (N) NET_RECURSO
```

As tabelas são criadas no Oracle por meio do Entity Framework Core Migrations.

## Funcionalidades

A API permite:

- Cadastrar, listar, consultar, atualizar e excluir pontos.
- Cadastrar, listar, consultar, atualizar e excluir recursos.
- Validar campos obrigatórios e valores permitidos.
- Impedir códigos públicos duplicados.
- Impedir recursos do mesmo tipo duplicados em um ponto.
- Impedir a exclusão de pontos com recursos vinculados.
- Preservar o código público durante a atualização de pontos.
- Utilizar DTOs para cadastro e atualização.
- Retornar respostas HTTP adequadas.

## Endpoints

### Pontos

| Método | Endpoint | Descrição |
|---|---|---|
| GET | `/api/pontos` | Lista pontos |
| GET | `/api/pontos/{id}` | Consulta ponto |
| POST | `/api/pontos` | Cadastra ponto |
| PUT | `/api/pontos/{id}` | Atualiza ponto |
| DELETE | `/api/pontos/{id}` | Exclui ponto |

### Recursos

| Método | Endpoint | Descrição |
|---|---|---|
| GET | `/api/recursos` | Lista recursos |
| GET | `/api/recursos/{id}` | Consulta recurso |
| POST | `/api/recursos` | Cadastra recurso |
| PUT | `/api/recursos/{id}` | Atualiza recurso |
| DELETE | `/api/recursos/{id}` | Exclui recurso |

## Regras de negócio

1. O código público de um ponto deve ser único e seguir o formato `REF-NNN`.
2. Os status permitidos são `ATIVO` e `INATIVO`.
3. Os recursos permitidos são `AGUA`, `SOMBRA`, `ASSENTO` e `ACESSIBILIDADE`.
4. Um recurso deve estar associado a um ponto existente.
5. Não podem existir dois recursos do mesmo tipo associados ao mesmo ponto.
6. Um ponto com recursos vinculados não pode ser excluído.
7. O código público de um ponto não pode ser alterado após o cadastro.
8. Os identificadores são gerados pelo banco de dados.

## Banco de dados

A aplicação utiliza o Oracle Database com duas tabelas:

- `NET_PONTO`
- `NET_RECURSO`

A criação das tabelas é realizada por meio do Entity Framework Core Migrations.

A entidade `Recurso` possui uma chave estrangeira para `Ponto`, configurada com exclusão restrita.

O campo `Disponivel` utiliza `bool` no C#, convertido para `CHAR(1)` no Oracle (`S` ou `N`).

## Como executar

### Pré-requisitos

- .NET SDK 8
- Acesso a um banco Oracle compatível
- Ferramenta `dotnet-ef` versão 8
- Credenciais próprias do Oracle

### 1. Clonar o repositório

```bash
git clone URL_DO_REPOSITORIO
cd NOME_DA_PASTA
```

### 2. Restaurar as dependências

```bash
dotnet restore
```

### 3. Configurar a conexão

O projeto utiliza User Secrets para evitar o versionamento de credenciais.

```bash
dotnet user-secrets init
```

Configure a conexão com os dados do seu ambiente:

```bash
dotnet user-secrets set "ConnectionStrings:Oracle" "User Id=USUARIO;Password=SENHA;Data Source=HOST:PORTA/SERVICO;"
```

Não compartilhe credenciais reais nem as inclua no repositório.

### 4. Aplicar as Migrations

```bash
dotnet ef database update
```

### 5. Executar a API

No PowerShell:

```powershell
$env:ASPNETCORE_ENVIRONMENT = "Development"
dotnet run --no-launch-profile
```

A aplicação utiliza a porta configurada no ambiente. Nos testes locais, foi utilizada a porta 5000.

## Exemplos de requisições

### POST — Cadastrar ponto

`POST /api/pontos`

```json
{
  "codigoPublico": "REF-013",
  "nome": "Centro Comunitario Solar",
  "bairro": "Centro Piloto",
  "horarioTexto": "08:00-18:00",
  "status": "ATIVO"
}
```

### POST — Cadastrar recurso

`POST /api/recursos`

```json
{
  "idPonto": 1,
  "tipo": "AGUA",
  "disponivel": true
}
```

O `idPonto` deve corresponder a um ponto existente.

### PUT — Atualizar disponibilidade

`PUT /api/recursos/21`

```json
{
  "idPonto": 2,
  "tipo": "ASSENTO",
  "disponivel": false
}
```

## Testes realizados

Durante o desenvolvimento foram realizados testes de:

- Cadastro e consulta de pontos e recursos.
- Atualização e exclusão de registros.
- Persistência no Oracle.
- Relacionamento 1:N.
- Bloqueio de códigos e recursos duplicados.
- Validação de campos obrigatórios.
- Rejeição de tipos de recursos inválidos.
- Respostas HTTP 400, 404 e 409.
- Atualização de dados com DTOs.

As operações de cadastro retornam HTTP 201 e as operações de atualização e exclusão bem-sucedidas retornam HTTP 204.

## Estrutura do projeto

```text
Refugio/
├── Controllers/
│   ├── PontosController.cs
│   └── RecursosController.cs
├── Data/
│   └── RefugioContext.cs
├── DTOs/
│   ├── PontoCreateDto.cs
│   ├── PontoUpdateDto.cs
│   ├── RecursoCreateDto.cs
│   └── RecursoUpdateDto.cs
├── Models/
│   ├── Ponto.cs
│   └── Recurso.cs
├── Migrations/
├── Program.cs
├── Refugio.API.csproj
├── global.json
└── README.md
```

## Segurança

As credenciais do banco de dados são armazenadas localmente utilizando User Secrets.

Arquivos de compilação, como `bin/` e `obj/`, não devem ser versionados no GitHub.

Esta API é um protótipo acadêmico e não possui autenticação própria. Não deve ser exposta publicamente sem controles adicionais de acesso.

## Equipe

**FIAP — Análise e Desenvolvimento de Sistemas**

**Turma:** [2TDSPB]

**Integrantes:**

- [Luis Fillipe Seripieri — RM567918]
- [Luiz Felipe Kichimoto — RM567726]
- [Gabriel Rocha de Souza — RM567023]
- [Gabriel Stuani — RM566682]
- [Guilherme Soares — RM568227]

## Considerações finais

A API .NET do REFÚGIO demonstra a utilização de tecnologias de desenvolvimento backend e banco de dados relacional para gerenciar informações de pontos de alívio térmico.

O módulo utiliza uma arquitetura REST, validações, regras de integridade e persistência com Entity Framework Core e Oracle, contribuindo para a proposta CitizenTech da Global Solution 2026/2.

**FIAP — Global Solution 2026/2 | REFÚGIO**