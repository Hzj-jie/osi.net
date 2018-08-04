
Option Explicit On
Option Infer Off
Option Strict On

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with unordered_set2.vbp ----------
'so change unordered_set2.vbp instead of this file



'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with unordered_set.vbp ----------
'so change unordered_set.vbp instead of this file



Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.template

Public Class unordered_set2(Of _
                 T,
                 _HASHER As _to_uint32(Of T),
                 _EQUALER As _equaler(Of T))
    Inherits hashtable(Of T, _true, _HASHER, _EQUALER)
    Implements ICloneable, ICloneable(Of unordered_set2(Of T, _HASHER, _EQUALER))


'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with hashtable.copy_constructor.vbp ----------
'so change hashtable.copy_constructor.vbp instead of this file


    <copy_constructor()>
    Protected Sub New(ByVal v As vector(Of array(Of hasher_node(Of T))),
                      ByVal s As UInt32,
                      ByVal c As UInt32)
        MyBase.New(v, s, c)
    End Sub
'finish hashtable.copy_constructor.vbp --------

    Public Sub New()
        MyBase.New()
    End Sub

    Protected Shadows Function clone(Of R As unordered_set2(Of T, _HASHER, _EQUALER))() As R
        Return MyBase.clone(Of R)()
    End Function

    Public Shadows Function Clone() As Object Implements ICloneable.Clone
        Return CloneT()
    End Function

    Public Shadows Function CloneT() As unordered_set2(Of T, _HASHER, _EQUALER) _
            Implements ICloneable(Of unordered_set2(Of T, _HASHER, _EQUALER)).Clone
        Return clone(Of unordered_set2(Of T, _HASHER, _EQUALER))()
    End Function

    Public Shared Shadows Function move(ByVal v As unordered_set2(Of T, _HASHER, _EQUALER)) _
                                       As unordered_set2(Of T, _HASHER, _EQUALER)
        Return hashtable(Of T, _true, _HASHER, _EQUALER) _
                   .move(Of unordered_set2(Of T, _HASHER, _EQUALER))(v)
    End Function

    Protected Shared Shadows Function move(Of R As unordered_set2(Of T, _HASHER, _EQUALER))(ByVal v As R) As R
        Return hashtable(Of T, _true, _HASHER, _EQUALER).move(Of R)(v)
    End Function

    Public Shared Shadows Function swap(ByVal this As unordered_set2(Of T, _HASHER, _EQUALER),
                                        ByVal that As unordered_set2(Of T, _HASHER, _EQUALER)) As Boolean
        Return hashtable(Of T, _true, _HASHER, _EQUALER).swap(this, that)
    End Function

    Public Shadows Function [erase](ByVal it As iterator) As Boolean
        Return MyBase.erase(it)
    End Function

    Public Shadows Function [erase](ByVal value As T) As Boolean
        Return MyBase.erase(value) = uint32_1
    End Function
End Class

Public Class unordered_set2(Of T)
    Inherits unordered_set2(Of T, fast_to_uint32(Of T), default_equaler(Of T))
    Implements ICloneable, ICloneable(Of unordered_set2(Of T))


'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with hashtable.copy_constructor.vbp ----------
'so change hashtable.copy_constructor.vbp instead of this file


    <copy_constructor()>
    Protected Sub New(ByVal v As vector(Of array(Of hasher_node(Of T))),
                      ByVal s As UInt32,
                      ByVal c As UInt32)
        MyBase.New(v, s, c)
    End Sub
'finish hashtable.copy_constructor.vbp --------

    Public Sub New()
        MyBase.New()
    End Sub

    Protected Shadows Function clone(Of R As unordered_set2(Of T))() As R
        Return MyBase.clone(Of R)()
    End Function

    Public Shadows Function Clone() As Object Implements ICloneable.Clone
        Return CloneT()
    End Function

    Public Shadows Function CloneT() As unordered_set2(Of T) _
            Implements ICloneable(Of unordered_set2(Of T)).Clone
        Return MyBase.clone(Of unordered_set2(Of T))()
    End Function

    Public Shared Shadows Function move(ByVal v As unordered_set2(Of T)) _
                                       As unordered_set2(Of T)
        Return unordered_set2(Of T, fast_to_uint32(Of T), default_equaler(Of T)) _
                  .move(Of unordered_set2(Of T))(v)
    End Function

    Public Shared Shadows Function swap(ByVal this As unordered_set2(Of T),
                                        ByVal that As unordered_set2(Of T)) As Boolean
        Return unordered_set2(Of T, fast_to_uint32(Of T), default_equaler(Of T)).swap(this, that)
    End Function
End Class
'finish unordered_set.vbp --------
'finish unordered_set2.vbp --------
