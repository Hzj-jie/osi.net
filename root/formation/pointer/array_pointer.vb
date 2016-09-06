
'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with array_pointer.vbp ----------
'so change array_pointer.vbp instead of this file



'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with pointer.vbp ----------
'so change pointer.vbp instead of this file



Imports System.Runtime.CompilerServices
Imports osi.root.lock
Imports osi.root.lock.slimlock
Imports osi.root.connector

Public Module _array_pointer
    Public Function make_array_pointer(Of T)(ByVal i As T()) As array_pointer(Of T)
        Return New array_pointer(Of T)(i)
    End Function

    Public Function make_array_pointers(Of T)(ByVal ParamArray i As T()()) As array_pointer(Of T)()
        Dim r() As array_pointer(Of T) = Nothing
        ReDim r(array_size(i) - 1)
        For j As Int32 = 0 To array_size(i) - 1
            r(j) = make_array_pointer(i(j))
        Next
        Return r
    End Function

    <Extension()> Public Function renew(Of T)(ByRef i As array_pointer(Of T)) As array_pointer(Of T)
        If i Is Nothing Then
            i = New array_pointer(Of T)()
        Else
            i.clear()
        End If
        Return i
    End Function
End Module

Public Class array_pointer(Of T)
    Implements IComparable, IComparable(Of array_pointer(Of T)), IComparable(Of T())

    Shared Sub New()
        Dim tp As Type = Nothing
        tp = GetType(T)
        If tp.IsValueType() Then
            assert(Not tp.implement(Of ilock)())
            assert(Not tp.implement(Of islimlock)())
            assert(Not tp Is GetType(singleentry))
            assert(Not tp Is GetType(forks))
        End If
    End Sub

    ' This event is not guaranteed to be executed when p is finalized, but if this object is the
    ' only reference of p, it would be eventually finalized afterward.
#If True Then
    Public Event finalized(ByVal p As T())
#Else
    Public Event finalized()
#End If

    Public Sub New()
        clear()
    End Sub

    Public Sub New(ByVal i As T())
        [set](i)
    End Sub

    Public Sub New(ByVal i As array_pointer(Of T))
        If i Is Nothing Then
            clear()
        Else
            [set](+i)
        End If
    End Sub


'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with array_pointer.override.vbp ----------
'so change array_pointer.override.vbp instead of this file


    Private Shared Function compare(ByVal i As T(), ByVal j As T()) As Int32
        Return memcmp(i, j)
    End Function

    Private Shared Function hash(ByVal i As T()) As Int32
        'WTF, the Array.GetHashCode returns some random number in .net 3.5
        Dim r As Int32 = 0
        For j As Int32 = 0 To min(32, array_size(i)) - 1
            If i(j) Is Nothing Then
                r = r Xor 0
            Else
                r = r Xor i(j).GetHashCode()
            End If
        Next
        Return r
    End Function
    

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with strong_pointer_override.vbp ----------
'so change strong_pointer_override.vbp instead of this file


    Private p As T()

    Public Sub clear()
        p = Nothing
    End Sub

    Public Function empty() As Boolean
        Return p Is Nothing
    End Function

    Public Function [get]() As T()
        Return p
    End Function

    Public Sub [set](ByVal i As T())
        p = i
    End Sub
'finish strong_pointer_override.vbp --------
'finish array_pointer.override.vbp --------

    Public Function release() As T()
        Dim r As T() = Nothing
        r = [get]()
        clear()
        Return r
    End Function

    Public Function CompareTo(ByVal obj As Object) As Int32 Implements IComparable.CompareTo
        Return CompareTo(cast(Of array_pointer(Of T))(obj, False))
    End Function

    Public Function CompareTo(ByVal that As array_pointer(Of T)) As Int32 _
                             Implements IComparable(Of array_pointer(Of T)).CompareTo
        Return CompareTo(+that)
    End Function

    Public Function CompareTo(ByVal that As T()) As Int32 _
                             Implements IComparable(Of T()).CompareTo
        Return compare([get](), that)
    End Function

#If Not (PocketPC OrElse Smartphone) Then
    Public Shared Operator ^(ByVal p As array_pointer(Of T), ByVal ji As Decimal) As Object
        On Error GoTo finish
        Dim p2 As Object = Nothing
        p2 = p
        Dim jumps As Int64 = 0
        jumps = Math.Truncate(ji)
        While jumps > 0
            p2 = +(cast(Of array_pointer(Of T))(p2, False))
            jumps -= 1
        End While
finish:
        Return p2
    End Operator
#End If

    Public Shared Operator +(ByVal p As array_pointer(Of T)) As T()
        Return If(p Is Nothing, Nothing, p.get())
    End Operator

    'special treatment for pointer, it compares reference equaling, instead of internal object
    Public Shared Operator <>(ByVal this As array_pointer(Of T), ByVal that As array_pointer(Of T)) As Boolean
        Return Not this = that
    End Operator

    Public Shared Operator =(ByVal this As array_pointer(Of T), ByVal that As array_pointer(Of T)) As Boolean
        If that Is Nothing OrElse that.get() Is Nothing Then
            Return this Is Nothing OrElse this.get() Is Nothing
        Else
            Return this = that.get()
        End If
    End Operator

    Public Shared Operator <>(ByVal this As array_pointer(Of T), ByVal that As T) As Boolean
        Return Not this = that
    End Operator

    Public Shared Operator =(ByVal this As array_pointer(Of T), ByVal that As T) As Boolean
        If this Is Nothing Then
            Return that Is Nothing
        Else
            Return object_compare(this.get(), that) = 0
        End If
    End Operator

    Public Shared Operator <>(ByVal this As T, ByVal that As array_pointer(Of T)) As Boolean
        Return Not this = that
    End Operator

    Public Shared Operator =(ByVal this As T, ByVal that As array_pointer(Of T)) As Boolean
        Return that = this
    End Operator

    Public Shared Operator =(ByVal this As array_pointer(Of T), ByVal obj As Object) As Boolean
        Dim that As array_pointer(Of T) = Nothing
        If cast(Of array_pointer(Of T))(obj, that) Then
            Return this = that
        Else
            Return this = cast(Of T)(obj, False)
        End If
    End Operator

    Public Shared Operator <>(ByVal this As array_pointer(Of T), ByVal obj As Object) As Boolean
        Return Not this = obj
    End Operator

    Public Shared Operator =(ByVal this As Object, ByVal that As array_pointer(Of T)) As Boolean
        Return that = this
    End Operator

    Public Shared Operator <>(ByVal this As Object, ByVal that As array_pointer(Of T)) As Boolean
        Return Not this = that
    End Operator

    Public Shared Operator <(ByVal this As array_pointer(Of T), ByVal that As T()) As Boolean
        If this Is Nothing Then
            Return False
        Else
            this.set(that)
            Return True
        End If
    End Operator

    Public Shared Operator >(ByVal this As array_pointer(Of T), ByVal that As T()) As Boolean
        Return assert(False)
    End Operator

    Public Shared Operator <(ByVal this As T(), ByVal that As array_pointer(Of T)) As Boolean
        Return that > this
    End Operator

    Public Shared Operator >(ByVal this As T(), ByVal that As array_pointer(Of T)) As Boolean
        Return that < this
    End Operator

    Public NotOverridable Overrides Function Equals(ByVal that As Object) As Boolean
        Return Me = that
    End Function

    'open for array_pointer
    Public NotOverridable Overrides Function GetHashCode() As Int32
        Dim i As T() = Nothing
        i = [get]()
        Return If(i Is Nothing, 0, hash(i))
    End Function

    Public NotOverridable Overrides Function ToString() As String
        Return Convert.ToString([get]())
    End Function

#If True Then
    Protected Overrides Sub Finalize()
#If True Then
        RaiseEvent finalized([get]())
#Else
        RaiseEvent finalized()
#End If
        MyBase.Finalize()
    End Sub
#End If
End Class
'finish pointer.vbp --------
'finish array_pointer.vbp --------
