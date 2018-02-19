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
      // creates data object model
      private readonly SchoolContext _context;
      public StudentsController(SchoolContext context) => _context = context;

      // GET: Students
      // accesses Students/index.html
      public async Task<IActionResult> Index(string sortOrder)
      {
        ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
        ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
        var students = from s in _context.Students
                        select s;
        
        switch (sortOrder)
        {
          case "name_desc":
            students = students.OrderByDescending(s => s.LastName);
            break;
          case "Date":
            students = students.OrderBy(s => s.EnrollmentDate);
            break;
          case "date_desc":
            students = students.OrderByDescending(s => s.EnrollmentDate);
            break;
          default:
            students = students.OrderBy(s => s.LastName);
            break;            
        }
        return View(await students.AsNoTracking().ToListAsync());
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

      // GET: Students/Create
        public IActionResult Create()
        {
          return View();
        }

      // POST: Student/Create
      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Create(
        [Bind("EnrollementDate,FirstMidName,LastName")] Student student
      )
      {
        try
        {
          if (ModelState.IsValid)
          {
            _context.Add(student);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
          }
        }
        catch (DbUpdateException /* ex */)
        {
          ModelState.AddModelError("", "No pudo crear nuevo alumno. Trate de nuevo.");
        }
        return View(student);
      }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var student = await _context.Students.SingleOrDefaultAsync(m => m.Id == id);

            if (student == null) return NotFound();
            
            return View(student);
        }

        // POST: Students/Edit/5
        // #Edit method is a post because HTML forms can only GET and POST
        // in a Webapi app, this would be PUT/PATCH
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
          if (id == null) return NotFound();

          // #SingleOrDefault will return an exception if more than one entry is returned matching id
          var studentToUpdate = await _context.Students.SingleOrDefaultAsync(s => s.Id == id);

          // #TryUpdateModelAsync will only update values provides
          // in the lambda, not the entire model.
          if (await TryUpdateModelAsync<Student>(
            studentToUpdate,
            "",
            s => s.FirstMidName, 
            s => s.LastName,
            s => s.EnrollmentDate
          ))
          {
            try
            {
              await _context.SaveChangesAsync();
              return RedirectToAction(nameof(Index));
            }
            catch
            {
              ModelState.AddModelError("", "No pudo actualizar nuevo alumno. Trate de nuevo.");
            }
          }

          return View(studentToUpdate);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .AsNoTracking() // #AsNoTracking is for read-only queries, pulls result of query w/o storing state, more performant
                .SingleOrDefaultAsync(m => m.Id == id);  // the actual #Delete will be done in a separate call
            if (student == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "No pudo apagar el almuno. Trate de nuevo";
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
          var student = await _context.Students
            .AsNoTracking()
            .SingleOrDefaultAsync(m => m.Id == id);
          
          if (student == null) RedirectToAction(nameof(Index));

          try
          {
            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
          }
          catch (DbUpdateException /* ex */)
          {
            return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
          }
        }
  }
}
