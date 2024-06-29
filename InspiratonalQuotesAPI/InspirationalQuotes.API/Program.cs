using Microsoft.EntityFrameworkCore;
using InspirationalQuotes.Application.Mappers;
using InspirationalQuotes.Application.Repositories;
using InspirationalQuotes.Application.Services;
using InspirationalQuotes.Infrastructure.Data;
using InspirationalQuotes.Infrastructure.Repositories;
using InspirationalQuotes.Infrastructure.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();


// Add CORS services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

// Add DB services to the container.
builder.Services.AddDbContext<QuoteContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IQuoteRepository, QuoteRepository>();
builder.Services.AddScoped<IQuoteService, QuoteService>();

builder.Services.AddAutoMapper(typeof(ObjectMapper));


var app = builder.Build();

app.UseCors("AllowAll");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
