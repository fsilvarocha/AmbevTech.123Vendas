# AmbevTech.123Vendas

**Projeto para avaliação técnica**

## Tecnologias Utilizadas

- **.NET 8**
- **SQL Server 12**
- **Swagger**
- **Serilog**
- **Microsoft Identity**
- **RabbitMQ**

## Descrição do Projeto

Este projeto visa demonstrar habilidades em desenvolvimento de software, abordando conceitos como autenticação, registro de logs e comunicação assíncrona.

## Funcionalidades

- Cadastro e autenticação de usuários.
- Processo para gerar vendas
- Registro de atividades com Serilog.
- Documentação da API com Swagger.
- Comunicação entre serviços com RabbitMQ.

## Estrutura do Projeto

### 1. Domain
A camada **Domain** é responsável pelas regras de negócio e lógica central do seu sistema. Ela contém as entidades, agregados e as interfaces de repositórios. Essa camada deve ser independente de qualquer tecnologia ou framework específico, focando apenas nas regras de negócio e na definição do modelo.

**Exemplo de Componentes:**
- Entidades
- Value Objects
- Interfaces de Repositórios

---

### 2. Application
A camada **Application** atua como intermediária entre a camada de domínio e a camada de apresentação (API). Ela contém a lógica de aplicação, casos de uso e serviços que orquestram as interações entre as entidades e repositórios. Aqui, você também pode incluir lógica de validação e manipulação de dados de entrada.

**Exemplo de Componentes:**
- Serviços de Aplicação
- DTOs (Data Transfer Objects)
- Interfaces de Serviços

---

### 3. IoC (Inversion of Control)
A camada **IoC** é responsável pela injeção de dependência e gerenciamento do ciclo de vida dos objetos. Ela configura os serviços e repositórios que serão utilizados nas outras camadas, permitindo que você separe a configuração da lógica de negócio e facilite o teste.

**Exemplo de Componentes:**
- Configurações de Injeção de Dependência
- Registradores de Serviços

---

### 4. API
A camada **API** é a interface que expõe os serviços do seu sistema para clientes externos (como front-ends, aplicativos móveis ou outras APIs). Ela define os controladores, rotas e serialização/deserialização de dados. O foco é em receber solicitações e retornar respostas apropriadas.

**Exemplo de Componentes:**
- Controladores
- Endpoints
- Middleware
- Configuração do Swagger

---

### 5. Tests
A camada **Tests** contém todos os testes automatizados do seu sistema, incluindo testes unitários e de integração. Ela deve validar se cada parte do sistema funciona como esperado e se a lógica de negócios está sendo aplicada corretamente. É uma prática importante para garantir a qualidade do código e prevenir regressões.

**Exemplo de Componentes:**
- Testes Unitários
- Testes de Integração
- Mocks


## Como Executar o Projeto
- Alterar a connectionString para sua base de dados do sqlserver
- Alterar a connectinString para seu servidor RabbitMQ
- Executar o Migration
  
1. Clone o repositório:
   ```bash
   git clone https://github.com/SEU_USUARIO/AmbevTech.123Vendas.git
