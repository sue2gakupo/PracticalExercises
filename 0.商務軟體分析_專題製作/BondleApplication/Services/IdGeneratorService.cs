using BondleApplication.Access.Data;
using Microsoft.EntityFrameworkCore;

public class IdGeneratorService
{
    private readonly BondleDBContext _context;

    public IdGeneratorService(BondleDBContext context)
    {
        _context = context;
    }

    public async Task<string> GenerateIdAsync(string entityName, string prefix)
    {
        string lastId = null;

        switch (entityName)
        {
            case "Product":
#pragma warning disable CS8600 // 正在將 Null 常值或可能的 Null 值轉換為不可為 Null 的型別。
                lastId = await _context.Product
                    .Where(p => p.ProductID.StartsWith(prefix))
                    .OrderByDescending(p => p.ProductID)
                    .Select(p => p.ProductID)
                    .FirstOrDefaultAsync();
#pragma warning restore CS8600 // 正在將 Null 常值或可能的 Null 值轉換為不可為 Null 的型別。
                break;

            case "ProductSeries":
#pragma warning disable CS8600 // 正在將 Null 常值或可能的 Null 值轉換為不可為 Null 的型別。
                lastId = await _context.ProductSeries
                    .Where(s => s.SeriesID.StartsWith(prefix))
                    .OrderByDescending(s => s.SeriesID)
                    .Select(s => s.SeriesID)
                    .FirstOrDefaultAsync();
#pragma warning restore CS8600 // 正在將 Null 常值或可能的 Null 值轉換為不可為 Null 的型別。
                break;

            case "ProductVariations":
#pragma warning disable CS8600 // 正在將 Null 常值或可能的 Null 值轉換為不可為 Null 的型別。
                lastId = await _context.ProductVariations
                    .Where(v => v.VariationID.StartsWith(prefix))
                    .OrderByDescending(v => v.VariationID)
                    .Select(v => v.VariationID)
                    .FirstOrDefaultAsync();
#pragma warning restore CS8600 // 正在將 Null 常值或可能的 Null 值轉換為不可為 Null 的型別。
                break;

            case "ProductImages":
#pragma warning disable CS8600 // 正在將 Null 常值或可能的 Null 值轉換為不可為 Null 的型別。
                lastId = await _context.ProductImages
                    .Where(i => i.ImageID.StartsWith(prefix))
                    .OrderByDescending(i => i.ImageID)
                    .Select(i => i.ImageID)
                    .FirstOrDefaultAsync();
#pragma warning restore CS8600 // 正在將 Null 常值或可能的 Null 值轉換為不可為 Null 的型別。
                break;



            default:
                throw new ArgumentException($"Unsupported entity: {entityName}");
        }

        int newNumber = 1;
        if (!string.IsNullOrEmpty(lastId))
        {
            if (int.TryParse(lastId.Substring(prefix.Length), out int lastNumber))
            {
                newNumber = lastNumber + 1;
            }
        }

        return $"{prefix}{newNumber:D6}";
    }
}


