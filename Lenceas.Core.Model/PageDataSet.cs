using System;
using System.Collections.Generic;
using System.Linq;

namespace Lenceas.Core.Model
{
    /// <summary>
    /// 通用分页数据类（默认每页10条记录）
    /// </summary>
    /// <typeparam name="T">分页数据的实体类型</typeparam>
    public class PageDataSet<T> : List<T>
    {
        #region 构造函数
        public PageDataSet(IList<T> entities, long totalRecords, int pageIndex, int pageSize)
            : base(entities)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalRecords = totalRecords;
            TotalPages = (int)Math.Ceiling(TotalRecords / (double)PageSize);
            ViewModelList = entities;
            HasPreviousPage = PageIndex > 1;
            HasNextPage = PageIndex < TotalPages;
        }

        public PageDataSet(IEnumerable<T> entities) : base((IList<T>)entities.ToList())
        {
        }

        public PageDataSet(IList<T> entities) : base(entities)
        {
        }

        public PageDataSet() : base()
        {
        }
        #endregion

        #region 属性
        private int _pageIndex = 1;
        private int _pageSize = 10;
        private long _totalRecords;

        /// <summary>
        /// 当前页数(从1计数）
        /// </summary>
        public int PageIndex
        {
            get
            {
                return _pageIndex;
            }
            set
            {
                _pageIndex = value;
            }
        }

        /// <summary>
        /// 分页大小
        /// </summary>
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = value;
            }
        }

        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// 总记录条数
        /// </summary>
        public long TotalRecords
        {
            get
            {
                return _totalRecords;
            }
            set
            {
                _totalRecords = value;
            }
        }

        /// <summary>
        /// 是否有上一页
        /// </summary>
        public bool HasPreviousPage { get; set; }

        /// <summary>
        /// 是否有下一页
        /// </summary>
        public bool HasNextPage { get; set; }

        /// <summary>
        /// 返回数据
        /// </summary>
        public IList<T> ViewModelList { get; set; }
        #endregion
    }
}
