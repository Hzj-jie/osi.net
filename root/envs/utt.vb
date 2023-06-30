
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
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

    Public Shared ReadOnly file_pattern As String =
        Function() As String
            Dim file_pattern As String = Nothing
            If Not env_value(env_keys("utt", "file", "pattern"), file_pattern) OrElse
               file_pattern.null_or_whitespace() Then
                Return "osi.*.dll"
            End If
            Return file_pattern
        End Function()

    Public Shared ReadOnly is_current As Boolean = strsame(application_name, "osi.root.utt")

    Private Sub New()
    End Sub
End Class
