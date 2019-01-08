
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.delegates

Partial Public NotInheritable Class bytes_serializer
    Public Shared Function [New](Of T) _
                                (Optional ByVal append_to As Func(Of T, MemoryStream, Boolean) = Nothing,
                                 Optional ByVal write_to As Func(Of T, MemoryStream, Boolean) = Nothing,
                                 Optional ByVal consume_from As _do_val_ref(Of MemoryStream, T, Boolean) = Nothing,
                                 Optional ByVal read_from As _do_val_ref(Of MemoryStream, T, Boolean) = Nothing) _
                                As bytes_serializer(Of T)
        Return New delegate_impl(Of T)(append_to, write_to, consume_from, read_from)
    End Function

    Private NotInheritable Class delegate_impl(Of T)
        Inherits bytes_serializer(Of T)

        Private ReadOnly _append_to As Func(Of T, MemoryStream, Boolean)
        Private ReadOnly _write_to As Func(Of T, MemoryStream, Boolean)
        Private ReadOnly _consume_from As _do_val_ref(Of MemoryStream, T, Boolean)
        Private ReadOnly _read_from As _do_val_ref(Of MemoryStream, T, Boolean)

        Public Sub New(ByVal append_to As Func(Of T, MemoryStream, Boolean),
                       ByVal write_to As Func(Of T, MemoryStream, Boolean),
                       ByVal consume_from As _do_val_ref(Of MemoryStream, T, Boolean),
                       ByVal read_from As _do_val_ref(Of MemoryStream, T, Boolean))
            If append_to Is Nothing Then
                _append_to = MyBase.append_to()
            Else
                _append_to = append_to
            End If

            If write_to Is Nothing Then
                _write_to = MyBase.write_to()
            Else
                _write_to = write_to
            End If

            If consume_from Is Nothing Then
                _consume_from = MyBase.consume_from()
            Else
                _consume_from = consume_from
            End If

            If read_from Is Nothing Then
                _read_from = MyBase.read_from()
            Else
                _read_from = read_from
            End If
        End Sub

        ' An implementation may suppport only part of the functions for certain scenarios. So the assertions are placed
        ' in side of the override functions rather than the constructor.
        Protected Overrides Function append_to() As Func(Of T, MemoryStream, Boolean)
            assert(Not _append_to Is Nothing)
            Return _append_to
        End Function

        Protected Overrides Function write_to() As Func(Of T, MemoryStream, Boolean)
            assert(Not _write_to Is Nothing)
            Return _write_to
        End Function

        Protected Overrides Function consume_from() As _do_val_ref(Of MemoryStream, T, Boolean)
            assert(Not _consume_from Is Nothing)
            Return _consume_from
        End Function

        Protected Overrides Function read_from() As _do_val_ref(Of MemoryStream, T, Boolean)
            assert(Not _read_from Is Nothing)
            Return _read_from
        End Function
    End Class
End Class
