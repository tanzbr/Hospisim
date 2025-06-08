# 🏥 HOSPISIM - Sistema de Gestão Hospitalar

Sistema completo de gestão hospitalar desenvolvido em **ASP.NET Core MVC** para o Hospital Vida Plena.

## 📋 Funcionalidades

- **👥 Gestão de Pacientes** - Cadastro, edição e consulta de pacientes
- **👨‍⚕️ Gestão de Profissionais de Saúde** - Controle de médicos e especialidades
- **📋 Atendimentos** - Registro e acompanhamento de consultas e emergências
- **🛏️ Internações** - Controle de internações hospitalares
- **🚪 Altas Hospitalares** - Gestão de altas e instruções pós-alta
- **📊 Dashboard** - Visão geral com estatísticas em tempo real

## 🚀 Como Executar

### Pré-requisitos

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/sql-server) ou [SQL Server LocalDB](https://docs.microsoft.com/sql/database-engine/configure-windows/sql-server-express-localdb)

### Passos para Execução

1. **Clone o repositório**
   ```bash
   git clone <url-do-repositorio>
   cd Hospisim
   ```

2. **Configure a string de conexão**
   
   Edite o arquivo `appsettings.json` e configure a conexão com o banco:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=HospisimDb;Trusted_Connection=true;MultipleActiveResultSets=true"
     }
   }
   ```

3. **Execute as migrações do banco de dados**
   ```bash
   dotnet ef database update
   ```

4. **Execute a aplicação**
   ```bash
   dotnet run
   ```

5. **Acesse a aplicação**
   
   Abra o navegador e acesse: `https://localhost:5001` ou `http://localhost:5000`

### Comandos Úteis

```bash
# Restaurar dependências
dotnet restore

# Compilar o projeto
dotnet build

# Executar testes (se houver)
dotnet test

# Criar nova migração
dotnet ef migrations add NomeDaMigracao

# Reverter migração
dotnet ef database update MigracaoAnterior
```

## 🗄️ Modelo de Dados

### Entidades Principais

#### 👤 **Paciente**
- Informações pessoais (nome, CPF, data nascimento, etc.)
- Contatos (telefone, email, endereço)
- Plano de saúde
- **Relacionamentos:**
  - `1:N` com Prontuários
  - `1:N` com Atendimentos (via Prontuário)
  - `1:N` com Internações

#### 📋 **Prontuário**
- Número único do prontuário
- Data de criação
- **Relacionamentos:**
  - `N:1` com Paciente
  - `1:N` com Atendimentos

#### 👨‍⚕️ **ProfissionalSaude**
- Dados pessoais e profissionais
- Registro profissional (CRM, CRO, etc.)
- Turno de trabalho
- Status (ativo/inativo)
- **Relacionamentos:**
  - `N:1` com Especialidade
  - `1:N` com Atendimentos
  - `1:N` com Prescrições

#### 🏥 **Especialidade**
- Nome da especialidade médica
- Descrição
- **Relacionamentos:**
  - `1:N` com ProfissionaisSaude

#### 📝 **Atendimento**
- Data e hora do atendimento
- Tipo (Consulta, Emergência, Internação)
- Status (Em Andamento, Finalizado, Cancelado)
- Local do atendimento
- **Relacionamentos:**
  - `N:1` com Paciente
  - `N:1` com ProfissionalSaude
  - `N:1` com Prontuário
  - `1:N` com Prescrições
  - `1:N` com Exames
  - `1:1` com Internação (opcional)

#### 🛏️ **Internação**
- Data de entrada e previsão de alta
- Setor, quarto e leito
- Motivo da internação
- Status (Ativa, Alta Concedida, Transferido, Óbito)
- Observações clínicas
- Plano de saúde utilizado
- **Relacionamentos:**
  - `1:1` com Atendimento
  - `N:1` com Paciente
  - `1:1` com AltaHospitalar (opcional)

#### 🚪 **AltaHospitalar**
- Data da alta
- Condição do paciente (Curado, Melhorado, etc.)
- Instruções pós-alta
- **Relacionamentos:**
  - `1:1` com Internação

#### 💊 **Prescrição**
- Medicamento prescrito
- Dosagem e frequência
- Data da prescrição
- Status (Ativa, Encerrada)
- **Relacionamentos:**
  - `N:1` com Atendimento
  - `N:1` com ProfissionalSaude

#### 🔬 **Exame**
- Tipo do exame
- Data de realização
- Resultado
- **Relacionamentos:**
  - `N:1` com Atendimento

### Diagrama de Relacionamentos

```
Paciente (1) ──────── (N) Prontuário (1) ──────── (N) Atendimento
    │                                                    │
    │                                                    │ (N)
    │ (N)                                                │
    │                                              ProfissionalSaude
    │                                                    │ (N)
    │                                                    │
    │                                              Especialidade (1)
    │
    │ (N)
    │
Internação (1) ──────── (1) AltaHospitalar
    │ (1)
    │
    │ (1)
Atendimento ──────── (N) Prescrição
    │                      │ (N)
    │ (N)                  │
    │                ProfissionalSaude (1)
    │
    │ (N)
Exame
```

## 🛠️ Tecnologias Utilizadas

- **Framework:** ASP.NET Core 8.0 MVC
- **ORM:** Entity Framework Core
- **Banco de Dados:** SQL Server
- **Frontend:** Bootstrap 5, Font Awesome, jQuery
- **Arquitetura:** Repository Pattern, Service Layer
- **Validação:** Data Annotations, FluentValidation

## 📁 Estrutura do Projeto

```
Hospisim/
├── Controllers/          # Controllers MVC
├── Models/              # ViewModels e DTOs
├── Views/               # Views Razor
├── Domain/
│   ├── Entities/        # Entidades do domínio
│   └── Enums/          # Enumerações
├── Data/               # Contexto do EF Core
├── Business/
│   ├── Contracts/      # Interfaces dos serviços
│   ├── Services/       # Implementação dos serviços
│   └── Validators/     # Validadores
├── Migrations/         # Migrações do EF Core
└── wwwroot/           # Arquivos estáticos
```

## 🎯 Funcionalidades por Módulo

### Dashboard
- Estatísticas em tempo real
- Ações rápidas
- Listagem de atendimentos recentes
- Internações ativas
- Altas recentes

### Pacientes
- CRUD completo
- Busca e filtros
- Histórico de atendimentos

### Profissionais de Saúde
- Gestão de profissionais
- Controle de especialidades
- Status ativo/inativo

### Atendimentos
- Registro de consultas e emergências
- Vinculação com pacientes e profissionais
- Controle de status

### Internações
- Gestão de leitos e setores
- Controle de status
- Previsão de alta

### Altas Hospitalares
- Registro de altas
- Instruções pós-alta
- Condição do paciente

## 📝 Licença

Este projeto foi desenvolvido para fins educacionais.

---

**Hospital Vida Plena** - Sistema de Gestão Hospitalar © 2025