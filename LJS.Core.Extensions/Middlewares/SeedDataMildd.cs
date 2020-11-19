using Lenceas.Core.Common;
using Lenceas.Core.Model;
using Microsoft.AspNetCore.Builder;
using System;


namespace Lenceas.Core.Extensions
{
    /// <summary>
    /// 生成种子数据中间件服务
    /// </summary>
    public static class SeedDataMildd
    {
        public static void UseSeedDataMildd(this IApplicationBuilder app, MySqlContext mySqlContext, string webRootPath)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));

            try
            {
                if (AppSettings.app("AppSettings", "SeedDB").ObjToBool() || AppSettings.app("AppSettings", "SeedDBData").ObjToBool())
                {
                    DBSeed.SeedAsyncByEFCore(mySqlContext, webRootPath).Wait();
                }
            }
            catch (Exception e)
            {
                throw new Exception($"自动初始化数据错误,错误信息:\n{e.Message}");
            }
        }
    }
}
