using Lexora.DataAccess.Context;
using Lexora.DataAccess.Entities;
using Lexora.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddOpenApi();

var connectionString = builder.Configuration.GetConnectionString("LexoraDb");

builder.Services.AddDbContext<LexoraDbContext>(options =>
    options.UseSqlServer(connectionString));

// Regsiter Repositories to DI
builder.Services.AddTransient<IVocabularyEntryRepository, VocabularyEntryRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
  app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
