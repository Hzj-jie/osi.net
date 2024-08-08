
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants

Public NotInheritable Class utt
    Public Shared ReadOnly concurrency As Int32 =
        Function() As Int32
            Dim concurrency As Int32
            If Not env_value(env_keys("utt", "concurrency"), concurrency) OrElse
               concurrency < 0 OrElse
               concurrency > Environment.ProcessorCount() Then
                Return npos
            End If
            Return concurrency
        End Function()

    Public Shared ReadOnly is_current As Boolean = application_name.Equals("osi.root.utt")

    Private Sub New()
    End Sub
End Class
