
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.delegates

Public Class convertor_register(Of FROM_T, TO_T, PROTECTOR)
    'Convert FROM_T.sub_str(ref UInt32, ?) to TO_T(from UInt32)
    Public Shared Sub assert_bind(ByVal f As _do_val_ref_val_ref(Of FROM_T, UInt32, TO_T, UInt32, Boolean))
        assert(bind(f))
    End Sub

    Public Shared Function bind(ByVal f As _do_val_ref_val_ref(Of FROM_T, UInt32, TO_T, UInt32, Boolean)) As Boolean
        If f Is Nothing Then
            Return False
        Else
            binder(Of _do_val_ref_val_ref(Of FROM_T, UInt32, TO_T, UInt32, Boolean), PROTECTOR).set_global(f)
            assert_bind(Function(i As FROM_T, ByRef index As UInt32, o As TO_T) As Boolean
                            Return f(i, index, o, uint32_0)
                        End Function)
            Return True
        End If
    End Function

    'Convert FROM_T.sub_str(ref UInt32, ?) to TO_T(from 0)
    Public Shared Sub assert_bind(ByVal f As _do_val_ref_val(Of FROM_T, UInt32, TO_T, Boolean))
        assert(bind(f))
    End Sub

    Public Shared Function bind(ByVal f As _do_val_ref_val(Of FROM_T, UInt32, TO_T, Boolean)) As Boolean
        If f Is Nothing Then
            Return False
        Else
            binder(Of _do_val_ref_val(Of FROM_T, UInt32, TO_T, Boolean), PROTECTOR).set_global(f)
            Return True
        End If
    End Function

    '-------------------------------------------------------------------------------------------------------------------

    'Convert FROM_T.sub_str(ref UInt32, ?) to TO_T
    Public Shared Sub assert_bind(ByVal f As _do_val_ref_ref(Of FROM_T, UInt32, TO_T, Boolean))
        assert(bind(f))
    End Sub

    Public Shared Function bind(ByVal f As _do_val_ref_ref(Of FROM_T, UInt32, TO_T, Boolean)) As Boolean
        If f Is Nothing Then
            Return False
        Else
            binder(Of _do_val_ref_ref(Of FROM_T, UInt32, TO_T, Boolean), PROTECTOR).set_global(f)
            assert_bind(Function(i As FROM_T, ByRef index As UInt32) As TO_T
                            Dim o As TO_T = Nothing
                            If f(i, index, o) Then
                                Return o
                            Else
                                Return Nothing
                            End If
                        End Function)
            Return True
        End If
    End Function

    ' Convert FROM_T.sub_str(ref UInt32, ?) to TO_T without failure detection
    Public Shared Sub assert_bind(ByVal f As _do_val_ref(Of FROM_T, UInt32, TO_T))
        assert(bind(f))
    End Sub

    Public Shared Function bind(ByVal f As _do_val_ref(Of FROM_T, UInt32, TO_T)) As Boolean
        If f Is Nothing Then
            Return False
        Else
            binder(Of _do_val_ref(Of FROM_T, UInt32, TO_T), PROTECTOR).set_global(f)
            Return True
        End If
    End Function

    '-------------------------------------------------------------------------------------------------------------------

    'Convert FROM_T.sub_str(UInt32, UInt32) to TO_T(from UInt32)
    Public Shared Sub assert_bind(ByVal f As _do_val_val_val_val_ref(Of FROM_T, UInt32, UInt32, TO_T, UInt32, Boolean))
        assert(bind(f))
    End Sub

    Public Shared Function bind(ByVal f As _do_val_val_val_val_ref(Of FROM_T, UInt32, UInt32, TO_T, UInt32, Boolean)) _
                               As Boolean
        If f Is Nothing Then
            Return False
        Else
            binder(Of _do_val_val_val_val_ref(Of FROM_T, UInt32, UInt32, TO_T, UInt32, Boolean), PROTECTOR) _
                  .set_global(f)
            assert_bind(Function(i As FROM_T, index As UInt32, len As UInt32, o As TO_T) As Boolean
                            Return f(i, index, len, o, uint32_0)
                        End Function)
            assert_bind(Function(i As FROM_T, len As UInt32, o As TO_T, ByRef index As UInt32) As Boolean
                            Return f(i, uint32_0, len, o, index)
                        End Function)
            Return True
        End If
    End Function

    'Convert FROM_T.sub_str(UInt32, UInt32) To TO_T(from 0)
    Public Shared Sub assert_bind(ByVal f As Func(Of FROM_T, UInt32, UInt32, TO_T, Boolean))
        assert(bind(f))
    End Sub

    Public Shared Function bind(ByVal f As Func(Of FROM_T, UInt32, UInt32, TO_T, Boolean)) As Boolean
        If f Is Nothing Then
            Return False
        Else
            ' Avoid assert failure
            binder(Of Func(Of FROM_T, UInt32, UInt32, TO_T, Boolean), PROTECTOR).set_global(Nothing)
            binder(Of Func(Of FROM_T, UInt32, UInt32, TO_T, Boolean), PROTECTOR).set_global(f)
            assert_bind(Function(i As FROM_T, len As UInt32, o As TO_T) As Boolean
                            Return f(i, uint32_0, len, o)
                        End Function)
            Return True
        End If
    End Function

    'Convert FROM_T.sub_str(UInt32, END) to TO_T(from UInt32)
    Public Shared Sub assert_bind(ByVal f As _do_val_val_val_ref(Of FROM_T, UInt32, TO_T, UInt32, Boolean),
                                  Optional ByVal sizeof As binder(Of Func(Of FROM_T, UInt32), 
                                                                     sizeof_binder_protector) = Nothing)
        assert(bind(f, sizeof))
    End Sub

    Public Shared Function bind(ByVal f As _do_val_val_val_ref(Of FROM_T, UInt32, TO_T, UInt32, Boolean),
                                Optional ByVal sizeof As binder(Of Func(Of FROM_T, UInt32), 
                                                                   sizeof_binder_protector) = Nothing) As Boolean
        assert(sizeof.has_value())
        If f Is Nothing Then
            Return False
        Else
            binder(Of _do_val_val_val_ref(Of FROM_T, UInt32, TO_T, UInt32, Boolean), PROTECTOR).set_global(f)
            assert_bind(Function(i As FROM_T, len As UInt32, o As TO_T) As Boolean
                            Return f(i, len, o, uint32_0)
                        End Function)
            assert_bind(Function(i As FROM_T, o As TO_T, ByRef index As UInt32) As Boolean
                            Return f(i, (+sizeof)(i), o, index)
                        End Function)
            Return True
        End If
    End Function

    'Convert FROM_T.sub_str(UInt32, END) to TO_T(from 0)
    Public Shared Sub assert_bind(ByVal f As Func(Of FROM_T, UInt32, TO_T, Boolean))
        assert(bind(f))
    End Sub

    Public Shared Function bind(ByVal f As Func(Of FROM_T, UInt32, TO_T, Boolean)) As Boolean
        If f Is Nothing Then
            Return False
        Else
            'Avoid assert failure
            binder(Of Func(Of FROM_T, UInt32, TO_T, Boolean), PROTECTOR).set_global(Nothing)
            binder(Of Func(Of FROM_T, UInt32, TO_T, Boolean), PROTECTOR).set_global(f)
            assert_bind(Function(i As FROM_T, o As TO_T) As Boolean
                            Return f(i, uint32_0, o)
                        End Function)
            Return True
        End If
    End Function

    'Convert FROM_T to TO_T(from UInt32)
    Public Shared Sub assert_bind(ByVal f As _do_val_val_ref(Of FROM_T, TO_T, UInt32, Boolean))
        assert(bind(f))
    End Sub

    Public Shared Function bind(ByVal f As _do_val_val_ref(Of FROM_T, TO_T, UInt32, Boolean),
                                Optional ByVal sizeof As binder(Of Func(Of TO_T, UInt32), 
                                                                   sizeof_binder_protector) = Nothing) As Boolean
        assert(sizeof.has_value())
        If f Is Nothing Then
            Return False
        Else
            binder(Of _do_val_val_ref(Of FROM_T, TO_T, UInt32, Boolean), PROTECTOR).set_global(f)
            assert_bind(Function(i As FROM_T, o As TO_T) As Boolean
                            Dim p As UInt32 = 0
                            Return f(i, o, p) AndAlso p = (+sizeof)(o)
                        End Function)
            Return True
        End If
    End Function

    'Convert FROM_T to TO_T(from 0)
    Public Shared Sub assert_bind(ByVal f As Func(Of FROM_T, TO_T, Boolean))
        assert(bind(f))
    End Sub

    Public Shared Function bind(ByVal f As Func(Of FROM_T, TO_T, Boolean)) As Boolean
        If f Is Nothing Then
            Return False
        Else
            ' Avoid assert failure
            binder(Of Func(Of FROM_T, TO_T, Boolean), PROTECTOR).set_global(Nothing)
            binder(Of Func(Of FROM_T, TO_T, Boolean), PROTECTOR).set_global(f)
            Return True
        End If
    End Function

    '-------------------------------------------------------------------------------------------------------------------

    ' Convert FROM_T.sub_str(UInt32, UInt32) to TO_T
    Public Shared Sub assert_bind(ByVal f As _do_val_val_val_ref(Of FROM_T, UInt32, UInt32, TO_T, Boolean))
        assert(bind(f))
    End Sub

    Public Shared Function bind(ByVal f As _do_val_val_val_ref(Of FROM_T, UInt32, UInt32, TO_T, Boolean)) As Boolean
        If f Is Nothing Then
            Return False
        Else
            binder(Of _do_val_val_val_ref(Of FROM_T, UInt32, UInt32, TO_T, Boolean), PROTECTOR).set_global(f)
            assert_bind(Function(i As FROM_T, len As UInt32, ByRef o As TO_T) As Boolean
                            Return f(i, uint32_0, len, o)
                        End Function)
            Return True
        End If
    End Function

    Public Shared Sub assert_bind(ByVal f As _do_val_val_ref(Of FROM_T, UInt32, TO_T, Boolean),
                                  Optional ByVal sizeof As binder(Of Func(Of FROM_T, UInt32), 
                                                                     sizeof_binder_protector) = Nothing)
        assert(bind(f, sizeof))
    End Sub

    ' Convert FROM_T.sub_str(0, UInt32) to TO_T
    Public Shared Function bind(ByVal f As _do_val_val_ref(Of FROM_T, UInt32, TO_T, Boolean),
                                Optional ByVal sizeof As binder(Of Func(Of FROM_T, UInt32), 
                                                                   sizeof_binder_protector) = Nothing) As Boolean
        assert(sizeof.has_value())
        If f Is Nothing Then
            Return False
        Else
            binder(Of _do_val_val_ref(Of FROM_T, UInt32, TO_T, Boolean), PROTECTOR).set_global(f)
            assert_bind(Function(i As FROM_T, ByRef o As TO_T) As Boolean
                            Return f(i, (+sizeof)(i), o)
                        End Function)
            Return True
        End If
    End Function

    Public Shared Sub assert_bind(ByVal f As _do_val_ref(Of FROM_T, TO_T, Boolean))
        assert(bind(f))
    End Sub

    ' Convert FROM_T to TO_T
    Public Shared Function bind(ByVal f As _do_val_ref(Of FROM_T, TO_T, Boolean)) As Boolean
        If f Is Nothing Then
            Return False
        Else
            binder(Of _do_val_ref(Of FROM_T, TO_T, Boolean), PROTECTOR).set_global(f)
            assert_bind(Function(i As FROM_T) As TO_T
                            Dim o As TO_T = Nothing
                            If f(i, o) Then
                                Return o
                            Else
                                Return Nothing
                            End If
                        End Function)
            Return True
        End If
    End Function

    ' Convert FROM_T to TO_T without failure detection
    Public Shared Sub assert_bind(ByVal f As Func(Of FROM_T, TO_T))
        assert(bind(f))
    End Sub

    Public Shared Function bind(ByVal f As Func(Of FROM_T, TO_T)) As Boolean
        If f Is Nothing Then
            Return False
        Else
            binder(Of Func(Of FROM_T, TO_T), PROTECTOR).set_global(f)
            Return True
        End If
    End Function

    Shared Sub New()
        sizeof_register_internal_types.init()
    End Sub

    'Enable inheritance only
    Protected Sub New()
    End Sub
End Class
