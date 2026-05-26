using Lexora.DataAccess.Context;
using Lexora.Repositories;
using Lexora.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddOpenApi();

var connectionString = builder.Configuration.GetConnectionString("LexoraDb");

builder.Services.AddDbContext<LexoraDbContext>(options =>
    options.UseSqlServer(connectionString));

// Regsiter Repositories to DI
builder.Services.AddTransient<IVocabularyEntryRepository, VocabularyEntryRepository>();

// Register Services to DI
builder.Services.AddTransient<IVocabularyEntryService, VocabularyEntryService>();


string CORS = "_cors";
builder.Services.AddCors(options =>
{
  options.AddPolicy(name: CORS, policy =>
              {
                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
                policy.AllowAnyOrigin();
              });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
  app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors(CORS);

app.UseAuthorization();

app.MapControllers();

app.Run();
