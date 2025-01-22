using System.Dynamic;

namespace Models;

class Compra
{
    public List<Libro> libros {get; set;}
    public DateTime FechaDeCompra {get ; set;}
    public Usuario usuario {get; set;}
    public Compra(List<Libro> libros, Usuario usuario)
    {
        this.usuario = usuario;
        this.libros = new List<Libro>(libros);
        FechaDeCompra = DateTime.Now;
    }

}
