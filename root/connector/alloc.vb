
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices

#Const cached_alloc = True

Public Module _alloc
    Public Function use_cached_alloc() As Boolean
#If cached_alloc Then
        Return True
#Else
        Return False
#End If
    End Function

    Public Function alloc(Of T)() As T
#If cached_alloc Then
        assert(Not type_info(Of T).dominated_constructor() Is Nothing)
        Return type_info(Of T).dominated_constructor()()
#Else
        Try
            'T As New
            Return Activator.CreateInstance(Of T)()
        Catch
            'there is not a constructor without parameters
            Return GetType(T).allocate()
        End Try
#End If
    End Function

    ' Provides a shortcut to write
    ' Dim x As a_very_long_type(Of some_parameter_1, some_parameter_2) = Nothing
    ' x = _new(x)
    ' Or even,
    ' _new(x)
    Public Function _new(Of T As New)(ByRef i As T) As T
        i = New T()
        Return i
    End Function

    <Extension()> Public Function [New](Of T As New)(ByRef i As T) As T
        Return _new(i)
    End Function
End Module
