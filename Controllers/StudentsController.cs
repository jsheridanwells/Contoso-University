using System.Threading.Tasks;
using Contoso.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Contoso.Controllers
{
    public class StudentsController : Controller
    {
        private readonly SchoolContext _context;
    public StudentsController(SchoolContext context) => _context = context;

    // GET: Students
    public async Task<IActionResult> Index()
    {
      return View(await _context.Students.ToListAsync());
    }

  }
}