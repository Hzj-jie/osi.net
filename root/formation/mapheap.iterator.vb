
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
Imports osi.root.constants
Imports osi.root.connector
'finish iterator.imports.vbp --------

Public Module _mapheap_iterator

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with iterator.ext.vbp ----------
'so change iterator.ext.vbp instead of this file


    <Extension()> Public Function null_or_end(Of mapKey As IComparable(Of mapKey), heapKey As IComparable(Of heapKey))(ByVal this As mapheap(Of mapKey, heapKey).iterator) As Boolean
        Return this Is Nothing OrElse this.is_end()
    End Function
'finish iterator.ext.vbp --------
End Module

Partial Public Class mapheap(Of mapKey As IComparable(Of mapKey), heapKey As IComparable(Of heapKey))
    Public Class iterator

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with static_iterator.vbp ----------
'so change static_iterator.vbp instead of this file


        Implements IComparable(Of iterator), IComparable

        Public Shared ReadOnly [end] As iterator

        Shared Sub New()
            [end] = New iterator()
        End Sub

        Private ReadOnly p As pair(Of heapKey, mapKey)

        Private Sub New()
        End Sub

#If True Then
        Friend Sub New(ByVal that As pair(Of heapKey, mapKey))
#Else
        Private Sub New(ByVal that As pair(Of heapKey, mapKey))
#End If
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

    #If True Then
        Public Shared Operator +(ByVal this As iterator) As pair(Of heapKey, mapKey)
            Return If(this = [end], Nothing, this.p)
        End Operator
    #End If
'finish static_iterator.vbp --------
    End Class
End Class
'finish mapheap.iterator.vbp --------
