Imports System.IO
Imports System.Net
Imports System.Timers



Module Module1
    ' Obtener la ruta completa del directorio actual donde se encuentra el ejecutable
    Dim directorioActual As String = AppDomain.CurrentDomain.BaseDirectory




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

        ' Construir la ruta completa del archivo a descargar
        Dim rutaDescarga As String = Path.Combine(directorioActual, nombreArchivo)

        ' Descargar el archivo y guardarlo en la ruta especificada
        Using client As New WebClient()
            client.DownloadFile(url, rutaDescarga)
        End Using

        Console.WriteLine("Archivo descargado correctamente.")

    End Sub
    Public Function Mover_archivos(archivoOrigen As String) As Boolean

        Dim carpetaRespaldo As String = "Respaldo_Firmado"

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
            Return True
        End If

        Console.WriteLine("El archivo se ha movido y respaldado correctamente.")
    End Function


    Public Function CompararVersiones(ByVal rutaArchivo1 As String, ByVal rutaArchivo2 As String, nombre As String) As Boolean
        If System.IO.File.Exists(rutaArchivo1) And System.IO.File.Exists(rutaArchivo2) Then
            Dim fileInfo1 As FileVersionInfo = FileVersionInfo.GetVersionInfo(rutaArchivo1)
            Dim fileInfo2 As FileVersionInfo = FileVersionInfo.GetVersionInfo(rutaArchivo2)

            Dim versionArchivo1 As Version = New Version(fileInfo1.ProductVersion)
            Dim versionArchivo2 As Version = New Version(fileInfo2.ProductVersion)

            ' Compara las versiones
            If versionArchivo1 < versionArchivo2 Then
                almacenarLog($"Version {nombre} desactualizada ({versionArchivo1}), actualizando a version ({versionArchivo2}) ")
                Return True
            Else
                almacenarLog($"Version de {nombre} esta actualizado {versionArchivo2}")
                Return False
            End If
        Else
            almacenarLog($"Uno o ambos archivos no existen ({nombre}).")
            Return False
        End If
    End Function
    Sub almacenarLog(mensaje As String)
        Dim rutaCrpetaLogs As String = Path.Combine(directorioActual, "Logs")
        Dim nombLog As String = "Log_act_pluggin.txt"

        If Not Directory.Exists(rutaCrpetaLogs) Then
            Directory.CreateDirectory(rutaCrpetaLogs)
        End If

        Using escritor As New StreamWriter(Path.Combine(rutaCrpetaLogs, nombLog), True)
            escritor.WriteLine($"{DateTime.Now:yyyy/MM/dd-HH:mm:ss}==> {mensaje}")
        End Using ' El parámetro True indica que se añadirá al final del archivo



    End Sub

    Sub EjecutarProgramaEsperar(rutaPrograma As String)
        ' Crear un proceso para el programa externo
        Dim proceso As New Process()

        ' Configurar las propiedades del proceso
        proceso.StartInfo.FileName = rutaPrograma

        ' Iniciar el programa externo
        proceso.Start()

        ' Configurar un temporizador para esperar un tiempo máximo
        Dim temporizador As New Timer(300000) ' 300,000 milisegundos = 5 minutos
        Dim programaTerminado As Boolean = False

        ' Manejador de evento para el temporizador
        AddHandler temporizador.Elapsed, Sub(sender, e)
                                             ' Este código se ejecutará cuando el temporizador alcance el tiempo máximo
                                             ' Si el programa externo aún no ha terminado, forzar el cierre
                                             If Not programaTerminado Then
                                                 almacenarLog("El programa externo no ha terminado en 5 minutos. Forzando el cierre.")
                                                 proceso.Kill()
                                             End If

                                             ' Detener el temporizador
                                             temporizador.Stop()
                                         End Sub

        ' Iniciar el temporizador
        temporizador.Start()

        ' Esperar a que el programa externo termine
        proceso.WaitForExit()

        ' Marcar que el programa ha terminado
        programaTerminado = True

        ' Puedes realizar acciones adicionales después de que el programa externo haya terminado
        ' Por ejemplo, obtener el código de salida del proceso:
        Dim codigoSalida As Integer = proceso.ExitCode
        almacenarLog($"El programa externo ha salido con el código: {codigoSalida}")

        ' Detener el temporizador después de que el programa ha terminado
        temporizador.Stop()
    End Sub

End Module
