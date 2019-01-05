
Option Explicit On
Option Infer Off
Option Strict On

' Use instance to allow extension methods in other projects.
Public NotInheritable Class enum_definition(Of T)
    Public Shared ReadOnly instance As enum_definition(Of T)
    Private Shared ReadOnly type As Type
    Private Shared ReadOnly _underlying_type As Type
    Private Shared ReadOnly values As Array

    Shared Sub New()
        assert(type_info(Of T).is_enum)
        instance = New enum_definition(Of T)()
        type = GetType(T)
        _underlying_type = type.GetEnumUnderlyingType()
        values = [Enum].GetValues(type)
        assert(Not values Is Nothing AndAlso values.Length() >= 0)
    End Sub

    Public Function count() As UInt32
        Return CUInt(count_i())
    End Function

    Public Function count_i() As Int32
        Return values.Length()
    End Function

    Public Function underlying_type() As Type
        Return _underlying_type
    End Function

    Public Function foreach(ByVal d As Func(Of T, String, Boolean)) As Boolean
        If d Is Nothing Then
            Return False
        End If
        For Each value As T In values
            If Not d(value, [Enum].GetName(type, value)) Then
                Exit For
            End If
        Next

        Return True
    End Function

    Public Function foreach(ByVal d As Action(Of T, String)) As Boolean
        If d Is Nothing Then
            Return False
        End If
        Return foreach(Function(ByVal i As T, ByVal s As String) As Boolean
                           d(i, s)
                           Return True
                       End Function)
    End Function

    Public Function foreach(ByVal d As Func(Of T, Boolean)) As Boolean
        If d Is Nothing Then
            Return False
        End If
        Return foreach(Function(ByVal i As T, ByVal s As String) As Boolean
                           Return d(i)
                       End Function)
    End Function

    Public Function foreach(ByVal d As Action(Of T)) As Boolean
        If d Is Nothing Then
            Return False
        End If
        Return foreach(Function(ByVal i As T) As Boolean
                           d(i)
                           Return True
                       End Function)
    End Function

    Public Function has(Of VT)(ByVal i As VT) As Boolean
        Dim r As Boolean = False
        assert(foreach(Function(ByVal x As T, ByVal y As String) As Boolean
                           If compare(cast_from(x).to(Of VT)(), i) = 0 Then
                               r = True
                               Return False
                           End If
                           Return True
                       End Function))
        Return r
    End Function

    Public Function has(ByVal i As String) As Boolean
        Dim r As Boolean = False
        assert(foreach(Function(ByVal x As T, ByVal y As String) As Boolean
                           If strsame(y, i) Then
                               r = True
                               Return False
                           End If
                           Return True
                       End Function))
        Return r
    End Function

    Public Function cast(Of VT)(ByVal i As VT, ByRef o As T) As Boolean
        If Not has(i) Then
            Return False
        End If
        o = cast_from([Enum].ToObject(type, i)).to(Of T)()
        Return True
    End Function

    Public Function cast(Of VT)(ByVal i As VT) As T
        Dim o As T = Nothing
        assert(cast(i, o))
        Return o
    End Function

    Public Function cast(ByVal i As String, ByRef o As T) As Boolean
        If Not has(i) Then
            Return False
        End If
        o = cast_from([Enum].Parse(type, i)).to(Of T)()
        Return True
    End Function

    Public Function cast(ByVal i As String) As T
        Dim o As T = Nothing
        assert(cast(i, o))
        Return o
    End Function

    Private Sub New()
    End Sub
End Class

' C#
Public NotInheritable Class enum_def
    Public Shared Function [of](Of T)() As enum_definition(Of T)
        Return enum_def(Of T)()
    End Function

    Public Shared Function cast(Of T, VT)(ByVal i As VT, ByRef o As T) As Boolean
        Return [of](Of T)().cast(i, o)
    End Function

    Public Shared Function cast(Of T)(ByVal i As String, ByRef o As T) As Boolean
        Return [of](Of T)().cast(i, o)
    End Function

    Private Sub New()
    End Sub
End Class

' VB.net
Public Module _enum_def
    Public Function enum_def(Of T)() As enum_definition(Of T)
        Return enum_definition(Of T).instance
    End Function
End Module