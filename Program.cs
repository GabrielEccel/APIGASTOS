using Microsoft.EntityFrameworkCore;
using APIGastos.Models;
using APIGastos.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

builder.Services.AddDbContext<Contexto>(options =>
    options.UseInMemoryDatabase("GastosDB"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "API Gastos", Version = "v1" });
});

builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Gastos v1");
    });

    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<Contexto>();

        db.Gastos.AddRange(
            new Gasto { Id = 1, Descricao = "Supermercado", Data = DateTime.Now.AddDays(-2), Valor = 150.75m },
            new Gasto { Id = 2, Descricao = "Combustível", Data = DateTime.Now.AddDays(-1), Valor = 200.00m },
            new Gasto { Id = 3, Descricao = "Restaurante", Data = DateTime.Now, Valor = 85.50m }
        );

        await db.SaveChangesAsync();
    }
}

app.UseHttpsRedirection();
app.UseCors("ReactFrontend");
app.UseAuthorization();
app.MapControllers();

app.Run();