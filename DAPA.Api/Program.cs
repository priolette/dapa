using DAPA.Database;
using DAPA.Database.Clients;
using DAPA.Database.Discounts;
using DAPA.Database.Loyalties;
using DAPA.Database.Products;
using DAPA.Database.Reservations;
using DAPA.Database.Roles;
using DAPA.Database.Services;
using DAPA.Database.Staff;
using DAPA.Models.Mappings;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder
    .Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(DiscountProfile), typeof(LoyaltyProfile), typeof(ServiceProfile),
    typeof(ClientProfile), typeof(StaffProfile), typeof(RoleProfile), typeof(ProductProfile), typeof(ReservationProfile)
);

builder.Services.AddScoped<IOrderContext>(
    provider => provider.GetRequiredService<OrderContext>()
);

builder.Services.AddDbContext<OrderContext>(
    options => options.UseNpgsql(builder.Configuration.GetConnectionString("DAPADatabase"))
);

builder.Services.AddScoped<IDiscountRepository, DiscountDatabaseRepository>();
builder.Services.AddScoped<ILoyaltyRepository, LoyaltyDatabaseRepository>();
builder.Services.AddScoped<IServiceRepository, ServiceDatabaseRepository>();
builder.Services.AddScoped<IClientRepository, ClientDatabaseRepository>();
builder.Services.AddScoped<IStaffRepository, StaffDatabaseRepository>();
builder.Services.AddScoped<IRoleRepository, RoleDatabaseRepository>();
builder.Services.AddScoped<IProductRepository, ProductDatabaseRepository>();
builder.Services.AddScoped<IReservationRepository, ReservationDatabaseRepository>();

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

app.UseExceptionHandler(c => c.Run(async context =>
{
    var exception = context.Features.Get<IExceptionHandlerPathFeature>()?.Error;
    var response = new { message = exception?.Message };
    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
    await context.Response.WriteAsJsonAsync(response);
    Console.WriteLine(exception?.Message);
}));

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();