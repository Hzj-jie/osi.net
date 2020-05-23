
Option Explicit On
Option Infer Off
Option Strict On

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with hasharray.iterator.vbp ----------
'so change hasharray.iterator.vbp instead of this file



'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with ..\..\codegen\iterator.imports.vbp ----------
'so change ..\..\codegen\iterator.imports.vbp instead of this file


Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants
'finish ..\..\codegen\iterator.imports.vbp --------
Imports osi.root.template


Public Module _hasharray_iterator

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with ..\..\codegen\iterator.ext.vbp ----------
'so change ..\..\codegen\iterator.ext.vbp instead of this file


    <Extension()> Public Function null_or_end(Of T, _UNIQUE As _boolean, _HASHER As _to_uint32(Of T), _EQUALER As _equaler(Of T))(ByVal this As hasharray(Of T, _UNIQUE, _HASHER, _EQUALER).iterator) As Boolean
#If True Then
        Return this.is_end()
#Else
        Return this.is_null() OrElse this.is_end()
#End If
    End Function
'finish ..\..\codegen\iterator.ext.vbp --------
End Module

Partial Public Class hasharray(Of T,
                                  _UNIQUE As _boolean,
                                  _HASHER As _to_uint32(Of T),
                                  _EQUALER As _equaler(Of T))
    Public Structure iterator

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with ..\..\codegen\random_access_iterator.single_step.vbp ----------
'so change ..\..\codegen\random_access_iterator.single_step.vbp instead of this file



'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with ..\..\codegen\random_access_iterator.vbp ----------
'so change ..\..\codegen\random_access_iterator.vbp instead of this file



'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with ..\..\codegen\static_iterator.vbp ----------
'so change ..\..\codegen\static_iterator.vbp instead of this file



        Implements IComparable(Of iterator), IComparable

        Public Shared ReadOnly [end] As iterator

        Shared Sub New()
            [end] = New iterator()
        End Sub

        Private ReadOnly p As ref

#If False Then
        <MethodImpl(method_impl_options.aggressive_inlining)>
        Friend Sub New(ByVal that As ref)
#Else
        <MethodImpl(method_impl_options.aggressive_inlining)>
        Private Sub New(ByVal that As ref)
#End If
#If Not True Then
            assert(Not that Is Nothing)
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
#If True Then
            Return p Is Nothing
#Else
            Return p Is Nothing AndAlso
                   assert(object_compare(Me, [end]) = 0)
#End If
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
            If this.null_or_end() AndAlso that.null_or_end() Then
                Return True
            End If
            If this.null_or_end() OrElse that.null_or_end() Then
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
        Public Shared Operator +(ByVal this As iterator) As ref
            Return If(this = [end], Nothing, this.p)
        End Operator
    #End If
'finish ..\..\codegen\static_iterator.vbp --------

        '1. iterator / reverse_iterator are combined together
        '2. do not allow to - from end, it's not must-have
        '3. operator+ / operator- should not impact current instance, considering i = j + 1
        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Shared Operator +(ByVal this As iterator, ByVal that As Int32) As iterator
            If this.null_or_end() OrElse that = 0 Then
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
'finish ..\..\codegen\random_access_iterator.vbp --------

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
'finish ..\..\codegen\random_access_iterator.single_step.vbp --------

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Friend Sub New(ByVal owner As hasharray(Of T, _UNIQUE, _HASHER, _EQUALER), ByVal column As UInt32, ByVal row As UInt32)
            Me.New(assert_which.of(owner).is_not_null().ref_at(column, row))
        End Sub

        Private Function move_next() As iterator
            Dim i As UInt32 = 0
            Dim j As UInt32 = 0
            i = p.column
            j = p.row + uint32_1
            While i < p.column_count()
                While j < p.row_count(i)
                    Dim r As ref = Nothing
                    r = p.ref_at(i, j)
                    If Not r.empty() Then
                        Return New iterator(r)
                    End If
                    j += uint32_1
                End While
                j = uint32_0
                i += uint32_1
            End While
            Return [end]
        End Function

        Private Function move_prev() As iterator
            Dim i As UInt32 = 0
            Dim j As UInt32 = 0
            i = p.column
            j = p.row
            While True
                While j > uint32_0
                    j -= uint32_1
                    Dim r As ref = Nothing
                    r = p.ref_at(i, j)
                    If Not r.empty() Then
                        Return New iterator(r)
                    End If
                End While
                If i = uint32_0 Then
                    Exit While
                End If
                i -= uint32_1
                j = p.row_count(i)
            End While
            Return [end]
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Private Shared Function is_equal(ByVal this As ref, ByVal that As ref) As Boolean
            Return this.is_equal_to(that)
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Friend Function ref() As ref
            Return p
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function value() As T
            Return +p
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Shared Operator +(ByVal this As iterator) As T
            Return If(this = [end], Nothing, this.value())
        End Operator
    End Structure
End Class
'finish hasharray.iterator.vbp --------
