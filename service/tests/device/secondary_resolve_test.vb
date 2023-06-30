
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utt
Imports osi.service.argument
Imports osi.service.device

Public Class secondary_resolve_test
    Inherits [case]

    Private Const secondary_type_name As String = "mode"
    Private Const c1_name As String = "c1"
    Private Const c2_name As String = "c2"

    Private Interface i1
    End Interface

    Private Class c1
        Implements i1
    End Class

    Private Class c2
        Implements i1
    End Class

    Private Interface i2
        Function i1() As i1
    End Interface

    Private Class c3
        Implements i2

        Private ReadOnly _i1 As i1

        Public Sub New(ByVal i1 As i1)
            assertion.is_not_null(i1)
            _i1 = i1
        End Sub

        Public Function i1() As i1 Implements i2.i1
            Return _i1
        End Function
    End Class

    Public Overrides Function prepare() As Boolean
        Return MyBase.prepare() AndAlso
               assertion.is_true(constructor.register(c1_name,
                                                Function(v As var) As i1
                                                    Return New c1()
                                                End Function)) AndAlso
               assertion.is_true(constructor.register(c2_name,
                                                Function(v As var) As i1
                                                    Return New c2()
                                                End Function)) AndAlso
               assertion.is_true(constructor.register(Of i2) _
                                               (Function(v As var, ByRef o As i2) As Boolean
                                                    Return secondary_resolve(v,
                                                                             secondary_type_name,
                                                                             Function(i As i1) As i2
                                                                                 Return New c3(i)
                                                                             End Function,
                                                                             o)
                                                End Function))
    End Function

    Public Overrides Function run() As Boolean
        Dim o As i2 = Nothing
        assertion.is_true(constructor.resolve(New var({strcat("--",
                                                        secondary_type_name,
                                                        "=",
                                                        c1_name)}),
                                         o))
        If assertion.is_not_null(o) AndAlso
           assertion.is_not_null(o.i1) Then
            assertion.is_true(TypeOf o.i1 Is c1)
        End If
        o = Nothing
        assertion.is_true(constructor.resolve(New var({strcat("--",
                                                        secondary_type_name,
                                                        "=",
                                                        c2_name)}),
                                        o))
        If assertion.is_not_null(o) AndAlso
           assertion.is_not_null(o.i1) Then
            assertion.is_true(TypeOf o.i1 Is c2)
        End If
        assertion.is_false(constructor.resolve(New var(), o))
        Return True
    End Function

    Public Overrides Function finish() As Boolean
        Return assertion.is_true(constructor(Of i1).erase(c1_name)) AndAlso
               assertion.is_true(constructor(Of i1).erase(c2_name)) AndAlso
               assertion.is_true(constructor(Of i2).erase()) AndAlso
               MyBase.finish()
    End Function
End Class
