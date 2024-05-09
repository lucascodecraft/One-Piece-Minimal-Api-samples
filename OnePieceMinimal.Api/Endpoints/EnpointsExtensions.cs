using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnePieceMinimal.Api.Context;
using OnePieceMinimal.Api.Models;

namespace OnePieceMinimal.Api.Endpoints
{
    public static class EnpointsExtensions
    {
        public static WebApplication MapCharacterEndpoints(this WebApplication app)
        {
            app.MapGet("/characters", async (
                [FromQuery] int skip = 0,
                [FromQuery] int take = 10, 
                CharacterContext? db = default) =>
            {
                skip = GetDefaultValue(skip, 0);
                take = GetDefaultValue(take, 10);

                return await db.Characters
                .Skip(skip)
                .Take(take)
                .ToListAsync();
            });

            app.MapGet("/characters/{id}", async (int id, CharacterContext db) =>
                await db.Characters.FindAsync(id) is Character character
                ? Results.Ok(character)
                : Results.NotFound("Character not found."));

            app.MapPost("/characters", async (Character character, CharacterContext db) =>
            {
                character.Id = 0;

                await db.Characters.AddAsync(character);
                await db.SaveChangesAsync();
                return Results.Created($"/characters/{character.Id}", character);

            });

            app.MapPut("/characters/{id}", async (int id, Character character, CharacterContext db) =>
            {
                var existingCharacter = await db.Characters.FindAsync(id);

                if (existingCharacter == null)
                {
                    return Results.NotFound("Character not found.");
                }

                existingCharacter.Name = character.Name;
                existingCharacter.Bounty = character.Bounty;

                db.Characters.Update(existingCharacter);
                await db.SaveChangesAsync();

                return Results.Ok(existingCharacter);
            });

            app.MapDelete("/characters/{id}", async (int id, CharacterContext db) =>
            {
                var character = await db.Characters.FindAsync(id);

                if (character is null)
                    return Results.NotFound();

                db.Characters.Remove(character);
                await db.SaveChangesAsync();
                return Results.NoContent();
            });

            return app;
        }

        private static int GetDefaultValue(int value, int defaultValue)
        {
            return value <= 0 ? defaultValue : value;
        }
    }
}
