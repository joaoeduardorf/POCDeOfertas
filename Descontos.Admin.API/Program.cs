using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Descontos.Admin.API.Data;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<DescontosAdminAPIContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DescontosAdminAPIContext") ?? throw new InvalidOperationException("Connection string 'DescontosAdminAPIContext' not found.")));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<DescontosAdminAPIContext>();
    context.Database.Migrate();

}
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
