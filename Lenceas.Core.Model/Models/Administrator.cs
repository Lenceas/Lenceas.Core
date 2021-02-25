using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lenceas.Core.Model
{
    /// <summary>
    /// 管理员类
    /// </summary>
    public class Administrator : BaseEntity
    {
        #region 构造函数
        public Administrator()
        {
            CreateTime = DateTime.Now.ToLocalTime();
            UpdateTime = DateTime.Now.ToLocalTime();
        }

        public Administrator(string account, string md5pwd)
        {
            Account = account;
            Password = md5pwd;
            Email = string.Empty;
            CreateTime = DateTime.Now.ToLocalTime();
            UpdateTime = DateTime.Now.ToLocalTime();
        }
        #endregion

        /// <summary>
        /// 账号
        /// </summary>
        [Column(TypeName = "varchar(30)")]
        [Description("账号")]
        [Required]
        public string Account { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Column(TypeName = "varchar(100)")]
        [Description("密码")]
        [Required]
        public string Password { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [Column(TypeName = "varchar(50)")]
        [Description("邮箱")]
        public string Email { get; set; }
    }
}
