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
using System.IdentityModel.Tokens.Jwt;

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

        // ����ʱ�����ô˷����� ʹ�ô˷�����������ӵ�������
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(new AppSettings(Configuration));

            // ȷ������֤���ķ��ص�ClaimType�������ģ���ʹ��Mapӳ��
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddMemoryCacheSetup();
            services.AddDistributedRedisCache(options =>
            {
                options.Configuration = AppSettings.app(new string[] { "Redis", "ConnectionString" }).ToString();
                options.InstanceName = "RedisDistributedCache";
            });
            services.AddDbSetup();
            services.AddAutoMapperSetup();
            services.AddMiniProfilerSetup();
            services.AddSwaggerSetup();
            services.AddHttpContextSetup();
            services.AddAuthorizationSetup();
            services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    //����ѭ������
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    //��ʹ���շ���ʽ��key
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                    //����ʱ���ʽ
                    options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                    //����Model��Ϊnull������
                    //options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                });

            _services = services;
        }

        // AutoFac
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new AutofacModuleRegister());
        }

        // ����ʱ�����ô˷����� ʹ�ô˷���������HTTP����ܵ���
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, MySqlContext mySqlContext)
        {
            // ��������
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            // ��װSwaggerչʾ
            app.UseSwaggerMildd();
            // ��תhttps
            //app.UseHttpsRedirection();
            // ʹ�þ�̬�ļ�
            app.UseStaticFiles();
            // ·��
            app.UseRouting();
            // ��֤
            app.UseAuthentication();
            // ��Ȩ
            app.UseAuthorization();
            // ���ܷ���
            app.UseMiniProfiler();
            // �ս��
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            // ������������
            app.UseSeedDataMildd(mySqlContext, Env.WebRootPath);
        }
    }
}
