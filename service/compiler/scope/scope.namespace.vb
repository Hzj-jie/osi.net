
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.delegates
Imports osi.root.formation

Partial Public Class scope(Of WRITER As {lazy_list_writer, New},
                              __BUILDER As func_t(Of String, WRITER, Boolean),
                              __CODE_GENS As func_t(Of code_gens(Of WRITER)),
                              T As scope(Of WRITER, __BUILDER, __CODE_GENS, T))
    Public NotInheritable Class current_namespace_t
        Private ReadOnly s As New stack(Of String)()

        Public Function define(ByVal name As String) As IDisposable
            assert(Not name.null_or_whitespace())
            assert(current().features().with_namespace())
            s.emplace(name)
            Return defer.to(Sub()
                                assert(Not s.empty())
                                s.pop()
                            End Sub)
        End Function

        Public Const namespace_separator As String = "::"

        Public Shared Function [of](ByVal i As String) As String
            assert(Not i.null_or_whitespace())
            If Not current().features().with_namespace() Then
                Return i
            End If
            Dim s As stack(Of String) = current().current_namespace().s
            assert(Not s Is Nothing)
            If s.empty() Then
                Return fully_qualified_name(i)
            End If
            Return fully_qualified_name(with_namespace(s.back(), i))
        End Function

        Public Shared Function of_namespace_and_name(ByVal i As String) As tuple(Of String, String)
            assert(current().features().with_namespace())
            Dim f As String = [of](i)
            Dim index As Int32 = f.LastIndexOf(namespace_separator)
            If index = npos Then
                Return tuple.of("", f)
            End If
            Return tuple.of(f.Substring(0, index), f.Substring(index + namespace_separator.Length()))
        End Function

        Public Shared Function fully_qualified_name(ByVal n As String) As String
            ' This function is allowed to be nested-ly called without impacting the results.
            ' I.e. full_qualified_name(full_qualified_name(x)) = full_qualified_name(x)
            Return with_namespace(Nothing, n)
        End Function

        Public Shared Function with_namespace(ByVal n As String, ByVal i As String) As String
            assert(current().features().with_namespace())
            assert(Not i.null_or_whitespace())
            assert(n.null_or_empty() OrElse Not n.null_or_whitespace())
            If i.StartsWith(namespace_separator) Then
                Return i
            End If
            Return String.Concat(n, namespace_separator, i)
        End Function
    End Class
End Class
