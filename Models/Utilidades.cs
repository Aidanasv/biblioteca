using System.Runtime.CompilerServices;
using System.Text.Json;
using Models;

namespace SerializeToFile
{
    public class Utilidades
    {
        public void GuardarDatos(string filePath, object datos)
        {
            string JsonString = JsonSerializer.Serialize(datos);
            File.WriteAllText(filePath, JsonString);
        }

        public T CargarDatos<T>(string filePath) where T : new()
        {
            if (!File.Exists(filePath))
            {
                return new T();
            }

            string datos = File.ReadAllText(filePath);

            var resultado = JsonSerializer.Deserialize<T>(datos);
            if (resultado == null)
            {
                throw new InvalidDataException($"No se pudo deserializar el contenido del archivo {filePath}.");
            }

            return resultado;
        }


    }
}