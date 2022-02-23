
Option Explicit On
Option Infer Off
Option Strict On

' Auto-infer is error-prone.
' E.g. implenentation_of.from_instance(New impl()) != implementation_of(Of inte).from_instance(New impl())
Partial Public Class implementation_of(Of T)
    Public Shared Function from_instance(ByVal i As T) As implementation_of(Of T)
        Return New from_i_impl(i)
    End Function

    Public Shared Function from_constructor(ByVal f As Func(Of T)) As implementation_of(Of T)
        Return New from_f_impl(f)
    End Function

    Private Class from_f_impl
        Inherits implementation_of(Of T)

        Private ReadOnly f As Func(Of T)

        Public Sub New(ByVal f As Func(Of T))
            assert(f IsNot Nothing)
            Me.f = f
        End Sub

        Public Overrides Function resolve(ByRef o As T) As Boolean
            o = f()
            Return True
        End Function
    End Class

    Private Class from_i_impl
        Inherits implementation_of(Of T)

        Private ReadOnly i As T

        Public Sub New(ByVal i As T)
            Me.i = i
        End Sub

        Public Overrides Function resolve(ByRef o As T) As Boolean
            o = i
            Return True
        End Function
    End Class
End Class
