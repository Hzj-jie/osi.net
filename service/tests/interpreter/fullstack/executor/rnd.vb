
Imports osi.root.connector
Imports osi.root.utils
Imports osi.service.interpreter.fullstack
Imports osi.service.interpreter.fullstack.executor

Namespace fullstack.executor
    Friend Module _rnd
        Public Class test_class
            Implements IComparable(Of test_class)

            Public ReadOnly s As String
            Public ReadOnly i As Int32

            Public Sub New()
                s = rndenchars(rnd_int(10, 100))
                i = rnd_int()
            End Sub

            Public Function CompareTo(ByVal other As test_class) As Int32 _
                                     Implements IComparable(Of test_class).CompareTo
                Dim c As Int32 = 0
                c = object_compare(Me, other)
                If c = object_compare_undetermined Then
                    assert(Not other Is Nothing)
                    c = compare(s, other.s)
                    If c = 0 Then
                        Return compare(i, other.i)
                    Else
                        Return c
                    End If
                Else
                    Return c
                End If
            End Function
        End Class

        Public Function a_sub_struct() As type
            Return New type({type.bool,
                             type.int,
                             type.float,
                             type.char,
                             type.string,
                             type.var})
        End Function

        Public Function a_struct() As type
            Return New type({type.bool,
                             type.int,
                             type.float,
                             type.char,
                             type.string,
                             type.var,
                             a_sub_struct()})
        End Function

        Public Function a_struct_instance() As variable
            Dim l1() As variable = Nothing
            ReDim l1(6 - 1)
            l1(0) = New variable(rnd_bool())
            l1(1) = New variable(rnd_int())
            l1(2) = New variable(rnd_double())
            l1(3) = New variable(rndchar())
            l1(4) = New variable(rndenchars(rnd_int(10, 100)))
            l1(5) = New variable(rnd_var())
            Dim l2() As variable = Nothing
            ReDim l2(7 - 1)
            l2(0) = New variable(rnd_bool())
            l2(1) = New variable(rnd_int())
            l2(2) = New variable(rnd_double())
            l2(3) = New variable(rndchar())
            l2(4) = New variable(rndenchars(rnd_int(10, 100)))
            l2(5) = New variable(rnd_var())
            l2(6) = New variable(a_sub_struct(), l1)
            Return New variable(a_struct(), l2)
        End Function

        Public Function rnd_var() As Object
            If rnd_int(0, 3) = 0 Then
                Return Nothing
            ElseIf rnd_bool() Then
                Return New Object()
            Else
                Return New test_class()
            End If
        End Function

        Public Function rnd_variable() As variable
            If rnd_bool() Then
                Return New variable(rnd_bool())
            ElseIf rnd_bool() Then
                Return New variable(rnd_int())
            ElseIf rnd_bool() Then
                Return New variable(rnd_double())
            ElseIf rnd_bool() Then
                Return New variable(rndchar())
            ElseIf rnd_bool() Then
                Return New variable(rndenchars(rnd_int(10, 100)))
            ElseIf rnd_bool() Then
                Return New variable(New Object())
            Else
                Return a_struct_instance()
            End If
        End Function
    End Module
End Namespace
