
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.delegates

' A functor to implement T to string and string to T operations.
Partial Public Class string_serializer(Of T, PROTECTOR)
    Public Shared ReadOnly [default] As string_serializer(Of T, PROTECTOR)

    Shared Sub New()
        ' To allow T to register its own serializer in the static constructor.
        static_constructor(Of T).execute()
        [default] = New string_serializer(Of T, PROTECTOR)()
    End Sub

    Private Shared Function is_string() As Boolean
        Return type_info(Of T, type_info_operators.equal, String).v
    End Function

    Private Shared Sub register_to_str(Of P)()
        type_resolver(Of Func(Of Object, StringWriter, Boolean), P).register(
            GetType(T),
            Function(ByVal i As Object, ByVal o As StringWriter) As Boolean
                Return string_serializer.to_str(direct_cast(Of T)(i), o)
            End Function)
    End Sub

    Private Shared Sub register_from_str(Of P)()
        type_resolver(Of _do_val_ref(Of StringReader, Object, Boolean), P).register(
            GetType(T),
            Function(ByVal i As StringReader, ByRef o As Object) As Boolean
                Dim r As T = Nothing
                If Not string_serializer.from_str(i, r) Then
                    Return False
                End If
                o = r
                Return True
            End Function)
    End Sub

    Public Shared Sub register(ByVal to_str As Action(Of T, StringWriter))
        global_resolver(Of Action(Of T, StringWriter), PROTECTOR).assert_first_register(to_str)
        assert(Not to_str Is Nothing)
        If string_serializer.protector(Of PROTECTOR).is_global Then
            register_to_str(Of string_serializer)()
        ElseIf string_serializer.protector(Of PROTECTOR).is_json Then
            register_to_str(Of json_serializer)()
        ElseIf string_serializer.protector(Of PROTECTOR).is_uri Then
            register_to_str(Of uri_serializer)()
        End If
    End Sub

    Public Shared Sub register(ByVal from_str As _do_val_ref(Of StringReader, T, Boolean))
        global_resolver(Of _do_val_ref(Of StringReader, T, Boolean), PROTECTOR).assert_first_register(from_str)
        assert(Not from_str Is Nothing)
        If string_serializer.protector(Of PROTECTOR).is_global Then
            register_from_str(Of string_serializer)()
        ElseIf string_serializer.protector(Of PROTECTOR).is_json Then
            register_from_str(Of json_serializer)()
        ElseIf string_serializer.protector(Of PROTECTOR).is_uri Then
            register_from_str(Of uri_serializer)()
        End If
    End Sub

    Protected Overridable Function to_str() As Action(Of T, StringWriter)
        Return global_resolver(Of Action(Of T, StringWriter), PROTECTOR).resolve_or_null()
    End Function

    ' Write i into StringWriter
    Public Function to_str(ByVal i As T, ByVal o As StringWriter) As Boolean
        If o Is Nothing Then
            Return False
        End If

        If is_string() Then
            o.Write(direct_cast(Of String)(i))
            Return True
        End If

        Dim f As Action(Of T, StringWriter) = Nothing
        f = to_str()
        If Not f Is Nothing Then
            f(i, o)
            Return True
        End If

        Dim f2 As Action(Of T, StringWriter) = Nothing
        f2 = uri_serializer(Of T).default.to_str()
        assert(object_compare(f, f2) <> 0)
        assert(Not f2 Is Nothing)
        f2(i, o)
        Return True
    End Function

    Protected Overridable Function from_str() As _do_val_ref(Of StringReader, T, Boolean)
        Return global_resolver(Of _do_val_ref(Of StringReader, T, Boolean), PROTECTOR).resolve_or_null()
    End Function

    Private Function do_from_str(ByVal i As StringReader, ByRef o As T) As Boolean
        assert(Not i Is Nothing)

        If is_string() Then
            o = direct_cast(Of T)(i.ReadToEnd())
            Return True
        End If

        Dim f As _do_val_ref(Of StringReader, T, Boolean) = Nothing
        f = from_str()
        If Not f Is Nothing Then
            Return f(i, o)
        End If

        Dim f2 As _do_val_ref(Of StringReader, T, Boolean) = Nothing
        f2 = uri_serializer(Of T).default.from_str()
        assert(object_compare(f, f2) <> 0)
        assert(Not f2 Is Nothing)
        Return f2(i, o)
    End Function

    ' Read enough characters from i and construct o.
    Public Function from_str(ByVal i As StringReader, ByRef o As T) As Boolean
        If i Is Nothing Then
            Return False
        End If

        Dim p As UInt32 = 0
        p = i.position()
        If do_from_str(i, o) Then
            Return True
        End If
        i.position(p)
        Return False
    End Function

    Public Shared Operator +(ByVal this As string_serializer(Of T, PROTECTOR)) As string_serializer(Of T, PROTECTOR)
        If this Is Nothing Then
            Return [default]
        End If
        Return this
    End Operator

    Protected Sub New()
    End Sub
End Class

Partial Public NotInheritable Class string_serializer
    Public NotInheritable Class protector(Of T)
        Public Shared ReadOnly is_global As Boolean
        Public Shared ReadOnly is_json As Boolean
        Public Shared ReadOnly is_uri As Boolean

        Shared Sub New()
            is_global = GetType(T).generic_type_is(GetType(string_serializer(Of )))
            is_json = GetType(T).generic_type_is(GetType(json_serializer(Of )))
            is_uri = GetType(T).generic_type_is(GetType(uri_serializer(Of )))
        End Sub

        Private Sub New()
        End Sub
    End Class

    Public Shared Sub register(Of T)(ByVal to_str As Action(Of T, StringWriter))
        string_serializer(Of T).register(to_str)
    End Sub

    Public Shared Sub register(Of T)(ByVal from_str As _do_val_ref(Of StringReader, T, Boolean))
        string_serializer(Of T).register(from_str)
    End Sub

    Public Shared Function from_str(Of T)(ByVal i As StringReader, ByRef o As T) As Boolean
        Return string_serializer(Of T).default.from_str(i, o)
    End Function

    Public Shared Function from_str(Of T)(ByVal i As String, ByRef o As T) As Boolean
        Return string_serializer(Of T).default.from_str(i, o)
    End Function

    Public Shared Function from_str(Of T)(ByVal i As String) As T
        Return string_serializer(Of T).default.from_str(i)
    End Function

    Public Shared Function from_str_or_default(Of T)(ByVal i As String, ByVal [default] As T) As T
        Return string_serializer(Of T).default.from_str_or_default(i, [default])
    End Function

    Public Shared Function to_str(Of T)(ByVal i As T, ByVal o As StringWriter) As Boolean
        Return string_serializer(Of T).default.to_str(i, o)
    End Function

    Public Shared Function to_str(Of T)(ByVal i As T, ByRef o As String) As Boolean
        Return string_serializer(Of T).default.to_str(i, o)
    End Function

    Public Shared Function to_str(Of T)(ByVal i As T) As String
        Return string_serializer(Of T).default.to_str(i)
    End Function

    Public Shared Function to_str_or_default(Of T)(ByVal i As T, ByVal [default] As String) As String
        Return string_serializer(Of T).default.to_str_or_default(i, [default])
    End Function

    Public Shared Function to_str_or_null(Of T)(ByVal i As T) As String
        Return string_serializer(Of T).default.to_str_or_null(i)
    End Function

    Private Sub New()
    End Sub
End Class

' Raw string operations without type information or conversions. I.e. Int 100 from or to String "100".
' This serializer works for simple types only, complex structures should support bytes_serializer or json_serializer.
Public Class string_serializer(Of T)
    Inherits string_serializer(Of T, string_serializer(Of T))

    Public Shared Shadows ReadOnly [default] As string_serializer(Of T)

    Shared Sub New()
        [default] = New string_serializer(Of T)()
    End Sub

    Public Shared Shadows Operator +(ByVal this As string_serializer(Of T)) As string_serializer(Of T)
        If this Is Nothing Then
            Return [default]
        End If
        Return this
    End Operator

    Protected Sub New()
    End Sub
End Class
