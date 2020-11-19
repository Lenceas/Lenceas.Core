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
        /// <summary>
        /// 名称
        /// </summary>
        [Column(TypeName = "varchar(255)")]
        [Required]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 创建时间
        /// </summary>
        [Column(TypeName = "datetime")]
        [Required]
        public DateTime CreateTime { get; set; }
    }
}
