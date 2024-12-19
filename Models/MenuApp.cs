using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using Models;
using SerializeToFile;

class MenuApp
{

    private List<Usuario> usuarios;
    private List<Compra> compras;
    private List<Libro> libros;
    private List<Libro> librosComprados;

    public MenuApp()
    {

    }

    public void MostrarMenuPrincipal()
    {

        CargarDatos();

        int opcion = 0;

        do
        {
            Console.WriteLine("--- Menú Principal ---");
            Console.WriteLine("1-Iniciar Sesión");
            Console.WriteLine("2-Registrarse");
            Console.WriteLine("3-Salir");

            if (!int.TryParse(Console.ReadLine(), out opcion) || opcion < 1 || opcion > 5)
            {
                Console.WriteLine("Error: Por favor selecciona una opción válida (1-5).");
                continue;
            }

            switch (opcion)
            {
                case 1:
                    IniciarSesion();
                    break;
                case 2:
                    RegistrarUsuario();
                    break;
                case 3:
                    break;
                default:
                    Console.WriteLine("Opción invalida.");
                    break;
            }
        } while (opcion != 3);

        GuardarDatos();
    }

    private void RegistrarUsuario()
    {
        Console.WriteLine("Introduce tu Nombre: ");
        string Nombre = Console.ReadLine();
        Console.WriteLine("Introduce el Nombre de Usuario: ");
        string NombreDeUsuario = Console.ReadLine();
        Console.WriteLine("Introduce tu contraseña: ");
        string Contrasena = Console.ReadLine();
        Console.WriteLine("Introduce tu correo: ");
        string Correo = Console.ReadLine();
        Console.WriteLine("Introduce tu fecha de nacimiento");

        int Year = int.Parse(Console.ReadLine());
        int Mes = int.Parse(Console.ReadLine());
        int Dia = int.Parse(Console.ReadLine());
        DateTime FechaDeNacimiento = new DateTime(Year, Mes, Dia);

        usuarios.Add(new Usuario(Nombre, NombreDeUsuario, Contrasena, Correo, FechaDeNacimiento));
    }

    private void IniciarSesion()
    {
        Console.WriteLine("Introduce tu usuario: ");
        string NombreDeUsuario = Console.ReadLine();
        Console.WriteLine("Introduce tu contraseña: ");
        string Contrasena = Console.ReadLine();

        bool bandera = false;
        int rolUsuario = -1;
        foreach (var usuario in usuarios)
        {

            Console.WriteLine($"{usuario.NombreDeUsuario} {usuario.Contrasena}");
            if (NombreDeUsuario == usuario.NombreDeUsuario && Contrasena == usuario.Contrasena)
            {
                bandera = true;
                rolUsuario = usuario.Rol;
            }
        }

        if (bandera == true)
        {
            Console.WriteLine("Usuario logeado");
            if (rolUsuario == 0)
            {
                MenuAdministrador();
            }
            else
            {
                MenuUsuario();
            }
        }
        else
        {
            Console.WriteLine("Usuario o contraseña incorrecta");
        }
    }

    private void MenuAdministrador()
    {

        int opcion = 0;

        do
        {
            Console.WriteLine("---Menú Administrador---");
            Console.WriteLine("1-Ver todos los Libros");
            Console.WriteLine("2-Añadir Libro");
            Console.WriteLine("3-Eliminar Libro");
            Console.WriteLine("4-Modificar Libro");
            Console.WriteLine("5-Ver Compras");
            Console.WriteLine("6-Cerrar Sesión");

            if (!int.TryParse(Console.ReadLine(), out opcion) || opcion < 1 || opcion > 6)
            {
                Console.WriteLine("Error: Por favor selecciona una opción válida (1-6).");
                continue;
            }

            switch (opcion)
            {
                case 1:
                    MostrarLibros();
                    break;
                case 2:
                    AgregarLibro();
                    break;
                case 3:
                    EliminarLibro();
                    break;
                case 4:
                    ModificarLibro();
                    break;
                case 5:
                    VerCompras();
                    break;
                case 6:
                    CerrarSesion();
                    break;
                default:
                    Console.WriteLine("Opción invalida.");
                    break;
            }
        } while (opcion != 6);

    }

    private void MostrarLibros()
    {
        Console.WriteLine("---Lista de Libros:---");
        foreach (var libro in libros)
        {
            Console.WriteLine($"{libro.Nombre} {libro.Precio}");
        }
    }

    private void AgregarLibro()
    {
        try
        {
            Console.WriteLine("---Introduce los datos del libro:---");
            Console.Write("Nombre del libro: ");
            string Nombre = Console.ReadLine();
            Console.Write("Autor: ");
            string Autor = Console.ReadLine();
            Console.Write("Precio: ");
            double Precio = double.Parse(Console.ReadLine());
            Console.Write("Género: ");
            string Genero = Console.ReadLine();
            Console.Write("Fecha de Publicación: ");
            int Year = int.Parse(Console.ReadLine());
            int Mes = int.Parse(Console.ReadLine());
            int Dia = int.Parse(Console.ReadLine());
            DateTime FechaDePublicacion = new DateTime(Year, Mes, Dia);
            Console.Write("Editorial: ");
            string Editorial = Console.ReadLine();
            Console.Write("Páginas: ");
            int Paginas = int.Parse(Console.ReadLine());
            Console.Write("Estatus: ");
            int Estatus = int.Parse(Console.ReadLine());

            libros.Add(new Libro(Nombre, Autor, Precio, Genero, FechaDePublicacion, Editorial, Paginas, Estatus));
        }
        catch (Exception ex)
        {
            var messageError = "ExceptionError" + ex.Message;
            Console.WriteLine(messageError);
        }
    }

    private void EliminarLibro()
    {

        int indice = 1;

        foreach (var libro in libros)
        {
            Console.WriteLine($"{indice} {libro.Nombre}");
            indice++;
        }

        Console.WriteLine("Selecciona el libro que deseas eliminar: ");
        int numeroLibro = int.Parse(Console.ReadLine());

        libros.RemoveAt(numeroLibro - 1);
    }

    private void ModificarLibro()
    {
        int indice = 1;

        foreach (var libro in libros)
        {
            Console.WriteLine($"{indice} {libro.Nombre}");
            indice++;
        }

        Console.WriteLine("Selecciona el libro que deseas modificar: ");
        int numeroLibro = int.Parse(Console.ReadLine());

        Libro libroModificado = libros[numeroLibro - 1];

        Console.WriteLine("---Introduce los nuevos datos del libro:---");
        Console.Write("Nombre del libro: ");
        libroModificado.Nombre = Console.ReadLine();
        Console.Write("Autor: ");
        libroModificado.Autor = Console.ReadLine();
        Console.Write("Precio: ");
        libroModificado.Precio = double.Parse(Console.ReadLine());
        Console.Write("Género: ");
        libroModificado.Genero = Console.ReadLine();
        Console.Write("Fecha de Publicación: ");
        int Year = int.Parse(Console.ReadLine());
        int Mes = int.Parse(Console.ReadLine());
        int Dia = int.Parse(Console.ReadLine());
        libroModificado.FechaDePublicacion = new DateTime(Year, Mes, Dia);
        Console.Write("Editorial: ");
        libroModificado.Editorial = Console.ReadLine();
        Console.Write("Páginas: ");
        libroModificado.Paginas = int.Parse(Console.ReadLine());
        Console.Write("Estatus: ");
        libroModificado.Estatus = int.Parse(Console.ReadLine());

        libros[numeroLibro - 1] = libroModificado;

    }

    private void VerCompras()
    {

    }

    private void CerrarSesion()
    {

    }

    private void MenuUsuario()
    {
        Console.WriteLine("---Menú Usuario---");
        Console.WriteLine("1-Ver todos los Libros");
        Console.WriteLine("2-Comprar Libro");
        Console.WriteLine("3-Devolver Libro");
        Console.WriteLine("4-Ver Compra");
        Console.WriteLine("5-Cerrar Sesión");
    }

    private void ComprarLibro()
    {
        int indice = 1;

        foreach (var libro in libros)
        {
            Console.WriteLine($"{indice} {libro.Nombre}");
            indice++;
        }

        Console.WriteLine("Selecciona el libro que deseas comprar: ");
        int numeroLibro = int.Parse(Console.ReadLine());

        Libro libroComprado = libros[numeroLibro - 1];

        librosComprados.Add(libroComprado);
    }

    private void GuardarDatos()
    {
        Utilidades datos = new Utilidades();

        string fileLibro = "Libro.json";
        datos.GuardarDatos(fileLibro, libros);

        string fileUsuario = "Usuario.json";
        datos.GuardarDatos(fileUsuario, usuarios);

        string fileCompra = "Compra.json";
        datos.GuardarDatos(fileCompra, compras);
    }

    private void CargarDatos()
    {
        Utilidades datos = new Utilidades();

        string fileLibro = "Libro.json";
        libros = datos.CargarDatos<List<Libro>>(fileLibro);

        string fileUsuario = "Usuario.json";
        usuarios = datos.CargarDatos<List<Usuario>>(fileUsuario);

        if (usuarios.Count == 0)
        {
            usuarios.Add(new Usuario("Admin", "admin", "1234", "admin1234@gmail.com", DateTime.Now, 0));
        }

        string fileCompra = "Compra.json";
        compras = datos.CargarDatos<List<Compra>>(fileCompra);

    }

}