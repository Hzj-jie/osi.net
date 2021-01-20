
Option Explicit On
Option Infer Off
Option Strict On

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with unordered_set.vbp ----------
'so change unordered_set.vbp instead of this file


Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.template


Partial Public Class unordered_set(Of _
                 T,
                 _HASHER As _to_uint32(Of T),
                 _EQUALER As _equaler(Of T))
    Inherits hasharray(Of T, _true, _HASHER, _EQUALER)
    Implements ICloneable, ICloneable(Of unordered_set(Of T, _HASHER, _EQUALER))


'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with hasharray.copy_constructor.vbp ----------
'so change hasharray.copy_constructor.vbp instead of this file


    <copy_constructor()>
    Protected Sub New(ByVal v As array(Of vector(Of hasher_node(Of T , _HASHER, _EQUALER))),
                      ByVal s As UInt32,
                      ByVal c As UInt32)
        MyBase.New(v, s, c)
    End Sub

'finish hasharray.copy_constructor.vbp --------

End Class

Partial Public NotInheritable Class unordered_set(Of T)
    Inherits unordered_set(Of T, fast_to_uint32(Of T), default_equaler(Of T))
    Implements ICloneable, ICloneable(Of unordered_set(Of T)), IEquatable(Of unordered_set(Of T))


'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with hasharray.copy_constructor.vbp ----------
'so change hasharray.copy_constructor.vbp instead of this file


    <copy_constructor()>
    Protected Sub New(ByVal v As array(Of vector(Of hasher_node(Of T , fast_to_uint32(Of T), default_equaler(Of T)))),
                      ByVal s As UInt32,
                      ByVal c As UInt32)
        MyBase.New(v, s, c)
    End Sub

'finish hasharray.copy_constructor.vbp --------


'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with ..\binary_tree\codegen\set.container_operator.vbp ----------
'so change ..\binary_tree\codegen\set.container_operator.vbp instead of this file



    Shared Sub New()
        container_operator(Of [unordered_set](Of T), T).size(
                Function(ByVal i As [unordered_set](Of T)) As UInt32
                    assert(Not i Is Nothing)
                    Return i.size()
                End Function)
        container_operator(Of [unordered_set](Of T), T).emplace(
                Function(ByVal i As [unordered_set](Of T), ByVal j As T) As Boolean
                    assert(Not i Is Nothing)
                    Return i.emplace(j).second
                End Function)
        container_operator(Of [unordered_set](Of T), T).enumerate(
                Function(ByVal i As [unordered_set](Of T)) As container_operator(Of T).enumerator
                    assert(Not i Is Nothing)
                    Return New enumerator(i)
                End Function)
        container_operator(Of [unordered_set](Of T), T).clear(
                Sub(ByVal i As [unordered_set](Of T))
                    assert(Not i Is Nothing)
                    i.clear()
                End Sub)
        bytes_serializer(Of [unordered_set](Of T)).container(Of T).register()
    End Sub

'finish ..\binary_tree\codegen\set.container_operator.vbp --------

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with ..\binary_tree\codegen\set.compare.vbp ----------
'so change ..\binary_tree\codegen\set.compare.vbp instead of this file


    ' Returns a new [unordered_set] containing all elements in Me but not in that.
    Public Function exclude(ByVal that As [unordered_set](Of T)) As [unordered_set](Of T)
        If that Is Nothing OrElse that.empty() Then
            Return CloneT()
        End If
  
        Dim r As [unordered_set](Of T) = Nothing
        r = New [unordered_set](Of T)()
        Dim it As [unordered_set](Of T).iterator = Nothing
        it = begin()
        While it <> [end]()
            If that.find(+it) = that.end() Then
                assert(r.insert(+it).second)
            End If
            it += 1
        End While
        Return r
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Overloads Function Equals(ByVal that As [unordered_set](Of T)) As Boolean _
            Implements IEquatable(Of [unordered_set](Of T)).Equals
        If that Is Nothing OrElse that.empty() Then
            Return empty()
        End If
        Return exclude(that).empty() AndAlso that.exclude(Me).empty()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Overrides Function Equals(ByVal that As Object) As Boolean
        Return Equals(cast(Of [unordered_set](Of T))(that, False))
    End Function

'finish ..\binary_tree\codegen\set.compare.vbp --------

End Class
'finish unordered_set.vbp --------
