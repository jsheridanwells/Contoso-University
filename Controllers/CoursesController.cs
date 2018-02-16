using Contoso.Data;
using Microsoft.AspNetCore.Mvc;

namespace Contoso.Controllers
{
    public class CoursesController : Controller
    {
        private readonly SchoolContext _context;
        public CoursesController(SchoolContext context)
        {
            _context = context;
        }
        
    }
}