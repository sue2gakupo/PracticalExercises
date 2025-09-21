using BondleApplication.Access.Data;
using Microsoft.EntityFrameworkCore;

public class IdGeneratorService
{
    private readonly BondleDBContext2 _context;

    public IdGeneratorService(BondleDBContext2 context)
    {
        _context = context;
    }

    public async Task<string> GenerateIdAsync(string entityID, string prefix)
    {
        string lastId = null;

        switch (entityID)
        {
            case "Product":

                lastId = await _context.Product
                    .Where(p => p.ProductID.StartsWith(prefix))
                    .OrderByDescending(p => p.ProductID)
                    .Select(p => p.ProductID)
                    .FirstOrDefaultAsync();

                break;

            case "ProductSeries":

                lastId = await _context.ProductSeries
                    .Where(s => s.SeriesID.StartsWith(prefix))
                    .OrderByDescending(s => s.SeriesID)
                    .Select(s => s.SeriesID)
                    .FirstOrDefaultAsync();

                break;

            case "ProductVariations":
                lastId = await _context.ProductVariations
                    .Where(v => v.VariationID.StartsWith(prefix))
                    .OrderByDescending(v => v.VariationID)
                    .Select(v => v.VariationID)
                    .FirstOrDefaultAsync();

                break;

            case "ProductImages":

                lastId = await _context.ProductImages
                    .Where(i => i.ImageID.StartsWith(prefix))
                    .OrderByDescending(i => i.ImageID)
                    .Select(i => i.ImageID)
                    .FirstOrDefaultAsync();

                break;



            default:
                throw new ArgumentException($"Unsupported entity: {entityID}");
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


