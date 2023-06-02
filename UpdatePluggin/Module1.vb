Imports System.IO
Imports System.Net

Module Module1

    Public Function DescargarArchivo(url As String)
        ' Obtener el nombre del archivo de la URL
        Dim nombreArchivo As String = Path.GetFileName(url)

        ' Obtener la ruta completa del directorio actual donde se encuentra el ejecutable
        Dim directorioActual As String = AppDomain.CurrentDomain.BaseDirectory

        ' Construir la ruta completa del archivo a descargar
        Dim rutaDescarga As String = Path.Combine(directorioActual, nombreArchivo)

        ' Descargar el archivo y guardarlo en la ruta especificada
        Using client As New WebClient()
            client.DownloadFile(url, rutaDescarga)
        End Using

        Console.WriteLine("Archivo descargado correctamente.")

    End Function
    Sub Mover_archivos()
        Dim archivoOrigen As String = "efClientICG.exe"
        Dim carpetaRespaldo As String = "Respaldo_Firmado"

        ' Obtener la ruta completa del directorio actual donde se encuentra el ejecutable
        Dim directorioActual As String = AppDomain.CurrentDomain.BaseDirectory

        ' Construir la ruta completa del archivo de origen
        Dim rutaArchivoOrigen As String = Path.Combine(directorioActual, archivoOrigen)

        ' Construir la ruta completa de la carpeta de respaldo
        Dim rutaCarpetaRespaldo As String = Path.Combine(directorioActual, carpetaRespaldo)

        ' Crear la carpeta de respaldo si no existe
        If Not Directory.Exists(rutaCarpetaRespaldo) Then
            Directory.CreateDirectory(rutaCarpetaRespaldo)
        End If

        ' Mover el archivo a la carpeta de respaldo con la fecha actual
        Dim nombreArchivoDestino As String = $"{Path.GetFileNameWithoutExtension(archivoOrigen)}_{DateTime.Now:yyyyMMddHHmmss}{Path.GetExtension(archivoOrigen)}"
        Dim rutaArchivoDestino As String = Path.Combine(rutaCarpetaRespaldo, nombreArchivoDestino)
        File.Move(rutaArchivoOrigen, rutaArchivoDestino)

        ' Verificar si hay más de 5 archivos en la carpeta de respaldo y eliminar el más antiguo si es necesario
        Dim archivosEnRespaldo As String() = Directory.GetFiles(rutaCarpetaRespaldo)
        If archivosEnRespaldo.Length > 5 Then
            Dim archivoMasAntiguo As String = archivosEnRespaldo.OrderBy(Function(f) File.GetCreationTime(f)).First()
            File.Delete(archivoMasAntiguo)
        End If

        Console.WriteLine("El archivo se ha movido y respaldado correctamente.")
    End Sub

End Module
