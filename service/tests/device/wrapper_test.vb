
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utils
Imports osi.root.utt
Imports osi.service.argument
Imports osi.service.device
Imports wrapper = osi.service.device.wrapper

Public Class wrapper_test
    Inherits [case]

    Private Interface int
    End Interface

    Private Class impl
        Implements int
    End Class

    Private Interface wrap
        Inherits int

        Function impl() As int
    End Interface

    Private Class wrap1
        Implements wrap, int

        Private ReadOnly i As int

        Public Function impl() As int Implements wrap.impl
            Return i
        End Function

        Public Sub New(ByVal i As int)
            Me.i = i
        End Sub
    End Class

    Private Class wrap2
        Implements wrap, int

        Private ReadOnly i As int

        Public Function impl() As int Implements wrap.impl
            Return i
        End Function

        Public Sub New(ByVal i As int)
            Me.i = i
        End Sub
    End Class

    Private Class wrap3
        Implements wrap, int

        Private ReadOnly i As int

        Public Function impl() As int Implements wrap.impl
            Return i
        End Function

        Public Sub New(ByVal i As int)
            Me.i = i
        End Sub
    End Class

    Private Class wrap4
        Implements wrap, int

        Private ReadOnly i As int

        Public Function impl() As int Implements wrap.impl
            Return i
        End Function

        Public Sub New(ByVal i As int)
            Me.i = i
        End Sub
    End Class

    Private ReadOnly type_def As String = guid_str()
    Private ReadOnly magic_key As String = guid_str()
    Private ReadOnly magic_value As String = guid_str()

    Private Sub assert_var(ByVal v As var)
        If assertion.is_not_null(v) Then
            Dim s As String = Nothing
            If assertion.is_true(v.value(magic_key, s)) Then
                assertion.equal(s, magic_value)
            End If
        End If
    End Sub

    Public Overrides Function prepare() As Boolean
        Return MyBase.prepare() AndAlso
               assertion.is_true(wrapper.register(Function(v As var, i As int, ByRef o As int) As Boolean
                                                      assert_var(v)
                                                      o = New wrap1(i)
                                                      Return True
                                                  End Function)) AndAlso
               assertion.is_true(wrapper.register(Function(v As var, i As int) As int
                                                      assert_var(v)
                                                      Return New wrap3(i)
                                                  End Function)) AndAlso
               assertion.is_true(wrapper.register(type_def,
                                            Function(v As var, i As int, ByRef o As int) As Boolean
                                                assert_var(v)
                                                o = New wrap2(i)
                                                Return True
                                            End Function)) AndAlso
               assertion.is_true(wrapper.register(type_def,
                                            Function(v As var, i As int) As int
                                                assert_var(v)
                                                Return New wrap4(i)
                                            End Function))
    End Function

    Private Shared Function assert_wrapped_by(Of T As {wrap, Class}, T2 As {int, Class}) _
                                             (ByVal i As int, ByRef o As T2) As Boolean
        Dim x As T = Nothing
        Return assertion.is_not_null(i) AndAlso
               assertion.is_true(cast(Of T)(i, x)) AndAlso
               assertion.is_not_null(x) AndAlso
               assertion.is_not_null(x.impl()) AndAlso
               cast(Of T2)(x.impl(), o) AndAlso
               assertion.is_not_null(o)
    End Function

    Private Function global_wrap_case() As Boolean
        Dim v As var = Nothing
        v = New var(strcat("--", magic_key, "=", magic_value))
        Dim i As int = Nothing
        i = New impl()
        Dim o As int = Nothing
        If assertion.is_true(wrapper.wrap(v, i, o)) Then
            Dim w1 As wrap1 = Nothing
            Dim impl As impl = Nothing
            If assert_wrapped_by(Of wrap3, wrap1)(o, w1) AndAlso
               assert_wrapped_by(Of wrap1, impl)(w1, impl) Then
                assertion.reference_equal(impl, i)
            End If
        End If
        Return True
    End Function

    Private Function typed_wrap_case() As Boolean
        Dim v As var = Nothing
        v = New var(strcat("--", magic_key, "=", magic_value, " ", "--type=", type_def))
        Dim i As int = Nothing
        i = New impl()
        Dim o As int = Nothing
        If assertion.is_true(wrapper.wrap(v, i, o)) Then
            Dim w1 As wrap1 = Nothing
            Dim w4 As wrap4 = Nothing
            Dim w2 As wrap2 = Nothing
            Dim impl As impl = Nothing
            If assert_wrapped_by(Of wrap3, wrap1)(o, w1) AndAlso
               assert_wrapped_by(Of wrap1, wrap4)(w1, w4) AndAlso
               assert_wrapped_by(Of wrap4, wrap2)(w4, w2) AndAlso
               assert_wrapped_by(Of wrap2, impl)(w2, impl) Then
                assertion.reference_equal(impl, i)
            End If
        End If
        Return True
    End Function

    Public Overrides Function run() As Boolean
        Return global_wrap_case() AndAlso
               typed_wrap_case()
    End Function

    Public Overrides Function finish() As Boolean
        wrapper.clear(Of int)()
        Return MyBase.finish()
    End Function
End Class
