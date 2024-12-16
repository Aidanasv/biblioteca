namespace Models;

public class Libro {
    public string Nombre {get;set;}
    public string Autor {get;set;}
    public double Precio {get;set;}
    public string Genero {get;set;}
    public DateTime FechaDePublicacion {get;set;}
    public string  Editorial {get;set;}
    public int Paginas {get;set;}
    public int Estatus {get;set;}
    

    public Libro(string nombre, string autor, double precio, string genero, DateTime fechaDePublicacion, string editorial, int paginas, int estatus) {
        Nombre = nombre;
        Autor = autor;
        Precio = precio;
        Genero = genero;
        FechaDePublicacion = fechaDePublicacion;
        Editorial = editorial;
        Paginas = paginas;
        Estatus = estatus;
    }

    public void MostrarDetalles() {
        Console.WriteLine(Nombre);
        Console.WriteLine($"Autor: \t\t{Autor}");
        Console.WriteLine($"Precio: \t\t{Precio}");
        Console.WriteLine($"Género: \t\t{Genero}");
        Console.WriteLine($"Editorial: \t\t{Editorial}");
        Console.WriteLine($"Fecha de Publicación: \t\t{FechaDePublicacion}");
        Console.WriteLine($"Páginas: \t\t{Paginas}");
        Console.WriteLine($"Estatus: \t\t{Estatus}");
    }
}

