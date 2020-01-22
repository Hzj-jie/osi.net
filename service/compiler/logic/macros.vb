
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
        Private Shared ReadOnly m As map(Of String, decoder)

        Private Delegate Function decoder(ByVal a As anchors,
                                          ByVal s As scope,
                                          ByVal t As types,
                                          ByVal n As String,
                                          ByRef o As String) As Boolean

        Shared Sub New()
            m = map.of(
                    pair.emplace_of(return_type_of_name, d(AddressOf return_type)),
                    pair.emplace_of(type_of_name, d(AddressOf type)),
                    pair.emplace_of(size_of_name, d(AddressOf size))
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
            If Not n.StartsWith(prefix) Then
                o = n
                Return True
            End If
            If Not n.EndsWith(suffix) Then
                errors.unexpected_macro(n)
                Return False
            End If
            If n.TrimStart(prefix).TrimEnd(suffix).null_or_whitespace() Then
                errors.unexpected_macro(n)
                Return False
            End If
            n = n.TrimStart(prefix).TrimEnd(suffix)
            Dim i As Int32 = 0
            i = n.IndexOf(separator)
            If i = npos Then
                errors.unexpected_macro(n)
                Return False
            End If
            Dim type As String = Nothing
            Dim origin As String = Nothing
            type = n.Substring(0, i)
            origin = n.Substring(i + 1)
            If type.null_or_whitespace() OrElse origin.null_or_whitespace() Then
                errors.unexpected_macro(n)
                Return False
            End If
            Dim it As map(Of String, decoder).iterator = Nothing
            it = m.find(type)
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
            Dim i As UInt32 = 0
            If Not t.retrieve(o, i) Then
                errors.type_undefined(o, n)
                Return False
            End If
            o = Convert.ToString(i)
            Return True
        End Function

        Public Shared Function size_of(ByVal s As String) As String
            Return encode(size_of_name, s)
        End Function

        Private Shared Function encode(ByVal s As String, ByVal n As String) As String
            Return strcat(prefix, s, separator, n, suffix)
        End Function

        Private Sub New()
        End Sub
    End Class
End Namespace
