using Microsoft.EntityFrameworkCore;
using OnePieceMinimal.Api.Context;
using OnePieceMinimal.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<CharacterContext>(options => 
    options.UseInMemoryDatabase("CharactersDB"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapCharacterEndpoints();

app.Run();