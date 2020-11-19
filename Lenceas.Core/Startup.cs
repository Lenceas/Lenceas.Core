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

        // ����ʱ�����ô˷����� ʹ�ô˷�����������ӵ�������
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(new AppSettings(Configuration));

            services.AddDbSetup();

            services.AddControllers();

            _services = services;
        }

        // ����ʱ�����ô˷����� ʹ�ô˷���������HTTP����ܵ���
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, MySqlContext mySqlContext)
        {
            // ��������
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            // Https
            app.UseHttpsRedirection();
            // ·��
            app.UseRouting();
            // ��Ȩ
            app.UseAuthorization();
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
