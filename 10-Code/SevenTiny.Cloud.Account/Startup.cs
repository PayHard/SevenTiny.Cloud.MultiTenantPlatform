﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SevenTiny.Cloud.Account.AuthManagement;
using SevenTiny.Cloud.Account.Core;
using SevenTiny.Cloud.Infrastructure.Configs;
using SevenTiny.Cloud.Infrastructure.Const;
using System;
using System.Text;
using System.Threading.Tasks;

namespace SevenTiny.Cloud.Account
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //添加策略鉴权模式
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Administrator", policy => policy.Requirements.Add(new PolicyRequirement()));
            })
            .AddAuthentication(s =>
            {
                //添加JWT Scheme
                s.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                s.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                s.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            //添加jwt验证：
            .AddJwtBearer(options =>
            {
                options.Events = new JwtBearerEvents()
                {
                    OnMessageReceived = context =>
                    {
                        //如果request请求中有，则直接从request获取
                        string tokenFromRequest = context.Request.Query[AccountConst.KEY_ACCESSTOKEN];
                        if (!string.IsNullOrEmpty(tokenFromRequest))
                        {
                            context.Token = tokenFromRequest;
                            return Task.CompletedTask;
                        }
                        //如果cookie中有token，则直接从cookie获取
                        string tokenFromCookie = context.Request.Cookies[AccountConst.KEY_ACCESSTOKEN];
                        if (!string.IsNullOrEmpty(tokenFromCookie))
                        {
                            context.Token = tokenFromCookie;
                            return Task.CompletedTask;
                        }
                        return Task.CompletedTask;
                    },
                    OnChallenge = context =>
                    {
                        //此处代码为终止.Net Core默认的返回类型和数据结果，这个很重要哦，必须
                        context.HandleResponse();
                        //re-login
                        if (context.Response.StatusCode == 401)
                        {
                            //未登录重新登陆
                            context.Response.Redirect("/UserAccount/Login?httpCode=401");
                        }
                        else if (context.Response.StatusCode == 403)
                        {
                            //无权限跳转到拒绝页面
                            context.Response.Redirect("/Home/HTTP403");
                        }
                        else
                        {
                            //无权限跳转到拒绝页面
                            context.Response.Redirect("/Home/HTTP403");
                        }
                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = context =>
                    {
                        //Token expired
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                        }
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = context => 
                    {
                        return Task.CompletedTask;
                    }
                };
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateLifetime = true,//是否验证失效时间
                    ClockSkew = TimeSpan.FromSeconds(30),
                    ValidateAudience = true,//是否验证Audience
                    ValidAudience = AccountConfig.Instance.TokenAudience,//Audience
                    ValidateIssuer = true,//是否验证Issuer
                    ValidIssuer = AccountConfig.Instance.TokenIssuer,//Issuer，这两项和前面签发jwt的设置一致
                    ValidateIssuerSigningKey = true,//是否验证SecurityKey
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AccountConfig.Instance.SecurityKey))//拿到SecurityKey
                };
            });

            //注入授权Handler
            services.AddSingleton<IAuthorizationHandler, PolicyHandler>();

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            //start 7tiny ---
            //session support
            services.AddDistributedMemoryCache();
            services.AddSession(o =>
            {
                o.IdleTimeout = TimeSpan.FromDays(1);
            });
            //DI
            services.InjectCore();
            services.AddScoped<TokenManagement>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            //start 7tiny ---
            ///add jwt
            app.UseAuthentication();
            //session support
            app.UseSession();
            //end 7tiny ---

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
