using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Mikrocop.ManagingUsers.Infrastructure;
using Mikrocop.ManagingUsers.Models;
using Serilog;

namespace Mikrocop.ManagingUsers
{
    public class Startup
    {
        #region Ctor

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            CurrentEnvironment = env;
        }

        private readonly IConfiguration _configuration;
        private IWebHostEnvironment CurrentEnvironment { get; set; }

        #endregion


        public virtual void ConfigureServices(IServiceCollection services)
        {
            if (CurrentEnvironment != null && CurrentEnvironment.EnvironmentName == "Test")
            {
                services.AddDbContext<AdministrationContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDb");
                });
            }
            else
            {
                services.AddDbContext<AdministrationContext>(options => options.UseNpgsql(_configuration["ConnectionString"]));
            }

            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API-Key Auth", Version = "v1" });
                c.AddSecurityDefinition("API-Key", new OpenApiSecurityScheme
                {
                    Description = "API-Key must appear in header",
                    Type = SecuritySchemeType.ApiKey,
                    Name = "API-Key",
                    In = ParameterLocation.Header,
                    Scheme = "ApiKeyScheme"
                });
                var key = new OpenApiSecurityScheme()
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "API-Key"
                    },
                    In = ParameterLocation.Header
                };
                var requirement = new OpenApiSecurityRequirement
                    {
                             { key, new List<string>() }
                    };
                c.AddSecurityRequirement(requirement);
            });
        }

        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            try
            {
                using var scope = app.ApplicationServices.CreateScope();
                using var context = scope.ServiceProvider.GetService<AdministrationContext>() ?? throw new Exception("Db context cannot be null");
                context.Database.Migrate();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                Log.Error(ex.StackTrace ?? string.Empty);
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else if (env.EnvironmentName == "Test")
            {
                app.UseMiddleware<FakeRemoteIpAddressMiddleware>();
            }
            app.UseMiddleware<LogMiddleware>();
            app.UseMiddleware<ApiKeyMiddleware>();

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}