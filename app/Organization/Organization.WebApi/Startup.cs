using AutoMapper;
using EasyX.Crud.Api.Data;
using EasyX.Crud.Api.Model;
using EasyX.Infra;
using EasyX.WebApi.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Organization.Share.Key;
using Organization.Storage;
using Organization.Storage.Mapping;
using Organization.Storage.Repo;
using Organization.Storage.Service;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.IO;
using System.Reflection;
using Entity = Organization.Storage.Entity;


namespace Organization.Api
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
            //add mapper
            MapperConfiguration mapperConfig = new (config =>
            {
                config.AddProfile(new OrganizationMapping());
                config.AddProfile(new EmployeeMapping());
                config.AddProfile(new PositionMapping());
            });
            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            //add db context
            services.AddDbContext<ApiDbContext>(SetDBContext);

            services.AddControllers();

            services.AddSwaggerGen(SetSwagger);

            #region service
            services.AddTransient<IModelService<OrganizationKey>, OrganizationService>();
            services.AddTransient<IModelService<EmployeeKey>, EmployeeService>();
            services.AddTransient<IModelService<PositionKey>, PositionService>();
            #endregion

            #region repo
            services.AddScoped<IQueryableDataProvider<Entity.Organization, OrganizationKey>, OrganizationRepository>();
            services.AddScoped<IDataManager<Entity.Organization, OrganizationKey>, OrganizationRepository>();
            services.AddScoped<IQueryableDataProvider<Entity.Employee, EmployeeKey>, EmployeeRepository>();
            services.AddScoped<IDataManager<Entity.Employee, EmployeeKey>, EmployeeRepository>();
            services.AddScoped<IQueryableDataProvider<Entity.Position, PositionKey>, PositionRepository>();
            services.AddScoped<IDataManager<Entity.Position, PositionKey>, PositionRepository>();
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(SetSwaggerUI);
            }
            app.UseMiddleware<JsonExceptionMiddleware>();
            app.UseRouting();

            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            using IServiceScope serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            IServiceProvider serviceProvider = serviceScope.ServiceProvider;
            using ApiDbContext dbContext = serviceProvider.GetService<ApiDbContext>();
            dbContext?.Database?.Migrate();
        }

        #region private
        private void SetDBContext(DbContextOptionsBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder), Constant.Errors.BuilderNull);
            }

            builder.UseSqlServer(Configuration.GetConnectionString("ApiDbContext"), option => option.MigrationsAssembly("Organization.WebApi"));
        }
        private static void SetSwagger(SwaggerGenOptions option)
        {
            if (option == null)
            {
                throw new ArgumentNullException(nameof(option), Constant.Errors.OptionNull);
            }

            AssemblyName assemblyName = Assembly.GetEntryAssembly().GetName();
            string version = assemblyName.Version.ToString();
            string title = assemblyName.Name;

            option.SwaggerDoc("v1", new OpenApiInfo { Title = title, Version = version });

            string webApiDocPath = Path.Combine(AppContext.BaseDirectory, "Organization.WebApi.xml");
            string shareDocPath = Path.Combine(AppContext.BaseDirectory, "Organization.Share.xml");
            string systemDataCoreDocPath = Path.Combine(AppContext.BaseDirectory, "EasyX.Data.Core.xml");
            if (File.Exists(webApiDocPath))
            {
                option.IncludeXmlComments(webApiDocPath);
            }
            if (File.Exists(shareDocPath))
            {
                option.IncludeXmlComments(shareDocPath);
            }
            if (File.Exists(systemDataCoreDocPath))
            {
                option.IncludeXmlComments(systemDataCoreDocPath);
            }
        }
        private static void SetSwaggerUI(SwaggerUIOptions option)
        {
            if (option == null)
            {
                throw new ArgumentNullException(nameof(option), Constant.Errors.OptionNull);
            }

            option.SwaggerEndpoint("/swagger/v1/swagger.json", "Person.Api");
        }
        #endregion
    }
}
