
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

    Private NotInheritable Class object_register(Of P)
        Shared Sub New()
            type_resolver(Of string_serializer(Of Object), P).assert_first_register(
                GetType(T),
                New string_serializer_object(Of T, PROTECTOR)([default]))
        End Sub

        Public Shared Sub init()
        End Sub

        Private Sub New()
        End Sub
    End Class

    Private Shared Sub register_object()
        If string_serializer.protector(Of PROTECTOR).is_global Then
            object_register(Of string_serializer).init()
        ElseIf string_serializer.protector(Of PROTECTOR).is_json Then
            object_register(Of json_serializer).init()
        ElseIf string_serializer.protector(Of PROTECTOR).is_uri Then
            object_register(Of uri_serializer).init()
        End If
    End Sub

    Public Shared Sub register(ByVal to_str As Action(Of T, StringWriter))
        global_resolver(Of Action(Of T, StringWriter), PROTECTOR).assert_first_register(to_str)
        register_object()
    End Sub

    Public Shared Sub register(ByVal from_str As _do_val_ref(Of StringReader, T, Boolean))
        global_resolver(Of _do_val_ref(Of StringReader, T, Boolean), PROTECTOR).assert_first_register(from_str)
        register_object()
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
