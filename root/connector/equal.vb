
Option Explicit On
Option Infer Off
Option Strict On

' TODO: Move to type_info.
Imports System.Runtime.CompilerServices
Imports osi.root.constants
Imports osi.root.delegates

' Use following order to check the equality of two unknown types:
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
        Private Shared ReadOnly f As _do_val_val_ref(Of T, T2, Boolean, Boolean) =
            Function() As _do_val_val_ref(Of T, T2, Boolean, Boolean)
                If type_info(Of T, type_info_operators.implement, IEquatable(Of T2)).v Then
                    Return always_succeed(AddressOf this_to_t2(Of T2))
                End If
                If type_info(Of T2, type_info_operators.implement, IEquatable(Of T)).v Then
                    Return always_succeed(AddressOf that_to_t(Of T))
                End If
                If type_info(Of T, type_info_operators.implement, IEquatable(Of Object)).v Then
                    Return always_succeed(AddressOf this_to_t2(Of Object))
                End If
                If type_info(Of T2, type_info_operators.implement, IEquatable(Of Object)).v Then
                    Return always_succeed(AddressOf that_to_t(Of Object))
                End If
                If type_info(Of T).is_object OrElse type_info(Of T2).is_object Then
                    raise_error(error_type.performance,
                                "equal_cache(Of *, Object) or equal_cache(Of Object, *) impacts performance seriously.")
                    If type_info(Of T).is_object AndAlso type_info(Of T2).is_object Then
                        Return AddressOf runtime_equal
                    End If
                    If type_info(Of T).is_object Then
                        Return AddressOf runtime_this_to_t2(Of T2)
                    End If
                    assert(type_info(Of T2).is_object)
                    Return AddressOf runtime_that_to_t(Of T)
                End If
                Return Nothing
            End Function()

        Private Shared Function always_succeed(ByVal f As Func(Of T, T2, Boolean)) _
                                              As _do_val_val_ref(Of T, T2, Boolean, Boolean)
            assert(Not f Is Nothing)
            Return Function(ByVal this As T, ByVal that As T2, ByRef o As Boolean) As Boolean
                       o = f(this, that)
                       Return True
                   End Function
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Shared Function equal(ByVal this As T, ByVal that As T2, ByRef o As Boolean) As Boolean
            If f Is Nothing Then
                Return False
            End If
            Return f(this, that, o)
        End Function

        Private Sub New()
        End Sub
    End Class

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Function this_to_t2(Of T)(ByVal this As Object, ByVal that As T) As Boolean
        Return direct_cast(Of IEquatable(Of T))(this).Equals(that)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Function that_to_t(Of T)(ByVal this As T, ByVal that As Object) As Boolean
        Return direct_cast(Of IEquatable(Of T))(that).Equals(this)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Function runtime_this_to_t2(Of T)(ByVal this As Object, ByVal that As T, ByRef o As Boolean) As Boolean
        If TypeOf this Is IEquatable(Of T) Then
            o = this_to_t2(this, that)
            Return True
        End If
        Return False
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Function runtime_that_to_t(Of T)(ByVal this As T, ByVal that As Object, ByRef o As Boolean) As Boolean
        If TypeOf that Is IEquatable(Of T) Then
            o = that_to_t(this, that)
            Return True
        End If
        Return False
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Function runtime_equal(ByVal this As Object, ByVal that As Object, ByRef o As Boolean) As Boolean
        Return runtime_this_to_t2(this, that, o) OrElse
               runtime_that_to_t(this, that, o)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Function object_equal(ByVal this As Object, ByVal that As Object, ByRef o As Boolean) As Boolean
        Dim cmp As Int32
        If object_compare(this, that, cmp) Then
            o = (cmp = 0)
            Return True
        End If
        Return False
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Function compare_equal(Of T, T2)(ByVal this As T, ByVal that As T2, ByRef o As Boolean) As Boolean
        Dim cmp As Int32
        If non_null_compare(this, that, cmp) Then
            o = (cmp = 0)
            Return True
        End If
        Return False
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Function runtime_compare_equal(ByVal this As Object, ByVal that As Object, ByRef o As Boolean) As Boolean
        Dim cmp As Int32
        If non_null_runtime_compare(this, that, cmp) Then
            o = (cmp = 0)
            Return True
        End If
        Return False
    End Function

    ' TODO: Remove non-extension versions.
    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function runtime_equal(ByVal this As Object, ByVal that As Object) As Boolean
        Dim o As Boolean
        If object_equal(this, that, o) Then
            Return o
        End If
        If runtime_this_to_t2(this, that, o) Then
            Return o
        End If
        If runtime_that_to_t(this, that, o) Then
            Return o
        End If
        If runtime_compare_equal(this, that, o) Then
            Return o
        End If
        Return Object.Equals(this, that)
    End Function

    <Extension()>
    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function runtime_equal_to(ByVal this As Object, ByVal that As Object) As Boolean
        Return runtime_equal(this, that)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Function do_non_null_equal(Of T, T2)(ByVal this As T, ByVal that As T2) As Boolean
        Dim o As Boolean
        If equaler(Of T, T2).defined() Then
            Return equaler.equal(this, that)
        End If
        If equal_cache(Of T, T2).equal(this, that, o) Then
            Return o
        End If
        If compare_equal(this, that, o) Then
            Return o
        End If

        ' This typically means reference-equality (for classes) or value-equality (for structures).
        ' But anyway, we always have a result.
        Return Object.Equals(this, that)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Function do_equal(Of T, T2)(ByVal this As T, ByVal that As T2) As Boolean
        Dim o As Boolean
        If object_equal(this, that, o) Then
            Return o
        End If
        Return do_non_null_equal(this, that)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Sub equal_error(Of T, T2)(ByVal ex As Exception)
        raise_error(error_type.exclamation,
                    "Failed to check the equality of ", GetType(T), " with ", GetType(T2), ", ex ", ex)
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function non_null_equal(Of T, T2)(ByVal this As T, ByVal that As T2, ByRef o As Boolean) As Boolean
        Try
            o = do_non_null_equal(this, that)
            Return True
        Catch ex As Exception
            equal_error(Of T, T2)(ex)
            Return False
        End Try
    End Function

    <Extension()>
    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function non_null_equal_to(Of T, T2)(ByVal this As T, ByVal that As T2, ByRef o As Boolean) As Boolean
        Return non_null_equal(this, that, o)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function non_null_equal(Of T, T2)(ByVal this As T, ByVal that As T2) As Boolean
        Dim o As Boolean
        assert(non_null_equal(this, that, o))
        Return o
    End Function

    <Extension()>
    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function non_null_equal_to(Of T, T2)(ByVal this As T, ByVal that As T2) As Boolean
        Return non_null_equal(this, that)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function equal(Of T, T2)(ByVal this As T, ByVal that As T2, ByRef o As Boolean) As Boolean
        Try
            o = do_equal(this, that)
            Return True
        Catch ex As Exception
            equal_error(Of T, T2)(ex)
            Return False
        End Try
    End Function

    <Extension()>
    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function equal_to(Of T, T2)(ByVal this As T, ByVal that As T2, ByRef o As Boolean) As Boolean
        Return equal(this, that, o)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function equal(Of T, T2)(ByVal this As T, ByVal that As T2) As Boolean
        Dim o As Boolean
        assert(equal(this, that, o))
        Return o
    End Function

    <Extension()>
    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function equal_to(Of T, T2)(ByVal this As T, ByVal that As T2) As Boolean
        Return equal(this, that)
    End Function
End Module
