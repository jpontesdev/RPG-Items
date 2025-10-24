using Microsoft.EntityFrameworkCore;
using RPGItemsAPI.Data;
using RPGItemsAPI.Models;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://localhost:5099");

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<RPGContext>(options =>
    options.UseSqlite("Data Source=rpg.db"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<RPGContext>();
    db.Database.Migrate();
}

Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.Title = "Sistema de Itens";
Console.CursorVisible = true;

var webTask = app.RunAsync();

Console.ForegroundColor = ConsoleColor.Cyan;
Console.WriteLine("=========================================");
Console.WriteLine("     Banco de Dados Iniciando.......   ");
Console.WriteLine("=========================================");
Console.ResetColor();

await MostrarCarregamento();

Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("  LOCALHOST:     http://localhost:5099/api/items");
Console.WriteLine("=========================================\n");
Console.ResetColor();

while (true)
{
    Console.Clear();
    
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine("\n╔════════════════════════════════╗");
    Console.WriteLine("║     MENU PRINCIPAL             ║");
    Console.WriteLine("╠════════════════════════════════╣");
    Console.WriteLine("║ 1 - Cadastrar Item             ║");
    Console.WriteLine("║ 2 - Listar Todos os Itens      ║");
    Console.WriteLine("║ 3 - Buscar Item por ID         ║");
    Console.WriteLine("║ 4 - Atualizar Item             ║");
    Console.WriteLine("║ 5 - Remover Item               ║");
    Console.WriteLine("║ 0 - Sair                       ║");
    Console.WriteLine("╚════════════════════════════════╝");
    Console.ResetColor();
    
    Console.CursorVisible = true;
    Console.Write("> ");

    var opcao = Console.ReadLine();

    if (opcao == "0") break;

    switch (opcao)
    {
        case "1": await CadastrarItemAsync(); break;
        case "2": await ListarItensAsync(); break;
        case "3": await BuscarItemPorIdAsync(); break;
        case "4": await AtualizarItemAsync(); break;
        case "5": await RemoverItemAsync(); break;
        default: 
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[ERRO] Opcao invalida!");
            Console.ResetColor();
            await Task.Delay(1500);
            break;
    }
}

Console.ForegroundColor = ConsoleColor.Yellow;
Console.WriteLine("\nEncerrando servidor...");
Console.ResetColor();
await app.StopAsync();
await webTask;
Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("Aplicacao encerrada com sucesso!");
Console.ResetColor();

async Task MostrarCarregamento()
{
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.Write("Carregando");
    for (int i = 0; i < 3; i++)
    {
        await Task.Delay(300);
        Console.Write(".");
    }
    Console.WriteLine(" Pronto!");
    Console.ResetColor();
}

async Task CadastrarItemAsync()
{
    Console.Clear();
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine("\n╔════════════════════════════════╗");
    Console.WriteLine("║     CADASTRAR NOVO ITEM        ║");
    Console.WriteLine("╚════════════════════════════════╝");
    Console.ResetColor();
    
    string nome;
    while (true)
    {
        Console.Write("Nome (2-100 caracteres): ");
        nome = (Console.ReadLine() ?? "").Trim();
        
        if (string.IsNullOrWhiteSpace(nome))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[ERRO] Nome nao pode estar vazio! Tente novamente.");
            Console.ResetColor();
            continue;
        }
        
        if (nome.Length < 2)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[ERRO] Nome muito curto! Precisa ter pelo menos 2 caracteres.");
            Console.ResetColor();
            continue;
        }
        
        if (nome.Length > 100)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[ERRO] Nome muito longo! Maximo de 100 caracteres.");
            Console.ResetColor();
            continue;
        }
        
        break;
    }

    int raridade;
    while (true)
    {
        Console.Write("Raridade (1-5): ");
        var raridadeInput = Console.ReadLine();
        
        if (!int.TryParse(raridadeInput, out raridade))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[ERRO] Valor invalido! Digite um numero inteiro.");
            Console.ResetColor();
            continue;
        }
        
        if (raridade < 1 || raridade > 5)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[ERRO] Raridade deve estar entre 1 e 5! Tente novamente.");
            Console.ResetColor();
            continue;
        }
        
        break;
    }

    decimal preco;
    while (true)
    {
        Console.Write("Preço (use ponto como separador, ex: 0.01): ");
        var precoInput = Console.ReadLine()?.Replace(",", ".");
        
        if (!decimal.TryParse(precoInput, NumberStyles.Any, CultureInfo.InvariantCulture, out preco))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[ERRO] Valor invalido! Use ponto como separador (ex: 0.01).");
            Console.ResetColor();
            continue;
        }
        
        if (preco < 0.01m)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[ERRO] Preço muito baixo! Minimo e 0.01.");
            Console.ResetColor();
            continue;
        }
        
        if (preco > 9999.99m)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[ERRO] Preço muito alto! Maximo e 9999.99.");
            Console.ResetColor();
            continue;
        }
        
        break;
    }

    using var db = new RPGContext(
        new DbContextOptionsBuilder<RPGContext>()
            .UseSqlite("Data Source=rpg.db")
            .Options);

    var item = new Item { Name = nome, Rarity = raridade, Price = preco };
    db.Items.Add(item);
    
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.Write("\nSalvando item");
    for (int i = 0; i < 3; i++)
    {
        await Task.Delay(200);
        Console.Write(".");
    }
    Console.ResetColor();
    
    await db.SaveChangesAsync();
    
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine($"\n[SUCESSO] Item cadastrado! ID: {item.Id}");
    Console.ResetColor();
    
    Console.WriteLine("\nPressione qualquer tecla para continuar...");
    Console.ReadKey();
}

async Task ListarItensAsync()
{
    Console.Clear();
    using var db = new RPGContext(
        new DbContextOptionsBuilder<RPGContext>()
            .UseSqlite("Data Source=rpg.db")
            .Options);

    var itens = await db.Items.OrderBy(i => i.Id).ToListAsync();

    if (itens.Count == 0)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\n[INFO] Nenhum item cadastrado.");
        Console.ResetColor();
        Console.WriteLine("\nPressione qualquer tecla para continuar...");
        Console.ReadKey();
        return;
    }

    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine("\n╔═══════════════════════════════════════════════════════════════════╗");
    Console.WriteLine("║                      LISTA DE ITENS RPG                           ║");
    Console.WriteLine("╠═════╦═══════════════════════════╦═══════════╦═════════════════════╣");
    Console.WriteLine("║ ID  ║ Nome                      ║ Raridade  ║ Preço               ║");
    Console.WriteLine("╠═════╬═══════════════════════════╬═══════════╬═════════════════════╣");
    Console.ResetColor();
    
    foreach (var item in itens)
    {
        string estrelas = new string('⭐', item.Rarity);
        int espacosNecessarios = 10 - (item.Rarity * 3);
        
        Console.Write($"║ {item.Id,-3} ║ {item.Name,-25} ║ {estrelas}");
        Console.Write(new string(' ', espacosNecessarios));
        Console.WriteLine($" ║ R$ {item.Price,15:F2}  ║");
    }
    
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine("╚═════╩═══════════════════════════╩═══════════╩═════════════════════╝");
    Console.ResetColor();
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine($"Total de itens: {itens.Count}");
    Console.ResetColor();
    
    Console.WriteLine("\nPressione qualquer tecla para continuar...");
    Console.ReadKey();
}

async Task BuscarItemPorIdAsync()
{
    Console.Clear();
    int id;
    while (true)
    {
        Console.Write("\nDigite o ID do item: ");
        if (int.TryParse(Console.ReadLine(), out id))
            break;
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("[ERRO] ID invalido! Digite um numero inteiro.");
        Console.ResetColor();
    }

    using var db = new RPGContext(
        new DbContextOptionsBuilder<RPGContext>()
            .UseSqlite("Data Source=rpg.db")
            .Options);

    var item = await db.Items.FirstOrDefaultAsync(i => i.Id == id);

    if (item == null)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("[ERRO] Item nao encontrado.");
        Console.ResetColor();
        Console.WriteLine("\nPressione qualquer tecla para continuar...");
        Console.ReadKey();
        return;
    }

    string estrelas = new string('⭐', item.Rarity);

    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine("\n╔════════════════════════════════╗");
    Console.WriteLine("║      DETALHES DO ITEM          ║");
    Console.WriteLine("╠════════════════════════════════╣");
    Console.ResetColor();
    Console.WriteLine($"║ ID:       {item.Id,-20} ║");
    Console.WriteLine($"║ Nome:     {item.Name,-20} ║");
    Console.Write("║ Raridade: ");
    Console.Write(estrelas);
    string espacos = new string(' ', 20 - (item.Rarity * 2));
    Console.WriteLine($"{espacos} ║");
    Console.WriteLine($"║ Preço:    R$ {item.Price,-16:F2} ║");
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine("╚════════════════════════════════╝");
    Console.ResetColor();
    
    Console.WriteLine("\nPressione qualquer tecla para continuar...");
    Console.ReadKey();
}

async Task AtualizarItemAsync()
{
    Console.Clear();
    int id;
    while (true)
    {
        Console.Write("\nDigite o ID do item a atualizar: ");
        if (int.TryParse(Console.ReadLine(), out id))
            break;
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("[ERRO] ID invalido! Digite um numero inteiro.");
        Console.ResetColor();
    }

    using var db = new RPGContext(
        new DbContextOptionsBuilder<RPGContext>()
            .UseSqlite("Data Source=rpg.db")
            .Options);

    var item = await db.Items.FirstOrDefaultAsync(i => i.Id == id);

    if (item == null)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("[ERRO] Item nao encontrado.");
        Console.ResetColor();
        Console.WriteLine("\nPressione qualquer tecla para continuar...");
        Console.ReadKey();
        return;
    }

    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine($"\n[INFO] Atualizando item ID {item.Id}");
    Console.WriteLine("Deixe em branco para manter o valor atual.\n");
    Console.ResetColor();

    Console.WriteLine($"Nome atual: {item.Name}");
    Console.Write("Novo nome: ");
    var novoNome = (Console.ReadLine() ?? "").Trim();

    if (!string.IsNullOrWhiteSpace(novoNome))
    {
        while (novoNome.Length < 2 || novoNome.Length > 100)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[ERRO] Nome deve ter entre 2 e 100 caracteres!");
            Console.ResetColor();
            Console.Write("Novo nome (deixe em branco para cancelar): ");
            novoNome = (Console.ReadLine() ?? "").Trim();
            if (string.IsNullOrWhiteSpace(novoNome)) break;
        }
        if (!string.IsNullOrWhiteSpace(novoNome))
            item.Name = novoNome;
    }

    Console.WriteLine($"Raridade atual: {item.Rarity}");
    Console.Write("Nova raridade (1-5): ");
    var raridadeInput = Console.ReadLine();

    if (!string.IsNullOrWhiteSpace(raridadeInput))
    {
        while (true)
        {
            if (int.TryParse(raridadeInput, out int novaRaridade) && novaRaridade >= 1 && novaRaridade <= 5)
            {
                item.Rarity = novaRaridade;
                break;
            }
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[ERRO] Raridade deve estar entre 1 e 5!");
            Console.ResetColor();
            Console.Write("Nova raridade (deixe em branco para cancelar): ");
            raridadeInput = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(raridadeInput)) break;
        }
    }

    Console.WriteLine($"Preço atual: R$ {item.Price:F2}");
    Console.Write("Novo preço (use ponto, ex: 0.01): ");
    var precoInput = Console.ReadLine()?.Replace(",", ".");

    if (!string.IsNullOrWhiteSpace(precoInput))
    {
        while (true)
        {
            if (decimal.TryParse(precoInput, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal novoPreco) 
                && novoPreco >= 0.01m && novoPreco <= 9999.99m)
            {
                item.Price = novoPreco;
                break;
            }
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[ERRO] Preço deve estar entre 0.01 e 9999.99! Use ponto.");
            Console.ResetColor();
            Console.Write("Novo preço (deixe em branco para cancelar): ");
            precoInput = Console.ReadLine()?.Replace(",", ".");
            if (string.IsNullOrWhiteSpace(precoInput)) break;
        }
    }

    await db.SaveChangesAsync();
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("[SUCESSO] Item atualizado!");
    Console.ResetColor();
    
    Console.WriteLine("\nPressione qualquer tecla para continuar...");
    Console.ReadKey();
}

async Task RemoverItemAsync()
{
    Console.Clear();
    int id;
    while (true)
    {
        Console.Write("\nDigite o ID do item a remover: ");
        if (int.TryParse(Console.ReadLine(), out id))
            break;
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("[ERRO] ID invalido! Digite um numero inteiro.");
        Console.ResetColor();
    }

    using var db = new RPGContext(
        new DbContextOptionsBuilder<RPGContext>()
            .UseSqlite("Data Source=rpg.db")
            .Options);

    var item = await db.Items.FirstOrDefaultAsync(i => i.Id == id);

    if (item == null)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("[ERRO] Item nao encontrado.");
        Console.ResetColor();
        Console.WriteLine("\nPressione qualquer tecla para continuar...");
        Console.ReadKey();
        return;
    }

    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.Write($"[AVISO] Tem certeza que deseja remover '{item.Name}'? (s/n): ");
    Console.ResetColor();
    var confirmacao = Console.ReadLine()?.ToLower();

    if (confirmacao != "s")
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("[INFO] Operacao cancelada.");
        Console.ResetColor();
        Console.WriteLine("\nPressione qualquer tecla para continuar...");
        Console.ReadKey();
        return;
    }

    db.Items.Remove(item);
    await db.SaveChangesAsync();
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("[SUCESSO] Item removido!");
    Console.ResetColor();
    
    Console.WriteLine("\nPressione qualquer tecla para continuar...");
    Console.ReadKey();
}