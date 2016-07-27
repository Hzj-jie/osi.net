
Imports System.Text
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation

<global_init(global_init_level.services)>
Friend Class sizeof_register_internal_types
    Shared Sub New()
        sizeof_register(Of String).assert_bind(Function(i As String) As UInt32
                                                   Return strlen(i)
                                               End Function)
        sizeof_register(Of Byte()).assert_bind(Function(i() As Byte) As UInt32
                                                   Return array_size(i)
                                               End Function)
        sizeof_register(Of StringBuilder).assert_bind(Function(i As StringBuilder) As UInt32
                                                          Return strlen(i)
                                                      End Function)
        sizeof_register(Of piece).assert_bind(Function(i As piece) As UInt32
                                                  Return If(i Is Nothing, uint32_0, i.count)
                                              End Function)
    End Sub

    Private Sub New()
    End Sub

    'sizeof needs to be initialized before convertor_register
    Friend Shared Sub init()
    End Sub
End Class

Public Class sizeof_register(Of T)
    Public Shared Function bind(ByVal f As Func(Of T, UInt64)) As Boolean
        If f Is Nothing Then
            Return False
        Else
            binder(Of Func(Of T, UInt64), sizeof_binder_protector).set_global(f)
            binder(Of Func(Of T, Int64), sizeof_binder_protector).set_global(
                Function(i As T) As Int64
                    Dim o As UInt64 = uint64_0
                    o = f(i)
                    assert(o <= max_int64)
                    Return CLng(o)
                End Function)
            binder(Of Func(Of T, UInt32), sizeof_binder_protector).set_global(
                Function(i As T) As UInt32
                    Dim o As UInt64 = uint64_0
                    o = f(i)
                    assert(o <= max_uint32)
                    Return CUInt(o)
                End Function)
            binder(Of Func(Of T, Int32), sizeof_binder_protector).set_global(
                Function(i As T) As Int32
                    Dim o As UInt64 = uint64_0
                    o = f(i)
                    assert(o <= max_int32)
                    Return CInt(o)
                End Function)
            Return True
        End If
    End Function

    Public Shared Sub assert_bind(ByVal f As Func(Of T, UInt64))
        assert(bind(f))
    End Sub

    Public Shared Function bind(ByVal f As Func(Of T, Int64)) As Boolean
        If f Is Nothing Then
            Return False
        Else
            binder(Of Func(Of T, UInt64), sizeof_binder_protector).set_global(
                Function(i As T) As UInt64
                    Dim o As Int64 = int64_0
                    o = f(i)
                    assert(o >= int64_0)
                    Return CULng(o)
                End Function)
            binder(Of Func(Of T, Int64), sizeof_binder_protector).set_global(f)
            binder(Of Func(Of T, UInt32), sizeof_binder_protector).set_global(
                Function(i As T) As UInt32
                    Dim o As Int64 = int64_0
                    o = f(i)
                    assert(o >= int64_0 AndAlso o <= max_uint32)
                    Return CUInt(o)
                End Function)
            binder(Of Func(Of T, Int32), sizeof_binder_protector).set_global(
                Function(i As T) As Int32
                    Dim o As Int64 = int64_0
                    o = f(i)
                    assert(o >= min_int32 AndAlso o <= max_int32)
                    Return CInt(o)
                End Function)
            Return True
        End If
    End Function

    Public Shared Sub assert_bind(ByVal f As Func(Of T, Int64))
        assert(bind(f))
    End Sub

    Public Shared Function bind(ByVal f As Func(Of T, UInt32)) As Boolean
        If f Is Nothing Then
            Return False
        Else
            binder(Of Func(Of T, UInt64), sizeof_binder_protector).set_global(
                Function(i As T) As UInt64
                    Dim o As UInt32 = uint32_0
                    o = f(i)
                    Return CULng(o)
                End Function)
            binder(Of Func(Of T, Int64), sizeof_binder_protector).set_global(
                Function(i As T) As Int64
                    Dim o As UInt32 = uint32_0
                    o = f(i)
                    Return CLng(o)
                End Function)
            binder(Of Func(Of T, UInt32), sizeof_binder_protector).set_global(f)
            binder(Of Func(Of T, Int32), sizeof_binder_protector).set_global(
                Function(i As T) As Int32
                    Dim o As UInt32 = uint32_0
                    o = f(i)
                    assert(o <= max_int32)
                    Return CInt(o)
                End Function)
            Return True
        End If
    End Function

    Public Shared Sub assert_bind(ByVal f As Func(Of T, UInt32))
        assert(bind(f))
    End Sub

    Public Shared Function bind(ByVal f As Func(Of T, Int32)) As Boolean
        If f Is Nothing Then
            Return False
        Else
            binder(Of Func(Of T, UInt64), sizeof_binder_protector).set_global(
                Function(i As T) As UInt64
                    Dim o As Int32 = int32_0
                    o = f(i)
                    assert(o >= int32_0)
                    Return CULng(o)
                End Function)
            binder(Of Func(Of T, Int64), sizeof_binder_protector).set_global(
                Function(i As T) As Int64
                    Dim o As Int32 = int32_0
                    o = f(i)
                    Return CLng(o)
                End Function)
            binder(Of Func(Of T, UInt32), sizeof_binder_protector).set_global(
                Function(i As T) As UInt32
                    Dim o As Int32 = int32_0
                    o = f(i)
                    assert(o >= int32_0)
                    Return CUInt(o)
                End Function)
            binder(Of Func(Of T, Int32), sizeof_binder_protector).set_global(f)
            Return True
        End If
    End Function

    Public Shared Sub assert_bind(ByVal f As Func(Of T, Int32))
        assert(bind(f))
    End Sub

    Private Sub New()
    End Sub
End Class
