using System.Text.Json.Serialization;

namespace MinimalApi;

public class Usuario(string nombre, string contrasena, DateTime cumpleanios)
{
    public int? Id { get; set; }
    public string NombreUsuario { get; set; } = nombre;
    public string Contrasena { get; set; } = contrasena;
    public DateTime Cumpleanios { get; set; } = cumpleanios;
}

public class UpdateUsuarioDto
{
    public string? NombreUsuario { get; set; }
    public string? Contrasena { get; set; }
    public DateTime? Cumpleanios { get; set; }
}