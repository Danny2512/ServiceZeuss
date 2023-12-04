using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceZeuss.Data;
using ServiceZeuss.Data.Entities;
using ServiceZeuss.Models;
using ServiceZeuss.Services.Zeuss;

namespace ServiceZeuss.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IZeussService _zeussService;

        public CategoryController(DataContext context, IZeussService zeussService)
        {
            _context = context;
            _zeussService = zeussService;
        }

        [HttpPost]
        public async Task<IActionResult> GetCategoriesExternalServiceZeuss([FromQuery] string? filter)
        {
            try
            {
                return Ok(new
                {
                    categories = await _zeussService.GetCategories(filter)
                });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Ha ocurrido un error, vuelva a intentarlo o contacte con su administrador" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetCategories([FromQuery] string? filter)
        {
            try
            {
                IQueryable<Category> query = _context.tblCategory.Include(c => c.Products).AsQueryable();

                if (!string.IsNullOrEmpty(filter))
                {
                    query = query.Where(c => c.StrName.Contains(filter));
                }

                var categoriesWithProducts = await query.ToListAsync();

                var response = categoriesWithProducts.Select(category => new
                {
                    Id = category.Id,
                    Name = category.StrName,
                    Active = category.BiActive,
                    Products = category.Products.Select(product => new
                    {
                        Id = product.Id,
                        Name = product.StrName,
                        Price = product.DePrice,
                        Image = product.StrImageUrl,
                        Active = product.BiActive
                    }).ToList()
                }).ToList();

                return Ok(new
                {
                    categories = response
                });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Ha ocurrido un error, vuelva a intentarlo o contacte con su administrador" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> SyncExternalServiceZeussWithLocal([FromQuery] Guid IdCategoryInternaToSync)
        {
            try
            {
                if (IdCategoryInternaToSync == null)
                {
                    return BadRequest(new { Message = "No es válido que IdCategoryInternaToSynchronize sea nulo." });
                }

                var categoryToSynchronize = await _context.tblCategory
                    .Include(c => c.Products)
                    .Where(c => c.Id == IdCategoryInternaToSync)
                    .FirstOrDefaultAsync();

                if (categoryToSynchronize != null)
                {
                    // Crear el modelo para sincronizar
                    SynchronizeExternalServiceZeussCategoryModel modelToSynchronize = new SynchronizeExternalServiceZeussCategoryModel
                    {
                        Name = categoryToSynchronize.StrName,
                        Active = categoryToSynchronize.BiActive,
                        Products = categoryToSynchronize.Products?.Select(p => new ProductModel
                        {
                            Name = p.StrName,
                            Price = p.DePrice,
                            ImageUrl = p.StrImageUrl,
                            Active = p.BiActive
                        }).ToList() ?? new List<ProductModel>()
                    };

                    // Utilizar el modelo para sincronizar la categoría
                    bool responseApi = await _zeussService.CreateCategory(modelToSynchronize);

                    if (responseApi)
                    {
                        return StatusCode(201);
                    }
                    else
                    {
                        return BadRequest(new { Message = @$"No fue posible sincronizar la categoría {categoryToSynchronize.StrName}." });
                    }
                }
                else
                {
                    return BadRequest(new { Message = "No se ha encontrado esta categoría en la base de datos." });
                }
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Ha ocurrido un error, vuelva a intentarlo o contacte con su administrador" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> SyncLocalWithExternalZeuss([FromQuery] string externalCategoryIdentifier)
        {
            try
            {
                if (string.IsNullOrEmpty(externalCategoryIdentifier))
                {
                    return BadRequest(new { Message = "El identificador de la categoría externa para sincronizar no es válido." });
                }

                var externalCategoryToSync = await _zeussService.GetCategories(externalCategoryIdentifier);

                if (externalCategoryToSync != null && externalCategoryToSync.Any())
                {
                    var syncCategory = new Category
                    {
                        StrName = externalCategoryToSync[0].Name,
                        BiActive = externalCategoryToSync[0].Active,
                        Products = externalCategoryToSync[0].Products?.Select(p => new Product
                        {
                            StrName = p.Name,
                            DePrice = p.Price,
                            StrImageUrl = p.Image,
                            BiActive = p.Active
                        }).ToList() ?? new List<Product>()
                    };

                    // Sincronizar la categoría local con el servicio externo de Zeuss
                    await _context.tblCategory.AddAsync(syncCategory);
                    await _context.SaveChangesAsync();

                    return StatusCode(201);
                }
                else
                {
                    return BadRequest(new { Message = "No se ha encontrado esta categoría en el servicio externo de Zeuss." });
                }
            }
            catch (Exception)
            {
                return StatusCode(500, new { Message = "Ha ocurrido un error, vuelva a intentarlo o contacte con su administrador" });
            }
        }
    }
}
