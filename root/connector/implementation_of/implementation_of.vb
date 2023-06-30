
Option Explicit On
Option Infer Off
Option Strict On

Partial Public Class implementation_of(Of T)
    Public Shared ReadOnly [default] As implementation_of(Of T) = New implementation_of(Of T)()

    Public Shared Sub register(ByVal f As Func(Of T))
        global_resolver(Of Func(Of T), implementation_of(Of T)).assert_first_register(f)
    End Sub

    Public Shared Sub register(ByVal i As T)
        register(Function() As T
                     Return i
                 End Function)
    End Sub

    Public Shared Function scoped_register(ByVal f As Func(Of T)) As IDisposable
        Return global_resolver(Of Func(Of T), implementation_of(Of T)).scoped_register(f)
    End Function

    Public Shared Function scoped_register(ByVal i As T) As IDisposable
        Return scoped_register(Function() As T
                                   Return i
                               End Function)
    End Function

    Public Overridable Function resolver(ByRef r As Func(Of T)) As Boolean
        Return global_resolver(Of Func(Of T), implementation_of(Of T)).resolve(r)
    End Function

    Public Overridable Function resolve(ByRef o As T) As Boolean
        Dim f As Func(Of T) = Nothing
        If resolver(f) Then
            assert(Not f Is Nothing)
            o = f()
            Return True
        Else
            Return False
        End If
    End Function

    Public Function resolve() As T
        Dim r As T = Nothing
        assert(resolve(r))
        Return r
    End Function

    Public Function resolve_or_null() As T
        Return resolve_or_default(Nothing)
    End Function

    Public Function resolve_or_default(ByVal [default] As T) As T
        Dim r As T = Nothing
        If resolve(r) Then
            Return r
        Else
            Return [default]
        End If
    End Function

    Public Shared Operator +(ByVal this As implementation_of(Of T)) As implementation_of(Of T)
        If this Is Nothing Then
            Return [default]
        Else
            Return this
        End If
    End Operator

    Protected Sub New()
    End Sub
End Class
