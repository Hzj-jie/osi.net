
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants
Imports osi.root.delegates

' Use follow order to check the equality of two unknown types:
' - object_compare()
' - equaler(of T, T2)
' (cached) {
'   - T.IEqualtable(Of T2) OR
'   - T2.IEqualtable(Of T) OR
'   - T.IEqualtable(Of Object) OR
'   - T2.IEquatable(Of Object) OR
'   if T == object {
'     - GetType(this).IEqualtable(Of T2) AND
'   }
'   if T2 == object {
'     - GetType(that).IEqualtable(Of T) AND
'   }
' }
' - compare(Of T, T2)
' - Object.Equals()
' equal() fails only when exceptions have been thrown when executing various Equals() function.
Public Module _equal
    Private NotInheritable Class equal_cache(Of T, T2)
        Private Shared ReadOnly f As _do_val_val_ref(Of T, T2, Boolean, Boolean)

        Shared Sub New()
            If type_info(Of T, type_info_operators.implement, IEquatable(Of T2)).v Then
                f = always_succeed(AddressOf this_to_t2(Of T2))
            ElseIf type_info(Of T2, type_info_operators.implement, IEquatable(Of T)).v Then
                f = always_succeed(AddressOf that_to_t(Of T))
            ElseIf type_info(Of T, type_info_operators.implement, IEquatable(Of Object)).v Then
                f = always_succeed(AddressOf this_to_t2(Of Object))
            ElseIf type_info(Of T2, type_info_operators.implement, IEquatable(Of Object)).v Then
                f = always_succeed(AddressOf that_to_t(Of Object))
            ElseIf type_info(Of T).is_object OrElse type_info(Of T2).is_object Then
                raise_error(error_type.performance,
                            "equal_cache(Of *, Object) or equal_cache(Of Object, *) impacts performance seriously.")
                If type_info(Of T).is_object AndAlso type_info(Of T2).is_object Then
                    f = AddressOf runtime_equal(Of T, T2)
                ElseIf type_info(Of T).is_object Then
                    f = AddressOf runtime_this_to_t2(Of T2)
                Else
                    assert(type_info(Of T2).is_object)
                    f = AddressOf runtime_that_to_t(Of T)
                End If
            Else
                f = Nothing
            End If
        End Sub

        Private Shared Function always_succeed(ByVal f As Func(Of T, T2, Boolean)) _
                                              As _do_val_val_ref(Of T, T2, Boolean, Boolean)
            assert(Not f Is Nothing)
            Return Function(ByVal this As T, ByVal that As T2, ByRef o As Boolean) As Boolean
                       o = f(this, that)
                       Return True
                   End Function
        End Function

        Public Shared Function equal(ByVal this As T, ByVal that As T2, ByRef o As Boolean) As Boolean
            If f Is Nothing Then
                Return False
            Else
                Try
                    Return f(this, that, o)
                Catch ex As Exception
                    raise_error(error_type.exclamation,
                                "Failed to check the equality of ", GetType(T), " with ", GetType(T2), ", ex ", ex)
                    Return False
                End Try
            End If
        End Function

        Private Sub New()
        End Sub
    End Class

    Private Function this_to_t2(Of T)(ByVal this As Object, ByVal that As T) As Boolean
        Return direct_cast(Of IEquatable(Of T))(this).Equals(that)
    End Function

    Private Function that_to_t(Of T)(ByVal this As T, ByVal that As Object) As Boolean
        Return direct_cast(Of IEquatable(Of T))(that).Equals(this)
    End Function

    Private Function runtime_this_to_t2(Of T)(ByVal this As Object, ByVal that As T, ByRef o As Boolean) As Boolean
        If TypeOf this Is IEquatable(Of T) Then
            o = this_to_t2(this, that)
            Return True
        End If
        Return False
    End Function

    Private Function runtime_that_to_t(Of T)(ByVal this As T, ByVal that As Object, ByRef o As Boolean) As Boolean
        If TypeOf that Is IEquatable(Of T) Then
            o = that_to_t(this, that)
            Return True
        End If
        Return False
    End Function

    Private Function runtime_equal(Of T, T2)(ByVal this As T, ByVal that As T2, ByRef o As Boolean) As Boolean
        Return runtime_this_to_t2(this, that, o) OrElse
               runtime_that_to_t(this, that, o)
    End Function

    Public Function equal(Of T, T2)(ByVal this As T, ByVal that As T2) As Boolean
        Dim cmp As Int32 = 0
        cmp = object_compare(this, that)
        Dim o As Boolean = False
        If cmp <> object_compare_undetermined Then
            Return (cmp = 0)
        ElseIf equaler(Of T, T2).defined() Then
            Return equaler.equal(this, that)
        ElseIf equal_cache(Of T, T2).equal(this, that, o) Then
            Return o
        ElseIf compare(this, that, cmp) Then
            Return (cmp = 0)
        Else
            ' This typically means reference-equality (for classes) or value-equality (for structures).
            ' But anyway, we always have a result.
            Return Object.Equals(this, that)
        End If
    End Function
End Module
