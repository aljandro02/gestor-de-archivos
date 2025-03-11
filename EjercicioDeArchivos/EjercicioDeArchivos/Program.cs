using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;

namespace EjercicioDeArchivos
{

    internal class Program
    {
        static void Main(string[] args)
        {
            string directorio;
            do
            {
                Console.WriteLine("por favor, ingresa la ruta del directorio: ");
                directorio = Console.ReadLine();

                if (!Directory.Exists(directorio))
                {
                    Console.WriteLine("la ruta especificada no exite, ingrese una ruta valida");
                }

            } while (!Directory.Exists(directorio));

            ExplorarDirectorio(directorio);
        }
        static void ExplorarDirectorio(string directorio)
        {
            bool continuar = true;
            while (continuar)
            {
                Console.Clear();
                Console.WriteLine($"Contenido de:  {directorio}\n");
                string[] archivosSubdirectorios = Directory.GetFileSystemEntries(directorio);
                MostrarTabla(archivosSubdirectorios);

                Console.WriteLine("Ingrese el numero de la opcion que deseas explorar(o 'a' para ir hacia atras, 'n' para ingresar en la nueva ruta, o 's' para salir):");
                string opcion = Console.ReadLine();
                if (opcion.ToLower() == "s")
                {
                    continuar = false;
                }
                else if (opcion.ToLower() == "a")
                {
                    directorio = Path.GetDirectoryName(directorio);
                }
                else if (opcion.ToLower() == "n")
                {
                    Console.Clear();
                    Console.WriteLine("Ingrese una nueva ruta: ");
                    string nuevaRuta = Console.ReadLine();
                    if (Directory.Exists(nuevaRuta))
                    {
                        directorio = nuevaRuta;
                    }
                    else
                    {
                        Console.WriteLine("ingresa una ruta valida");
                        Console.WriteLine("Presiona una tecla para continuar..");
                        Console.ReadKey();
                    }
                }
                else if (Convert.ToInt32(opcion) >= 0 && Convert.ToInt32(opcion) < archivosSubdirectorios.Length)
                {
                    int opcionEscogida = Convert.ToInt32(opcion);
                    if (Directory.Exists(archivosSubdirectorios[opcionEscogida]))
                    {
                        directorio = archivosSubdirectorios[opcionEscogida];
                    }
                    else
                    {
                        OperacionesArchivo(archivosSubdirectorios[opcionEscogida]);
                    }
                }
                else
                {
                    Console.WriteLine("ingresa un numero valido, o 'a' para regresar, o 's' para salir, o 'n' para una nueva ruta");
                }
            }

        }
        static void MostrarTabla(string[] archivosSub)
        {
            Console.WriteLine($"{"Indice",-8}{"Nombre",50}{"Tipo",-13}");
            string guion = new string('-', 71);
            Console.WriteLine(guion);
            string nombre, tipo;
            for (int i = 0; i < archivosSub.Length; i++)
            {
                nombre = Path.GetFileName(archivosSub[i]);
                if (Directory.Exists(archivosSub[i]))
                {
                    tipo = "Subdirectorio";
                }
                else
                {
                    tipo = Path.GetExtension(archivosSub[i]);
                }
                Console.WriteLine($"{i,-8}{nombre,-50}{tipo,-13}");

            }
            Console.WriteLine();
        }
        static void OperacionesArchivo(string rutaArchivo)
        {
            string rutaCopiarArchivo, rutaMoverArchivo, destinoArchivo, respuestaReemplazo, respuestaEliminar, respuestaRenombrar, nuevoNombreArchivo, rutaArchivoRenombrado;
            string nombreArchivo = Path.GetFileName(rutaArchivo);
            Console.WriteLine($"\n\nQue quieres hacer con el archivo [{nombreArchivo}]");
            Console.WriteLine("1. copiar");
            Console.WriteLine("2. mover");
            Console.WriteLine("3. eliminar");
            Console.WriteLine("4. renombrar");
            Console.WriteLine("Elige una opcion: ");
            int opcionArchivo = Convert.ToInt32(Console.ReadLine());
            switch (opcionArchivo)
            {
                case 1:
                    Console.WriteLine("Ingrese la ruta donde quieres copiar el archivo: ");
                    rutaCopiarArchivo = Console.ReadLine();
                    if (Directory.Exists(rutaCopiarArchivo))
                    {
                        destinoArchivo = Path.Combine(rutaCopiarArchivo, nombreArchivo);
                        if (!File.Exists(destinoArchivo))
                        {
                            File.Copy(rutaArchivo, destinoArchivo);
                            MensajeRealizadoConExito("copiado");
                        }
                        else
                        {

                            Console.WriteLine($"\nEl archivo {nombreArchivo} ya existe, desea reemplazarlo? (si/no)");
                            respuestaReemplazo = Console.ReadLine();
                            if (respuestaReemplazo.ToLower() == "si")
                            {
                                File.Copy(rutaArchivo, destinoArchivo, true);
                                MensajeRealizadoConExito("copiado");
                            }
                            else
                            {
                                MensajeOperacionCancelada();
                            }
                        }
                    }
                    else
                    {
                        MensajeRutaNoValida();
                    }
                    break;
                case 2:
                    Console.WriteLine($"ingresa la ruta a donde quiere mover el archivo: ");
                    rutaMoverArchivo = Console.ReadLine();
                    if (Directory.Exists(rutaMoverArchivo))
                    {
                        destinoArchivo = Path.Combine(rutaMoverArchivo, nombreArchivo);
                        if (!File.Exists(destinoArchivo))
                        {
                            File.Move(rutaArchivo, destinoArchivo);
                            MensajeRealizadoConExito("movido");
                        }
                        else
                        {
                            Console.WriteLine($"\nEl archivo {nombreArchivo} ya existe, desea reemplazarlo? (si/no)");
                            respuestaReemplazo = Console.ReadLine();
                            if (respuestaReemplazo.ToLower() == "si")
                            {
                                File.Delete(rutaArchivo);
                                File.Move(rutaArchivo, destinoArchivo);
                            }
                            else
                            {
                                MensajeRutaNoValida();
                            }
                        }
                    }
                    break;
                case 3:
                    Console.WriteLine($"\nEsta seguro de eliminar el archivo {nombreArchivo})(s/n)");
                    respuestaEliminar = Console.ReadLine();
                    if (respuestaEliminar.ToLower() == "s")
                    {
                        File.Delete(rutaArchivo);
                        MensajeRealizadoConExito("eliminado");
                    }
                    else
                    {
                        MensajeOperacionCancelada();
                    }
                    break;
                case 4:
                    Console.WriteLine("\nIngresa el nuevo nombre del arhivo (con extension): ");
                    nuevoNombreArchivo = Console.ReadLine();
                    Console.WriteLine($"El nuevo nombre {nombreArchivo} sera {nuevoNombreArchivo},es correcto? (s/n)");
                    respuestaRenombrar = Console.ReadLine();
                    if (respuestaRenombrar.ToLower() == "s")
                    {
                        rutaArchivoRenombrado = Path.Combine(Path.GetDirectoryName(rutaArchivo), nuevoNombreArchivo);
                        File.Move(rutaArchivo, rutaArchivoRenombrado);
                        MensajeRealizadoConExito("movido");
                    }
                    else if (respuestaRenombrar.ToLower() == "n")
                    {
                        MensajeOperacionCancelada();
                    }
                    break;
                default:
                    Console.WriteLine("Escoge una opcion correcta");
                    Console.WriteLine("presiona una tecla para continuar...");
                    Console.ReadKey();
                    break;
            }
        }



        static void MensajeRutaNoValida()
        {
            Console.WriteLine("\nIngresa una ruta valida");
            Console.WriteLine("Presione una tecla para continuar");
            Console.ReadKey();
        }
        static void MensajeOperacionCancelada()
        {
            Console.WriteLine("\nOperacion cancelada por el usuario");
            Console.WriteLine("Presione una tecla para continuar");
            Console.ReadKey();
        }
        static void MensajeRealizadoConExito(string tipoMovimiento)
        {
            Console.WriteLine($"\nEl archivo se ha {tipoMovimiento} con exito");
            Console.WriteLine("Presione una tecla para continuar");
            Console.ReadKey();
        }
    }
}
