
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Collections.Generic
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.service.resource

Partial Public NotInheritable Class wordtracer
    Partial Public NotInheritable Class cjk
        Public NotInheritable Class oneplus
            Inherits trainer
            Private ReadOnly m As unordered_map(Of String, Double)
            Private ReadOnly l As UInt32
            Private ReadOnly t As onebound(Of String).trainer

            Public Sub New(ByVal m As unordered_map(Of String, Double), ByVal sample_rate As Double)
                MyBase.New(sample_rate)
                assert(Not m Is Nothing)
                assert(Not m.empty())
                Me.m = m
                Me.l = (+(m.begin())).first.strlen()
                Me.t = New onebound(Of String).trainer()
            End Sub

            Public Sub New(ByVal m As onebound(Of Char).model, ByVal sample_rate As Double)
                Me.New(m.to_map(Of String)(), sample_rate)
            End Sub

            Public Sub New(ByVal m As onebound(Of String).model, ByVal sample_rate As Double)
                Me.New(m.to_map(Of String)(), sample_rate)
            End Sub

            Public Sub New(ByVal m As unordered_map(Of String, Double))
                Me.New(m, 1.0)
            End Sub

            Public Sub New(ByVal m As onebound(Of Char).model)
                Me.New(m.to_map(Of String)())
            End Sub

            Public Sub New(ByVal m As onebound(Of String).model)
                Me.New(m.to_map(Of String)())
            End Sub

            Protected Overrides Sub sentence(ByVal s As String, ByVal start As UInt32, ByVal [end] As UInt32)
                assert([end] >= start)
                If [end] - start <= l Then
                    Return
                End If
                For i As UInt32 = start To [end] - l - uint32_1
                    Dim it As unordered_map(Of String, Double).iterator = m.find(s.strmid(i, l))
                    If it = m.end() Then
                        Continue For
                    End If
                    If m.find(s.strmid(i + uint32_1, l)) = m.end() Then
                        Continue For
                    End If
                    t.accumulate(s.strmid(i, l), s.strmid(i + l, uint32_1), (+it).second)
                Next
            End Sub

            Public Shadows Function train(ByVal s As String) As onebound(Of String).model
                MyBase.train(s)
                Return t.dump()
            End Function

            Public Shadows Function train(ByVal ss As IEnumerable(Of String)) As onebound(Of String).model
                MyBase.train(ss)
                Return t.dump()
            End Function

            Public Shadows Function train(ByVal reader As tar.reader) As onebound(Of String).model
                MyBase.train(reader)
                Return t.dump()
            End Function
        End Class
    End Class
End Class
