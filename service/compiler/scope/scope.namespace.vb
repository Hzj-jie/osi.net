
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
        Private ReadOnly s As New vector(Of String)()

        Public Function define(ByVal name As String) As IDisposable
            assert(Not name.null_or_whitespace())
            assert(current().features().with_namespace())
            s.emplace_back(name)
            Return defer.to(Sub()
                                assert(Not s.empty())
                                s.pop_back()
                            End Sub)
        End Function

        Public Const namespace_separator As String = "::"

        Public Shared Function full_namespace() As String
            If Not current().features().with_namespace() Then
                Return Nothing
            End If
            Dim s As vector(Of String) = current().current_namespace().s
            assert(Not s Is Nothing)
            Return assert_fully_qualified_name(String.Concat(namespace_separator, s.str(namespace_separator)))
        End Function

        Public Shared Function [of](ByVal i As String) As String
            Return [of](full_namespace(), i)
        End Function

        Public Shared Function [of](ByVal n As String, ByVal i As String) As String
            assert(Not i.null_or_whitespace())
            assert(n Is Nothing OrElse n.StartsWith(namespace_separator))
            If n Is Nothing OrElse i.StartsWith(namespace_separator) Then
                Return i
            End If
            Return assert_fully_qualified_name(String.Concat(n, namespace_separator, i))
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

        Private Shared Function assert_fully_qualified_name(ByVal s As String) As String
            assert(Not s.null_or_whitespace())
            assert(String.Equals(s, fully_qualified_name(s)))
            Return s
        End Function

        ' This function is allowed to be nested-ly called without impacting the results.
        ' I.e. fully_qualified_name(fully_qualified_name(x)) = fully_qualified_name(x)
        Public Shared Function fully_qualified_name(ByVal n As String) As String
            assert(current().features().with_namespace())
            assert(Not n.null_or_whitespace())
            If n.StartsWith(namespace_separator) Then
                Return n
            End If
            Return String.Concat(namespace_separator, n)
        End Function

        Public Shared Function fully_qualified_name(ByVal n As String, ByVal i As String) As String
            assert(current().features().with_namespace())
            assert(Not i.null_or_whitespace())
            assert(n.null_or_empty() OrElse Not n.null_or_whitespace())
            If i.StartsWith(namespace_separator) Then
                Return i
            End If
            Return fully_qualified_name(String.Concat(n, namespace_separator, i))
        End Function
    End Class
End Class
