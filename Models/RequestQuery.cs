using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentItAPI.Models
{
    public class RequestQuery
    {  
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchPhrase { get; set; }
        public string SortBy { get; set; }
        public SortDirection SortDirection { get; set; }
    }
}

