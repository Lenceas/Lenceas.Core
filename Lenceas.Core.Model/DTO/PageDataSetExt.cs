using System;
using System.Collections.Generic;
using System.Text;

namespace Lenceas.Core.Model
{
    public static class PageDataSetExt
    {
        public static PageViewModels<T> AsPageViewModel<T>(this IList<T> viewModelList, int pageIndex, int pageSize)
        {
            return new PageViewModels<T>()
            {
                ViewModelList = viewModelList,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalRecords = viewModelList.Count,
                TotalPages = (int)Math.Ceiling(viewModelList.Count / (double)pageSize),
                HasPreviousPage = pageIndex > 1,
                HasNextPage = pageIndex < (int)Math.Ceiling(viewModelList.Count / (double)pageSize)
            };
        }

        public static PageViewModels<T> AsPageViewModel<T, N>(this PageDataSet<N> pageEntity, Func<IList<N>, IList<T>> func)
        {
            return new PageViewModels<T>()
            {
                ViewModelList = func(pageEntity),
                PageIndex = pageEntity.PageIndex,
                PageSize = pageEntity.PageSize,
                TotalRecords = (int)pageEntity.TotalRecords,
                TotalPages = pageEntity.TotalPages,
                HasPreviousPage = pageEntity.HasPreviousPage,
                HasNextPage = pageEntity.HasNextPage
            };
        }
    }
}
