using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Contoso.Data;
using Contoso.Models;

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

      // GET: Students/Details/5
      public async Task<IActionResult> Details(int? id)
      {
        if (id == null) return NotFound();

        var student = await _context.Students
          .Include(s => s.Enrollments)
            .ThenInclude(e => e.Course)
          .AsNoTracking()
          .SingleOrDefaultAsync(m => m.Id == id);
        
        if (student == null) return NotFound();

        return View(student);
      }
  }
}