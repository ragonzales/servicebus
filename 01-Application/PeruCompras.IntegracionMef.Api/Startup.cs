using PeruCompras.IntegracionMef.Api.Configuration;
namespace PeruCompras.IntegracionMef.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        public void ConfigureServices(IServiceCollection services)
        {   
            services.ConfigureAutoMapper();
            services.AddSingleton(Configuration);
            services.ConfigureServices(Configuration);
            services.AddControllers();
            services.AddEndpointsApiExplorer();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment environment)
        {
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseHttpsRedirection();
        }
    }
}
