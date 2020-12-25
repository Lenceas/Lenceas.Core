using System;
using System.Collections.Generic;
using System.Text;

namespace Lenceas.Core.Model
{
    public class PageViewModels<T>
    {
        public IList<T> ViewModelList { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int TotalRecords { get; set; }

        public int TotalPages { get; set; }

        public bool HasPreviousPage { get; set; }

        public bool HasNextPage { get; set; }
    }

    public class PageViewModels<T1, T2> : PageViewModels<T1>
    {
        public T2 Data { get; set; }
    }
}
