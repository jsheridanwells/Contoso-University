using Contoso.Data;
using Microsoft.AspNetCore.Mvc;

namespace Contoso.Controllers
{
    public class StudentsController : Controller
    {
        private readonly SchoolContext _context;
        public StudentsController(SchoolContext context)
        {
            _context = context;
            
        }
    }
}