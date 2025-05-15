using Microsoft.AspNetCore.Mvc;

namespace TransferGo.Notifications.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotificationsController : ControllerBase
{
    private readonly AppDbContext _context;

    public NotificationsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Product>> GetAll() => _context.Products.ToList();

    [HttpPost]
    public IActionResult Add(Product product)
    {
        _context.Products.Add(product);
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetAll), product);
    }
}