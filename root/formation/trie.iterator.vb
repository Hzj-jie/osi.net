
Option Explicit On
Option Infer Off
Option Strict On

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with trie.iterator.vbp ----------
'so change trie.iterator.vbp instead of this file



'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with iterator.imports.vbp ----------
'so change iterator.imports.vbp instead of this file


Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants
'finish iterator.imports.vbp --------
Imports osi.root.template

Partial Public Class trie(Of KEY_T, VALUE_T, _CHILD_COUNT As _int64, _KEY_TO_INDEX As _to_uint32(Of KEY_T))

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with random_access_iterator.single_step.vbp ----------
'so change random_access_iterator.single_step.vbp instead of this file



'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with random_access_iterator.vbp ----------
'so change random_access_iterator.vbp instead of this file



'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with static_iterator.vbp ----------
'so change static_iterator.vbp instead of this file



    Partial Public Structure iterator
        Implements IComparable(Of iterator), IComparable

        Public Shared ReadOnly [end] As iterator = New iterator()

        Private ReadOnly p As node

#If True Then
        <MethodImpl(method_impl_options.aggressive_inlining)>
        Friend Sub New(ByVal that As node)
#Else
        <MethodImpl(method_impl_options.aggressive_inlining)>
        Private Sub New(ByVal that As node)
#End If
            p = that
        End Sub

#If False Then
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

#If True Then
        Private Shared Function is_equal(ByVal this As node, ByVal that As node) As Boolean
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

#If True Then
        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Shared Operator +(ByVal this As iterator) As node
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
        Implements container_operator(Of node).enumerator

        Private it As trie(Of KEY_T, VALUE_T, _CHILD_COUNT, _KEY_TO_INDEX).iterator

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Sub New(ByVal m As trie(Of KEY_T, VALUE_T, _CHILD_COUNT, _KEY_TO_INDEX))
            assert(Not m Is Nothing)
            it = m.begin()
        End Sub

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Sub [next]() Implements container_operator(Of node).enumerator.next
            it += 1
        End Sub

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function current() As node _
                Implements container_operator(Of node).enumerator.current
            Return +it
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function [end]() As Boolean _
                Implements container_operator(Of node).enumerator.end
            Return it.is_end()
        End Function
    End Structure

'finish iterator_to_enumerator.vbp --------
'finish random_access_iterator.vbp --------

    Partial Public Structure iterator
        Private Function move_next(ByVal i As UInt32) As iterator
            assert(i > uint32_0)
            Dim n As iterator = Nothing
            n = Me
            While Not n.is_end() AndAlso i > uint32_0
                n = n.move_next()
                i -= uint32_1
            End While
            Return n
        End Function

        Private Function move_prev(ByVal i As UInt32) As iterator
            assert(i > uint32_0)
            Dim n As iterator = Nothing
            n = Me
            While Not n.is_end() AndAlso i > uint32_0
                n = n.move_prev()
                i -= uint32_1
            End While
            Return n
        End Function
    End Structure
'finish random_access_iterator.single_step.vbp --------

    Partial Public Structure iterator
        Private Function move_next() As iterator
            Dim p As node = Nothing
            p = Me.p
            Dim this_index As UInt32 = uint32_0
            While Not p Is Nothing
                For i As Int32 = CInt(this_index) To p.child.Length() - 1
                    If Not p.child(i) Is Nothing Then
                        p = p.child(i)
                        Exit While
                    End If
                Next

                If find_father_index(p, this_index) Then
                    this_index += uint32_1
                    p = p.father
                Else
                    Return _end
                End If
            End While

            Return New iterator(p)
        End Function

        Private Function move_prev() As iterator
            Dim p As node = Nothing
            p = Me.p
            Dim this_index As UInt32 = uint32_0
            If find_father_index(p, this_index) Then
                If this_index > 0 Then
                    Dim i As Int32
                    Dim this_node As node = Nothing
                    this_node = p
                    For i = CInt(this_index - uint32_1) To 0 Step -1
                        p = this_node.father.child(i)
                        If max(p) Then
                            Exit For
                        End If
                    Next
                    If i < 0 Then
                        p = this_node.father
                    End If
                Else
                    p = p.father
                End If
            Else
                Return _end
            End If

            Return New iterator(p)
        End Function

        Friend Function [get]() As node
            Return p
        End Function
    End Structure
End Class
'finish trie.iterator.vbp --------
