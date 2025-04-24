using OnlineStore.Models;
using OnlineStore.Models.Dto;

public class DataAccessor
{
    private readonly AppDbContext _context;

    public DataAccessor(AppDbContext context)
    {
        _context = context;
    }

    public List<CategoryDto> CategoriesList()
    {
        return _context.Categories
            .Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                ImageUrl = "/images/categories/" + c.ImageUrl
            })
            .ToList();
    }
}
