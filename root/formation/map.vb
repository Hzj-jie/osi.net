
'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with bmap2.vbp ----------
'so change bmap2.vbp instead of this file



'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with map.vbp ----------
'so change map.vbp instead of this file


Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Public Class map(Of KEY_T, VALUE_T)
    Inherits bbst(Of first_const_pair(Of KEY_T, VALUE_T))
    Implements ICloneable, ICloneable(Of map(Of KEY_T, VALUE_T))

    Public Sub New()
        MyBase.New(AddressOf first_compare)
    End Sub

    Private Shared Function first_compare(ByVal this As first_const_pair(Of KEY_T, VALUE_T),
                                          ByVal that As first_const_pair(Of KEY_T, VALUE_T)) As Int32
        Dim c As Int32 = 0
        c = object_compare(this, that)
        If c = object_compare_undetermined Then
            assert(Not this Is Nothing)
            assert(Not that Is Nothing)
            Return connector.compare(this.first, that.first)
        Else
            Return c
        End If
    End Function

    Public Shared Shadows Function move(ByVal v As map(Of KEY_T, VALUE_T)) As map(Of KEY_T, VALUE_T)
        If v Is Nothing Then
            Return Nothing
        Else
            Dim r As map(Of KEY_T, VALUE_T) = Nothing
            r = New map(Of KEY_T, VALUE_T)()
            move_to(v, r)
            Return r
        End If
    End Function

    Public Shadows Function clone() As map(Of KEY_T, VALUE_T)
        Dim r As map(Of KEY_T, VALUE_T) = Nothing
        r = New map(Of KEY_T, VALUE_T)()
        clone_to(Me, r)
        Return r
    End Function

    Public Function CloneT() As map(Of KEY_T, VALUE_T) Implements ICloneable(Of map(Of KEY_T, VALUE_T)).Clone
        Return clone()
    End Function

    Public Function ICloneable_Clone() As Object Implements ICloneable.Clone
        Return clone()
    End Function

    Public Overloads Function emplace(ByVal key As KEY_T,
                                      ByVal value As VALUE_T) As iterator
        Dim it As iterator = Nothing
        it = find(key)
        If it.is_not_end() Then
            it.value().second = value
            Return it
        Else
            Dim r As pair(Of iterator, Boolean) = Nothing
            r = MyBase.emplace(emplace_make_first_const_pair(key, value))
            assert(Not r Is Nothing)
            assert(r.second)
            Return r.first
        End If
    End Function

    Public Overloads Function insert(ByVal key As KEY_T,
                                     ByVal value As VALUE_T) As iterator
        Return emplace(copy_no_error(key), copy_no_error(value))
    End Function

    Public Shadows Function [erase](ByVal it As iterator) As Boolean
        Return MyBase.erase(it)
    End Function

    Public Shadows Function [erase](ByVal key As KEY_T) As Boolean
        Return MyBase.erase(find(key))
    End Function

    Public Shadows Function find(ByVal key As KEY_T) As iterator
        Return MyBase.find(emplace_make_first_const_pair(Of KEY_T, VALUE_T)(key))
    End Function

    Default Public Property at(ByVal key As KEY_T) As VALUE_T
        Get
            Dim r As iterator = Nothing
            r = find(key)
            If r = [end]() Then
                r = emplace(copy_no_error(key), alloc(Of VALUE_T)())
            End If
            Return (+r).second
        End Get
        Set(ByVal value As VALUE_T)
            insert(key, value)
        End Set
    End Property
End Class
'finish map.vbp --------
'finish bmap2.vbp --------
