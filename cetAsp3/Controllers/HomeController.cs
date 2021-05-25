using cetAsp3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using cetAsp3.Data;
using Microsoft.EntityFrameworkCore;

namespace cetAsp3.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        // data yı kullanabilmek için db injection yaptık!!
        private readonly ApplicationDbContext _dbContext;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        //Task ve async ekliyoruzz!!
        public async Task<IActionResult> Index()
        {
            //todos itemların tamamlanmayalarını due date e göre sıralyıp ilk 3 ü aldık!!
            //select top (3) * from todoItems t inner join categories c on t.catedoryId=c.Id
            //where isCompleted=0 order by dueDate 
            var query= _dbContext.TodoItems.Include(t=>t.Category).
                Where(t => !t.isCompleted).
                OrderBy(t => t.DueDate).Take(3);

            List<TodoItem> result = await query.ToListAsync();
            //List<TodoItem> result2 = query.ToList();

            //fark yok async performans!!
            return View(result);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
