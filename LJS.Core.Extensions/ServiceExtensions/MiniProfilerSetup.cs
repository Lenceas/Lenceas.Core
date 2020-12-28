using Microsoft.Extensions.DependencyInjection;
using StackExchange.Profiling.Storage;
using System;

namespace Lenceas.Core.Extensions
{
    /// <summary>
    /// MiniProfiler 启动服务
    /// </summary>
    public static class MiniProfilerSetup
    {
        public static void AddMiniProfilerSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddMiniProfiler(options =>
            {
                options.RouteBasePath = "/profiler";
                (options.Storage as MemoryCacheStorage).CacheDuration = TimeSpan.FromMinutes(1);
                options.PopupRenderPosition = StackExchange.Profiling.RenderPosition.Left;
                options.PopupShowTimeWithChildren = true;
            });
        }
    }
}
