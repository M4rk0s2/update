Imports System.IO

Public Class Form1
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Dim nombreArchivo As String = "archivo.txt"

        ' Obtener la ruta completa del directorio actual donde se encuentra el ejecutable
        Dim directorioActual As String = AppDomain.CurrentDomain.BaseDirectory

        ' Construir la ruta completa del archivo a comprobar
        Dim rutaArchivo As String = Path.Combine(directorioActual, nombreArchivo)

        ' Comprobar si el archivo existe en la ruta especificada
        If File.Exists(rutaArchivo) Then
            Console.WriteLine("El archivo existe en la ruta: " & rutaArchivo)
            Module
        Else
            Console.WriteLine("El archivo no existe en la ruta: " & rutaArchivo)
        End If



    End Sub
End Class
