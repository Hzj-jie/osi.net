
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utils
Imports osi.root.utt
Imports osi.service.argument
Imports osi.service.device

Public Class constructor_wrapper_test
    Inherits [case]

    Private Interface int
    End Interface

    Private Class impl
        Implements int
    End Class

    Private Class wrap
        Implements int

        Public ReadOnly x As int

        Public Sub New(ByVal x As int)
            Me.x = x
        End Sub
    End Class

    Public Overrides Function prepare() As Boolean
        Return MyBase.prepare() AndAlso
               assert_true(constructor.register(Function(i As var) As int
                                                    assert_not_nothing(i)
                                                    Return New impl()
                                                End Function)) AndAlso
               assert_true(wrapper.register(Function(v As var, i As int) As int
                                                assert_not_nothing(v)
                                                assert_not_nothing(i)
                                                assert_true(TypeOf i Is impl)
                                                Return New wrap(i)
                                            End Function))
    End Function

    Public Overrides Function run() As Boolean
        Dim o As int = Nothing
        assert_true(constructor.resolve(New var(), o))
        If assert_not_nothing(o) Then
            Dim w As wrap = Nothing
            If assert_true(cast(Of wrap)(o, w)) Then
                assert(Not w Is Nothing)
                assert_not_nothing(w.x)
                assert_true(TypeOf w.x Is impl)
            End If
        End If
        Return True
    End Function

    Public Overrides Function finish() As Boolean
        Return assert_true(constructor(Of int).erase()) AndAlso
               assert_true(wrapper.erase(Of int)()) AndAlso
               MyBase.finish()
    End Function
End Class
