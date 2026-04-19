using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ToDo_Web_App.Data;
using ToDo_Web_App.Models;

namespace ToDo_Web_App.Controllers
{

    [Authorize]
    public class AssignmentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<ApplicationUser> _userManager;

       

        public AssignmentsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        private string[] GetArrayOfCategories()
        {
            string[] categories = { "School", "Work", "Home", "Other" };
            return categories;
        }

        // GET: Assignments

        public async Task<IActionResult> Index(string? title, string? sortOrder, string? category, int? pageNumber)
        {
            ViewBag.Categories = GetArrayOfCategories();

            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            var assignmentsQuery = _context.Assignment
                .Where(a => a.User.UserName == user.UserName);

            switch (sortOrder)
            {
                case "date_desc":
                    assignmentsQuery = assignmentsQuery.OrderByDescending(a => a.DueDate);
                    break;
                case "date_asc":
                    assignmentsQuery = assignmentsQuery.OrderBy(a => a.DueDate);
                    break;
                case "type_asc":
                    assignmentsQuery = assignmentsQuery.OrderBy(a => a.Type);
                    break;
                case "type_desc":
                    assignmentsQuery = assignmentsQuery.OrderByDescending(a => a.Type);
                    break;
                case "name_asc":
                    assignmentsQuery = assignmentsQuery.OrderBy(a => a.Name);
                    break;
                case "name_desc":
                    assignmentsQuery = assignmentsQuery.OrderByDescending(a => a.Name);
                    break;
                default:
                    assignmentsQuery = assignmentsQuery.OrderBy(a => a.DueDate);
                    break;
            }

            if (!string.IsNullOrWhiteSpace(title))
            {
                assignmentsQuery = assignmentsQuery.Where(a => a.Name.ToLower().Contains(title.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(category))
            {
                assignmentsQuery = assignmentsQuery.Where(a => a.Type.ToLower().Contains(category.ToLower()));
            }

            ViewBag.CurrentTitle = title;
            ViewBag.CurrentSort = sortOrder;
            ViewBag.CurrentCategory = category;

            ViewBag.TotalAssignments = await assignmentsQuery.CountAsync();

            int pageSize = 3;

            var paginatedList = await PaginatedList<Assignment>.CreateAsync(
                assignmentsQuery.AsNoTracking(),
                pageNumber ?? 1,
                pageSize);

            return View(paginatedList);
        }









        // GET: Assignments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Assignment == null)
            {
                return NotFound();
            }

            var assignment = await _context.Assignment
                .Include(a => a.User).Where(u => u.User.UserName == User.Identity.Name)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (assignment == null)
            {
                return NotFound();
            }

            return View(assignment);
        }

        // GET: Assignments/Create
        public IActionResult Create()
        {
            var categories = GetArrayOfCategories();
            ViewBag.Categories = categories;

            return View();
        }

        // POST: Assignments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Assignment assignment)
        {
          
            var user = _userManager.FindByNameAsync(User.Identity.Name).Result;
            assignment.UserId = user.Id;

            if (ModelState.IsValid)
            {
               
                _context.Add(assignment);
                await _context.SaveChangesAsync();
                TempData["Message"] = "An assignment was added successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(assignment);
        }

        // GET: Assignments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Assignment == null)
            {
                return NotFound();
            }

            var categories = GetArrayOfCategories();
            ViewBag.Categories = categories;
            var assignment = await _context.Assignment.FindAsync(id);
            if (assignment == null)
            {
                return NotFound();
            }
           
            return View(assignment);
        }

        // POST: Assignments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Assignment assignment)
        {
            if (id != assignment.Id)
            {
                return NotFound();
            }
            var user = _userManager.FindByNameAsync(User.Identity.Name).Result;

            if (ModelState.IsValid)
            {
                try
                {
                    assignment.UserId = user.Id;
                    _context.Update(assignment);


                    await _context.SaveChangesAsync();
                    TempData["Message"] = "An assignment was updated successfully";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AssignmentExists(assignment.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
           
            return View(assignment);
        }

        // GET: Assignments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Assignment == null)
            {
                return NotFound();
            }

            var assignment = await _context.Assignment
                .Include(a => a.User).Where(u => u.User.UserName == User.Identity.Name)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (assignment == null)
            {
                return NotFound();
            }

            return View(assignment);
        }

        // POST: Assignments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Assignment == null)
            {
                return Problem("Connection problem with the database!");
            }

            var assignment = await _context.Assignment.FindAsync(id);
            if (assignment == null)
            {
                return NotFound(); // Assignment not found
            }

            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null)
            {
                return NotFound(); // User not found
            }
            if (assignment.User.UserName != user.UserName)
            {
                return Forbid(); // User doesn't have permission
            }
            
                _context.Assignment.Remove(assignment);
                await _context.SaveChangesAsync();
            TempData["Message"] = "An assignment was deleted successfully";
                return RedirectToAction(nameof(Index));
            






         
        }


        private bool AssignmentExists(int id)
        {
          return (_context.Assignment?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
