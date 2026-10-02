using System.Diagnostics;
using MinimalApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();


var users = new List<Usuario>();

app.UseHttpsRedirection();

app.MapGet("/users", () => Results.Ok(users)).WithName("GetUsers");

app.MapPost(
    "/users/register",
    (Usuario user) =>
    {
        user.Id = users.Count == 0 ? 1 : users.Max(u => u.Id) + 1;
        user.Contrasena = BCrypt.Net.BCrypt.HashPassword(user.Contrasena);
        users.Add(user);
        return Results.Created($"/users/{user.Id}", user);
    }
);

app.MapPost(
    "/users/login",
    (Usuario userLog) =>
    {
        var user = users.FirstOrDefault(u => u.NombreUsuario == userLog.NombreUsuario);
        if (user is null) return Results.NotFound();

        if (!BCrypt.Net.BCrypt.Verify(userLog.Contrasena, user.Contrasena))
            return Results.Unauthorized();

        return Results.Ok("Todo bien pa");
    }
);

app.MapDelete(
    "/users/{id}",
    (int id) =>
    {
        var user = users.FirstOrDefault(u => u.Id == id);
        if (user is null)
            return Results.NotFound();

        users.Remove(user);
        return Results.NoContent();
    }
);

app.MapPatch(
    "/users/{id}",
    (int id, UpdateUsuarioDto dto) =>
    {
        var user = users.FirstOrDefault(u => u.Id == id);
        if (user is null) return Results.NotFound();

        if (dto.NombreUsuario is not null) user.NombreUsuario = dto.NombreUsuario;
        if (dto.Contrasena is not null) user.Contrasena = dto.Contrasena;
        if (dto.Cumpleanios is not null) user.Cumpleanios = (DateTime)dto.Cumpleanios;

        return Results.Ok(user);
    }
);

app.Run();
