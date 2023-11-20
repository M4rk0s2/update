Imports System.IO

Public Class Form1
    Dim URL As String = "https://belitech.blob.core.windows.net/deployments/efClientICG/efClientICG.exe"
    Dim URLbs As String = "https://belitech.blob.core.windows.net/deployments/efClientICG/BSImportador.exe"
    Dim URLbsA As String = "https://belitech.blob.core.windows.net/deployments/efClientICG/BSAcciones.exe"

    Dim tiempoLimite As Integer = 5000

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Dim nombreArchivo As String = "efClientICG.exe"
        Dim nombreArchivoVer As String = "efClientICGver.exe"
        Dim nombreArchivoBs As String = "BSImportador.exe"
        Dim nombreArchivoBsVer As String = "BSImportadorver.exe"
        Dim nombreArchivoBsAcciones As String = "BsAcciones.exe"
        Dim nombreArchivoBsAccionesVer As String = "BsAccionesver.exe"
        Dim compVersion, compVersionBs, compVersionBsAcc As Boolean
        ' Obtener la ruta completa del directorio actual donde se encuentra el ejecutable
        Dim directorioActual As String = AppDomain.CurrentDomain.BaseDirectory

        ' Construir la ruta completa del archivo a comprobar
        Dim rutaArchivo As String = Path.Combine(directorioActual, nombreArchivo)
        Dim rutaArchivoVer As String = Path.Combine(directorioActual, nombreArchivoVer)
        Dim rutaArchivoBs As String = Path.Combine(directorioActual, nombreArchivoBs)
        Dim rutaArchiboBsVer As String = Path.Combine(directorioActual, nombreArchivoBsVer)
        Dim rutaArchiboBsAcc As String = Path.Combine(directorioActual, nombreArchivoBsAcciones)
        Dim rutaArchiboBsAccVer As String = Path.Combine(directorioActual, nombreArchivoBsAccionesVer)
        ' comparamos las verciones del  firmador 

        If DesArchivoYGuardarConNombre(URL, nombreArchivoVer) Then
            compVersion = CompararVersiones(rutaArchivo, nombreArchivoVer, "efClientICG")
            File.Delete(nombreArchivoVer)
        Else
            Console.WriteLine("La descarga falló.")
        End If

        If DesArchivoYGuardarConNombre(URLbs, nombreArchivoBsVer) Then
            compVersionBs = CompararVersiones(rutaArchivoBs, nombreArchivoBsVer, "BSImportador")
            File.Delete(nombreArchivoBsVer)
        Else
            Console.WriteLine("La descarga falló.")
        End If

        If DesArchivoYGuardarConNombre(URLbsA, nombreArchivoBsAccionesVer) Then
            compVersionBsAcc = CompararVersiones(rutaArchiboBsAcc, nombreArchivoBsAccionesVer, "BsAcciones")
            File.Delete(nombreArchivoBsAccionesVer)
        Else
            Console.WriteLine("La descarga falló.")
        End If


        ' Comprobar si el archivo existe en la ruta especificada

        If File.Exists(rutaArchivo) Then
            Console.WriteLine("El archivo existe en la ruta: " & rutaArchivo)
            If compVersion Then
                Mover_archivos(nombreArchivo)
                DescargarArchivo(URL)
            End If

        Else
            Console.WriteLine("El archivo no existe en la ruta: " & rutaArchivo)
            DescargarArchivo(URL)
        End If

        If File.Exists(rutaArchivoBs) Then
            Console.WriteLine("El archivo existe en la ruta: " & rutaArchivoBs)
            If compVersionBs Then
                Mover_archivos(nombreArchivoBs)
                DescargarArchivo(URLbs)
            End If

        Else
            Console.WriteLine("El archivo no existe en la ruta: " & rutaArchivo)
            DescargarArchivo(URLbs)
        End If



        If File.Exists(rutaArchiboBsAcc) Then
            If compVersionBsAcc Then
                Mover_archivos(nombreArchivoBsAcciones)
                DescargarArchivo(URLbsA)
                ' Llamada a la función que inicia el programa externo y espera su finalización
                EjecutarProgramaEsperar(rutaArchiboBsAcc)

                ' El código continuará aquí después de que el programa externo haya terminado
                Console.WriteLine("El programa externo ha terminado. Continuando con el código principal.")
                Console.ReadLine()
            End If
            DescargarArchivo(URLbsA)
            EjecutarProgramaEsperar(rutaArchiboBsAcc)

            ' El código continuará aquí después de que el programa externo haya terminado
            Console.WriteLine("El programa externo ha terminado. Continuando con el código principal.")
            Console.ReadLine()
        End If

        Me.Close()

    End Sub
End Class
