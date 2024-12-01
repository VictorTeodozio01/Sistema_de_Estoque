using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<EstoqueContext>(options => options.UseSqlite("Data Source=estoque.db"));
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMyOrigin", builder =>
        builder.WithOrigins("http://localhost:3000") 
               .AllowAnyMethod() 
               .AllowAnyHeader());
});
builder.Services.AddControllers();
var app = builder.Build();
app.UseCors("AllowMyOrigin");
app.MapControllers();

app.Run();
