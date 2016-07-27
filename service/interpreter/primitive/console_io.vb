
Imports System.IO
Imports osi.root.connector

Namespace primitive
    Public Class console_io
        Private Shared [in] As TextReader
        Private Shared out As TextWriter
        Private Shared err As TextWriter

        Private Shared Function [get](Of T)(ByVal i As T, ByVal r As T) As T
            assert(Not r Is Nothing)
            Return If(i Is Nothing, r, i)
        End Function

        Public Shared Function input() As TextReader
            Return [get]([in], Console.In())
        End Function

        Public Shared Function output() As TextWriter
            Return [get](out, Console.Out())
        End Function

        Public Shared Function [error]() As TextWriter
            Return [get](err, Console.Error())
        End Function

        Public Shared Sub redirect_input(Optional ByVal i As TextReader = Nothing)
            [in] = i
        End Sub

        Public Shared Sub redirect_output(Optional ByVal i As TextWriter = Nothing)
            out = i
        End Sub

        Public Shared Sub redirect_error(Optional ByVal i As TextWriter = Nothing)
            err = i
        End Sub
    End Class
End Namespace
