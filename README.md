<h1 align="center"> RPG Items Manager API </h1>

![RPG-ITEMS-BANNER](https://img.shields.io/badge/RPG-Items%20Manager-orange?style=for-the-badge)

![C#](https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=csharp&logoColor=white) ![.Net](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white) ![SQLite](https://img.shields.io/badge/sqlite-%2307405e.svg?style=for-the-badge&logo=sqlite&logoColor=white) ![Visual Studio Code](https://img.shields.io/badge/Visual%20Studio%20Code-0078d7.svg?style=for-the-badge&logo=visual-studio-code&logoColor=white) ![Badge em Desenvolvimento](http://img.shields.io/static/v1?label=STATUS&message=CONCLUÍDO&color=GREEN&style=for-the-badge)

Sistema completo de gerenciamento de itens RPG com API REST e interface CLI interativa desenvolvido em C# com ASP.NET Core e SQLite.

## 💻 Funcionamento

O **RPG Items Manager** é uma aplicação que combina uma API RESTful com uma interface de linha de comando (CLI) interativa para gerenciar itens de jogos RPG. O sistema permite:

- **Cadastrar** itens com nome, raridade (1-5 estrelas ⭐) e preço
- **Listar** todos os itens cadastrados com tabela formatada e colorida
- **Buscar** itens específicos por ID
- **Atualizar** informações de itens existentes
- **Remover** itens do banco de dados

A aplicação utiliza **Entity Framework Core** com **SQLite** para persistência de dados e oferece validação completa de entrada com loops de retry, garantindo que dados inválidos não sejam aceitos. A interface CLI possui cores personalizadas, estrelas emoji para representar raridade e barra de progresso animada durante operações.

Além do CLI, a API REST está disponível em `http://localhost:5099/api/items` com documentação interativa via **Swagger** em `/swagger`.

## ⚙ Técnicas e tecnologias utilizadas

- **ASP.NET Core 8.0** - Framework web
- **Entity Framework Core** - ORM para acesso ao banco de dados
- **SQLite** - Banco de dados relacional leve
- **Swagger/OpenAPI** - Documentação interativa da API
- **C# 12** - Linguagem de programação
- **Migrations** - Versionamento do banco de dados
- **Console UI** - Interface colorida com UTF-8 encoding

## 🚀 Como executar o projeto

### Pré-requisitos

- .NET SDK 8.0 ou superior
- Visual Studio Code (opcional, mas recomendado)
-
