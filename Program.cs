using Microsoft.EntityFrameworkCore;
using Refugio.API.Data;
using Oracle.EntityFrameworkCore.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

var connectionString = builder.Configuration.GetConnectionString("Oracle");

if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException(
        "A conexão com o Oracle não foi configurada."
    );
}


builder.Services.AddDbContext<RefugioContext>(options =>
    options.UseOracle(
        connectionString,
        oracleOptions => oracleOptions.UseOracleSQLCompatibility(
            OracleSQLCompatibility.DatabaseVersion19
        )
    )
);


builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
