
Imports System.IO
Imports osi.root.connector

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
    End Class
End Namespace
