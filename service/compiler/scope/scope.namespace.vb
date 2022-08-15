
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

Partial Public Class scope(Of T As scope(Of T))
    Public NotInheritable Class current_namespace_t
        Private ReadOnly s As New stack(Of String)()

        Public Function define(ByVal name As String) As IDisposable
            assert(Not name.null_or_whitespace())
            s.emplace(name)
            Return defer.to(Sub()
                                assert(Not s.empty())
                                s.pop()
                            End Sub)
        End Function

        Public Function name() As String
            If s.empty() Then
                Return ""
            End If
            Return s.back()
        End Function

        Public Const namespace_separator As String = "::"

        Public Shared Function [of](ByVal i As String) As String
            Return scope(Of T).current().
                               current_namespace_opt().
                               map(Function(ByVal x As current_namespace_t) As String
                                       assert(Not x Is Nothing)
                                       Return with_namespace(x.name(), i)
                                   End Function).
                               or_else(i)
        End Function

        Public Shared Function of_namespace_and_name(ByVal i As String) As tuple(Of String, String)
            Dim f As String = [of](i)
            Dim index As Int32 = f.LastIndexOf(namespace_separator)
            assert(index <> npos)
            Return tuple.of(f.Substring(0, index), f.Substring(index + namespace_separator.Length()))
        End Function

        Public Shared Function with_global_namespace(ByVal n As String) As String
            Return with_namespace("", n)
        End Function

        Public Shared Function with_namespace(ByVal n As String, ByVal i As String) As String
            assert(Not i.null_or_whitespace())
            If i.StartsWith(namespace_separator) Then
                Return i
            End If
            Return String.Concat(n, namespace_separator, i)
        End Function
    End Class
End Class
