﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Public Class hashmap(Of KEY_T As IComparable(Of KEY_T), VALUE_T)
    Inherits hashmap(Of KEY_T, VALUE_T, default_to_uint32(Of KEY_T))
    Implements ICloneable(Of hashmap(Of KEY_T, VALUE_T)), ICloneable

    Public Sub New(ByVal hash_size As UInt32)
        MyBase.New(hash_size)
    End Sub

    Public Sub New()
        MyBase.New()
    End Sub

    <copy_constructor>
    Protected Sub New(ByVal hash_size As UInt32, ByVal data() As map(Of KEY_T, VALUE_T))
        MyBase.New(hash_size, data)
    End Sub

    Public Shadows Function CloneT() As hashmap(Of KEY_T, VALUE_T) _
                                     Implements ICloneable(Of hashmap(Of KEY_T, VALUE_T)).Clone
        Return MyBase.clone(Of hashmap(Of KEY_T, VALUE_T))()
    End Function

    Public Shadows Function Clone() As Object Implements ICloneable.Clone
        Return CloneT()
    End Function
End Class
