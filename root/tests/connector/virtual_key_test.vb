
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.utt

Public Class virtual_key_test
    Inherits commandline_specified_case_wrapper

    Public Sub New()
        MyBase.New(New virtual_key_case())
    End Sub

    Private Class virtual_key_case
        Inherits [case]

        Public Overrides Function run() As Boolean
            While True
                Dim type As virtual_key.map_type = Nothing
                Dim code As UInt32 = 0
                Dim s As String = Nothing
                s = Console.ReadLine()
                If Not enum_def.from(s, type) Then
                    Return True
                End If

                s = Console.ReadLine()
                If Not UInt32.TryParse(s, code) Then
                    Return True
                End If

                Console.WriteLine(virtual_key.map(code, type))
            End While

            assert(False)
            Return True
        End Function
    End Class
End Class
