// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Configuration;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Services;
using IdentityServer4.Test;
using IdentityServerHost.Quickstart.UI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Net;
using System.Reflection;

namespace IdentityServer
{
    public class Startup
    {
        public IWebHostEnvironment Environment { get; }

        public Startup(IWebHostEnvironment environment)
        {
            Environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            const string connectionString = @"Data Source=(LocalDb)\V11.0;database=IdentityServerDb;trusted_connection=yes;";

            // uncomment, if you want to add an MVC-based UI
            services.AddControllersWithViews();
            services.AddCors(setup =>
            {
                setup.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyHeader();
                    policy.AllowAnyMethod();
                   // policy.AllowAnyOrigin();
                   policy.WithOrigins("http://localhost:4300", "http://localhost:4500", "http://localhost:4200", "http://localhost:8081");
                     policy.AllowCredentials();
                });
            });
            services.AddTransient<IReturnUrlParser, ReturnUrlParser>();
            var builder = services.AddIdentityServer(options =>
            {
                // see https://identityserver4.readthedocs.io/en/latest/topics/resources.html
                //options.EmitStaticAudienceClaim = true;
                options.UserInteraction.LoginUrl = "http://localhost:4300/login";
                // "http://localhost:8081/index.html"; 
            })
              //            .AddInMemoryIdentityResources(Config.IdentityResources)
              //.AddInMemoryApiResources(Config.Apis)
              //.AddInMemoryClients(Config.Clients)
              //.AddInMemoryApiScopes(Config.ApiScopes)
              //.AddTestUsers(TestUsers.Users);
              .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = b => b.UseSqlServer(connectionString);
                    //options.ConfigureDbContext = b => b.UseSqlServer(connectionString,
                    //    sql => sql.MigrationsAssembly(migrationsAssembly));
                })
        .AddOperationalStore(options =>
        {
            options.ConfigureDbContext = b => b.UseSqlServer(connectionString);
            //options.ConfigureDbContext = b => b.UseSqlServer(connectionString,
            //    sql => sql.MigrationsAssembly(migrationsAssembly));
        }).AddTestUsers(TestUsers.Users);
            //PersistedGrantDbContext

            // not recommended for production - you need to store your key material somewhere secure
            builder.AddDeveloperSigningCredential();

            //var cors = new DefaultCorsPolicyService(new LoggerFactory().CreateLogger<DefaultCorsPolicyService>())
            //{
            //    AllowedOrigins = { "http://localhost:4200", "http://localhost:4300", "http://localhost:8081" }
            //};
            //services.AddSingleton<ICorsPolicyService>(cors);

            //var cors = new CustomCorsPolicy();
            //services.AddSingleton<ICorsPolicyService>(cors);
        }

        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

    /*        app.Use(async (context, next) =>
            {
                

                var headers = context.Response.Headers;
                if (headers.ContainsKey("Access-Control-Allow-Origin"))
                {
                    headers["Access-Control-Allow-Origin"] = "*"; //string.Join(",", new[]{ "http://localhost:4300", "http://localhost:4200" });

                    //headers["Access-Control-Allow-Origin"] = string.Join(",", context.Request.Headers["Referer"].Select(x => x.Substring(0, x.Length - 1)));
                }
                else
                {
                    context.Response.Headers.Append("Access-Control-Allow-Origin", "*");
                }
                if (headers.ContainsKey("Access-Control-Allow-Headers"))
                {
                    headers["Access-Control-Allow-Headers"] = "Origin, Content-Type, Accept, Client, Authorization, X-Auth-Token, X-Requested-With";
                }
                else
                {
                    context.Response.Headers.Append("Access-Control-Allow-Headers", "Origin, Content-Type, Accept, Client, Authorization, X-Auth-Token, X-Requested-With");
                }
                if (headers.ContainsKey("Access-Control-Allow-Methods"))
                {
                    headers["Access-Control-Allow-Credentials"] = "GET, POST, PATCH, PUT, DELETE, OPTIONS";
                }
                else
                {
                    context.Response.Headers.Append("Access-Control-Allow-Methods", "GET, POST, PATCH, PUT, DELETE, OPTIONS");
                }
                if (headers.ContainsKey("Access-Control-Allow-Credentials"))
                {
                    headers["Access-Control-Allow-Credentials"] = "true";
                }
                else
                {
                    context.Response.Headers.Append("Access-Control-Allow-Credentials", "true");
                }
                context.Response.Headers.Append("Access-Control-Expose-Headers", "X-Auth-Token");
                context.Response.Headers.Append("Vary", "Origin");
                if (context.Request.Method == "OPTIONS")
                {
                    context.Response.StatusCode = (int)HttpStatusCode.OK;
                    return;
                }
                await next();
            });
    */
            // uncomment if you want to add MVC
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors();

          //  InitializeDatabase(app);



            app.UseIdentityServer();

            
            // uncomment, if you want to add MVC
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }

        private void InitializeDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

                var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                context.Database.Migrate();
                if (!context.Clients.Any())
                {
                    foreach (var client in Config.Clients)
                    {
                        context.Clients.Add(client.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.IdentityResources.Any())
                {
                    foreach (var resource in Config.IdentityResources)
                    {
                        context.IdentityResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.ApiResources.Any())
                {
                    foreach (var resource in Config.Apis)
                    {
                        context.ApiResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }
            }
        }

    }
}
