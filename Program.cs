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
    "/users",
    (Usuario user) =>
    {
        user.Id = users.Count == 0 ? 1 : users.Max(u => u.Id) + 1;
        users.Add(user);
        return Results.Created($"/users/{user.Id}", user);
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
        if (user is null)
            return Results.NotFound();

        if (dto.Nombre is not null) user.Nombre = dto.Nombre;
        if (dto.Contrasena is not null) user.Contrasena = dto.Contrasena;
        if (dto.Cumpleanios is not null) user.Cumpleanios = (DateTime)dto.Cumpleanios;

        return Results.Ok(user);
    }
);

app.Run();
