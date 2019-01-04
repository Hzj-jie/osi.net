﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO

Public NotInheritable Class type_string_serializer
    Private Shared ReadOnly ss As type_resolver(Of string_serializer(Of Object))

    Shared Sub New()
        ss = type_resolver(Of string_serializer(Of Object), string_serializer).r
    End Sub

    Public Shared Function serializer(ByVal type As Type, ByRef o As string_serializer(Of Object)) As Boolean
        Return ss.from_type(type, o)
    End Function

    Public Shared Function serializer(ByVal type As Type) As string_serializer(Of Object)
        Dim r As string_serializer(Of Object) = Nothing
        assert(serializer(type, r))
        Return r
    End Function

    Public Shared Function to_str(ByVal type As Type,
                                  ByRef implemented As Boolean,
                                  ByVal i As Object,
                                  ByVal o As StringWriter) As Boolean
        Dim s As string_serializer(Of Object) = Nothing
        implemented = serializer(type, s)
        If Not implemented Then
            Return False
        End If
        assert(Not s Is Nothing)
        Return s.to_str(i, o)
    End Function

    Public Shared Function to_str(ByVal type As Type,
                                  ByRef implemented As Boolean,
                                  ByVal i As Object,
                                  ByRef o As String) As Boolean
        Dim s As string_serializer(Of Object) = Nothing
        implemented = serializer(type, s)
        If Not implemented Then
            Return False
        End If
        assert(Not s Is Nothing)
        Return s.to_str(i, o)
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
                                    ByVal str As StringReader,
                                    ByRef o As Object) As Boolean
        Dim s As string_serializer(Of Object) = Nothing
        implemented = ss.from_type(type, s)
        If Not implemented Then
            Return False
        End If
        assert(Not s Is Nothing)
        Return s.from_str(str, o)
    End Function

    Public Shared Function from_str(ByVal type As Type,
                                    ByRef implemented As Boolean,
                                    ByVal str As String,
                                    ByRef o As Object) As Boolean
        Dim s As string_serializer(Of Object) = Nothing
        implemented = ss.from_type(type, s)
        If Not implemented Then
            Return False
        End If
        assert(Not s Is Nothing)
        Return s.from_str(str, o)
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

    Private Sub New()
    End Sub
End Class