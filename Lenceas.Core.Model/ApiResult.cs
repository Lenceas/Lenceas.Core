using System;
using System.Collections.Generic;
using System.Text;

namespace Lenceas.Core.Model
{
    /// <summary>
    /// Api通用返回信息类
    /// </summary>
    public class ApiResult<T>
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public int status { get; set; } = 200;

        /// <summary>
        /// 返回信息
        /// </summary>
        public string msg { get; set; } = "服务器异常";

        /// <summary>
        /// 返回数据集合
        /// </summary>
        public T data { get; set; }
    }
}
