
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.template

Partial Public Class unordered_set(Of
                 T,
                 _HASHER As _to_uint32(Of T),
                 _EQUALER As _equaler(Of T))
    Inherits hasharray(Of T, _true, _HASHER, _EQUALER)
    Implements ICloneable, ICloneable(Of unordered_set(Of T, _HASHER, _EQUALER))

    Public Sub New()
        MyBase.New()
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Protected Shadows Function clone(Of R As unordered_set(Of T, _HASHER, _EQUALER))() As R
        Return MyBase.clone(Of R)()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shadows Function Clone() As Object Implements ICloneable.Clone
        Return CloneT()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shadows Function CloneT() As unordered_set(Of T, _HASHER, _EQUALER) _
            Implements ICloneable(Of unordered_set(Of T, _HASHER, _EQUALER)).Clone
        Return clone(Of unordered_set(Of T, _HASHER, _EQUALER))()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Shadows Function move(ByVal v As unordered_set(Of T, _HASHER, _EQUALER)) _
                                       As unordered_set(Of T, _HASHER, _EQUALER)
        Return hasharray(Of T, _true, _HASHER, _EQUALER) _
                   .move(Of unordered_set(Of T, _HASHER, _EQUALER))(v)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Protected Shared Shadows Function move(Of R As unordered_set(Of T, _HASHER, _EQUALER))(ByVal v As R) As R
        Return hasharray(Of T, _true, _HASHER, _EQUALER).move(Of R)(v)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Shadows Function swap(ByVal this As unordered_set(Of T, _HASHER, _EQUALER),
                                        ByVal that As unordered_set(Of T, _HASHER, _EQUALER)) As Boolean
        Return hasharray(Of T, _true, _HASHER, _EQUALER).swap(this, that)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shadows Function [erase](ByVal it As iterator) As iterator
        Return MyBase.erase(it)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shadows Function [erase](ByVal value As T) As Boolean
        Return MyBase.erase(value) = uint32_1
    End Function
End Class

Partial Public NotInheritable Class unordered_set(Of T)
    Inherits unordered_set(Of T, fast_to_uint32(Of T), default_equaler(Of T))
    Implements ICloneable, ICloneable(Of unordered_set(Of T)), IEquatable(Of unordered_set(Of T))

    Public Sub New()
        MyBase.New()
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shadows Function Clone() As Object Implements ICloneable.Clone
        Return CloneT()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shadows Function CloneT() As unordered_set(Of T) _
            Implements ICloneable(Of unordered_set(Of T)).Clone
        Return MyBase.clone(Of unordered_set(Of T))()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Shadows Function move(ByVal v As unordered_set(Of T)) _
                                       As unordered_set(Of T)
        Return unordered_set(Of T, fast_to_uint32(Of T), default_equaler(Of T)) _
                  .move(Of unordered_set(Of T))(v)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Shadows Function swap(ByVal this As unordered_set(Of T),
                                        ByVal that As unordered_set(Of T)) As Boolean
        Return unordered_set(Of T, fast_to_uint32(Of T), default_equaler(Of T)).swap(this, that)
    End Function
End Class
