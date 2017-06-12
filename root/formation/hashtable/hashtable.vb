
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.template

Partial Public Class hashtable(Of T,
                                  _UNIQUE As _boolean,
                                  _SIZE As _int64,
                                  _HASHER As _to_uint32(Of T),
                                  _COMPARER As _comparer(Of T))
    Private Shared ReadOnly unique As Boolean
    Private Shared ReadOnly hasher As _HASHER
    Private Shared ReadOnly compare As _comparer(Of T)

    Shared Sub New()
        unique = +(alloc(Of _UNIQUE)())
        hasher = alloc(Of _HASHER)()
        compare = alloc(Of _COMPARER)()
    End Sub

    Private ReadOnly v As vector(Of array(Of constant(Of T), _SIZE))
    Private s As UInt32

    <copy_constructor()>
    Protected Sub New(ByVal v As vector(Of array(Of constant(Of T), _SIZE)), ByVal s As UInt32)
        assert(Not v Is Nothing)
        Me.v = v
        Me.s = s
    End Sub

    Public Sub New()
        v = _new(v)
        new_row()
    End Sub

    Public Function size() As UInt32
        Return s
    End Function

    Public Function empty() As Boolean
        Return size() = uint32_0
    End Function

    Public Function begin() As iterator
        Dim it As iterator = Nothing
        it = iterator_at(0, 0)
        If cell(0, 0) Is Nothing Then
            it += 1
        End If
        Return it
    End Function

    Public Function [end]() As iterator
        Return iterator.end
    End Function

    Public Function rbegin() As iterator
        Dim it As iterator = Nothing
        it = iterator_at(last_row(), last_column())
        If cell(last_row(), last_column()) Is Nothing Then
            it -= 1
        End If
        Return it
    End Function

    Public Function rend() As iterator
        Return iterator.end
    End Function
End Class
