
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.delegates

<global_init(global_init_level.foundamental)>
Public NotInheritable Class type_string_serializer
    Private Shared ReadOnly to_strs As type_resolver(Of Func(Of Object, StringWriter, Boolean))
    Private Shared ReadOnly from_strs As type_resolver(Of _do_val_ref(Of StringReader, Object, Boolean))

    Shared Sub New()
        to_strs = New type_resolver(Of Func(Of Object, StringWriter, Boolean))()
        from_strs = New type_resolver(Of _do_val_ref(Of StringReader, Object, Boolean))()
        AddHandler string_serializer.to_str_registered,
                   Sub(ByVal type As Type,
                       ByVal protector As Type,
                       ByVal to_str As Func(Of Object, StringWriter, Boolean))
                       If string_serializer.is_global_protector(protector) Then
                           to_strs.assert_first_register(type, to_str)
                       End If
                   End Sub
        AddHandler string_serializer.from_str_registered,
                   Sub(ByVal type As Type,
                       ByVal protector As Type,
                       ByVal from_str As _do_val_ref(Of StringReader, Object, Boolean))
                       If string_serializer.is_global_protector(protector) Then
                           from_strs.assert_first_register(type, from_str)
                       End If
                   End Sub
    End Sub

    Public Shared Function to_str(ByVal type As Type,
                                  ByRef implemented As Boolean,
                                  ByVal i As Object,
                                  ByVal o As StringWriter) As Boolean
        Dim a As Func(Of Object, StringWriter, Boolean) = Nothing
        implemented = to_strs.from_type(type, a)
        If Not implemented Then
            Return False
        End If
        assert(Not a Is Nothing)
        Return a(i, o)
    End Function

    Public Shared Function to_str(ByVal type As Type,
                                  ByRef implemented As Boolean,
                                  ByVal i As Object,
                                  ByRef o As String) As Boolean
        Dim a As Func(Of Object, StringWriter, Boolean) = Nothing
        implemented = to_strs.from_type(type, a)
        If Not implemented Then
            Return False
        End If
        assert(Not a Is Nothing)
        Return string_serializer.forward_to_str(i, o, a)
    End Function

    Public Shared Function to_str(ByVal type As Type, ByVal i As Object, ByVal o As StringWriter) As Boolean
        Dim implemented As Boolean = False
        Dim r As Boolean = False
        r = to_str(type, implemented, i, o)
        assert(implemented)
        Return r
    End Function

    Public Shared Function to_str(ByVal type As Type, ByVal i As Object, ByRef o As String) As Boolean
        Dim implemented As Boolean = False
        Dim r As Boolean = False
        r = to_str(type, implemented, i, o)
        assert(implemented)
        Return r
    End Function

    Public Shared Function to_str(ByVal type As Type, ByVal i As Object) As String
        Dim o As String = Nothing
        assert(to_str(type, i, o))
        Return o
    End Function

    Public Shared Function to_str_or_default(ByVal type As Type,
                                             ByVal i As Object,
                                             ByVal [default] As String) As String
        Dim o As String = Nothing
        If to_str(type, i, o) Then
            Return o
        End If
        Return [default]
    End Function

    Public Shared Function to_str_or_null(ByVal type As Type, ByVal i As Object) As String
        Return to_str_or_default(type, i, Nothing)
    End Function

    Public Shared Function from_str(ByVal type As Type,
                                    ByRef implemented As Boolean,
                                    ByVal s As StringReader,
                                    ByRef o As Object) As Boolean
        Dim a As _do_val_ref(Of StringReader, Object, Boolean) = Nothing
        implemented = from_strs.from_type(type, a)
        If Not implemented Then
            Return False
        End If
        assert(Not a Is Nothing)
        Return a(s, o)
    End Function

    Public Shared Function from_str(ByVal type As Type,
                                    ByRef implemented As Boolean,
                                    ByVal s As String,
                                    ByRef o As Object) As Boolean
        Dim a As _do_val_ref(Of StringReader, Object, Boolean) = Nothing
        implemented = from_strs.from_type(type, a)
        If Not implemented Then
            Return False
        End If
        assert(Not a Is Nothing)
        Return string_serializer.forward_from_str(s, o, a)
    End Function

    Public Shared Function from_str(ByVal type As Type, ByVal s As StringReader, ByRef o As Object) As Boolean
        Dim implemented As Boolean = False
        Dim r As Boolean = False
        r = from_str(type, implemented, s, o)
        assert(implemented)
        Return r
    End Function

    Public Shared Function from_str(ByVal type As Type, ByVal s As String, ByRef o As Object) As Boolean
        Dim implemented As Boolean = False
        Dim r As Boolean = False
        r = from_str(type, implemented, s, o)
        assert(implemented)
        Return r
    End Function

    Public Shared Function from_str(ByVal type As Type, ByVal s As String) As Object
        Dim o As Object = Nothing
        assert(from_str(type, s, o))
        Return o
    End Function

    Public Shared Function from_str_or_default(ByVal type As Type,
                                               ByVal s As String,
                                               ByVal [default] As Object) As Object
        Dim o As Object = Nothing
        If from_str(type, s, o) Then
            Return o
        End If
        Return [default]
    End Function

    Public Shared Function from_str_or_null(ByVal type As Type, ByVal s As String) As Object
        Return from_str_or_default(type, s, Nothing)
    End Function

    Private Shared Sub init()
    End Sub

    Private Sub New()
    End Sub
End Class
