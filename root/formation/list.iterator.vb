
'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with list.iterator.vbp ----------
'so change list.iterator.vbp instead of this file



'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with iterator.imports.vbp ----------
'so change iterator.imports.vbp instead of this file


Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.constants
Imports osi.root.connector
'finish iterator.imports.vbp --------

Public Module _list_iterator

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with iterator.ext.vbp ----------
'so change iterator.ext.vbp instead of this file


    <Extension()> Public Function null_or_end(Of T)(ByVal this As list(Of T).iterator) As Boolean
        Return this Is Nothing OrElse this.is_end()
    End Function
'finish iterator.ext.vbp --------
End Module

Partial Public Class list(Of T)
    Public Class iterator

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with random_access_iterator.vbp ----------
'so change random_access_iterator.vbp instead of this file



'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with static_iterator.vbp ----------
'so change static_iterator.vbp instead of this file


        Implements IComparable(Of iterator), IComparable

        Public Shared ReadOnly [end] As iterator

        Shared Sub New()
            [end] = New iterator()
        End Sub

        Private ReadOnly p As node

        Private Sub New()
        End Sub

        Friend Sub New(ByVal that As node)
            assert(Not that Is Nothing)
            p = that
        End Sub

        Public Function is_end() As Boolean
            Return p Is Nothing AndAlso
                   (Not isdebugmode() OrElse
                    assert(object_compare(Me, [end]) = 0))
        End Function

        Public Function is_not_end() As Boolean
            Return Not is_end()
        End Function

        Public Shared Operator =(ByVal this As iterator, ByVal that As iterator) As Boolean
            If this.null_or_end() AndAlso that.null_or_end() Then
                Return True
            ElseIf this.null_or_end() OrElse that.null_or_end() Then
                Return False
            Else
                assert(Not this Is Nothing AndAlso Not that Is Nothing)
                Return object_compare(this.p, that.p) = 0
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

    #If False
        Public Shared Operator +(ByVal this As iterator) As node
            Return If(this = [end], Nothing, this.p)
        End Operator
    #End If
'finish static_iterator.vbp --------

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
'finish random_access_iterator.vbp --------

        Private Function move_prev(ByVal s As UInt32) As iterator
            assert(s > uint32_0)
            Dim n As node = Nothing
            n = p
            While s > 0
                n = n.last()
                If n Is Nothing Then
                    Return [end]
                End If
                s -= uint32_1
            End While
            Return New iterator(n)
        End Function

        Private Function move_next(ByVal s As UInt32) As iterator
            assert(s > uint32_0)
            Dim n As node = Nothing
            n = p
            While s > 0
                n = n.next()
                If n Is Nothing Then
                    Return [end]
                End If
                s -= uint32_1
            End While
            Return New iterator(n)
        End Function

        Public Shared Operator +(ByVal this As iterator) As T
            Return If(this = [end], Nothing, +(this.p))
        End Operator

        Friend Function node() As node
            Return p
        End Function
    End Class
End Class
'finish list.iterator.vbp --------
