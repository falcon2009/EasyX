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
using Person.Share.Key;
using Person.Storage;
using Person.Storage.Mapping;
using Person.Storage.Repo;
using Person.Storage.Service;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.IO;
using System.Reflection;
using Entity = Person.Storage.Entity;

namespace Person.Api
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
            MapperConfiguration mapperConfig = new MapperConfiguration(config =>
            {
                config.AddProfile(new PersonMapping());
                config.AddProfile(new PersonContactMapping());
            });
            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            //add db context
            services.AddDbContext<ApiDbContext>(SetDBContext);

            services.AddControllers();

            services.AddSwaggerGen(SetSwagger);

            #region service
            services.AddTransient<IModelService<PersonKey>, PersonService>();
            services.AddTransient<IModelService<PersonContactKey>, PersonContactService>();
            #endregion

            #region repo
            services.AddScoped<IQueryableDataProvider<Entity.Person, PersonKey>, PersonRepository>();
            services.AddScoped<IDataManager<Entity.Person, PersonKey>, PersonRepository>();
            services.AddScoped<IQueryableDataProvider<Entity.PersonContact, PersonContactKey>, PersonContactRepository>();
            services.AddScoped<IDataManager<Entity.PersonContact, PersonContactKey>, PersonContactRepository>();
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(SetSwaggerUI);
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

            builder.UseSqlServer(Configuration.GetConnectionString("ApiDbContext"), option => option.MigrationsAssembly("Person.Api"));
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

            string webApiDocPath = Path.Combine(AppContext.BaseDirectory, "Person.Api.xml");
            string shareDocPath = Path.Combine(AppContext.BaseDirectory, "Person.Share.xml");
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
