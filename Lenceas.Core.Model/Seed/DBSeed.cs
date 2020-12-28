using Lenceas.Core.Common;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lenceas.Core.Model
{
    public class DBSeed
    {
        private static string SeedDataFolder = "/Lenceas.Core.Data.json/{0}.tsv";

        /// <summary>
        /// 异步添加种子数据 EF Core
        /// </summary>
        /// <param name="mySqlContext"></param>
        /// <param name="WebRootPath"></param>
        /// <returns></returns>
        public static async Task SeedAsyncByEFCore(MySqlContext mySqlContext, string WebRootPath)
        {
            try
            {
                if (string.IsNullOrEmpty(WebRootPath))
                {
                    throw new Exception("获取wwwroot路径时，异常！");
                }

                Console.WriteLine();
                Console.WriteLine($"WebRootPath:{WebRootPath}");
                Console.WriteLine();
                Console.WriteLine("************ 开始自动初始化数据 *****************");
                Console.WriteLine();

                if (AppSettings.app(new string[] { "AppSettings", "SeedDB" }).ObjToBool())
                {
                    Console.WriteLine("开始重置数据库...");
                    await mySqlContext.Database.EnsureDeletedAsync();
                    await mySqlContext.Database.EnsureCreatedAsync();
                    Console.WriteLine("数据库重置成功!");
                    Console.WriteLine();
                }

                Console.WriteLine("开始初始化数据...");
                Console.WriteLine();

                if (AppSettings.app(new string[] { "AppSettings", "SeedDBData" }).ObjToBool())
                {
                    JsonSerializerSettings setting = new JsonSerializerSettings();
                    JsonConvert.DefaultSettings = new Func<JsonSerializerSettings>(() =>
                    {
                        //日期类型默认格式化处理
                        setting.DateFormatHandling = DateFormatHandling.MicrosoftDateFormat;
                        setting.DateFormatString = "yyyy-MM-dd HH:mm:ss";

                        //空值处理
                        setting.NullValueHandling = NullValueHandling.Ignore;

                        //高级用法九中的Bool类型转换 设置
                        //setting.Converters.Add(new BoolConvert("是,否"));

                        return setting;
                    });

                    #region test
                    if (!await mySqlContext.test.AnyAsync())
                    {
                        await mySqlContext.test.AddRangeAsync(JsonHelper.ParseFormByJson<List<Test>>(FileHelper.ReadFile(string.Format(WebRootPath + SeedDataFolder, "test"), Encoding.UTF8)));
                        await mySqlContext.SaveChangesAsync();
                        Console.WriteLine("表 test 数据初始化成功!");
                    }
                    else
                    {
                        Console.WriteLine("表 test 已存在数据!");
                    }
                    #endregion

                    Console.WriteLine();
                    Console.WriteLine("数据初始化完成!");
                    Console.WriteLine();
                }

                Console.WriteLine("************ 自动初始化数据完成 *****************");
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                throw new Exception("错误信息：" + ex.Message);
            }
        }
    }
}
