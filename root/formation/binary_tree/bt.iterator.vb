
Option Explicit On
Option Infer Off
Option Strict On

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with bt.iterator.vbp ----------
'so change bt.iterator.vbp instead of this file



'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with ..\..\codegen\iterator.imports.vbp ----------
'so change ..\..\codegen\iterator.imports.vbp instead of this file


Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants
'finish ..\..\codegen\iterator.imports.vbp --------

Partial Public Class bt(OF T)

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with ..\..\codegen\random_access_iterator.vbp ----------
'so change ..\..\codegen\random_access_iterator.vbp instead of this file



'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with ..\..\codegen\static_iterator.vbp ----------
'so change ..\..\codegen\static_iterator.vbp instead of this file



    Partial Public Structure iterator
        Implements IComparable(Of iterator), IComparable

        Public Shared ReadOnly [end] As iterator

        Shared Sub New()
            [end] = New iterator()
        End Sub

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
            assert(Not this.is_null() AndAlso Not this.is_null())
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
        Public Shared Operator +(ByVal this As iterator) As node
            Return If(this = [end], Nothing, this.p)
        End Operator
#End If
    End Structure
'finish ..\..\codegen\static_iterator.vbp --------

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

        Public Shared Operator -(ByVal this As iterator, ByVal that As Int32) As iterator
            Return this + (-that)
        End Operator
    End Structure
'finish ..\..\codegen\random_access_iterator.vbp --------

    Partial Public Structure iterator
        Private Function move_next(ByVal that As UInt32) As iterator
            assert(that > 0)
            Dim n As node = Nothing
            n = p
            For i As UInt32 = 0 To that - uint32_1
                assert(Not n Is Nothing)
                If n.has_right_child() Then
                    n = n.right_child().min()
                ElseIf n.is_left_subtree() Then
                    n = n.parent()
                Else
                    While n.is_right_subtree()
                        n = n.parent()
                    End While
                    If n.is_root() Then
                        Return [end]
                    Else
                        assert(n.is_left_subtree())
                        n = n.parent()
                    End If
                End If
            Next
            assert(Not n Is Nothing)
            Return New iterator(n)
        End Function

        Private Function move_prev(ByVal that As UInt32) As iterator
            assert(that > 0)
            Dim n As node = Nothing
            n = p
            For i As UInt32 = 0 To that - uint32_1
                assert(Not n Is Nothing)
                If n.has_left_child() Then
                    n = n.left_child().max()
                ElseIf n.is_right_subtree() Then
                    n = n.parent()
                Else
                    While n.is_left_subtree()
                        n = n.parent()
                    End While
                    If n.is_root() Then
                        Return [end]
                    Else
                        assert(n.is_right_subtree())
                        n = n.parent()
                    End If
                End If
            Next
            assert(Not n Is Nothing)
            Return New iterator(n)
        End Function

        Public Function value() As T
            Return +Me
        End Function

        Public Shared Operator +(ByVal this As iterator) As T
            Return If(this = [end], Nothing, this.p.value())
        End Operator

        Friend Function node() As node
            Return p
        End Function
    End Structure
End Class
'finish bt.iterator.vbp --------
