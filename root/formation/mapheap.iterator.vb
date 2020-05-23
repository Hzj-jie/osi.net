
Option Explicit On
Option Infer Off
Option Strict On

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with mapheap.iterator.vbp ----------
'so change mapheap.iterator.vbp instead of this file



'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with iterator.imports.vbp ----------
'so change iterator.imports.vbp instead of this file


Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants
'finish iterator.imports.vbp --------


Public Module _mapheap_iterator

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with iterator.ext.vbp ----------
'so change iterator.ext.vbp instead of this file


    <Extension()> Public Function null_or_end(Of MAP_KEY As IComparable(Of MAP_KEY), HEAP_KEY As IComparable(Of HEAP_KEY))(ByVal this As mapheap(Of MAP_KEY, HEAP_KEY).iterator) As Boolean
#If True Then
        Return this.is_end()
#Else
        Return this.is_null() OrElse this.is_end()
#End If
    End Function
'finish iterator.ext.vbp --------
End Module

Partial Public Class mapheap(Of MAP_KEY As IComparable(Of MAP_KEY), HEAP_KEY As IComparable(Of HEAP_KEY))
    Public Structure iterator

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with static_iterator.vbp ----------
'so change static_iterator.vbp instead of this file



        Implements IComparable(Of iterator), IComparable

        Public Shared ReadOnly [end] As iterator

        Shared Sub New()
            [end] = New iterator()
        End Sub

        Private ReadOnly p As pair(Of HEAP_KEY, MAP_KEY)

#If True Then
        <MethodImpl(method_impl_options.aggressive_inlining)>
        Friend Sub New(ByVal that As pair(Of HEAP_KEY, MAP_KEY))
#Else
        <MethodImpl(method_impl_options.aggressive_inlining)>
        Private Sub New(ByVal that As pair(Of HEAP_KEY, MAP_KEY))
#End If
#If Not True Then
            assert(Not that Is Nothing)
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

#If True Then
        Private Shared Function is_equal(ByVal this As pair(Of HEAP_KEY, MAP_KEY), ByVal that As pair(Of HEAP_KEY, MAP_KEY)) As Boolean
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

    #If True Then
        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Shared Operator +(ByVal this As iterator) As pair(Of HEAP_KEY, MAP_KEY)
            Return If(this = [end], Nothing, this.p)
        End Operator
    #End If
'finish static_iterator.vbp --------
    End Structure
End Class
'finish mapheap.iterator.vbp --------
