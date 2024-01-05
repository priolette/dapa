using DAPA.Database;
using DAPA.Models.Mappings;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder
    .Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(DiscountProfile), typeof(LoyaltyProfile));

builder.Services.AddScoped<IOrderContext>(
    provider => provider.GetRequiredService<OrderContext>()
);

builder.Services.AddDbContext<OrderContext>(
    options => options.UseNpgsql(builder.Configuration.GetConnectionString("DAPADatabase"))
);

builder.Services.AddScoped<IDiscountRepository, DiscountDatabaseRepository>();
builder.Services.AddScoped<ILoyaltyRepository, LoyaltyDatabaseRepository>();
builder.Services.AddScoped<IServiceRepository, ServiceDatabaseRepository>();

var app = builder.Build();

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
    var discountContext = services.GetRequiredService<IOrderContext>();

    discountContext.Instance.Database.Migrate();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();