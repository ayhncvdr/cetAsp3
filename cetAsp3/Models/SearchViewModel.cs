using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cetAsp3.Models
{
    public class SearchViewModel
    {
        public string SearchText { get; set; }
        public bool ShowAll { get; set; }
        public bool ShowinDesc { get; set; }    

        public List<TodoItem> Result { get; set; }
        public List<Category> CResult { get; set; }
    }
}
