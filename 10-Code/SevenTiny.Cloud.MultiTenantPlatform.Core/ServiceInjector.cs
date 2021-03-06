﻿using Microsoft.Extensions.DependencyInjection;
using Seventiny.Cloud.ScriptEngine;
using SevenTiny.Cloud.MultiTenantPlatform.Core.DataAccess;
using SevenTiny.Cloud.MultiTenantPlatform.Infrastructure.DependencyInjection;
using SevenTiny.Cloud.MultiTenantPlatform.UIModel.DataAccess;
using System.Reflection;

namespace SevenTiny.Cloud.MultiTenantPlatform.Core
{
    public static class ServiceInjector
    {
        public static void InjectCore(this IServiceCollection services)
        {
            services.AddScoped(Assembly.GetExecutingAssembly());

            //脚本引擎
            services.AddSingleton<IScriptEngineProvider, ScriptEngineProvider>();
            services.AddScoped<MultiTenantPlatformDbContext>();
            services.AddScoped<MultiTenantDataDbContext>();
        }
    }
}
