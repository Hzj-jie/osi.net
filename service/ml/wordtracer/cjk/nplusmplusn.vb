
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.utils

Partial Public NotInheritable Class wordtracer
    Partial Public NotInheritable Class cjk
        Public NotInheritable Class nplusmplusn
            Inherits trainer(Of nplusmplusn)

            Private ReadOnly s As shard(Of String)
            Private ReadOnly n As UInt32
            Private ReadOnly m As UInt32
            Private ReadOnly f As New onebound(Of String).trainer()
            Private ReadOnly b As New onebound(Of String).trainer()

            Public Sub New(ByVal s As shard(Of String), ByVal n As UInt32, ByVal m As UInt32)
                MyBase.New()
                assert(Not s Is Nothing)
                assert(n > 0)
                assert(m > 0)
                Me.s = s
                Me.n = n
                Me.m = m
            End Sub

            Public Sub New(ByVal n As UInt32, ByVal m As UInt32)
                Me.New(shard(Of String).all, n, m)
            End Sub

            Protected Overrides Sub sentence(ByVal s As String, ByVal start As UInt32, ByVal [end] As UInt32)
                If [end] - start < n + m Then
                    Return
                End If

                For i As UInt32 = start To [end] - n - m
                    Dim l As String = s.strmid(i, n)
                    Dim r As String = s.strmid(i + n, m)
                    If Not Me.s(l) Then
                        Continue For
                    End If

                    f.accumulate(l, r)
                    b.accumulate(r, l)
                Next
            End Sub

            Public Function dump(ByVal percent As Double) As onebound(Of String).model
                Return onebound(Of String).model.bidirectional(f.dump().normalize().filter(percent),
                                                               b.dump().normalize().filter(percent))
            End Function
        End Class
    End Class
End Class
