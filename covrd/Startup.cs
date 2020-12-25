using covrd.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Swashbuckle.AspNetCore.SwaggerUI;
using Microsoft.Extensions.ML;

//https://xuanwang91.github.io/2020-03-20-cord19-ner/

namespace covrd
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
            var connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<PapContext>(options => options.UseSqlServer(connection));

            var mvc = services.AddControllersWithViews(options =>
            {
            });

#if (DEBUG)
            mvc.AddRazorRuntimeCompilation();
#endif

            //optionsBuilder.UseSqlServer(connection, b => b.MigrationsAssembly("covrd.Model"))
            services.AddRazorPages();
            services.AddMvc();

            services.AddSwaggerGen(c => c.SwaggerDoc(name: "v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "CORD19 API", Version = "v1" }));
            services.AddServerSideBlazor();

            services.AddPredictionEnginePool<TextData, TransformedTextFeaturesData>()
                    .FromFile(modelName: "LDAEstimatorModel", filePath: "./ML/lda_predictionengine_all.zip", watchForChanges: true);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
            });

            

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(url: "/swagger/v1/swagger.json", name: "CORD-19 API V1");
                c.DocExpansion(DocExpansion.None);
            });
        }
    }
}
