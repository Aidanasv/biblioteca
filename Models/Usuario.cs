namespace Models;

public class Usuario {
    public string Nombre {get;set;}
    public string NombreDeUsuario {get;set;}
    public string Contrasena {get;set;}
    public string Correo {get;set;}
    public DateTime FechaDeNacimiento {get;set;}
    public DateTime FechaDeCreacion {get;set;}
    public int Rol {get;set;}


    public Usuario(string nombre, string nombreDeUsuario, string contrasena, string correo, DateTime fechaDeNacimiento, int rol= 1) {
        Nombre = nombre;
        NombreDeUsuario = nombreDeUsuario;
        Contrasena = contrasena;
        Correo = correo;
        FechaDeNacimiento = fechaDeNacimiento;
        FechaDeCreacion = DateTime.Now;
        Rol = rol;
    }
}