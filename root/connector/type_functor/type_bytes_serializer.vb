
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO

Public NotInheritable Class type_bytes_serializer
    Private Shared ReadOnly ss As type_resolver(Of bytes_serializer(Of Object)) =
        type_resolver(Of bytes_serializer(Of Object)).default

    Public Shared Function serializer(ByVal type As Type, ByRef o As bytes_serializer(Of Object)) As Boolean
        Return ss.from_type(type, o)
    End Function

    Public Shared Function serializer(ByVal type As Type) As bytes_serializer(Of Object)
        Dim r As bytes_serializer(Of Object) = Nothing
        assert(serializer(type, r))
        Return r
    End Function

    Public Shared Function append_to(ByVal type As Type,
                                     ByRef implemented As Boolean,
                                     ByVal i As Object,
                                     ByVal o As MemoryStream) As Boolean
        Dim s As bytes_serializer(Of Object) = Nothing
        implemented = serializer(type, s)
        If Not implemented Then
            Return False
        End If
        assert(Not s Is Nothing)
        Return s.append_to(i, o)
    End Function

    Public Shared Function append_to(ByVal type As Type, ByVal i As Object, ByVal o As MemoryStream) As Boolean
        Dim implemented As Boolean = False
        Dim r As Boolean = False
        r = append_to(type, implemented, i, o)
        assert(implemented)
        Return r
    End Function

    Public Shared Function write_to(ByVal type As Type,
                                    ByRef implemented As Boolean,
                                    ByVal i As Object,
                                    ByVal o As MemoryStream) As Boolean
        Dim s As bytes_serializer(Of Object) = Nothing
        implemented = serializer(type, s)
        If Not implemented Then
            Return False
        End If
        assert(Not s Is Nothing)
        Return s.write_to(i, o)
    End Function

    Public Shared Function write_to(ByVal type As Type, ByVal i As Object, ByVal o As MemoryStream) As Boolean
        Dim implemented As Boolean = False
        Dim r As Boolean = False
        r = write_to(type, implemented, i, o)
        assert(implemented)
        Return r
    End Function

    Public Shared Function consume_from(ByVal type As Type,
                                        ByRef implemented As Boolean,
                                        ByVal i As MemoryStream,
                                        ByRef o As Object) As Boolean
        Dim s As bytes_serializer(Of Object) = Nothing
        implemented = serializer(type, s)
        If Not implemented Then
            Return False
        End If
        assert(Not s Is Nothing)
        Return s.consume_from(i, o)
    End Function

    Public Shared Function consume_from(ByVal type As Type, ByVal i As MemoryStream, ByRef o As Object) As Boolean
        Dim implemented As Boolean = False
        Dim r As Boolean = False
        r = consume_from(type, implemented, i, o)
        assert(implemented)
        Return r
    End Function

    Public Shared Function read_from(ByVal type As Type,
                                     ByRef implemented As Boolean,
                                     ByVal i As MemoryStream,
                                     ByRef o As Object) As Boolean
        Dim s As bytes_serializer(Of Object) = Nothing
        implemented = serializer(type, s)
        If Not implemented Then
            Return False
        End If
        assert(Not s Is Nothing)
        Return s.read_from(i, o)
    End Function

    Public Shared Function read_from(ByVal type As Type, ByVal i As MemoryStream, ByRef o As Object) As Boolean
        Dim implemented As Boolean = False
        Dim r As Boolean = False
        r = read_from(type, implemented, i, o)
        assert(implemented)
        Return r
    End Function

    Private Sub New()
    End Sub
End Class