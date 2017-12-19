
Option Explicit On
Option Infer Off
Option Strict On

Public Module _enum
    Private Function traversal(Of T)(ByVal d As Func(Of T, String, Boolean),
                                     ByVal count As Func(Of Int32, Boolean)) As Boolean
        If d Is Nothing Then
            Return False
        Else
            Dim ct As Type = Nothing
            ct = GetType(T)
            If ct.IsEnum() Then
                Dim values As Array = Nothing
                values = [Enum].GetValues(ct)
                If Not count Is Nothing AndAlso Not count(values.Length()) Then
                    Return True
                End If
                For Each value As T In values
                    If Not d(value, [Enum].GetName(ct, value)) Then
                        Exit For
                    End If
                Next

                Return True
            Else
                Return False
            End If
        End If
    End Function

    Public Function enum_traversal(Of T)(ByVal d As Func(Of T, String, Boolean),
                                         Optional ByVal count As Func(Of Int32, Boolean) = Nothing) As Boolean
        Return traversal(Of T)(d, count)
    End Function

    Private Function action_to_func(Of T)(ByVal d As Action(Of T, String),
                                          ByRef o As Func(Of T, String, Boolean)) As Boolean
        If d Is Nothing Then
            Return False
        Else
            o = Function(x As T, y As String) As Boolean
                    d(x, y)
                    Return True
                End Function
            Return True
        End If
    End Function

    Private Function func_to_func(Of T)(ByVal d As Func(Of T, Boolean),
                                        ByRef o As Func(Of T, String, Boolean)) As Boolean
        If d Is Nothing Then
            Return False
        Else
            o = Function(x As T, y As String) As Boolean
                    Return d(x)
                End Function
            Return True
        End If
    End Function

    Private Function action_to_func(Of T)(ByVal d As Action(Of T),
                                          ByRef o As Func(Of T, String, Boolean)) As Boolean
        If d Is Nothing Then
            Return False
        Else
            o = Function(x As T, y As String) As Boolean
                    d(x)
                    Return True
                End Function
            Return True
        End If
    End Function

    Private Function action_to_func(ByVal d As Action(Of Int32)) As Func(Of Int32, Boolean)
        Return Function(x As Int32) As Boolean
                   If Not d Is Nothing Then
                       d(x)
                   End If
                   Return True
               End Function
    End Function

    Public Function enum_traversal(Of T)(ByVal d As Action(Of T, String),
                                         Optional ByVal count As Action(Of Int32) = Nothing) As Boolean
        Dim f As Func(Of T, String, Boolean) = Nothing
        If action_to_func(d, f) Then
            Return traversal(f, action_to_func(count))
        Else
            Return False
        End If
    End Function

    Public Function enum_traversal(Of T)(ByVal d As Func(Of T, String, Boolean),
                                         ByVal count As Action(Of Int32)) As Boolean
        Return traversal(d, action_to_func(count))
    End Function

    Public Function enum_traversal(Of T)(ByVal d As Action(Of T, String),
                                         ByVal count As Func(Of Int32, Boolean)) As Boolean
        Dim f As Func(Of T, String, Boolean) = Nothing
        If action_to_func(d, f) Then
            Return traversal(f, count)
        Else
            Return False
        End If
    End Function

    Public Function enum_traversal(Of T)(ByVal d As Func(Of T, Boolean),
                                         Optional ByVal count As Func(Of Int32, Boolean) = Nothing) As Boolean
        Dim f As Func(Of T, String, Boolean) = Nothing
        If func_to_func(d, f) Then
            Return traversal(f, count)
        Else
            Return False
        End If
    End Function

    Public Function enum_traversal(Of T)(ByVal d As Func(Of T, Boolean),
                                         ByVal count As Action(Of Int32)) As Boolean
        Dim f As Func(Of T, String, Boolean) = Nothing
        If func_to_func(d, f) Then
            Return traversal(f, action_to_func(count))
        Else
            Return False
        End If
    End Function

    Public Function enum_traversal(Of T)(ByVal d As Action(Of T),
                                         Optional ByVal count As Action(Of Int32) = Nothing) As Boolean
        Dim f As Func(Of T, String, Boolean) = Nothing
        If action_to_func(d, f) Then
            Return traversal(f, action_to_func(count))
        Else
            Return False
        End If
    End Function

    Public Function enum_traversal(Of T)(ByVal d As Action(Of T),
                                         ByVal count As Func(Of Int32, Boolean)) As Boolean
        Dim f As Func(Of T, String, Boolean) = Nothing
        If action_to_func(d, f) Then
            Return traversal(f, count)
        Else
            Return False
        End If
    End Function

    Public Function enum_has(Of T, VT)(ByVal i As VT) As Boolean
        Dim r As Boolean = False
        Return traversal(Function(x As T, y As String) As Boolean
                             If compare(cast(Of VT)(x), i) = 0 Then
                                 r = True
                                 Return False
                             Else
                                 Return True
                             End If
                         End Function,
                         Nothing) AndAlso
               r
    End Function

    Public Function enum_has(Of T)(ByVal i As String) As Boolean
        Dim r As Boolean = False
        Return traversal(Function(x As T, y As String) As Boolean
                             If strsame(i, y) Then
                                 r = True
                                 Return False
                             Else
                                 Return True
                             End If
                         End Function,
                         Nothing) AndAlso
               r
    End Function

    Public Function enum_cast(Of T, VT)(ByVal i As VT, ByRef o As T) As Boolean
        If enum_has(Of T, VT)(i) Then
            o = cast(Of T)([Enum].ToObject(GetType(T), i))
            Return True
        Else
            Return False
        End If
    End Function

    Public Function enum_cast(Of T, VT)(ByVal i As VT) As T
        Dim o As T = Nothing
        assert(enum_cast(i, o))
        Return o
    End Function

    Public Function enum_cast(Of T)(ByVal i As String, ByRef o As T) As Boolean
        If enum_has(Of T)(i) Then
            o = cast(Of T)([Enum].Parse(GetType(T), i))
            Return True
        Else
            Return False
        End If
    End Function

    Public Function enum_cast(Of T)(ByVal i As String) As T
        Dim o As T = Nothing
        assert(enum_cast(i, o))
        Return o
    End Function

    ' Do not use it, use [Enum].GetNames() instead: see enum_to_string_perf.
    Public Function enum_to_string(Of T)(ByRef o() As String) As Boolean
        Dim i As Int32 = 0
        Dim r() As String = Nothing
        If enum_traversal(Sub(ByVal x As T, ByVal s As String)
                              r(i) = s
                              i += 1
                          End Sub,
                          Sub(ByVal c As Int32)
                              ReDim r(c - 1)
                          End Sub) Then
            o = r
            Return True
        Else
            Return False
        End If
    End Function

    Public Function enum_to_string(Of T)() As String()
        Dim r() As String = Nothing
        assert(enum_to_string(Of T)(r))
        Return r
    End Function
End Module
