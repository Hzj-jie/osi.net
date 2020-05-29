
Option Explicit On
Option Infer Off
Option Strict On

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with vector.iterator.vbp ----------
'so change vector.iterator.vbp instead of this file


'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with iterator.imports.vbp ----------
'so change iterator.imports.vbp instead of this file


Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants
'finish iterator.imports.vbp --------

Partial Public NotInheritable Class vector(Of T)

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with random_access_iterator.vbp ----------
'so change random_access_iterator.vbp instead of this file



'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with static_iterator.vbp ----------
'so change static_iterator.vbp instead of this file



    Partial Public Structure iterator
        Implements IComparable(Of iterator), IComparable

        Public Shared ReadOnly [end] As iterator

        Shared Sub New()
            [end] = New iterator()
        End Sub

        Private ReadOnly p As ref

#If True Then
        <MethodImpl(method_impl_options.aggressive_inlining)>
        Friend Sub New(ByVal that As ref)
#Else
        <MethodImpl(method_impl_options.aggressive_inlining)>
        Private Sub New(ByVal that As ref)
#End If
            p = that
        End Sub

#If True Then
        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function is_end() As Boolean
            Return p.is_end()
        End Function
#Else
        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function is_end() As Boolean
            Return p Is Nothing
        End Function
#End If

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function is_not_end() As Boolean
            Return Not is_end()
        End Function

#If False Then
        Private Shared Function is_equal(ByVal this As ref, ByVal that As ref) As Boolean
            Return object_compare(this, that) = 0
        End Function
#End If

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Shared Operator =(ByVal this As iterator, ByVal that As iterator) As Boolean
            If this.is_end() AndAlso that.is_end() Then
                Return True
            End If
            If this.is_end() OrElse that.is_end() Then
                Return False
            End If
            Return is_equal(this.p, that.p)
        End Operator

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Shared Operator <>(ByVal this As iterator, ByVal that As iterator) As Boolean
            Return Not (this = that)
        End Operator

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function CompareTo(ByVal other As iterator) As Int32 Implements IComparable(Of iterator).CompareTo
            Return If(Me = other, 0, -1)
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function CompareTo(ByVal other As Object) As Int32 Implements IComparable.CompareTo
            Return CompareTo(cast(Of iterator)(other, False))
        End Function

#If False Then
        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Shared Operator +(ByVal this As iterator) As ref
            Return If(this = [end], Nothing, this.p)
        End Operator
#End If
    End Structure
'finish static_iterator.vbp --------

    Partial Public Structure iterator
        '1. iterator / reverse_iterator are combined together
        '2. do not allow to - from end, it's not must-have
        '3. operator+ / operator- should not impact current instance, considering i = j + 1
        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Shared Operator +(ByVal this As iterator, ByVal that As Int32) As iterator
            If this.is_end() OrElse that = 0 Then
                Return this
            End If
            If that > 0 Then
                Return this.move_next(CUInt(that))
            End If
            assert(that < 0)
            Return this.move_prev(CUInt(-that))
        End Operator

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Shared Operator -(ByVal this As iterator, ByVal that As Int32) As iterator
            Return this + (-that)
        End Operator
    End Structure


'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with iterator_to_enumerator.vbp ----------
'so change iterator_to_enumerator.vbp instead of this file


    Public Structure enumerator
        Implements container_operator(Of T).enumerator

        Private it As vector(Of T).iterator

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Sub New(ByVal m As vector(Of T))
            assert(Not m Is Nothing)
            it = m.begin()
        End Sub

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Sub [next]() Implements container_operator(Of T).enumerator.next
            it += 1
        End Sub

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function current() As T _
                Implements container_operator(Of T).enumerator.current
            Return +it
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function [end]() As Boolean _
                Implements container_operator(Of T).enumerator.end
            Return it.is_end()
        End Function
    End Structure

'finish iterator_to_enumerator.vbp --------
'finish random_access_iterator.vbp --------

    Partial Public Structure iterator
        Private Function move_next(ByVal s As UInt32) As iterator
            assert(s > uint32_0)
            s += p.index()
            If s >= p.size() Then
                Return [end]
            End If
            Return New iterator(p.ref_at(s))
        End Function

        Private Function move_prev(ByVal s As UInt32) As iterator
            assert(s > uint32_0)
            If s > p.index() Then
                Return [end]
            End If
            Return New iterator(p.ref_at(p.index() - s))
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Private Shared Function is_equal(ByVal this As ref, ByVal that As ref) As Boolean
            Return this.is_equal_to(that)
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Shared Operator +(ByVal this As iterator) As T
            Return If(this = [end], Nothing, this.p.value())
        End Operator
    End Structure
End Class

'finish vector.iterator.vbp --------