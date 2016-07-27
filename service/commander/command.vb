
Option Strict On
Imports osi.root.constants
Imports osi.root.delegates
Imports osi.root.formation
Imports osi.root.connector
Imports osi.root.utils
Imports osi.service.convertor

<global_init(global_init_level.server_services)>
Public Class command
    Private ReadOnly a As array_pointer(Of Byte)
    Private ReadOnly ps As map(Of array_pointer(Of Byte), Byte())

    Shared Sub New()
        assert(global_init_level.services < global_init_level.server_services)
        bytes_sbyte_convertor_register(Of constants.action).assert_bind()
        bytes_sbyte_convertor_register(Of constants.response).assert_bind()
        bytes_sbyte_convertor_register(Of constants.parameter).assert_bind()
        'TODO better solution without copy byte() or string
        bytes_convertor_register(Of command).assert_bind(
            Function(i() As Byte, ByRef offset As UInt32, ByRef o As command) As Boolean
                Dim b() As Byte = Nothing
                If bytes_bytes_ref(i, b, offset) Then
                    o.renew()
                    Return o.from_bytes(b)
                Else
                    Return False
                End If
            End Function,
            Function(i() As Byte, ii As UInt32, il As UInt32, ByRef o As command) As Boolean
                Dim b() As Byte = Nothing
                If bytes_bytes_ref(i, ii, il, b) Then
                    o.renew()
                    Return o.from_bytes(b)
                Else
                    Return False
                End If
            End Function,
            Function(i As command, o() As Byte, ByRef offset As UInt32) As Boolean
                Dim b() As Byte = Nothing
                b = i.to_bytes()
                Return bytes_bytes_val(b, o, offset)
            End Function,
            Function(i As command, ByRef o() As Byte) As Boolean
                If i Is Nothing Then
                    Return False
                Else
                    o = i.to_bytes()
                    Return True
                End If
            End Function)
        string_convertor_register(Of command).assert_bind(
            Function(i As String, ByRef ii As UInt32, ByRef o As command) As Boolean
                If ii >= strlen(i) Then
                    Return False
                Else
                    o.renew()
                    Return o.from_str(strmid(i, ii))
                End If
            End Function,
            Function(i As String, ii As UInt32, il As UInt32, ByRef o As command) As Boolean
                If ii + il > strlen(i) Then
                    Return False
                Else
                    o.renew()
                    Return o.from_str(strmid(i, ii, il))
                End If
            End Function,
            Function(i As command, ByRef o As String) As Boolean
                If i Is Nothing Then
                    Return False
                Else
                    o = i.to_str()
                    Return True
                End If
            End Function)
        uri_convertor_register(Of command).assert_bind(
            Function(i As String, ByRef ii As UInt32, ByRef o As command) As Boolean
                If ii >= strlen(i) Then
                    Return False
                Else
                    o.renew()
                    Return o.from_uri(strmid(i, ii))
                End If
            End Function,
            Function(i As String, ii As UInt32, il As UInt32, ByRef o As command) As Boolean
                If ii + il > strlen(i) Then
                    Return False
                Else
                    o.renew()
                    Return o.from_uri(strmid(i, ii, il))
                End If
            End Function,
            Function(i As command, ByRef o As String) As Boolean
                If i Is Nothing Then
                    Return False
                Else
                    o = i.to_uri()
                    Return True
                End If
            End Function)
    End Sub

    Private Shared Sub init()
    End Sub

    '( cannot be the first character of a line in vb.net
    Public Shared Function [New]() As command
        Return New command()
    End Function

    Public Shared Function [New](Of T)(ByVal action As T,
                                       Optional ByVal T_bytes As  _
                                           binder(Of Func(Of T, Byte()), 
                                                     bytes_conversion_binder_protector) = Nothing) _
                                      As command
        Return (New command()).attach(Of T)(action, T_bytes)
    End Function

    Public Sub New()
        a = New array_pointer(Of Byte)()
        ps = New map(Of array_pointer(Of Byte), Byte())
    End Sub

    Public Sub clear()
        a.clear()
        ps.clear()
    End Sub

    Public Function empty() As Boolean
        Return Not has_action() AndAlso
               Not has_parameters()
    End Function

    Public Function has_action() As Boolean
        Return Not a.get() Is Nothing
    End Function

    Public Function has_parameters() As Boolean
        Return Not ps.empty()
    End Function

    Public Function action() As Byte()
        Return +a
    End Function

    Public Function action_is(ByVal b() As Byte) As Boolean
        Return has_action() AndAlso
               (memcmp(action(), b) = 0)
    End Function

    Public Function action_is(Of T)(ByVal k As T,
                                    Optional ByVal T_bytes As  _
                                        binder(Of _do_val_ref(Of T, Byte(), Boolean), 
                                                  bytes_conversion_binder_protector) = Nothing) _
                                   As Boolean
        assert(T_bytes.has_value())
        Dim key() As Byte = Nothing
        Return (+T_bytes)(k, key) AndAlso
               action_is(key)
    End Function

    Public Function foreach(ByVal v As Action(Of Byte(), Byte())) As Boolean
        If v Is Nothing Then
            Return False
        Else
            Dim it As map(Of array_pointer(Of Byte), Byte()).iterator = Nothing
            it = ps.begin()
            While it <> ps.end()
                v(+((+it).first), (+it).second)
                it += 1
            End While
            Return True
        End If
    End Function

    Public Function parameter(ByVal key() As Byte, ByRef v() As Byte) As Boolean
        If isemptyarray(key) Then
            Return False
        Else
            Dim it As map(Of array_pointer(Of Byte), Byte()).iterator = Nothing
            it = ps.find(make_array_pointer(key))
            If it = ps.end() Then
                Return False
            Else
                v = (+it).second
                Return True
            End If
        End If
    End Function

    Public Function parameter(Of KT)(ByVal k As KT,
                                     ByRef v() As Byte,
                                     Optional ByVal KT_bytes As  _
                                         binder(Of _do_val_ref(Of KT, Byte(), Boolean), 
                                                   bytes_conversion_binder_protector) = Nothing) _
                                    As Boolean
        assert(KT_bytes.has_value())
        Dim key() As Byte = Nothing
        Return (+KT_bytes)(k, key) AndAlso
               parameter(key, v)
    End Function

    Public Function parameter(Of KT)(ByVal k As KT,
                                     Optional ByVal KT_bytes As  _
                                         binder(Of _do_val_ref(Of KT, Byte(), Boolean), 
                                                   bytes_conversion_binder_protector) = Nothing) _
                                    As Byte()
        Dim value() As Byte = Nothing
        If parameter(k, value, KT_bytes) Then
            Return value
        Else
            Return Nothing
        End If
    End Function

    Public Function parameter(Of VT)(ByVal key() As Byte,
                                     ByRef v As VT,
                                     Optional ByVal bytes_VT As  _
                                         binder(Of _do_val_ref(Of Byte(), VT, Boolean), 
                                                   bytes_conversion_binder_protector) = Nothing) _
                                    As Boolean
        assert(bytes_VT.has_value())
        Dim value() As Byte = Nothing
        Return parameter(key, value) AndAlso
               (+bytes_VT)(value, v)
    End Function

    Public Function parameter(Of KT, VT)(ByVal k As KT,
                                         ByRef v As VT,
                                         Optional ByVal KT_bytes As  _
                                             binder(Of _do_val_ref(Of KT, Byte(), Boolean), 
                                                       bytes_conversion_binder_protector) = Nothing,
                                         Optional ByVal bytes_VT As  _
                                             binder(Of _do_val_ref(Of Byte(), VT, Boolean), 
                                                       bytes_conversion_binder_protector) = Nothing) _
                                        As Boolean
        assert(KT_bytes.has_value())
        assert(bytes_VT.has_value())
        Dim key() As Byte = Nothing
        Dim value() As Byte = Nothing
        Return (+KT_bytes)(k, key) AndAlso
               parameter(key, value) AndAlso
               (+bytes_VT)(value, v)
    End Function

    Public Function parameter(Of KT, VT)(ByVal k As KT,
                                         Optional ByVal KT_bytes As  _
                                             binder(Of _do_val_ref(Of KT, Byte(), Boolean), 
                                                       bytes_conversion_binder_protector) = Nothing,
                                         Optional ByVal bytes_VT As  _
                                             binder(Of _do_val_ref(Of Byte(), VT, Boolean), 
                                                       bytes_conversion_binder_protector) = Nothing) _
                                        As VT
        Dim v As VT = Nothing
        If parameter(k, v, KT_bytes, bytes_VT) Then
            Return v
        Else
            Return Nothing
        End If
    End Function

    Public Function parameter(ByVal key() As Byte,
                              ByVal r As pointer(Of Byte())) As Boolean
        Dim v() As Byte = Nothing
        Return parameter(key, v) AndAlso
               eva(r, v)
    End Function

    Public Function parameter(Of VT)(ByVal key() As Byte,
                                     ByVal r As pointer(Of VT),
                                     Optional ByVal bytes_VT As  _
                                         binder(Of _do_val_ref(Of Byte(), VT, Boolean), 
                                                   bytes_conversion_binder_protector) = Nothing) _
                                    As Boolean
        Dim v As VT = Nothing
        Return parameter(key, v, bytes_VT) AndAlso
               eva(r, v)
    End Function

    Public Function parameter(Of KT)(ByVal k As KT,
                                     ByVal r As pointer(Of Byte()),
                                     Optional ByVal KT_bytes As  _
                                         binder(Of _do_val_ref(Of KT, Byte(), Boolean), 
                                                   bytes_conversion_binder_protector) = Nothing) _
                                    As Boolean
        Dim value() As Byte = Nothing
        Return parameter(k, value, KT_bytes) AndAlso
               eva(r, value)
    End Function

    Public Function parameter(Of KT, VT)(ByVal k As KT,
                                         ByVal r As pointer(Of VT),
                                         Optional ByVal KT_bytes As  _
                                             binder(Of _do_val_ref(Of KT, Byte(), Boolean), 
                                                       bytes_conversion_binder_protector) = Nothing,
                                         Optional ByVal bytes_VT As  _
                                             binder(Of _do_val_ref(Of Byte(), VT, Boolean), 
                                                       bytes_conversion_binder_protector) = Nothing) _
                                        As Boolean
        Dim v As VT = Nothing
        Return parameter(k, v, KT_bytes, bytes_VT) AndAlso
               eva(r, v)
    End Function

    Public Function has_parameter(ByVal key() As Byte) As Boolean
        Return parameter(key, default_bytes)
    End Function

    Public Function has_parameter(Of KT)(ByVal k As KT,
                                         Optional ByVal KT_bytes As  _
                                             binder(Of _do_val_ref(Of KT, Byte(), Boolean), 
                                                       bytes_conversion_binder_protector) = Nothing) _
                                        As Boolean
        assert(KT_bytes.has_value())
        Dim key() As Byte = Nothing
        Return (+KT_bytes)(k, key) AndAlso
               has_parameter(key)
    End Function

    Public Function parameter(ByVal key() As Byte) As Byte()
        Dim v() As Byte = Nothing
        If parameter(key, v) Then
            Return v
        Else
            Return Nothing
        End If
    End Function

    'for bytes / uri
    Friend Sub set_action_no_copy(ByVal action() As Byte)
        If Not isemptyarray(action) Then
            a.set(action)
        End If
    End Sub

    Public Sub set_action(ByVal action() As Byte)
        set_action_no_copy(copy(action))
    End Sub

    Public Sub set_action(Of T)(ByVal i As T,
                                Optional ByVal T_bytes As  _
                                    binder(Of Func(Of T, Byte()), 
                                              bytes_conversion_binder_protector) = Nothing)
        assert(T_bytes.has_value())
        set_action_no_copy((+T_bytes)(i))
    End Sub

    'for bytes / uri
    Friend Sub set_parameter_no_copy(ByVal k() As Byte, ByVal v() As Byte)
        If Not isemptyarray(k) Then
            ps(make_array_pointer(k)) = v
        End If
    End Sub

    Public Sub set_parameter(ByVal k() As Byte, ByVal v() As Byte)
        set_parameter_no_copy(copy(k), copy(v))
    End Sub

    Public Sub set_parameter(Of KT)(ByVal k As KT,
                                    ByVal v() As Byte,
                                    Optional ByVal KT_bytes As  _
                                        binder(Of Func(Of KT, Byte()), 
                                                  bytes_conversion_binder_protector) = Nothing)
        assert(KT_bytes.has_value())
        set_parameter_no_copy((+KT_bytes)(k), copy(v))
    End Sub

    Public Sub set_parameter(Of VT)(ByVal k() As Byte,
                                    ByVal v As VT,
                                    Optional ByVal VT_bytes As  _
                                        binder(Of Func(Of VT, Byte()), 
                                                  bytes_conversion_binder_protector) = Nothing)
        assert(VT_bytes.has_value())
        set_parameter_no_copy(copy(k), (+VT_bytes)(v))
    End Sub

    Public Sub set_parameter(Of KT, VT)(ByVal k As KT,
                                        ByVal v As VT,
                                        Optional ByVal KT_bytes As  _
                                            binder(Of Func(Of KT, Byte()), 
                                                      bytes_conversion_binder_protector) = Nothing,
                                        Optional ByVal VT_bytes As  _
                                            binder(Of Func(Of VT, Byte()), 
                                                      bytes_conversion_binder_protector) = Nothing)
        assert(KT_bytes.has_value())
        assert(VT_bytes.has_value())
        set_parameter_no_copy((+KT_bytes)(k), (+VT_bytes)(v))
    End Sub
End Class
