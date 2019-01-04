
Option Explicit On
Option Infer Off
Option Strict On

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with weak_pointer.vbp ----------
'so change weak_pointer.vbp instead of this file



'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with pointer.vbp ----------
'so change pointer.vbp instead of this file



Imports System.Diagnostics.CodeAnalysis
Imports System.Runtime.CompilerServices
Imports osi.root.lock
Imports osi.root.lock.slimlock
Imports osi.root.connector

' TODO: Remove
Public Module _weak_pointer
    Public Function make_weak_pointer(Of T)(ByVal i As T) As weak_pointer(Of T)
        Return weak_pointer.of(i)
    End Function

    Public Function make_weak_pointers(Of T)(ByVal ParamArray i As T()) As weak_pointer(Of T)()
        Return weak_pointer.of(i)
    End Function

    <Extension()> Public Function renew(Of T)(ByRef i As weak_pointer(Of T)) As weak_pointer(Of T)
        If i Is Nothing Then
            i = New weak_pointer(Of T)()
        Else
            i.clear()
        End If
        Return i
    End Function
End Module

Public NotInheritable Class weak_pointer
    Public Shared Function [of](Of T)(ByVal i As T) As weak_pointer(Of T)
        Return New weak_pointer(Of T)(i)
    End Function

    Public Shared Function [of](Of T)(ByVal ParamArray i As T()) As weak_pointer(Of T)()
        Dim r() As weak_pointer(Of T) = Nothing
        ReDim r(array_size_i(i) - 1)
        For j As Int32 = 0 To array_size_i(i) - 1
            r(j) = make_weak_pointer(i(j))
        Next
        Return r
    End Function

    Private Sub New()
    End Sub
End Class

<SuppressMessage("Microsoft.Design", "BC42333")>
Public Class weak_pointer(Of T)
    Implements IComparable, IComparable(Of weak_pointer(Of T)), IComparable(Of T),
               ICloneable, ICloneable(Of weak_pointer(Of T))

    Shared Sub New()
        Dim tp As Type = Nothing
        tp = GetType(T)
        If tp.IsValueType() Then
            assert(Not tp.implement(Of ilock)())
            assert(Not tp.implement(Of islimlock)())
            assert(Not tp Is GetType(singleentry))
            assert(Not tp Is GetType(forks))
        End If
        bytes_serializer(Of weak_pointer(Of T)).forward_registration.from(Of T)()
    End Sub

    Public Shared Function move(ByVal that As weak_pointer(Of T)) As weak_pointer(Of T)
        If that Is Nothing Then
            Return Nothing
        Else
            Dim r As weak_pointer(Of T) = Nothing
            r = New weak_pointer(Of T)(that)
            that.clear()
            Return r
        End If
    End Function

    Public Sub New()
        clear()
    End Sub

    Public Sub New(ByVal i As T)
        [set](i)
    End Sub

    Public Sub New(ByVal i As weak_pointer(Of T))
        If i Is Nothing Then
            clear()
        Else
            [set](+i)
        End If
    End Sub


'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with weak_pointer.override.vbp ----------
'so change weak_pointer.override.vbp instead of this file




'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with single_obj_pointer_operator.vbp ----------
'so change single_obj_pointer_operator.vbp instead of this file


    Private Shared Function compare(ByVal this As T, ByVal that As T) As Int32
        Return connector.compare(this, that)
    End Function

    Private Shared Function hash(ByVal i As T) As Int32
        assert(Not i Is Nothing)
        Return i.GetHashCode()
    End Function
'finish single_obj_pointer_operator.vbp --------

    Private p As WeakReference
    
    Public Sub clear()
        p = Nothing
    End Sub

    Public Function empty() As Boolean
        Dim p As WeakReference = Nothing
        p = Me.p
        Return p Is Nothing OrElse
               Not p.IsAlive() OrElse
               p.Target() Is Nothing
    End Function

#If "T" = "T" Then
    Public Function [get](ByRef o As T) As Boolean
#Else
    Private Function [get](ByRef o As T) As Boolean
#End If
        Dim p As WeakReference = Nothing
        p = Me.p
        If p Is Nothing Then
            Return False
        Else
            Try
                o = DirectCast(p.Target(), T)
            Catch
                assert(False)
            End Try
            Return p.IsAlive()
        End If
    End Function

    Public Function alive() As Boolean
        Dim p As WeakReference = Nothing
        p = Me.p
        Return Not p Is Nothing AndAlso p.IsAlive()
    End Function

    Public Function [get]() As T
        Dim o As T = Nothing
        Return If([get](o), o, Nothing)
    End Function

#If "T" = "T" Then
    Public Sub [set](ByVal i As T)
#Else
    Private Sub [set](ByVal i As T)
#End If
        p = New WeakReference(i)
    End Sub
'finish weak_pointer.override.vbp --------

    Public Function release() As T
        Dim r As T = Nothing
        r = [get]()
        clear()
        Return r
    End Function

    Public Function CompareTo(ByVal obj As Object) As Int32 Implements IComparable.CompareTo
        Return CompareTo(cast(Of weak_pointer(Of T))(obj, False))
    End Function

    Public Function CompareTo(ByVal that As weak_pointer(Of T)) As Int32 _
                             Implements IComparable(Of weak_pointer(Of T)).CompareTo
        Return CompareTo(+that)
    End Function

    Public Function CompareTo(ByVal that As T) As Int32 _
                             Implements IComparable(Of T).CompareTo
        Return compare([get](), that)
    End Function

#If Not (PocketPC OrElse Smartphone) Then
    Public Shared Operator ^(ByVal p As weak_pointer(Of T), ByVal ji As Decimal) As Object
        On Error GoTo finish
        Dim p2 As Object = Nothing
        p2 = p
        Dim jumps As Int64 = 0
        jumps = CLng(Math.Truncate(ji))
        While jumps > 0
            p2 = +(cast(Of weak_pointer(Of T))(p2, False))
            jumps -= 1
        End While
finish:
        Return p2
    End Operator
#End If

    Public Function Clone() As Object Implements ICloneable.Clone
        Return CloneT()
    End Function

    Public Function CloneT() As weak_pointer(Of T) Implements ICloneable(Of weak_pointer(Of T)).Clone
        Return New weak_pointer(Of T)(Me)
    End Function

    Public Shared Operator +(ByVal p As weak_pointer(Of T)) As T
        Return If(p Is Nothing, Nothing, p.get())
    End Operator

    'special treatment for pointer, it compares reference equaling, instead of internal object
    Public Shared Operator <>(ByVal this As weak_pointer(Of T), ByVal that As weak_pointer(Of T)) As Boolean
        Return Not this = that
    End Operator

    Public Shared Operator =(ByVal this As weak_pointer(Of T), ByVal that As weak_pointer(Of T)) As Boolean
        If that Is Nothing OrElse that.get() Is Nothing Then
            Return this Is Nothing OrElse this.get() Is Nothing
        Else
            Return this = that.get()
        End If
    End Operator

    Public Shared Operator <>(ByVal this As weak_pointer(Of T), ByVal that As T) As Boolean
        Return Not this = that
    End Operator

    Public Shared Operator =(ByVal this As weak_pointer(Of T), ByVal that As T) As Boolean
        If this Is Nothing Then
            Return that Is Nothing
        Else
            Return object_compare(this.get(), that) = 0
        End If
    End Operator

    Public Shared Operator <>(ByVal this As T, ByVal that As weak_pointer(Of T)) As Boolean
        Return Not this = that
    End Operator

    Public Shared Operator =(ByVal this As T, ByVal that As weak_pointer(Of T)) As Boolean
        Return that = this
    End Operator

    Public Shared Operator =(ByVal this As weak_pointer(Of T), ByVal obj As Object) As Boolean
        Dim that As weak_pointer(Of T) = Nothing
        If cast(Of weak_pointer(Of T))(obj, that) Then
            Return this = that
        Else
            Return this = cast(Of T)(obj, False)
        End If
    End Operator

    Public Shared Operator <>(ByVal this As weak_pointer(Of T), ByVal obj As Object) As Boolean
        Return Not this = obj
    End Operator

    Public Shared Operator =(ByVal this As Object, ByVal that As weak_pointer(Of T)) As Boolean
        Return that = this
    End Operator

    Public Shared Operator <>(ByVal this As Object, ByVal that As weak_pointer(Of T)) As Boolean
        Return Not this = that
    End Operator

    Public Shared Operator <(ByVal this As weak_pointer(Of T), ByVal that As T) As Boolean
        If this Is Nothing Then
            Return False
        Else
            this.set(that)
            Return True
        End If
    End Operator

    Public Shared Operator >(ByVal this As weak_pointer(Of T), ByVal that As T) As Boolean
        Return assert(False)
    End Operator

    Public Shared Operator <(ByVal this As T, ByVal that As weak_pointer(Of T)) As Boolean
        Return that > this
    End Operator

    Public Shared Operator >(ByVal this As T, ByVal that As weak_pointer(Of T)) As Boolean
        Return that < this
    End Operator

    Public Shared Widening Operator CType(ByVal this As weak_pointer(Of T)) As Boolean
        Return Not this Is Nothing AndAlso Not this.empty()
    End Operator

    Public Shared Narrowing Operator CType(ByVal i As T) As weak_pointer(Of T)
        Return New weak_pointer(Of T)(i)
    End Operator

    Public Shared Narrowing Operator CType(ByVal i As weak_pointer(Of T)) As T
        Return +i
    End Operator

    Public Shared Operator Not(ByVal this As weak_pointer(Of T)) As Boolean
        Return this Is Nothing OrElse this.empty()
    End Operator

    Public NotOverridable Overrides Function Equals(ByVal that As Object) As Boolean
        Return Me = that
    End Function

    'open for array_pointer
    Public NotOverridable Overrides Function GetHashCode() As Int32
        Dim i As T = Nothing
        i = [get]()
        Return If(i Is Nothing, 0, hash(i))
    End Function

    Public NotOverridable Overrides Function ToString() As String
        Return Convert.ToString([get]())
    End Function
End Class
'finish pointer.vbp --------
'finish weak_pointer.vbp --------
