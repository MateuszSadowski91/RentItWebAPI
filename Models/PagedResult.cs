using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentItAPI.Models
{
    public class PagedResult<T>
    {
        public PagedResult(List<T> elements, int totalCount, int pageSize, int pageNumber)
        {
            Elements = elements;
            TotalElementsCount = totalCount;
            ElementsFrom = pageSize * (pageNumber - 1) + 1;
            ElementsTo = ElementsFrom + pageSize - 1;
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        }
        public List<T> Elements { get; set; }
        public int TotalPages { get; set; }
        public int ElementsFrom { get; set; }
        public int ElementsTo { get; set; }
        public int TotalElementsCount { get; set; }
    }
}
