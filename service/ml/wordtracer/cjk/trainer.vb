
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Collections.Generic
Imports System.IO
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.utils
Imports osi.service.resource

Partial Public NotInheritable Class wordtracer
    Partial Public NotInheritable Class cjk
        Public MustInherit Class trainer
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

            Public Sub train(ByVal s As String)
                assert(Not s.null_or_whitespace())
                train({s})
            End Sub

            Private Sub one_str(ByVal s As String)
                If s.null_or_whitespace() Then
                    Return
                End If
                s.strsep(AddressOf _character.not_cjk,
                         Sub(ByVal l As UInt32, ByVal i As UInt32)
                             If Me.s.sampled() Then
                                 sentence(s, l, i)
                             End If
                         End Sub)
            End Sub

            Public Sub train(ByVal ss As IEnumerable(Of String))
                For Each s As String In ss
                    one_str(s)
                Next
            End Sub

            Public Sub train(ByVal reader As tar.reader)
                assert(Not reader Is Nothing)
                reader.foreach(Sub(ByVal name As String, ByVal p As Double, ByVal r As StreamReader)
                                   If p < 0.8 Then
                                       raise_error(error_type.user,
                                                   "ignroe ",
                                                   name,
                                                   ", the encoding possibility is ",
                                                   p)
                                       Return
                                   End If
                                   Dim line As String = r.ReadLine()
                                   While Not line Is Nothing
                                       one_str(line)
                                       line = r.ReadLine()
                                   End While
                               End Sub)
            End Sub
        End Class
    End Class
End Class
