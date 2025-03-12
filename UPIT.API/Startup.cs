using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using UPIT.API.Services;
using UPIT.Domain.Interfaces;
using UPIT.Domain.Interfaces.PPI;
using UPIT.Domain.Interfaces.Prefactibilidad;
using UPIT.Domain.Interfaces.ProyectosInternos;
using UPIT.Domain.Interfaces.Repositories;
using UPIT.Domain.Interfaces.Transversales;
using UPIT.Infraestructure.Logging;
using UPIT.Infraestructure.Repositories;
using UPIT.Infraestructure.Repositories.PPI;
using UPIT.Infraestructure.Repositories.ProyectosInternos;
using UPIT.Infraestructure.Repositories.Repositories;
using UPIT.Infraestructure.Repositories.Transversales;

namespace UPIT.API
{
    public class Startup
    {
        private readonly string _pathResourceMessages = "Resources/responseMessages.json";
        private readonly string _loggerNameDapper = "Dapper";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddMicrosoftIdentityWebApi(Configuration.GetSection("EntraID"));

            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddAuthorization();
            /*services.AddAuthorization(options =>
            {
                options.AddPolicy("AppPolicyInternal", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.AuthenticationSchemes.Add("App1");
                });

                options.AddPolicy("AppPolicyExternal", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.AuthenticationSchemes.Add("App2");
                });
            });*/

            services.AddHttpContextAccessor();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<LoggingService>();

            //Create logger Dapper
            var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
            var loggerDapper = loggerFactory.CreateLogger(_loggerNameDapper);

            //DI to Controllers with messages
            services.AddSingleton(sp => new ResponseAPIManager(_pathResourceMessages));
            
            //Connections
            //DB
            string connectionString = Configuration.GetSection("ConnectionStrings:DefaultConnection").Value!;
            //Blob
            string connectionStringBlob = Configuration.GetSection("BlobConnectionStrings:Blob").Value!;
            string defaultContainerBlob = Configuration.GetSection("BlobConnectionStrings:DefaultContainer").Value!;
            
            /*Container de IoC (Inversion of Control)*/
            //Repositories proyectos internos
            services.AddTransient<IContratistaRepository, ContratistaRepository>(sp => new ContratistaRepository(connectionString, loggerDapper));
            services.AddTransient<IInterventoriaRepository, InterventoriaRepository>(sp => new InterventoriaRepository(connectionString, loggerDapper));
            services.AddTransient<ISubdireccionRepository, SubdireccionRepository>(sp => new SubdireccionRepository(connectionString, loggerDapper));
            services.AddTransient<IProyectoRepository, ProyectoRepository>(sp => new ProyectoRepository(connectionString, loggerDapper));
            services.AddTransient<IAvanceRepository, AvanceRepository>(sp => new AvanceRepository(connectionString, loggerDapper));
            services.AddTransient<IObligacionRepository, ObligacionRepository>(sp => new ObligacionRepository(connectionString, loggerDapper));

            //Repositories prefactibilidad
            services.AddTransient<IPreProyectoRepository, PreProyectoRepository>(sp => new PreProyectoRepository(connectionString, loggerDapper));
            services.AddTransient<IPreAvanceRepository, PreAvanceRepository>(sp => new PreAvanceRepository(connectionString, loggerDapper));
            services.AddTransient<IPreTipoAvanceRepository, PreTipoAvanceRepository>(sp => new PreTipoAvanceRepository(connectionString, loggerDapper));

            //Repositories PPI
            services.AddTransient<IPPIProyectoRepository, PPIProyectoRepository>(sp => new PPIProyectoRepository(connectionString, loggerDapper));
            services.AddTransient<IPPIAvanceRepository, PPIAvanceRepository>(sp => new PPIAvanceRepository(connectionString, loggerDapper));
            services.AddTransient<IPPIContratoRepository, PPIContratoRepository>(sp => new PPIContratoRepository(connectionString, loggerDapper));
            services.AddTransient<IPPINovedadRepository, PPINovedadRepository>(sp => new PPINovedadRepository(connectionString, loggerDapper));
            services.AddTransient<IPPIAlertsRepository, PPIAlertsRepository>(sp => new PPIAlertsRepository(connectionString, loggerDapper));
            services.AddTransient<IPPINecesidadInversionRepository, PPINecesidadInversionRepository>(sp => new PPINecesidadInversionRepository(connectionString, loggerDapper));

            //Transversal
            services.AddTransient<IEntidadRepository, EntidadRepository>(sp => new EntidadRepository(connectionString, loggerDapper));
            services.AddTransient<IParametricaRepository, ParametricaRepository>(sp => new ParametricaRepository(connectionString, loggerDapper));
            services.AddTransient<IUserRepository, UserRepository>(sp => new UserRepository(connectionString, loggerDapper));
            services.AddTransient<IScopeRepository, ScopeRepository>(sp => new ScopeRepository(connectionString, loggerDapper));
            services.AddTransient<IRoleRepository, RoleRepository>(sp => new RoleRepository(connectionString, loggerDapper));
            services.AddTransient<IRoleScopeRepository, RoleScopeRepository>(sp => new RoleScopeRepository(connectionString, loggerDapper));
            services.AddTransient<IBlobStorageRepository, AzureBlobStorageRepository>(sp => new AzureBlobStorageRepository(connectionStringBlob, defaultContainerBlob));
            services.AddTransient<ILogsRepository, LogsRepository>(sp => new LogsRepository(connectionString, loggerDapper));

            // Add CORS policy
            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder => builder.WithOrigins("http://localhost:4200", "https://upitwebexternal-a0fcase2fud7hzaz.eastus2-01.azurewebsites.net")
                                    .AllowAnyHeader()
                                    .AllowAnyMethod());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }    

            // Use CORS policy
            app.UseCors("AllowSpecificOrigin");        
            
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                //CORS
               /* app.UseCors(x =>
                {
                    if (env.IsDevelopment())
                    {
                        x.AllowAnyHeader()
                        .AllowAnyMethod()
                        .SetIsOriginAllowed(origin => true) // allow any origin
                        .AllowCredentials();
                    }
                    else
                    {
                        x.AllowAnyHeader()
                        //.WithOrigins("https://upit.com")
                        .WithMethods("GET", "POST")
                        .AllowCredentials();
                    }
                });*/
            });            
        }
    }
}
