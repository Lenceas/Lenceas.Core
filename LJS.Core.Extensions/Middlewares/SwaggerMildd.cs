using Lenceas.Core.Common;
using Microsoft.AspNetCore.Builder;
using System;
using System.Linq;
using static Lenceas.Core.Extensions.CustomApiVersion;

namespace Lenceas.Core.Extensions
{
    /// <summary>
    /// Swagger中间件
    /// </summary>
    public static class SwaggerMildd
    {
        public static void UseSwaggerMildd(this IApplicationBuilder app)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                var ApiName = AppSettings.app(new string[] { "Startup", "ApiName" });
                typeof(ApiVersions).GetEnumNames().OrderByDescending(e => e).ToList().ForEach(version =>
                {
                    c.SwaggerEndpoint($"/swagger/{version}/swagger.json", $"{ApiName} {version}");
                });
                c.RoutePrefix = "";
                c.HeadContent = @$"<script async='async' 
                    id='mini-profiler' 
                    src='/profiler/includes.min.js?v=4.2.22+4563a9e1ab' 
                    data-version='4.2.22+4563a9e1ab' 
                    data-path='/profiler/'
                    data-current-id='59e91ce8-3995-4c36-8a8c-a91f552259d1' 
                    data-ids='59e91ce8-3995-4c36-8a8c-a91f552259d1' 
                    data-position='Left'
                    data-scheme='Light''
                    data-authorized='true' 
                    data-max-traces='5' 
                    data-toggle-shortcut='Alt+P'
                    data-trivial-milliseconds='2.0' 
                    data-ignored-duplicate-execute-types='Open,OpenAsync,Close,CloseAsync'>
                </script>";
            });
        }
    }
}
