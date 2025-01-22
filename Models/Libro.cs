namespace Models;
using Spectre.Console;

public class Libro
{
    public string Nombre { get; set; }
    public string Autor { get; set; }
    public double Precio { get; set; }
    public string Genero { get; set; }
    public DateTime FechaDePublicacion { get; set; }
    public string Editorial { get; set; }
    public int Paginas { get; set; }
    public int Estatus { get; set; }


    public Libro(string nombre, string autor, double precio, string genero, DateTime fechaDePublicacion, string editorial, int paginas, int estatus)
    {
        Nombre = nombre;
        Autor = autor;
        Precio = precio;
        Genero = genero;
        FechaDePublicacion = fechaDePublicacion;
        Editorial = editorial;
        Paginas = paginas;
        Estatus = estatus;
    }

    public void MostrarDetalles()
    {
        var tablaLibros = new Table();

        tablaLibros.Title = new TableTitle($"[bold green]{Nombre}[/]");
        tablaLibros.HideHeaders();

        tablaLibros.Border = TableBorder.Square;

        tablaLibros.AddColumn(string.Empty);
        tablaLibros.AddColumn(string.Empty);

        tablaLibros.AddRow("[underline deepskyblue2]Nombre[/]", Nombre);
        tablaLibros.AddRow("[underline deepskyblue2]Autor[/]", Autor);
        tablaLibros.AddRow("[underline deepskyblue2]Precio[/]", Precio.ToString());
        tablaLibros.AddRow("[underline deepskyblue2]Género[/]", Genero);
        tablaLibros.AddRow("[underline deepskyblue2]Editorial[/]", Editorial);
        tablaLibros.AddRow("[underline deepskyblue2]Fecha de Publicación[/]", FechaDePublicacion.ToShortDateString());
        tablaLibros.AddRow("[underline deepskyblue2]Páginas[/]", Paginas.ToString());
       
        AnsiConsole.Write(tablaLibros);
    }

    public bool Equals(Libro other)
    {
        if (other == null) return false;
        return (this.Nombre.Equals(other.Nombre));
    }
}

