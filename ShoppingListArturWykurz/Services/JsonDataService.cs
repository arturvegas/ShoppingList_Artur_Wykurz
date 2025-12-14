using ShoppingListArturWykurz.Models;
using System.Text.Json;

namespace ShoppingListArturWykurz.Services;

public class JsonDataService : IDataService
{
    private const string FileName = "shopping_list.json";

    private string GetFilePath()
    {
        return Path.Combine(FileSystem.AppDataDirectory, FileName);
    }

    public async Task<List<Category>> LoadDataAsync()
    {
        try
        {
            string filePath = GetFilePath();

            if (!File.Exists(filePath))
            {
                return CreateDefaultCategories();
            }

            string json = await File.ReadAllTextAsync(filePath);
            var categories = JsonSerializer.Deserialize<List<Category>>(json) ?? new();

            return categories.Count > 0 ? categories : CreateDefaultCategories();
        }
        catch
        {
            return CreateDefaultCategories();
        }
    }

    public async Task SaveDataAsync(List<Category> categories)
    {
        try
        {
            string filePath = GetFilePath();
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(categories, options);

            await File.WriteAllTextAsync(filePath, json);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error saving data: {ex.Message}");
        }
    }

    public async Task<string> ExportDataAsync(List<Category> categories)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        return await Task.FromResult(JsonSerializer.Serialize(categories, options));
    }

    public async Task<List<Category>> ImportDataAsync(string json)
    {
        try
        {
            var categories = JsonSerializer.Deserialize<List<Category>>(json) ?? new();
            return await Task.FromResult(categories);
        }
        catch
        {
            return new();
        }
    }

    private List<Category> CreateDefaultCategories()
    {
        return new()
        {
            new Category { Name = "Nabiał" },
            new Category { Name = "Warzywa" },
            new Category { Name = "Elektronika" },
            new Category { Name = "AGD" }
        };
    }
}