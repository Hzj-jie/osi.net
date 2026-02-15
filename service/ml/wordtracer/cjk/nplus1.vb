
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

            Private Sub accumulate(ByVal l As String, ByVal r As String)
                If s(l) Then
                    t.accumulate(l, r)
                End If
            End Sub

            Protected Overrides Sub sentence(ByVal s As String, ByVal start As UInt32, ByVal [end] As UInt32)
                assert([end] >= start)
                If [end] - start < n Then
                    Return
                End If

                Dim i As UInt32 = start
                While i < [end] - n
                    accumulate(s.strmid(i, n), s.char_at(i + n))
                    i += uint32_1
                End While

                accumulate(s.strmid([end] - n, n), character.null)
            End Sub

            Public Function dump(ByVal percentage As Double) As onebound(Of String).model
                Return onebound(Of String).selector.exponential(t.dump(), percentage)
            End Function

            Public Function dump_raw(ByVal percent As Double) As onebound(Of String).model
                Return t.dump().normalize().filter(percent)
            End Function
        End Class
    End Class
End Class
