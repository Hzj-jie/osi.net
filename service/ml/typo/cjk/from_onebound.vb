
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.delegates
Imports osi.root.formation
Imports osi.root.utils
Imports osi.service.resource

Partial Public NotInheritable Class typo
    Partial Public NotInheritable Class cjk
        Public NotInheritable Class from_onebound
            Private Shared threshold As argument(Of Double)
            Private ReadOnly s As shard(Of String)
            Private ReadOnly m As onebound(Of String).model

            Private Sub New(ByVal s As shard(Of String), ByVal m As onebound(Of String).model)
                assert(Not s Is Nothing)
                assert(Not m Is Nothing)
                Me.s = s
                Me.m = m
            End Sub

            Public Shared Function from_dump(ByVal index As UInt32,
                                             ByVal count As UInt32,
                                             ByVal filename As String) As from_onebound
                Dim m As onebound(Of String).model = onebound(Of String).model.load(filename)
                Return New from_onebound(New shard(Of String)(index, count), m)
            End Function

            Public Shared Function from_dump(ByVal filename As String) As from_onebound
                Return from_dump(0, 1, filename)
            End Function

            Private Sub eva_word(ByVal f As String, ByVal s As String, ByVal r As vector(Of String))
                If Not Me.s(f) Then
                    Return
                End If
                If m(f, s) < (threshold Or 0.01) Then
                    r.emplace_back(f + s)
                End If
            End Sub

            Default Public ReadOnly Property eva(ByVal reader As tar.reader) As vector(Of String)
                Get
                    Dim p As const_pair(Of String, String) = m.peak()
                    assert(Not p Is Nothing)
                    Dim fl As UInt32 = p.first.len()
                    Dim sl As UInt32 = p.second.len()
                    Dim r As New vector(Of String)()
                    ml.cjk.per_str_from(reader, Sub(ByVal s As String, ByVal a As UInt32, ByVal b As UInt32)
                                                    assert(b >= a)
                                                    If b - a < fl Then
                                                        Return
                                                    End If
                                                    Dim i As UInt32 = a
                                                    While i < b - fl
                                                        eva_word(s.strmid(i, fl), s.char_at(i + fl), r)
                                                        i += uint32_1
                                                    End While
                                                    eva_word(s.strmid(b - fl, fl), character.null, r)
                                                End Sub)
                    Return r
                End Get
            End Property
        End Class
    End Class

    Private Sub New()
    End Sub
End Class
