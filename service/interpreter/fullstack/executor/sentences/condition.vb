
Imports osi.root.connector

Namespace fullstack.executor
    Public Class condition
        Implements sentence

        Private ReadOnly c As expression
        Private ReadOnly tp As sentence
        Private ReadOnly fp As sentence

        Public Sub New(ByVal condition As expression,
                       Optional ByVal true_path As sentence = Nothing,
                       Optional ByVal false_path As sentence = Nothing)
            assert(Not condition Is Nothing)
            assert(Not (true_path Is Nothing AndAlso false_path Is Nothing))
            Me.c = condition
            Me.tp = true_path
            Me.fp = false_path
        End Sub

        Public Sub execute(ByVal domain As domain) Implements sentence.execute
            If c.execute(domain).true() Then
                If Not tp Is Nothing Then
                    tp.execute(domain)
                End If
            Else
                If Not fp Is Nothing Then
                    fp.execute(domain)
                End If
            End If
        End Sub
    End Class
End Namespace
