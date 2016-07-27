
Imports osi.root.envs
Imports osi.root.connector
Imports System.IO

Namespace counter
    Public Class file_counter_writer
        Inherits application_info_writer
        Implements icounter_writer
        Public Shared ReadOnly instance As locked_writer(Of file_counter_writer) = Nothing

        Shared Sub New()
            instance = New locked_writer(Of file_counter_writer)(New file_counter_writer())
        End Sub

        Public Sub New(Optional ByVal counterfile As String = Nothing)
            MyBase.New(counter_folder, counterfile, "counter.log")
        End Sub

        Public Sub write(ByVal s As String) Implements icounter_writer.write
            'the counter_backend_writer should add newline character already
            writer().Write(s)
        End Sub
    End Class
End Namespace
