using System.ComponentModel;
using System.Globalization;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using Models;
using SerializeToFile;
using Spectre.Console;

class MenuApp
{
    private List<Usuario> usuarios;
    private List<Compra> compras;
    private List<Libro> libros;
    private List<Libro> librosComprados;
    private Usuario usuario;

    public MenuApp()
    {

    }

    public void MostrarMenuPrincipal()
    {

        var title = new FigletText("Libreria")
            .Centered()
            .Color(Color.Green);

        AnsiConsole.Write(title);

        CargarDatos();

        int opcion = 0;

        do
        {
            var opciones = new Dictionary<int, string>
            {
                { 1, "Buscador" },
                { 2, "Iniciar Sesión" },
                { 3, "Registrarse" },
                { 4, "Salir" }
            };


            opcion = AnsiConsole.Prompt(
                new SelectionPrompt<int>()
                    .Title("[bold underline green]Menú Principal[/]")
                    .MoreChoicesText("[grey](Mueve de arriba hacia abajo para seleccionar tu opción)[/]")
                    .AddChoices(opciones.Keys)
                    .UseConverter(choice => $"{choice}- {opciones[choice]}"));

            switch (opcion)
            {
                case 1:
                    Buscador();
                    break;
                case 2:
                    IniciarSesion();
                    break;
                case 3:
                    RegistrarUsuario();
                    break;
                case 4:
                    break;

            }
        } while (opcion != 4);

        GuardarDatos();
    }

    private void Buscador()
    {

        var busqueda = AnsiConsole.Ask<string>("[bold underline green]Introduce un libro, autor o editorial:[/]");
        AnsiConsole.Clear();
        var librosFiltrados = libros.Where((libro) => libro.Nombre.ToLower().Contains(busqueda.ToLower()) || libro.Autor.ToLower().Contains(busqueda.ToLower()) ||
        libro.Editorial.ToLower().Contains(busqueda.ToLower()) || libro.Genero.ToLower().Contains(busqueda.ToLower()));

        int opcion = 0;

        do
        {
            var opciones = new Dictionary<int, string>
            {
                {0, "Volver"}
            };

            int indice = 1;

            AnsiConsole.WriteLine();
            foreach (var libro in librosFiltrados)
            {
                opciones.Add(indice, libro.Nombre);
                indice++;
            }

            opcion = AnsiConsole.Prompt(
            new SelectionPrompt<int>()
                .Title($"[blue]Estos son los resultados de tu búsqueda:[/][bold green] {busqueda}[/]")
                .MoreChoicesText("[grey](Mueve de arriba hacia abajo para seleccionar tu opción)[/]")
                .AddChoices(opciones.Keys)
                .UseConverter(choice => $"{choice}- {opciones[choice]}"));

            if (opcion != 0)
            {
                Console.Clear();
                librosFiltrados.ElementAt(opcion - 1).MostrarDetalles();

                AnsiConsole.MarkupLine("[grey]Pulsa cualquier tecla para volver...[/]");
                Console.ReadKey();
            }

            AnsiConsole.Clear();

        } while (opcion != 0);
    }

    private void RegistrarUsuario()
    {
        AnsiConsole.MarkupLine("[bold underline green]Registrar Usuario:[/]");
        AnsiConsole.WriteLine();

        var Nombre = AnsiConsole.Ask<string>("Introduce tu nombre: ");
        var NombreDeUsuario = AnsiConsole.Ask<string>("Introduce tu nombre de usuario: ");
        var Contrasena = AnsiConsole.Ask<string>("Introduce tu contraseña: ");
        var Correo = AnsiConsole.Ask<string>("Introduce tu correo: ");

        DateTime FechaDeNacimiento;

        while (true)
        {
            string inputFecha = AnsiConsole.Ask<string>("Introduce tu fecha de nacimiento en formato [yellow]DD-MM-YYYY[/]: ");

            try
            {
                FechaDeNacimiento = DateTime.ParseExact(inputFecha, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                break;
            }
            catch (FormatException)
            {
                AnsiConsole.WriteLine();
                AnsiConsole.MarkupLine("[red]El formato de la fecha no es valido. Por favor, intentalo nuevamente en formato [yellow]DD-MM-YYYY[/].[/]");
                AnsiConsole.WriteLine();
            }
        }

        usuarios.Add(new Usuario(Nombre, NombreDeUsuario, Contrasena, Correo, FechaDeNacimiento));

        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("[green]Usuario registrado correctamente[/]");
        GuardarDatos();

    }

    private async void IniciarSesion()
    {
        AnsiConsole.MarkupLine("[bold underline green]Iniciar Sesión:[/]");
        AnsiConsole.WriteLine();

        var NombreDeUsuario = AnsiConsole.Ask<string>("Usuario: ");
        var Contrasena = AnsiConsole.Ask<string>("Contraseña: ");

        bool bandera = false;
        int rolUsuario = -1;
        foreach (var usuario in usuarios)
        {
            if (NombreDeUsuario == usuario.NombreDeUsuario && Contrasena == usuario.Contrasena)
            {
                bandera = true;
                rolUsuario = usuario.Rol;

                this.usuario = usuario;
            }
        }

        if (bandera == true)
        {
            AnsiConsole.MarkupLine("[green]Usuario logeado[/]");

            AnsiConsole.Clear();
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
            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine("[red]Usuario o contraseña incorrecta[/]");
            Thread.Sleep(3000);
        }

        AnsiConsole.Clear();
    }

    private void MenuAdministrador()
    {

        int opcion = 0;

        do
        {
            var opciones = new Dictionary<int, string>
            {
                { 1, "Ver todos los libros" },
                { 2, "Añadir Libro" },
                { 3, "Eliminar Libro" },
                { 4, "Modificar Libro"},
                { 5, "Ver Compras"},
                { 6, "Cerrar Sesión"}
            };


            opcion = AnsiConsole.Prompt(
                new SelectionPrompt<int>()
                    .Title("[bold underline green]Menú Administrador[/]")
                    .MoreChoicesText("[grey](Mueve de arriba hacia abajo para seleccionar tu opción)[/]")
                    .AddChoices(opciones.Keys)
                    .UseConverter(choice => $"{choice}- {opciones[choice]}"));

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
            }
        } while (opcion != 6);

    }

    private void MostrarLibros()
    {
        int opcion = 0;

        do
        {
            var opciones = new Dictionary<int, string>
            {
                {0, "Volver"}
            };

            int indice = 1;

            AnsiConsole.WriteLine();
            foreach (var libro in libros)
            {
                opciones.Add(indice, libro.Nombre);
                indice++;
            }

            opcion = AnsiConsole.Prompt(
            new SelectionPrompt<int>()
                .Title("[bold underline green]Selecciona un libro para ver mas detalles: [/]")
                .MoreChoicesText("[grey](Mueve de arriba hacia abajo para seleccionar tu opción)[/]")
                .AddChoices(opciones.Keys)
                .UseConverter(choice => $"{choice}- {opciones[choice]}"));

            if (opcion != 0)
            {

                Console.Clear();
                libros.ElementAt(opcion - 1).MostrarDetalles();

                if (usuario.Rol != 0)
                {
                    var opcionCompra = new Dictionary<int, string>
                    {
                        { 1, "Añadir al carrito" },
                        { 2, "Volver" },
                    };

                    int seleccion = AnsiConsole.Prompt(
                        new SelectionPrompt<int>()
                            .MoreChoicesText("[grey](Mueve de arriba hacia abajo para seleccionar tu opción)[/]")
                            .AddChoices(opcionCompra.Keys)
                            .UseConverter(choice => $"{choice}- {opcionCompra[choice]}"));

                    if (seleccion == 1)
                    {
                        Libro libroComprado = libros[opcion - 1];

                        librosComprados.Add(libroComprado);
                    }
                }
                else
                {
                    AnsiConsole.MarkupLine("[grey]Pulsa cualquier tecla para volver...[/]");
                    Console.ReadKey();
                }

            }

            AnsiConsole.Clear();

        } while (opcion != 0);
    }

    private void AgregarLibro()
    {
        AnsiConsole.MarkupLine("[bold underline green]Introduce los datos del libro:[/]");
        AnsiConsole.WriteLine();
        var Nombre = AnsiConsole.Ask<string>("[blue]Nombre: [/]");
        var Autor = AnsiConsole.Ask<string>("[blue]Autor: [/]");
        var Precio = AnsiConsole.Ask<double>("[blue]Precio: [/]");
        var Genero = AnsiConsole.Ask<string>("[blue]Género: [/]");

        DateTime FechaDePublicacion;

        while (true)
        {
            string inputFecha = AnsiConsole.Ask<string>("[blue]Fecha de Publicación [yellow](DD-MM-YYYY)[/]:[/] ");

            try
            {
                FechaDePublicacion = DateTime.ParseExact(inputFecha, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                break;
            }
            catch (FormatException)
            {
                AnsiConsole.WriteLine();
                AnsiConsole.MarkupLine("[red]El formato de la fecha no es valido. Por favor, intentalo nuevamente en formato [yellow]DD-MM-YYYY[/].[/]");
                AnsiConsole.WriteLine();
            }
        }

        var Editorial = AnsiConsole.Ask<string>("[blue]Editorial: [/]");
        var Paginas = AnsiConsole.Ask<int>("[blue]Páginas: [/]");
        var Estatus = AnsiConsole.Ask<int>("[blue]Estatus: [/]");


        libros.Add(new Libro(Nombre, Autor, Precio, Genero, FechaDePublicacion, Editorial, Paginas, Estatus));

        AnsiConsole.Clear();
    }

    private void EliminarLibro()
    {
        var opciones = new Dictionary<int, string>
        {
            {0,"Volver"}
        };

        int indice = 1;

        foreach (var libro in libros)
        {
            opciones.Add(indice, libro.Nombre);
            indice++;
        }

        int opcion = AnsiConsole.Prompt(
                new SelectionPrompt<int>()
                    .Title("[underline green]Selecciona el libro que deseas eliminar: [/]")
                    .MoreChoicesText("[grey](Mueve de arriba hacia abajo para seleccionar tu opción)[/]")
                    .AddChoices(opciones.Keys)
                    .UseConverter(choice => $"{choice}- {opciones[choice]}"));

        if (opcion != 0)
        {
            libros.RemoveAt(opcion - 1);
        }

    }

    private void ModificarLibro()
    {
        var opciones = new Dictionary<int, string>
        {

        };

        int indice = 1;

        foreach (var libro in libros)
        {
            opciones.Add(indice, libro.Nombre);
            indice++;
        }

        int opcion = AnsiConsole.Prompt(
                new SelectionPrompt<int>()
                    .Title("[underline green]Selecciona el libro que deseas modificar: [/]")
                    .MoreChoicesText("[grey](Mueve de arriba hacia abajo para seleccionar tu opción)[/]")
                    .AddChoices(opciones.Keys)
                    .UseConverter(choice => $"{choice}- {opciones[choice]}"));

        Libro libroModificado = libros[opcion - 1];

        AnsiConsole.MarkupLine("[underline blue]Introduce los nuevos datos del libro:[/]");
        AnsiConsole.WriteLine();
        var Nombre = AnsiConsole.Ask<string>("[blue]Nombre: [/]");
        var Autor = AnsiConsole.Ask<string>("[blue]Autor: [/]");
        var Precio = AnsiConsole.Ask<double>("[blue]Precio: [/]");
        var Genero = AnsiConsole.Ask<string>("[blue]Género: [/]");

        DateTime FechaDePublicacion;

        while (true)
        {
            string inputFecha = AnsiConsole.Ask<string>("[blue]Fecha de Publicación [yellow](DD-MM-YYYY)[/]:[/] ");

            try
            {
                FechaDePublicacion = DateTime.ParseExact(inputFecha, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                break;
            }
            catch (FormatException)
            {
                AnsiConsole.WriteLine();
                AnsiConsole.MarkupLine("[red]El formato de la fecha no es valido. Por favor, intentalo nuevamente en formato [yellow]DD-MM-YYYY[/].[/]");
                AnsiConsole.WriteLine();
            }
        }

        var Editorial = AnsiConsole.Ask<string>("[blue]Editorial: [/]");
        var Paginas = AnsiConsole.Ask<int>("[blue]Páginas: [/]");
        var Estatus = AnsiConsole.Ask<int>("[blue]Estatus: [/]");

        libros[opcion - 1] = libroModificado;
    }

    private void VerCompras()
    {
        var tablaCompras = new Table();

        tablaCompras.Border = TableBorder.Rounded;
        tablaCompras.Title = new TableTitle("[bold underline]Compras por Usuario[/]");

        tablaCompras.AddColumn("Usuario");
        tablaCompras.AddColumn("Fecha de Compra");
        tablaCompras.AddColumn("Detalles de los Libros");

        compras.ForEach(compra =>
        {
            var detallesLibros = new List<string>();

            compra.libros.ForEach(libro =>
            {
                detallesLibros.Add($"[green]{libro.Nombre}[/] - [yellow]{libro.Precio:C}[/]");
            });

            tablaCompras.AddRow(
                compra.usuario.Nombre,
                compra.FechaDeCompra.ToString("dd/MM/yyyy"),
                string.Join("\n", detallesLibros)
            );
        });

        AnsiConsole.Write(tablaCompras);
    }

    private void CerrarSesion()
    {

    }

    private void MenuUsuario()
    {
        int opcion = 0;
        librosComprados = new List<Libro>();

        do
        {
            var opciones = new Dictionary<int, string>
            {
                { 1, "Mostrar libros" },
                { 2, "Devolver Libro" },
                { 3, "Ver Compra"},
                { 4, "Cerrar Sesión"},
            };

            opcion = AnsiConsole.Prompt(
               new SelectionPrompt<int>()
                   .Title("[underline bold green]Menú Usuario[/]")
                   .MoreChoicesText("[grey](Mueve de arriba hacia abajo para seleccionar tu opción)[/]")
                   .AddChoices(opciones.Keys)
                   .UseConverter(choice => $"{choice}- {opciones[choice]}"));

            switch (opcion)
            {
                case 1:
                    MostrarLibros();
                    break;
                case 2:
                    DevolverLibro();
                    break;
                case 3:
                    VerCompra();
                    break;
                case 4:
                    CerrarSesion();
                    break;
            }

        } while (opcion != 4);
    }

    private void DevolverLibro()
    {
        if (librosComprados.Count == 0)
        {
            AnsiConsole.Markup("[yellow]No hay libros añadidos al carrito.[/]");
            AnsiConsole.WriteLine();
            AnsiConsole.WriteLine();
            AnsiConsole.Markup("[grey]Pulsa cualquier tecla para volver...[/]");
            Console.ReadKey();
        }
        else
        {
            var opciones = new Dictionary<int, string>
            {

            };

            int indice = 1;

            foreach (var libro in librosComprados)
            {
                opciones.Add(indice, libro.Nombre);
                indice++;
            }

            int opcion = AnsiConsole.Prompt(
                    new SelectionPrompt<int>()
                        .Title("[underline green]Selecciona el libro que deseas devolver: [/]")
                        .MoreChoicesText("[grey](Mueve de arriba hacia abajo para seleccionar tu opción)[/]")
                        .AddChoices(opciones.Keys)
                        .UseConverter(choice => $"{choice}- {opciones[choice]}"));

            librosComprados.RemoveAt(opcion - 1);
        }
        Console.Clear();
    }

    private void VerCompra()
    {
        if (librosComprados.Count == 0)
        {
            AnsiConsole.Markup("[yellow]No hay libros añadidos al carrito.[/]");
            AnsiConsole.WriteLine();
            AnsiConsole.WriteLine();
            AnsiConsole.Markup("[grey]Pulsa cualquier tecla para volver...[/]");
            Console.ReadKey();
        }
        else
        {
            int opcion = 0;
            double totalPrecio = 0;

            var tablaCompra = new Table();

            tablaCompra.Border = TableBorder.Rounded;
            tablaCompra.Title = new TableTitle("[bold underline green] Lista de Compra[/]");

            tablaCompra.AddColumn("Descripción");
            tablaCompra.AddColumn("Precio Unitario");
            tablaCompra.AddColumn("Subtotal");

            foreach (var libro in librosComprados)
            {
                tablaCompra.AddRow($"{libro.Nombre}", $"{libro.Precio:C}", $"{libro.Precio:C}");
                totalPrecio += libro.Precio;
            }

            double iva = totalPrecio * 0.21;
            double totalConIva = totalPrecio + iva;

            tablaCompra.AddEmptyRow();

            tablaCompra.AddRow(
                "[bold]Subtotal[/]",
                string.Empty,
                $"[bold yellow]{totalPrecio:C}[/]"
            );

            tablaCompra.AddRow(
                "[bold]IVA (21%)[/]",
                string.Empty,
                $"[bold yellow]{iva:C}[/]"
            );

            tablaCompra.AddRow(
                "[bold]Total[/]",
                string.Empty,
                $"[bold green]{totalConIva:C}[/]"
            );

            AnsiConsole.Write(tablaCompra);

            do
            {
                var opciones = new Dictionary<int, string>
                {
                    { 1, "Finalizar Compra" },
                    { 2, "Volver" },
                };

                opcion = AnsiConsole.Prompt(
                    new SelectionPrompt<int>()
                        .MoreChoicesText("[grey](Mueve de arriba hacia abajo para seleccionar tu opción)[/]")
                        .AddChoices(opciones.Keys)
                        .UseConverter(choice => $"{choice}- {opciones[choice]}"));

                switch (opcion)
                {
                    case 1:
                        FinalizarCompra();
                        break;
                    case 2:
                        break;
                }
            } while (opcion != 1 && opcion != 2);

        }

        Console.Clear();
    }

    private void FinalizarCompra()
    {
        Compra compraFinal = new Compra(librosComprados, this.usuario);

        compras.Add(compraFinal);

        librosComprados.Clear();
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