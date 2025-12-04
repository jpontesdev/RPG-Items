using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using Microsoft.EntityFrameworkCore;
using RPG_Items.Core.Data;
using RPG_Items.Core.Models;

namespace RPG_Items.WPF;

public partial class MainWindow : Window
{
    private ObservableCollection<Item> _items = new ObservableCollection<Item>();
    
    public MainWindow()
    {
        InitializeComponent();
        LoadItemsFromDatabase();
        dgItems.ItemsSource = _items;
    }
    
    public void LoadItemsFromDatabase()
    {
        try
        {
            using (var context = CreateContext())
            {
                context.Database.EnsureCreated();
                var itemsFromDb = context.Items.OrderBy(i => i.Id).ToList();
                _items = new ObservableCollection<Item>(itemsFromDb);
                dgItems.ItemsSource = _items;
            }
        }
        catch (Exception ex)
        {
            ShowError($"Erro ao carregar itens: {ex.Message}");
        }
    }
    
    private void Adicionar_Click(object sender, RoutedEventArgs e)
    {
        // Validar nome
        if (string.IsNullOrWhiteSpace(txtName.Text))
        {
            ShowWarning("Por favor, digite o nome do item!");
            txtName.Focus();
            return;
        }
        
        // Validar raridade
        if (!int.TryParse(txtRarity.Text, out int rarity) || rarity < 1 || rarity > 5)
        {
            ShowWarning("Raridade deve ser um número entre 1 e 5!");
            txtRarity.Focus();
            return;
        }
        
        // Validar preço
        if (!decimal.TryParse(txtPrice.Text, out decimal price) || price < 0)
        {
            ShowWarning("Digite um preço válido!");
            txtPrice.Focus();
            return;
        }
        
        // Adicionar à coleção
        var item = new Item
        {
            Name = txtName.Text.Trim(),
            Rarity = rarity,
            Price = price
        };
        
        _items.Add(item);
        
        // Limpar campos
        txtName.Clear();
        txtRarity.Clear();
        txtPrice.Clear();
        txtName.Focus();
        
        // Feedback visual
        ShowSuccess($"Item '{item.Name}' adicionado! Clique em 'Salvar no Banco' para confirmar.");
    }
    
    private void Remover_Click(object sender, RoutedEventArgs e)
    {
        if (dgItems.SelectedItem is not Item itemSelected)
        {
            ShowWarning("Selecione um item na lista para remover!");
            return;
        }
        
        var result = MessageBox.Show(
            $"Deseja realmente remover o item:\n\n'{itemSelected.Name}' (Raridade: {itemSelected.Rarity}⭐)?",
            "Confirmar Exclusão",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);
        
        if (result == MessageBoxResult.Yes)
        {
            try
            {
                // Se já está no banco, remove de lá também
                if (itemSelected.Id > 0)
                {
                    using (var context = CreateContext())
                    {
                        context.Items.Remove(itemSelected);
                        context.SaveChanges();
                    }
                }
                
                // Remove da coleção
                _items.Remove(itemSelected);
                ShowSuccess($"Item '{itemSelected.Name}' removido com sucesso!");
            }
            catch (Exception ex)
            {
                ShowError($"Erro ao remover item: {ex.Message}");
            }
        }
    }
    
    private void SalvarBanco_Click(object sender, RoutedEventArgs e)
    {
        if (_items.Count == 0)
        {
            ShowWarning("Não há itens para salvar!");
            return;
        }
        
        try
        {
            using (var context = CreateContext())
            {
                context.Database.EnsureCreated();
                
                int novos = 0;
                int atualizados = 0;
                
                foreach (var item in _items)
                {
                    if (item.Id == 0)
                    {
                        context.Items.Add(item);
                        novos++;
                    }
                    else
                    {
                        context.Items.Update(item);
                        atualizados++;
                    }
                }
                
                context.SaveChanges();
                
                // Mensagem detalhada
                string mensagem = "Dados salvos com sucesso!\n\n";
                if (novos > 0) mensagem += $" {novos} item(ns) novo(s)\n";
                if (atualizados > 0) mensagem += $" {atualizados} item(ns) atualizado(s)";
                
                MessageBox.Show(mensagem, "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                
                // Recarregar para pegar IDs gerados
                LoadItemsFromDatabase();
            }
        }
        catch (Exception ex)
        {
            ShowError($"Erro ao salvar no banco: {ex.Message}");
        }
    }
    
    // Helpers para mensagens
    private void ShowSuccess(string message)
    {
        MessageBox.Show(message, "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
    }
    
    private void ShowWarning(string message)
    {
        MessageBox.Show(message, "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
    }
    
    private void ShowError(string message)
    {
        MessageBox.Show(message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
    }
    
    private RPGContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<RPGContext>()
            .UseSqlite("Data Source=rpg.db")
            .Options;
        
        return new RPGContext(options);
    }
}
