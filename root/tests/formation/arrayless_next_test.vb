
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utt

Public Class arrayless_next_test
    Inherits [case]

    Private Shared Function alloc_fail_case() As Boolean
        Const size As UInt32 = 100
        Dim p As UInt32 = 0
        Dim v As Object = Nothing
        Using code_block
            Dim current As Object = Nothing
            Dim a As arrayless(Of Object) = Nothing
            a = arrayless.[New](Function() As Object
                                    Return current
                                End Function,
                                size)
            current = Nothing
            assertion.is_false(a.next(p, v))
        End Using
        Using code_block
            Dim a As arrayless(Of Object) = Nothing
            a = arrayless.[New](Function(ByVal i As UInt32) As Object
                                    If (i And 1) = 0 Then
                                        Return Nothing
                                    Else
                                        Return New Object()
                                    End If
                                End Function,
                                size)
            For i As UInt32 = 0 To (size >> 1) - 1
                assertion.is_true(a.next(p, v))
                assertion.equal(p, (i << 1) + 1)
                assertion.is_not_null(v)
            Next
            assertion.is_false(a.next(p, v))
        End Using
        Return True
    End Function

    Private Shared Function value_type_never_fail_case(Of T)() As Boolean
        Const size As UInt32 = 100
        Dim a As arrayless(Of T) = Nothing
        a = arrayless.[New](Function() As T
                                Return Nothing
                            End Function,
                            size)
        Dim v As T = Nothing
        Dim p As UInt32 = 0
        For i As UInt32 = 0 To size - 1
            assertion.is_true(a.[next](p, v))
            assertion.equal(p, i)
            assertion.equal(v, Nothing)
        Next
        assertion.is_false(a.[next](p, v))
        Return True
    End Function

    Private Shared Function value_type_never_fail_case() As Boolean
        Return value_type_never_fail_case(Of Int32)() AndAlso
               value_type_never_fail_case(Of Int64)() AndAlso
               value_type_never_fail_case(Of UInt32)()
    End Function

    Public Overrides Function run() As Boolean
        Return value_type_never_fail_case() AndAlso
               alloc_fail_case()
    End Function
End Class
