Imports System.IO
Imports System.Net


Module Module1
    Function DesArchivoYGuardarConNombre(url As String, nombreGuardado As String) As Boolean
        Try
            Dim webClient As New WebClient()

            ' Descarga el archivo desde la URL y guárdalo con el nombre deseado
            webClient.DownloadFile(url, nombreGuardado)
            Return True
        Catch ex As Exception
            Console.WriteLine("Error al descargar el archivo: " & ex.Message)
            Return False
        End Try
    End Function


    Sub DescargarArchivo(url As String)
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

    End Sub
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

        ' Verificar si hay más de 10 archivos en la carpeta de respaldo y eliminar el más antiguo si es necesario
        Dim archivosEnRespaldo As String() = Directory.GetFiles(rutaCarpetaRespaldo)
        If archivosEnRespaldo.Length > 10 Then
            Dim archivoMasAntiguo As String = archivosEnRespaldo.OrderBy(Function(f) File.GetCreationTime(f)).First()
            File.Delete(archivoMasAntiguo)
        End If

        Console.WriteLine("El archivo se ha movido y respaldado correctamente.")
    End Sub


    Public Function CompararVersiones(ByVal rutaArchivo1 As String, ByVal rutaArchivo2 As String) As Boolean
        If System.IO.File.Exists(rutaArchivo1) And System.IO.File.Exists(rutaArchivo2) Then
            Dim fileInfo1 As FileVersionInfo = FileVersionInfo.GetVersionInfo(rutaArchivo1)
            Dim fileInfo2 As FileVersionInfo = FileVersionInfo.GetVersionInfo(rutaArchivo2)

            Dim versionArchivo1 As Version = New Version(fileInfo1.ProductVersion)
            Dim versionArchivo2 As Version = New Version(fileInfo2.ProductVersion)

            ' Compara las versiones
            If versionArchivo1 < versionArchivo2 Then
                Return True
            Else
                Return False
            End If
        Else
            Console.WriteLine("Uno o ambos archivos no existen.")
            Return False
        End If
    End Function
End Module
