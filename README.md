# 📦 Sistema de Estoque (Console App)

Gerenciador de Estoque desenvolvido em **C#/.NET**, com persistência em **MySQL**.  
Permite cadastrar, editar e consultar produtos, registrar movimentações de estoque e gerar relatórios.  
Todas as operações críticas são registradas em **arquivos de log**, garantindo rastreabilidade.

---

## 🚀 Funcionalidades

- 🔑 Autenticação simples (usuário/senha com criptografia).  
- 📋 Cadastro e edição de produtos (nome, categoria, quantidade, preço, etc.).  
- ➕ Registro de entradas de estoque.  
- ➖ Registro de saídas de estoque.  
- 🔍 Consultas usando **LINQ** (ex.: filtro por categoria ou estoque baixo).  
- 📝 Geração de relatórios em **CSV**.  
- 📂 Registro de operações em arquivos de log.  

---

## 🛠️ Tecnologias utilizadas

- **C# / .NET (Console App)**  
- **MySQL**  
- **Entity Framework Core**  
- **LINQ**  
- **System.IO** (manipulação de arquivos)  
- **System.Security.Cryptography** (hash de senhas)  

---

## 📚 Conceitos explorados

- LINQ em coleções genéricas e entidades.  
- Repositório genérico para CRUD com classes genéricas (`List<T>`).  
- Acesso a dados via **EF Core** e/ou **ADO.NET**.  
- Manipulação de arquivos (logs e relatórios CSV).  
- Criptografia de senhas (MD5/SHA).  

---

## ⚙️ Pré-requisitos

- [.NET 6+ SDK](https://dotnet.microsoft.com/en-us/download)  
- [MySQL](https://dev.mysql.com/downloads/)  
- Configuração do **connection string** no `appsettings.json` ou no código.  

---

## ▶️ Como executar

1. Clone o repositório:
   ```bash
   git clone https://github.com/VictoRGBC/Sistema-De-Estoque.git
