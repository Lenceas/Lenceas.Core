using Lenceas.Core.Common;
using Lenceas.Core.Extensions;
using Lenceas.Core.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Lenceas.Core
{
    public class Startup
    {
        private IServiceCollection _services;
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Env { get; }

        // 运行时将调用此方法。 使用此方法将服务添加到容器。
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(new AppSettings(Configuration));

            services.AddDbSetup();

            services.AddControllers();

            _services = services;
        }

        // 运行时将调用此方法。 使用此方法来配置HTTP请求管道。
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, MySqlContext mySqlContext)
        {
            // 开发环境
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            // Https
            app.UseHttpsRedirection();
            // 路由
            app.UseRouting();
            // 授权
            app.UseAuthorization();
            // 终结点
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            // 生成种子数据
            app.UseSeedDataMildd(mySqlContext, Env.WebRootPath);
        }
    }
}
