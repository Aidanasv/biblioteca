namespace Models;

class Compra {
    private List<(Libro,int)> libros;
    public DateTime FechaDeCompra;

    public Compra() {
        libros = new List<(Libro,int)>();
        FechaDeCompra = DateTime.Now;
    }

    public void AñadirLibro(Libro libro, int cantidad = 1 ) {
        libros.Add((libro,cantidad));
        Console.WriteLine($"Libro añadido: {libro.Nombre}");
    }

    public void EliminarLibro(Libro libro, int cantidad = 1) {
        libros.Remove((libro,cantidad));
        Console.WriteLine($"Libro eliminado: {libro.Nombre}");
    }

    public void MostrarCompra() {
        Console.WriteLine($"Libros Comprados:");
        foreach(var libro in libros)
        {
         Console.WriteLine($"{libro.Item2} {libro.Item1.Nombre} {libro.Item1.Precio}");
        }
    }
}
