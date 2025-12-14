using ShoppingListArturWykurz.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShoppingListArturWykurz.Services;

public interface IDataService
{
    Task<List<Category>> LoadDataAsync();
    Task SaveDataAsync(List<Category> categories);
    Task<string> ExportDataAsync(List<Category> categories);
    Task<List<Category>> ImportDataAsync(string json);
}