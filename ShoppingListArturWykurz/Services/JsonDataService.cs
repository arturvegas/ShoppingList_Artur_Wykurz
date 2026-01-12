using ShoppingListArturWykurz.Models;
using System.Text.Json;

namespace ShoppingListArturWykurz.Services;

public class JsonDataService
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