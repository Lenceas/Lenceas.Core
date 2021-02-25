using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lenceas.Core.Model
{
    /// <summary>
    /// 测试类
    /// </summary>
    public class Test : BaseEntity
    {
        #region 构造函数
        public Test()
        {
            Name = string.Empty;
            CreateTime = DateTime.Now.ToLocalTime();
            UpdateTime = DateTime.Now.ToLocalTime();
        }

        public Test(string name)
        {
            Name = name;
            CreateTime = DateTime.Now.ToLocalTime();
            UpdateTime = DateTime.Now.ToLocalTime();
        }
        #endregion

        #region 属性
        /// <summary>
        /// 名称
        /// </summary>
        [Column(TypeName = "varchar(255)")]
        [Description("名称")]
        [Required]
        public string Name { get; set; }
        #endregion
    }
}
