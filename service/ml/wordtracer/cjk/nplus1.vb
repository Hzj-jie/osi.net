
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.utils

Partial Public NotInheritable Class wordtracer
    Partial Public NotInheritable Class cjk
        Public NotInheritable Class nplus1
            Inherits trainer

            Private ReadOnly s As shard(Of String)
            Private ReadOnly n As UInt32
            Private ReadOnly f As New onebound(Of String).trainer()
            Private ReadOnly b As New onebound(Of String).trainer()

            Public Sub New(ByVal s As shard(Of String), ByVal n As UInt32)
                MyBase.New()
                assert(Not s Is Nothing)
                assert(n > 0)
                Me.s = s
                Me.n = n
            End Sub

            Protected Overrides Sub sentence(ByVal s As String, ByVal start As UInt32, ByVal [end] As UInt32)
                If [end] - start <= n Then
                    Return
                End If

                For i As UInt32 = start To [end] - n - uint32_1
                    Dim l As String = s.strmid(i, n)
                    Dim r As String = s.strmid(i + n, 1)
                    If Not Me.s(l) Then
                        Continue For
                    End If

                    f.accumulate(l, r)
                    b.accumulate(r, l)
                Next
            End Sub
        End Class
    End Class
End Class
