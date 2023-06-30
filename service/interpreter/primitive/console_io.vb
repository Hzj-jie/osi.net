
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.connector
Imports osi.root.formation

Namespace primitive
    Public NotInheritable Class console_io
        Private [in] As TextReader
        Private out As TextWriter
        Private err As TextWriter

        Private Function [get](Of T)(ByVal i As T, ByVal r As T) As T
            assert(Not r Is Nothing)
            Return If(i Is Nothing, r, i)
        End Function

        Public Function input() As TextReader
            Return [get]([in], Console.In())
        End Function

        Public Function output() As TextWriter
            Return [get](out, Console.Out())
        End Function

        Public Function [error]() As TextWriter
            Return [get](err, Console.Error())
        End Function

        Public Sub redirect_input(Optional ByVal i As TextReader = Nothing)
            [in] = i
        End Sub

        Public Sub redirect_output(Optional ByVal i As TextWriter = Nothing)
            out = i
        End Sub

        Public Sub redirect_error(Optional ByVal i As TextWriter = Nothing)
            err = i
        End Sub

        Public Shared Function null() As console_io
            Dim r As New console_io()
            r.redirect_output(TextWriter.Null)
            r.redirect_error(TextWriter.Null)
            Return r
        End Function

        Public NotInheritable Class test_wrapper
            Private ReadOnly out As disposer(Of StringWriter)
            Private ReadOnly err As disposer(Of StringWriter)
            Private ReadOnly [in] As disposer(Of StringReader)
            Private ReadOnly c As console_io

            Public Sub New()
                Me.New(Nothing)
            End Sub

            Public Sub New(ByVal input As String)
                out = make_disposer(New StringWriter())
                err = make_disposer(New StringWriter())
                If Not input Is Nothing Then
                    [in] = make_disposer(New StringReader(input))
                End If
                c = New console_io()
                c.redirect_output(+out)
                c.redirect_error(+err)
                If Not [in] Is Nothing Then
                    c.redirect_input(+[in])
                End If
            End Sub

            Public Shared Operator +(ByVal this As test_wrapper) As console_io
                assert(Not this Is Nothing)
                Return this.c
            End Operator

            Public Function output() As String
                Return Convert.ToString(+out)
            End Function

            Public Function [error]() As String
                Return Convert.ToString(+err)
            End Function
        End Class
    End Class
End Namespace
