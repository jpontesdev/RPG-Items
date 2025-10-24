<h1 align="center"> RPG Items Manager API </h1>

![C#](https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=csharp&logoColor=white) ![.Net](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white) ![SQLite](https://img.shields.io/badge/sqlite-%2307405e.svg?style=for-the-badge&logo=sqlite&logoColor=white) ![Visual Studio Code](https://img.shields.io/badge/Visual%20Studio%20Code-0078d7.svg?style=for-the-badge&logo=visual-studio-code&logoColor=white) ![Entity Framework](https://img.shields.io/badge/Entity%20Framework-512BD4?style=for-the-badge&logo=.net&logoColor=white) ![Badge Concluído](http://img.shields.io/static/v1?label=STATUS&message=CONCLUÍDO&color=GREEN&style=for-the-badge)

Sistema de gerenciamento de itens RPG com API REST e CLI interativo desenvolvido em C# com ASP.NET Core 8.0 e SQLite.

## 💻 Funcionamento

O **RPG Items Manager** é uma aplicação que combina uma API RESTful com interface CLI colorida e interativa para gerenciar itens de jogos RPG. O sistema permite cadastrar, listar, buscar, atualizar e remover itens com nome, raridade (1-5 estrelas ⭐) e preço. A interface CLI possui validação completa com loops de retry, tabelas formatadas com bordas Unicode, cores personalizadas por tipo de mensagem e barra de progresso animada durante operações de salvamento. A API REST está disponível em http://localhost:5099/api/items com documentação interativa via Swagger em /swagger. O banco de dados SQLite gerencia a persistência com Entity Framework Core e migrations automáticas.

## ⚙ Técnicas e tecnologias utilizadas

- ``ASP.NET Core 9.0``
- ``Entity Framework Core``
- ``SQLite``
- ``C# 12``

## 📂 Acesso ao projeto

Você pode acessar os arquivos do projeto clicando [aqui](https://github.com/seu-usuario/RPG-Items-main).

**Como executar:**
- dotnet restore
- dotnet ef database update
- dotnet run

# Autores

| [<img loading="lazy" src="https://avatars.githubusercontent.com/u/134797061?v=4" width=115><br><sub>João Vitor Jardim</sub>](https://github.com/JoaoPontes05) |  [<img loading="lazy" src="https://avatars.githubusercontent.com/u/126476225?v=4" width=115><br><sub>Gabriel Duarte</sub>](https://github.com/GabrielRainwalker) |
| :---: | :---: |
