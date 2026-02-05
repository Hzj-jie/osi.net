
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Collections.Generic
Imports osi.root.connector
Imports osi.root.utils
Imports osi.service.resource

Partial Public NotInheritable Class wordtracer
    Partial Public NotInheritable Class cjk
        Public MustInherit Class trainer(Of RT As trainer(Of RT))
            Private ReadOnly s As sampler

            Protected Sub New(ByVal sampler As sampler)
                assert(Not sampler Is Nothing)
                Me.s = sampler
            End Sub

            Protected Sub New(ByVal sample_rate As Double)
                Me.New(New sampler(sample_rate))
            End Sub

            Protected Sub New()
                Me.New(sampler.all)
            End Sub

            Protected MustOverride Sub sentence(ByVal s As String, ByVal start As UInt32, ByVal [end] As UInt32)

            Private Function this() As RT
                Return direct_cast(Of RT)(Me)
            End Function

            Public Function train(ByVal s As String) As RT
                assert(Not s.null_or_whitespace())
                train({s})
                Return this()
            End Function

            Private Sub _sentence(ByVal s As String, ByVal a As UInt32, ByVal b As UInt32)
                If Me.s.sampled() Then
                    sentence(s, a, b)
                End If
            End Sub

            Public Function train(ByVal ss As IEnumerable(Of String)) As RT
                ml.cjk.per_str_from(ss, AddressOf _sentence)
                Return this()
            End Function

            Public Function train(ByVal reader As tar.reader) As RT
                ml.cjk.per_str_from(reader, AddressOf _sentence)
                Return this()
            End Function
        End Class
    End Class
End Class
