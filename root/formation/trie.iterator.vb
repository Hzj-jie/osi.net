
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
Imports osi.root.constants
Imports osi.root.connector
'finish iterator.imports.vbp --------
Imports osi.root.template

Public Module _trie_iterator

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with iterator.ext.vbp ----------
'so change iterator.ext.vbp instead of this file


    <Extension()> Public Function null_or_end(Of keyT, valueT, _child_count As _int64, _key_to_index As _to_uint32(Of keyT))(ByVal this As trie(Of keyT, valueT, _child_count, _key_to_index).iterator) As Boolean
        Return this Is Nothing OrElse this.is_end()
    End Function
'finish iterator.ext.vbp --------
End Module

Partial Public Class trie(Of keyT, valueT, _child_count As _int64, _key_to_index As _to_uint32(Of keyT))
    Public Class iterator

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with random_access_iterator.single_step.vbp ----------
'so change random_access_iterator.single_step.vbp instead of this file



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

#If True Then
        Friend Sub New(ByVal that As node)
#Else
        Private Sub New(ByVal that As node)
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
'finish random_access_iterator.single_step.vbp --------

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
    End Class
End Class
'finish trie.iterator.vbp --------
