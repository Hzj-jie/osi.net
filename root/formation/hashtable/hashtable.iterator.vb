
Option Explicit On
Option Infer Off
Option Strict On

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with hashtable.iterator.vbp ----------
'so change hashtable.iterator.vbp instead of this file



'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with ..\..\codegen\iterator.imports.vbp ----------
'so change ..\..\codegen\iterator.imports.vbp instead of this file


Imports System.Runtime.CompilerServices
Imports osi.root.constants
Imports osi.root.connector
'finish ..\..\codegen\iterator.imports.vbp --------
Imports osi.root.template

Public Module _hashtable_iterator

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with ..\..\codegen\iterator.ext.vbp ----------
'so change ..\..\codegen\iterator.ext.vbp instead of this file


    <Extension()> Public Function null_or_end(Of T, _UNIQUE As _boolean, _HASHER As _to_uint32(Of T), _EQUALER As _equaler(Of T))(ByVal this As hashtable(Of T, _UNIQUE, _HASHER, _EQUALER).iterator) As Boolean
        Return this.is_null() OrElse this.is_end()
    End Function
'finish ..\..\codegen\iterator.ext.vbp --------
End Module

Partial Public Class hashtable(Of T,
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
        Friend Sub New(ByVal that As ref)
#Else
        Private Sub New(ByVal that As ref)
#End If
            assert(Not that Is Nothing)
            p = that
        End Sub

        Public Function is_end() As Boolean
            If type_info(Of iterator).is_valuetype Then
                Return p Is Nothing
            Else
                Return p Is Nothing AndAlso
                       (Not isdebugmode() OrElse
                        assert(object_compare(Me, [end]) = 0))
            End If
        End Function

        Public Function is_not_end() As Boolean
            Return Not is_end()
        End Function

#If False Then
        Private Shared Function is_equal(ByVal this As ref, ByVal that As ref) As Boolean
            Return object_compare(this, that) = 0
        End Function
#End If

        Public Shared Operator =(ByVal this As iterator, ByVal that As iterator) As Boolean
            If this.null_or_end() AndAlso that.null_or_end() Then
                Return True
            ElseIf this.null_or_end() OrElse that.null_or_end() Then
                Return False
            Else
                assert(Not this.is_null() AndAlso Not this.is_null())
                Return is_equal(this.p, that.p)
            End If
        End Operator

        Public Shared Operator <>(ByVal this As iterator, ByVal that As iterator) As Boolean
            Return Not (this = that)
        End Operator

        Public Function CompareTo(ByVal other As iterator) As Int32 Implements IComparable(Of iterator).CompareTo
            Return If(Me = other, 0, -1)
        End Function

        Public Function CompareTo(ByVal other As Object) As Int32 Implements IComparable.CompareTo
            Return CompareTo(cast(Of iterator)(other, False))
        End Function

    #If False Then
        Public Shared Operator +(ByVal this As iterator) As ref
            Return If(this = [end], Nothing, this.p)
        End Operator
    #End If
'finish ..\..\codegen\static_iterator.vbp --------

        '1. iterator / reverse_iterator are combined together
        '2. do not allow to - from end, it's not must-have
        '3. operator+ / operator- should not impact current instance, considering i = j + 1
        Public Shared Operator +(ByVal this As iterator, ByVal that As Int32) As iterator
            If this.null_or_end() OrElse that = 0 Then
                Return this
            ElseIf that > 0 Then
                Return this.move_next(CUInt(that))
            Else
                assert(that < 0)
                Return this.move_prev(CUInt(-that))
            End If
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

        Friend Sub New(ByVal owner As hashtable(Of T, _UNIQUE, _HASHER, _EQUALER),
                       ByVal row As UInt32,
                       ByVal column As UInt32)
            Me.New(assert_not_nothing_return(owner).ref_at(row, column))
        End Sub

        Private Function move_next() As iterator
            Dim i As UInt32 = 0
            i = p.cell_id() + uint32_1
            While i < p.cell_count()
                If Not p.cell(i) Is Nothing Then
                    Return New iterator(p.ref_at(i))
                End If
                i += uint32_1
            End While
            Return [end]
        End Function

        Private Function move_prev() As iterator
            If p.cell_id() > uint32_0 Then
                Dim i As UInt32 = 0
                i = p.cell_id() - uint32_1
                While i >= uint32_0
                    If Not p.cell(i) Is Nothing Then
                        Return New iterator(p.ref_at(i))
                    End If
                    i -= uint32_1
                End While
            End If
            Return [end]
        End Function

        Private Shared Function is_equal(ByVal this As ref, ByVal that As ref) As Boolean
            assert(Not this Is Nothing AndAlso Not that Is Nothing)
            Return this.is_equal_to(that)
        End Function

        Friend Function ref() As ref
            Return p
        End Function

        Public Function value() As T
            Return +p
        End Function

        Public Shared Operator +(ByVal this As iterator) As T
            Return If(this = [end], Nothing, this.value())
        End Operator
    End Structure
End Class
'finish hashtable.iterator.vbp --------
