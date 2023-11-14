Imports System.IO

Public Class Form1
    Dim URL As String = "https://belitech.blob.core.windows.net/deployments/efClientICG/efClientICG.exe"
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Dim nombreArchivo As String = "efClientICG.exe"
        Dim nombrArchivoVer As String = "efClientICGver.exe"
        Dim compVersion As Boolean
        ' Obtener la ruta completa del directorio actual donde se encuentra el ejecutable
        Dim directorioActual As String = AppDomain.CurrentDomain.BaseDirectory

        ' Construir la ruta completa del archivo a comprobar
        Dim rutaArchivo As String = Path.Combine(directorioActual, nombreArchivo)
        Dim rutaArchivoVer As String = Path.Combine(directorioActual, nombrArchivoVer)
        ' comparamos las verciones 

        If DesArchivoYGuardarConNombre(URL, nombrArchivoVer) Then
            compVersion = CompararVersiones(rutaArchivo, nombrArchivoVer)
            File.Delete(nombrArchivoVer)
        Else
            Console.WriteLine("La descarga falló.")
        End If

        ' Comprobar si el archivo existe en la ruta especificada

        If File.Exists(rutaArchivo) Then
            Console.WriteLine("El archivo existe en la ruta: " & rutaArchivo)
            If compVersion Then
                Mover_archivos()
                DescargarArchivo(URL)
            End If

        Else
            Console.WriteLine("El archivo no existe en la ruta: " & rutaArchivo)
            DescargarArchivo(URL)
        End If

        Me.Close()

    End Sub
End Class
