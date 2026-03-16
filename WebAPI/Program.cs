using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using QuestPDF.Infrastructure;
using System.Text;
using WebAPI.Data;
using WebAPI.Services;
QuestPDF.Settings.License = LicenseType.Community;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<InvoicikaDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .WithExposedHeaders("Authorization");
        });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<ICustomerInvoiceService, CustomerInvoiceService>();
builder.Services.AddScoped<IItemService, ItemService>();
builder.Services.AddScoped<IItemGroupService, ItemGroupService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IVATService, VATService>();
builder.Services.AddSingleton<IFileProvider>(
    new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads")));


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddProblemDetails();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<SeedData>>();
    var context = services.GetRequiredService<InvoicikaDbContext>();

    try
    {   // Ensure the latest migrations are applied
        context.Database.Migrate();
        SeedData.Initialize(context, logger);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseExceptionHandler();

//app.UseHttpsRedirection();
app.UseCors("AllowAllOrigins");
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.WebRootPath, "uploads")),
    RequestPath = "/uploads"
});
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
