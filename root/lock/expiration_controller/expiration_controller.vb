
Imports osi.root.connector

Partial Public MustInherit Class expiration_controller
    Public MustOverride Function expired() As Boolean

    Public Function alive() As Boolean
        Return Not expired()
    End Function

    Public Function stopping_signal() As Func(Of Boolean)
        Return AddressOf expired
    End Function

    Public MustInherit Class settable
        Inherits expiration_controller

        Public MustOverride Function [stop]() As Boolean

        Public Shared Function [New]() As settable
            Return New se()
        End Function

        Public Shared Shadows Widening Operator CType(ByVal this As ref(Of singleentry)) As settable
            Return from_singleentry(this)
        End Operator
    End Class

    Public Shared Function from_lifetime_ms(ByVal ms As Int64, ByRef o As expiration_controller) As Boolean
        o = New ms(ms)
        Return True
    End Function

    Public Shared Function from_lifetime_ms(ByVal ms As Int64) As expiration_controller
        Dim o As expiration_controller = Nothing
        assert(from_lifetime_ms(ms, o))
        Return o
    End Function

    Public Shared Function from_func_bool(ByVal stopping As Func(Of Boolean),
                                          ByRef o As expiration_controller) As Boolean
        If stopping Is Nothing Then
            Return False
        Else
            o = New func_bool(stopping)
            Return True
        End If
    End Function

    Public Shared Function from_func_bool(ByVal stopping As Func(Of Boolean)) As expiration_controller
        Dim o As expiration_controller = Nothing
        assert(from_func_bool(stopping, o))
        Return o
    End Function

    Public Shared Function from_singleentry(ByVal stopping As ref(Of singleentry),
                                            ByRef o As expiration_controller.settable) As Boolean
        If stopping Is Nothing Then
            Return False
        Else
            o = New se(stopping)
            Return True
        End If
    End Function

    Public Shared Function from_singleentry(ByVal stopping As ref(Of singleentry)) As expiration_controller.settable
        Dim o As expiration_controller.settable = Nothing
        assert(from_singleentry(stopping, o))
        Return o
    End Function

    Public Shared Widening Operator CType(ByVal this As Int64) As expiration_controller
        Return from_lifetime_ms(this)
    End Operator

    Public Shared Widening Operator CType(ByVal this As Func(Of Boolean)) As expiration_controller
        Return from_func_bool(this)
    End Operator

    Public Shared Widening Operator CType(ByVal this As ref(Of singleentry)) As expiration_controller
        Return from_singleentry(this)
    End Operator
End Class
