
Option Explicit On
Option Infer Off
Option Strict On

Public NotInheritable Class thread_static_implementation_of(Of T)
    Public Shared Sub register(ByVal f As Func(Of T))
        thread_static_resolver(Of Func(Of T), impl).register(f)
    End Sub

    Public Shared Sub register(ByVal i As T)
        register(Function() As T
                     Return i
                 End Function)
    End Sub

    Public Shared Function scoped_register(ByVal f As Func(Of T)) As IDisposable
        Return thread_static_resolver(Of Func(Of T), impl).scoped_register(f)
    End Function

    Public Shared Function scoped_register(ByVal i As T) As IDisposable
        Return scoped_register(Function() As T
                                   Return i
                               End Function)
    End Function

    Public Shared Function resolve() As T
        Return impl.default.resolve()
    End Function

    Public Shared Function resolve(ByRef o As T) As Boolean
        Return impl.default.resolve(o)
    End Function

    Public Shared Function resolve_or_null() As T
        Return impl.default.resolve_or_null()
    End Function

    Public Shared Function resolve_or_default(ByVal [default] As T) As T
        Return impl.default.resolve_or_default([default])
    End Function

    Private NotInheritable Class impl
        Inherits implementation_of(Of T)

        Public Shared Shadows ReadOnly [default] As impl

        Shared Sub New()
            [default] = New impl()
        End Sub

        Public Overrides Function resolver(ByRef r As Func(Of T)) As Boolean
            Return thread_static_resolver(Of Func(Of T), impl).resolve(r)
        End Function

        Public Shared Shadows Operator +(ByVal this As impl) As impl
            assert(object_compare([default], this) = 0 OrElse this Is Nothing)
            Return [default]
        End Operator

        Private Sub New()
        End Sub
    End Class

    Private Sub New()
    End Sub
End Class
