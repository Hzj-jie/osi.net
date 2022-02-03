
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.utils
Imports osi.root.utt
Imports osi.service.resource

Public MustInherit Class b2style_self_test_runner
    Private ReadOnly filter As String
    Private ReadOnly data() As Byte

    Protected Sub New(ByVal filter As String, ByVal data() As Byte)
        assert(Not filter Is Nothing)
        assert(Not data.null_or_empty())
        Me.filter = filter
        Me.data = data
    End Sub

    Protected Sub run()
        Dim a As New vector(Of Action)()
        tar.gen.reader_of(data).foreach(
            Sub(ByVal name As String,
                ByVal enc_precision As Double,
                ByVal content As StreamReader)
                Dim text As String = content.ReadToEnd()
                a.emplace_back(Sub()
                                   If Not name.match_pattern(filter) AndAlso
                                      Not name.match_pattern((filter) + ".txt") Then
                                       raise_error(error_type.user, "Ignore test case ", name)
                                       Return
                                   End If
                                   raise_error(error_type.user, "Execute test case ", name)
                                   execute(name, text)
                               End Sub)
            End Sub)
        assertions.of(a).not_empty()
        concurrency_runner.execute(+a)
    End Sub

    Protected MustOverride Sub execute(ByVal name As String, ByVal text As String)
End Class
