using System.ComponentModel.DataAnnotations;
using DAPA.Database;
using DAPA.Models.Mappings;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder
    .Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(DiscountProfile));

builder.Services.AddScoped<IDiscountContext>(
    provider => provider.GetRequiredService<DiscountContext>()
);
builder.Services.AddDbContext<DiscountContext>(
    options => options.UseNpgsql(builder.Configuration.GetConnectionString("DAPADatabase"))
);

builder.Services.AddScoped<IDiscountRepository, DiscountDatabaseRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    var provider = new FileExtensionContentTypeProvider
    {
        Mappings =
        {
            [".yaml"] = "application/x-yaml"
        }
    };
    app.UseStaticFiles(new StaticFileOptions { ContentTypeProvider = provider });

    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger.yaml", "POS system"));

    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    var discountContext = services.GetRequiredService<IDiscountContext>();

    discountContext.Instance.Database.Migrate();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();