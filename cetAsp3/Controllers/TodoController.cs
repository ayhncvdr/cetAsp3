﻿using System;
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
    public class TodoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TodoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Todo
        public async Task<IActionResult> Index(SearchViewModel searchModel)
        {
           /* var applicationDbContext2 = _context.TodoItems.Include(t => t.Category)
                .Where(t => showall || !t.isCompleted).OrderBy(t => t.DueDate);*/

           //searchmodel eklendi!!
          
            var query = _context.TodoItems.Include(t => t.Category).AsQueryable();// select * from TodoItems t inner join Categories c on t.CategoryId=c.Id
            if (!searchModel.ShowAll)
            {
                query = query.Where(t => !t.isCompleted);// where t.isCompleted=0
            }

            if (!string.IsNullOrWhiteSpace(searchModel.SearchText))
            {
                query = query.Where(t => t.Title.Contains(searchModel.SearchText));//where t.Title like '%searchtext%'
            }

            query = query.OrderBy(t => t.DueDate);
               // .Where(t=>!t.isCompleted).OrderBy(t=>t.DueDate);
               searchModel.Result = await query.ToListAsync();
            return View(searchModel);
        }
        public async Task<IActionResult> Index2(SearchViewModel searchModel)
        {
            /* var applicationDbContext2 = _context.TodoItems.Include(t => t.Category)
                 .Where(t => showall || !t.isCompleted).OrderBy(t => t.DueDate);*/

            var query = _context.TodoItems.Include(t => t.Category).Where(t => searchModel.ShowAll || !t.isCompleted && t.CategoryId==1).AsQueryable();

            if (!searchModel.ShowAll)
            {
                query = query.Where(t => !t.isCompleted && t.CategoryId==1);// where t.isCompleted=0
            }

            if (!string.IsNullOrWhiteSpace(searchModel.SearchText))
            {
                query = query.Where(t => t.Title.Contains(searchModel.SearchText));//where t.Title like '%searchtext%'
            }

            query = query.OrderBy(t => t.DueDate);

            searchModel.Result = await query.ToListAsync();

            return View(searchModel);
        }
        public async Task<IActionResult> Index3(SearchViewModel searchModel)
        {
            /* var applicationDbContext2 = _context.TodoItems.Include(t => t.Category)
                 .Where(t => showall || !t.isCompleted).OrderBy(t => t.DueDate);*/

            var query = _context.TodoItems.Include(t => t.Category).Where(t => searchModel.ShowAll || !t.isCompleted && t.CategoryId == 2).AsQueryable();

            searchModel.Result = await query.ToListAsync();

            return View(searchModel);
        }
        public async Task<IActionResult> Index4(SearchViewModel searchModel)
        {
            /* var applicationDbContext2 = _context.TodoItems.Include(t => t.Category)
                 .Where(t => showall || !t.isCompleted).OrderBy(t => t.DueDate);*/

            var query = _context.TodoItems.Include(t => t.Category).Where(t => searchModel.ShowAll || !t.isCompleted && t.CategoryId == 8).AsQueryable();

            searchModel.Result = await query.ToListAsync();

            return View(searchModel);
        }

        // GET: Todo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todoItem = await _context.TodoItems
                .Include(t => t.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (todoItem == null)
            {
                return NotFound();
            }

            return View(todoItem);
        }

        // GET: Todo/Create
        public IActionResult Create()
        {
            ViewBag.CategorySelectList = new SelectList(_context.Categories, "Id", "Name");// datatextfield'ı id yerine name yaptık böylece category adları gözüktü
            return View();
        }

        // POST: Todo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,isCompleted,DueDate,CategoryId")] TodoItem todoItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(todoItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", todoItem.CategoryId);
            return View(todoItem);
        }

        // GET: Todo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", todoItem.CategoryId);
            return View(todoItem);
        }

        // POST: Todo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,isCompleted,DueDate,CategoryId")] TodoItem todoItem)
        {
            if (id != todoItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(todoItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TodoItemExists(todoItem.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", todoItem.CategoryId);
            return View(todoItem);
        }

        // GET: Todo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todoItem = await _context.TodoItems
                .Include(t => t.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (todoItem == null)
            {
                return NotFound();
            }

            return View(todoItem);
        }

        // POST: Todo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);
            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> MakeComplete(int id, bool showAll)
        {
            return await changeStatus(id,true,showAll);
        }
        public async Task<IActionResult> MakeInComplete(int id, bool showAll)
        {
            return await changeStatus(id, false,showAll);
        }

        private async Task<IActionResult> changeStatus(int id,bool status,bool currentshowall)
        {
            var todoItem = _context.TodoItems.FirstOrDefault(t => t.Id == id);
            if (todoItem == null)
            {
                return NotFound();
            }

            todoItem.isCompleted = status;
            todoItem.CompletedDate = DateTime.Now;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new {showall= currentshowall});
        }
        
        private bool TodoItemExists(int id)
        {
            return _context.TodoItems.Any(e => e.Id == id);
        }
    }
}
