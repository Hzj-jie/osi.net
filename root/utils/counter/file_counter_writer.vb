
Option Explicit On
Option Infer Off
Option Strict On

Namespace counter
    Public NotInheritable Class file_counter_writer
        Inherits application_info_writer
        Implements icounter_writer
        Public Shared ReadOnly instance As locked_writer(Of file_counter_writer) =
            New locked_writer(Of file_counter_writer)(New file_counter_writer())

        Public Sub New(Optional ByVal counterfile As String = Nothing)
            MyBase.New(envs.deploys.counter_folder, counterfile, "counter.log")
        End Sub

        Public Sub write(ByVal s As String) Implements icounter_writer.write
            'the counter_backend_writer should add newline character already
            writer().Write(s)
        End Sub
    End Class
End Namespace
