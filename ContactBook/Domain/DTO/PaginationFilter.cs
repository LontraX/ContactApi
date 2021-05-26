using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactBook.Domain.DTO
{
    public class PaginationFilter
    {
       // public int PerPage { get; set; }
        public int CurrentPage { get; set; }
        public PaginationFilter()
        {
           // this.PerPage = 3;
            this.CurrentPage = 1;
        }

        public PaginationFilter( int _CurrentPage)
        {
           // this.PerPage = 5;
            this.CurrentPage = _CurrentPage <= 0 ? 1 : _CurrentPage;
        }

    }
}
