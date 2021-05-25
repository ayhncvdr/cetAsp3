using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using cetAsp3.Data;
using cetAsp3.Models;

namespace cetAsp3.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Categories
        public async Task<IActionResult> Index(SearchViewModel searchModel)
        {
            /* var applicationDbContext2 = _context.TodoItems
                 .Where(c => showall || !t.isCompleted).OrderBy(t => t.DueDate);*/

           
            var query = _context.Categories.FromSqlRaw("select * from Categories").AsQueryable();

            //searchmodel eklendi!!
            

           // select * from TodoItems t inner join Categories c on t.CategoryId=c.Id
            
            if (!searchModel.ShowinDesc)
            {
                query = query.Where(c => c.Name.Contains(searchModel.SearchText) );// where c.Name like '%searchtext%'
            }

            if (!string.IsNullOrWhiteSpace(searchModel.SearchText))
            {
                query = query.Where(c => c.Name.Contains(searchModel.SearchText) || c.Description.Contains(searchModel.SearchText));//where c.Description like '%searchtext%'
            }


            // .Where(t=>!t.isCompleted).OrderBy(t=>t.DueDate);
            searchModel.CResult = await query.ToListAsync();
            return View(searchModel);
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description")] Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
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
            return View(category);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> MakeDescriptionShow(int id, bool showAll)
        {
            showAll = true;
            return await changeStatus(id,showAll);
        }
        public async Task<IActionResult> MakeInComplete(int id, bool showAll)
        {
            showAll = false;
            return await changeStatus(id, showAll);
        }
        private async Task<IActionResult> changeStatus(int id,  bool descriptionshow)
        {
            var categoryItem = _context.Categories.FirstOrDefault(c => c.Id == id);
            if (categoryItem == null)
            {
                return NotFound();
            }

            
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { showall = descriptionshow });
        }


        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }
    }
}
