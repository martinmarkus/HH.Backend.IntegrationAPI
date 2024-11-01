using HH.Backend.Common.Core.Configuration;
using HH.Backend.Common.Core.Constants;
using HH.Backend.Common.DataAccess.DbContexts;
using HH.Backend.Common.Web.Request.Filters;
using HH.Backend.Common.Web.WireUp;
using HH.Backend.IntegrationAPI.Request.Filters;
using Microsoft.EntityFrameworkCore;

namespace HH.Backend.IntegrationAPI
{
    public class Startup : BaseStartup
    {
        public Startup(IConfiguration configuration, IHostEnvironment env) : base(configuration, env)
        {
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);

            services.RegisterDynamically("HH.Backend.IntegrationAPI");

            // INFO: Manual registering of custom config files
            services.AddConfiguration<HungarianZipCodeOptions>(Configuration);
            services.AddConfiguration<CountryOptions>(Configuration);

            services.AddScoped<ValidateClient>();

            services.AddScoped<AuthorizeIntegration>();

            services.AddDbContext<HHDbContext>(
                options => options
                    .UseSqlServer(
                        DbOptions.ConnectionString,
                        optionsBuilder => optionsBuilder.MigrationsAssembly(DbOptions.MigrationAssembly)),
                ServiceLifetime.Scoped);
        }

        public override void Configure(IApplicationBuilder app, IServiceScopeFactory serviceScopeFactory)
        {
            // INFO: A Cors policy setting must be the first
            app.UseCors(builder => builder
                .WithOrigins(BaseOptions.AllowedCORSPolicyURLs)
                .SetIsOriginAllowedToAllowWildcardSubdomains()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .WithExposedHeaders(
                    SessionConstants.HeaderAuthToken,
                    SessionConstants.RefreshToken)
                .AllowCredentials()
            );

            base.Configure(app, serviceScopeFactory);
        }
    }
}
