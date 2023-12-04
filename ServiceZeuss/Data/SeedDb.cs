using Microsoft.Extensions.Configuration.UserSecrets;
using ServiceZeuss.Data.Entities;

namespace ServiceZeuss.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;

        public SeedDb(DataContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckCategoriesAsync();
            await CheckProductsAsync();
        }
        private async Task CheckCategoriesAsync()
        {
            if (!_context.tblCategory.Any())
            {
                await _context.tblCategory.AddRangeAsync(
                    new Category { Id = Guid.NewGuid(), StrName = "Neumáticos", BiActive = true },
                    new Category { Id = Guid.NewGuid(), StrName = "Filtros", BiActive = true }
                );

                await _context.SaveChangesAsync();
            }
        }

        private async Task CheckProductsAsync()
        {
            if (!_context.tblProduct.Any())
            {
                var categoryId1 = _context.tblCategory.First(c => c.StrName == "Neumáticos").Id;
                var categoryId2 = _context.tblCategory.First(c => c.StrName == "Filtros").Id;

                await _context.tblProduct.AddRangeAsync(
                    new Product { Id = Guid.NewGuid(), CategoryFK = categoryId1, StrName = "Neumático para todo terreno", StrImageUrl = "https://example.com/tire1.png", DePrice = 12000, BiActive = true },
                    new Product { Id = Guid.NewGuid(), CategoryFK = categoryId1, StrName = "Neumático de verano ABC", StrImageUrl = "https://example.com/tire2.png", DePrice = 8000, BiActive = true },
                    new Product { Id = Guid.NewGuid(), CategoryFK = categoryId1, StrName = "Neumático de invierno XYZ", StrImageUrl = "https://example.com/tire3.png", DePrice = 10000, BiActive = true },
                    new Product { Id = Guid.NewGuid(), CategoryFK = categoryId2, StrName = "Filtro de aceite XYZ", StrImageUrl = "https://example.com/filter1.png", DePrice = 1500, BiActive = true },
                    new Product { Id = Guid.NewGuid(), CategoryFK = categoryId2, StrName = "Filtro de aire ABC", StrImageUrl = "https://example.com/filter2.png", DePrice = 2000, BiActive = true },
                    new Product { Id = Guid.NewGuid(), CategoryFK = categoryId2, StrName = "Filtro de combustible XYZ", StrImageUrl = "https://example.com/filter3.png", DePrice = 1800, BiActive = true }
                );

                await _context.SaveChangesAsync();
            }
        }
    }
}
