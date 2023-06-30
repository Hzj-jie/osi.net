
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
               assertion.is_true(constructor.register(Function(i As var) As int
                                                    assertion.is_not_null(i)
                                                    Return New impl()
                                                End Function)) AndAlso
               assertion.is_true(wrapper.register(Function(v As var, i As int) As int
                                                assertion.is_not_null(v)
                                                assertion.is_not_null(i)
                                                assertion.is_true(TypeOf i Is impl)
                                                Return New wrap(i)
                                            End Function))
    End Function

    Public Overrides Function run() As Boolean
        Dim o As int = Nothing
        assertion.is_true(constructor.resolve(New var(), o))
        If assertion.is_not_null(o) Then
            Dim w As wrap = Nothing
            If assertion.is_true(cast(Of wrap)(o, w)) Then
                assert(Not w Is Nothing)
                assertion.is_not_null(w.x)
                assertion.is_true(TypeOf w.x Is impl)
            End If
        End If
        Return True
    End Function

    Public Overrides Function finish() As Boolean
        Return assertion.is_true(constructor(Of int).erase()) AndAlso
               assertion.is_true(wrapper.erase(Of int)()) AndAlso
               MyBase.finish()
    End Function
End Class
