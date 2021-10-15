
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

Namespace logic
    Public NotInheritable Class macros
        Private Const prefix As Char = "{"c
        Private Const suffix As Char = "}"c
        Private Const separator As Char = "-"c
        Private Const return_type_of_name As String = "return_type_of"
        Private Const type_of_name As String = "type_of"
        Private Const size_of_name As String = "size_of"
        Private Const size_of_type_of_name As String = "size_of_type"
        Private Shared ReadOnly m As unordered_map(Of String, decoder)

        Private Delegate Function decoder(ByVal a As anchors,
                                          ByVal s As scope,
                                          ByVal t As types,
                                          ByVal n As String,
                                          ByRef o As String) As Boolean

        Shared Sub New()
            m = unordered_map.emplace_of(
                    pair.emplace_of(return_type_of_name, d(AddressOf return_type)),
                    pair.emplace_of(type_of_name, d(AddressOf type)),
                    pair.emplace_of(size_of_name, d(AddressOf size)),
                    pair.emplace_of(size_of_type_of_name, d(AddressOf type_size))
                )
        End Sub

        Private Shared Function d(ByVal f As decoder) As decoder
            assert(Not f Is Nothing)
            Return f
        End Function

        Public Shared Function decode(ByVal a As anchors,
                                      ByVal s As scope,
                                      ByVal t As types,
                                      ByVal n As String,
                                      ByRef o As String) As Boolean
            assert(Not n.null_or_whitespace())
            o = n
            Dim begin As Int32 = o.LastIndexOf(prefix)
            While begin <> npos
                Dim [end] As Int32 = o.IndexOf(suffix, begin)
                If [end] = npos OrElse [end] = begin + 1 Then
                    errors.unexpected_macro(n)
                    Return False
                End If
                [end] += 1
                Dim replacement As String = Nothing
                If Not decode_one(a, s, t, o.Substring(begin, [end] - begin), replacement) Then
                    Return False
                End If
                o = strcat(o.Substring(0, begin), replacement, o.Substring([end]))
                begin = o.LastIndexOf(prefix)
            End While
            Return True
        End Function

        Private Shared Function decode_one(ByVal a As anchors,
                                           ByVal s As scope,
                                           ByVal t As types,
                                           ByVal n As String,
                                           ByRef o As String) As Boolean
            assert(Not n.null_or_whitespace())
            assert(n.StartsWith(prefix) AndAlso n.EndsWith(suffix))
            assert(n.Length() > strlen(prefix) + strlen(suffix))
            n = n.TrimStart(prefix).TrimEnd(suffix)
            assert(Not n.null_or_whitespace())
            Dim i As Int32 = n.IndexOf(separator)
            If i = npos Then
                errors.unexpected_macro(n)
                Return False
            End If
            Dim type As String = n.Substring(0, i)
            Dim origin As String = n.Substring(i + 1)
            If type.null_or_whitespace() OrElse origin.null_or_whitespace() Then
                errors.unexpected_macro(n)
                Return False
            End If
            Dim it As unordered_map(Of String, decoder).iterator = m.find(type)
            If it = m.end() Then
                errors.unknown_macro(type, origin)
                Return False
            End If
            Return (+it).second(a, s, t, origin, o)
        End Function

        Private Shared Function return_type(ByVal a As anchors,
                                            ByVal s As scope,
                                            ByVal t As types,
                                            ByVal n As String,
                                            ByRef o As String) As Boolean
            assert(Not a Is Nothing)
            assert(Not s Is Nothing)
            assert(Not t Is Nothing)
            assert(Not n.null_or_whitespace())
            If Not a.return_type_of(n, o) Then
                errors.anchor_undefined(n)
                Return False
            End If
            Return True
        End Function

        Public Shared Function return_type_of(ByVal s As String) As String
            Return encode(return_type_of_name, s)
        End Function

        Private Shared Function type(ByVal a As anchors,
                                     ByVal s As scope,
                                     ByVal t As types,
                                     ByVal n As String,
                                     ByRef o As String) As Boolean
            assert(Not a Is Nothing)
            assert(Not s Is Nothing)
            assert(Not t Is Nothing)
            assert(Not n.null_or_whitespace())
            If Not s.type(n, o) Then
                errors.variable_undefined(n)
                Return False
            End If
            Return True
        End Function

        Public Shared Function type_of(ByVal s As String) As String
            Return encode(type_of_name, s)
        End Function

        Private Shared Function size(ByVal a As anchors,
                                     ByVal s As scope,
                                     ByVal t As types,
                                     ByVal n As String,
                                     ByRef o As String) As Boolean
            assert(Not a Is Nothing)
            assert(Not s Is Nothing)
            assert(Not t Is Nothing)
            assert(Not n.null_or_whitespace())
            If Not s.type(n, o) Then
                errors.variable_undefined(n)
                Return False
            End If
            Return type_size(a, s, t, copy_no_error(o), o)
        End Function

        Public Shared Function size_of(ByVal s As String) As String
            Return encode(size_of_name, s)
        End Function

        Private Shared Function type_size(ByVal a As anchors,
                                          ByVal s As scope,
                                          ByVal t As types,
                                          ByVal n As String,
                                          ByRef o As String) As Boolean
            assert(Not a Is Nothing)
            assert(Not s Is Nothing)
            assert(Not t Is Nothing)
            assert(Not n.null_or_whitespace())
            Dim i As UInt32 = 0
            If Not t.retrieve(n, i) Then
                errors.type_undefined(n)
                Return False
            End If
            o = Convert.ToString(i)
            Return True
        End Function

        Public Shared Function size_of_type_of(ByVal s As String) As String
            Return encode(size_of_type_of_name, s)
        End Function

        Private Shared Function encode(ByVal s As String, ByVal n As String) As String
            Return strcat(prefix, s, separator, n, suffix)
        End Function

        Private Sub New()
        End Sub
    End Class
End Namespace
