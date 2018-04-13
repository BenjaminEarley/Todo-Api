using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using TodoApi.Model;
using TodoApi.Persistence;
using Microsoft.Extensions.Configuration;
using TodoApi.Repo;

namespace TodoApi
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddMvc();
      services.AddSwaggerGen(c => c.SwaggerDoc("v1", new Info { Title = "Todo API", Version = "v1" }));
      services.Configure<Settings>(options =>
        {
          options.ConnectionString = Configuration.GetSection("MongoConnection:ConnectionString").Value;
          options.Database = Configuration.GetSection("MongoConnection:Database").Value;
        }
      );
      services.AddTransient<ITodoRepository, TodoRepository>();
    }

    public void Configure(IApplicationBuilder app)
    {
      app.UseSwagger();
      app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Todo API V1"));
      app.UseMvc();
    }
  }
}