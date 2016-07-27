﻿
Imports osi.root.connector

Public NotInheritable Class stack(Of T)
    Implements ICloneable

    Private ReadOnly d As vector(Of T) = Nothing

    Public Sub clear()
        d.clear()
    End Sub

    Public Function empty() As Boolean
        Return d.empty()
    End Function

    Public Function size() As UInt32
        Return d.size()
    End Function

    Public Function back() As T
        Return d.back()
    End Function

    Public Sub push(ByVal v As T)
        d.push_back(v)
    End Sub

    Public Sub emplace(ByVal v As T)
        d.emplace_back(v)
    End Sub

    Public Sub pop()
        d.pop_back()
    End Sub

    Public Sub New()
        d = New vector(Of T)()
    End Sub

    Public Sub New(ByVal that As stack(Of T))
        d = copy_no_error(that.d)
    End Sub

    Public Sub New(ByVal that As vector(Of T))
        d = copy_no_error(that)
    End Sub

    Public Function Clone() As Object Implements ICloneable.Clone
        Return New stack(Of T)(d)
    End Function
End Class
