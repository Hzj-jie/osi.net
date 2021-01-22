
Option Explicit On
Option Infer Off
Option Strict On

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with weak_ref_ref.vbp ----------
'so change weak_ref_ref.vbp instead of this file



'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with ref.vbp ----------
'so change ref.vbp instead of this file



Imports System.Diagnostics.CodeAnalysis
Imports System.Runtime.CompilerServices
Imports osi.root.constants

Public Module _weak_ref_ref
    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function renew(Of T)(ByRef i As weak_ref_ref(Of T)) As weak_ref_ref(Of T)
        If i Is Nothing Then
            i = New weak_ref_ref(Of T)()
        Else
            i.clear()
        End If
        Return i
    End Function
End Module

Public NotInheritable Class weak_ref_refs
    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function [of](Of T)(ByVal ParamArray i As T()) As weak_ref_ref(Of T)()
        Dim r() As weak_ref_ref(Of T) = Nothing
        ReDim r(array_size_i(i) - 1)
        For j As Int32 = 0 To array_size_i(i) - 1
            r(j) = weak_ref_ref.[of](i(j))
        Next
        Return r
    End Function

    Private Sub New()
    End Sub
End Class

Public NotInheritable Class weak_ref_ref
    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function [of](Of T)(ByVal i As T) As weak_ref_ref(Of T)
        Return New weak_ref_ref(Of T)(i)
    End Function

    Private Sub New()
    End Sub
End Class

<SuppressMessage("Microsoft.Design", "BC42333")>
Public Class weak_ref_ref(Of T)
#Disable Warning BC42333
    Implements IComparable,
               IComparable(Of weak_ref_ref(Of T)),
               IComparable(Of T),
               ICloneable,
               ICloneable(Of weak_ref_ref(Of T)),
               IEquatable(Of weak_ref_ref(Of T)),
               IEquatable(Of T)
#Enable Warning BC42333

    Shared Sub New()
        bytes_serializer(Of weak_ref_ref(Of T)).forward_registration.from(Of T)()
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function move(ByVal that As weak_ref_ref(Of T)) As weak_ref_ref(Of T)
        If that Is Nothing Then
            Return Nothing
        End If
        Dim r As weak_ref_ref(Of T) = Nothing
        r = New weak_ref_ref(Of T)(that)
        that.clear()
        Return r
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub New()
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub New(ByVal i As T)
        [set](i)
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub New(ByVal i As weak_ref_ref(Of T))
        [set](+i)
    End Sub


'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with weak_ref_ref.override.vbp ----------
'so change weak_ref_ref.override.vbp instead of this file



'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with weak_ref.override.vbp ----------
'so change weak_ref.override.vbp instead of this file




'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with single_obj_ref_operator.vbp ----------
'so change single_obj_ref_operator.vbp instead of this file


    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Shared Function compare(ByVal this As T, ByVal that As T) As Int32
        Return connector.compare(this, that)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Shared Function hash(ByVal i As T) As Int32
        assert(Not i Is Nothing)
        Return i.GetHashCode()
    End Function
'finish single_obj_ref_operator.vbp --------

    Private p As WeakReference
    
    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function clear() As T
        Dim r As T = Nothing
        r = [get]()
        p = Nothing
        Return r
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function empty() As Boolean
        Dim p As WeakReference = Nothing
        p = Me.p
        Return p Is Nothing OrElse
               Not p.IsAlive() OrElse
               p.Target() Is Nothing
    End Function

#If "ref(Of T)" = "T" Then
    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function [get](ByRef o As ref(Of T)) As Boolean
#Else
    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Function [get](ByRef o As ref(Of T)) As Boolean
#End If
        Dim p As WeakReference = Nothing
        p = Me.p
        If p Is Nothing Then
            Return False
        End If
        Try
            o = DirectCast(p.Target(), ref(Of T))
        Catch
            assert(False)
        End Try
        Return p.IsAlive()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function alive() As Boolean
        Dim p As WeakReference = Nothing
        p = Me.p
        Return Not p Is Nothing AndAlso p.IsAlive()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function [get]() As T
        Dim o As T = Nothing
        Return If([get](o), o, Nothing)
    End Function

#If "ref(Of T)" = "T" Then
    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub [set](ByVal i As ref(Of T))
#Else
    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Sub [set](ByVal i As ref(Of T))
#End If
        p = New WeakReference(i)
    End Sub
'finish weak_ref.override.vbp --------

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function [get](ByRef o As T) As Boolean
        Dim p As ref(Of T) = Nothing
        If [get](p) AndAlso Not p Is Nothing AndAlso Not p.empty() Then
            o = p.get()
            Return True
        End If
        Return False
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub [set](ByVal i As T)
        [set](ref.of(i))
    End Sub
'finish weak_ref_ref.override.vbp --------

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function CompareTo(ByVal obj As Object) As Int32 Implements IComparable.CompareTo
        Return CompareTo(cast(Of weak_ref_ref(Of T))(obj, False))
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function CompareTo(ByVal that As weak_ref_ref(Of T)) As Int32 _
                             Implements IComparable(Of weak_ref_ref(Of T)).CompareTo
        Return CompareTo(+that)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function CompareTo(ByVal that As T) As Int32 _
                             Implements IComparable(Of T).CompareTo
        Return compare([get](), that)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Overloads Function Equals(ByVal that As weak_ref_ref(Of T)) As Boolean _
                                    Implements IEquatable(Of weak_ref_ref(Of T)).Equals
        Return CompareTo(that) = 0
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Overloads Function Equals(ByVal that As T) As Boolean _
                                    Implements IEquatable(Of T).Equals
        Return CompareTo(that) = 0
    End Function

#If Not (PocketPC OrElse Smartphone) Then
    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Operator ^(ByVal p As weak_ref_ref(Of T), ByVal ji As Decimal) As Object
        On Error GoTo finish
        Dim p2 As Object = Nothing
        p2 = p
        Dim jumps As Int64 = 0
        jumps = CLng(Math.Truncate(ji))
        While jumps > 0
            p2 = +(cast(Of weak_ref_ref(Of T))(p2, False))
            jumps -= 1
        End While
finish:
        Return p2
    End Operator
#End If

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function Clone() As Object Implements ICloneable.Clone
        Return CloneT()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function CloneT() As weak_ref_ref(Of T) Implements ICloneable(Of weak_ref_ref(Of T)).Clone
        Return New weak_ref_ref(Of T)(Me)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Operator +(ByVal p As weak_ref_ref(Of T)) As T
        Return If(p Is Nothing, Nothing, p.get())
    End Operator

    'special treatment for ref, it compares reference equaling, instead of internal object
    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Operator <>(ByVal this As weak_ref_ref(Of T), ByVal that As weak_ref_ref(Of T)) As Boolean
        Return Not this = that
    End Operator

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Operator =(ByVal this As weak_ref_ref(Of T), ByVal that As weak_ref_ref(Of T)) As Boolean
        If that Is Nothing OrElse that.get() Is Nothing Then
            Return this Is Nothing OrElse this.get() Is Nothing
        End If
        Return this = that.get()
    End Operator

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Operator <>(ByVal this As weak_ref_ref(Of T), ByVal that As T) As Boolean
        Return Not this = that
    End Operator

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Operator =(ByVal this As weak_ref_ref(Of T), ByVal that As T) As Boolean
        If this Is Nothing Then
            Return that Is Nothing
        End If
        Return object_compare(this.get(), that) = 0
    End Operator

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Operator <>(ByVal this As T, ByVal that As weak_ref_ref(Of T)) As Boolean
        Return Not this = that
    End Operator

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Operator =(ByVal this As T, ByVal that As weak_ref_ref(Of T)) As Boolean
        Return that = this
    End Operator

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Operator =(ByVal this As weak_ref_ref(Of T), ByVal obj As Object) As Boolean
        Dim that As weak_ref_ref(Of T) = Nothing
        If cast(Of weak_ref_ref(Of T))(obj, that) Then
            Return this = that
        End If
        Return this = cast(Of T)(obj, False)
    End Operator

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Operator <>(ByVal this As weak_ref_ref(Of T), ByVal obj As Object) As Boolean
        Return Not this = obj
    End Operator

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Operator =(ByVal this As Object, ByVal that As weak_ref_ref(Of T)) As Boolean
        Return that = this
    End Operator

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Operator <>(ByVal this As Object, ByVal that As weak_ref_ref(Of T)) As Boolean
        Return Not this = that
    End Operator

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Operator <(ByVal this As weak_ref_ref(Of T), ByVal that As T) As Boolean
        If this Is Nothing Then
            Return False
        End If
        this.set(that)
        Return True
    End Operator

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Operator >(ByVal this As weak_ref_ref(Of T), ByVal that As T) As Boolean
        Return assert(False)
    End Operator

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Operator <(ByVal this As T, ByVal that As weak_ref_ref(Of T)) As Boolean
        Return that > this
    End Operator

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Operator >(ByVal this As T, ByVal that As weak_ref_ref(Of T)) As Boolean
        Return that < this
    End Operator

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Widening Operator CType(ByVal this As weak_ref_ref(Of T)) As Boolean
        Return Not this Is Nothing AndAlso Not this.empty()
    End Operator

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Narrowing Operator CType(ByVal i As T) As weak_ref_ref(Of T)
        Return New weak_ref_ref(Of T)(i)
    End Operator

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Narrowing Operator CType(ByVal i As weak_ref_ref(Of T)) As T
        Return +i
    End Operator

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Operator Not(ByVal this As weak_ref_ref(Of T)) As Boolean
        Return this Is Nothing OrElse this.empty()
    End Operator

    Public NotOverridable Overrides Function Equals(ByVal that As Object) As Boolean
        Return Me = that
    End Function

    'open for array_ref
    Public NotOverridable Overrides Function GetHashCode() As Int32
        Dim i As T = Nothing
        i = [get]()
        Return If(i Is Nothing, 0, hash(i))
    End Function

    Public NotOverridable Overrides Function ToString() As String
        Return Convert.ToString([get]())
    End Function
End Class
'finish ref.vbp --------
'finish weak_ref_ref.vbp --------
