
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.utils

Partial Public NotInheritable Class wordtracer
    Partial Public NotInheritable Class cjk
        Public NotInheritable Class nplus1
            Inherits trainer(Of nplus1)

            Private ReadOnly s As shard(Of String)
            Private ReadOnly n As UInt32
            Private ReadOnly t As New onebound(Of String).trainer()

            Public Sub New(ByVal s As shard(Of String), ByVal n As UInt32)
                MyBase.New()
                assert(Not s Is Nothing)
                assert(n > 0)
                Me.s = s
                Me.n = n
            End Sub

            Public Sub New(ByVal n As UInt32)
                Me.New(shard(Of String).all, n)
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

                    t.accumulate(l, r)
                Next
            End Sub

            Public Function dump(ByVal percentage As Double) As onebound(Of String).model
                Return onebound(Of String).selector.exponential(t.dump(), percentage)
            End Function
        End Class
    End Class
End Class
