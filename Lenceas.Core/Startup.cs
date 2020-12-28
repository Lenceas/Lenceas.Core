using Autofac;
using Lenceas.Core.Common;
using Lenceas.Core.Extensions;
using Lenceas.Core.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

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

            services.AddMemoryCacheSetup();
            services.AddDbSetup();
            services.AddAutoMapperSetup();
            services.AddMiniProfilerSetup();
            services.AddSwaggerSetup();
            services.AddHttpContextSetup();

            services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    //忽略循环引用
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    //不使用驼峰样式的key
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                    //设置时间格式
                    options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                    //忽略Model中为null的属性
                    //options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                });

            _services = services;
        }

        // AutoFac
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new AutofacModuleRegister());
        }

        // 运行时将调用此方法。 使用此方法来配置HTTP请求管道。
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, MySqlContext mySqlContext)
        {
            // 开发环境
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            // 封装Swagger展示
            app.UseSwaggerMildd();
            // 跳转https
            //app.UseHttpsRedirection();
            // 使用静态文件
            app.UseStaticFiles();
            // 路由
            app.UseRouting();
            // 授权
            app.UseAuthorization();
            // 性能分析
            app.UseMiniProfiler();
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
