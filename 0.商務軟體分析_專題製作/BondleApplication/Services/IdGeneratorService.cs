using BondleApplication.Access.Data;
using Microsoft.EntityFrameworkCore;

public class IdGeneratorService
{
    private readonly BondleDBContext _context;

    public IdGeneratorService(BondleDBContext context)
    {
        _context = context;
    }

    public async Task<string> GenerateSeriesIdAsync()
    {
        string prefix = "SE";
        string? lastId = await _context.ProductSeries
            .OrderByDescending(s => s.SeriesID)
            .Select(s => s.SeriesID)
            .FirstOrDefaultAsync();

        return GenerateNextId(prefix, lastId);
    }

    public async Task<string> GenerateProductIdAsync(bool isDigital)
    {
        string prefix = isDigital ? "DI" : "PH";
        string? lastId = await _context.Product
            .Where(p => p.ProductID.StartsWith(prefix))
            .OrderByDescending(p => p.ProductID)
            .Select(p => p.ProductID)
            .FirstOrDefaultAsync();

        return GenerateNextId(prefix, lastId);
    }

    public async Task<string> GenerateVariationIdAsync()
    {
        string prefix = "PV";
        string? lastId = await _context.ProductVariations
            .OrderByDescending(v => v.VariationID)
            .Select(v => v.VariationID)
            .FirstOrDefaultAsync();

        return GenerateNextId(prefix, lastId);
    }

    public async Task<string> GenerateImageIdAsync()
    {
        string prefix = "IM";
        string? lastId = await _context.ProductImages
            .OrderByDescending(i => i.ImageID)
            .Select(i => i.ImageID)
            .FirstOrDefaultAsync();

        return GenerateNextId(prefix, lastId);
    }

    private string GenerateNextId(string prefix, string? lastId)
    {
        int nextNumber = 1;

        if (!string.IsNullOrEmpty(lastId) && lastId.Length >= prefix.Length + 6)
        {
            string numberPart = lastId.Substring(prefix.Length);
            if (int.TryParse(numberPart, out int parsed))
            {
                nextNumber = parsed + 1;
            }
        }

        return $"{prefix}{nextNumber:D6}";
    }
}
