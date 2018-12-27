
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.utils

' TODO: Remove. Basic types and collections should be able to be used directly with dev_T.
Partial Public Class command
    Private ReadOnly a As array_pointer(Of Byte)
    Private ReadOnly ps As map(Of array_pointer(Of Byte), Byte())

    '( cannot be the first character of a line in vb.net
    Public Shared Function [New]() As command
        Return New command()
    End Function

    Public Shared Function [New](Of T)(ByVal action As T,
                                       Optional ByVal T_bytes As bytes_serializer(Of T) = Nothing) As command
        Return (New command()).attach(Of T)(action, T_bytes)
    End Function

    <copy_constructor>
    Protected Sub New(ByVal a As array_pointer(Of Byte), ByVal ps As map(Of array_pointer(Of Byte), Byte()))
        Me.a = a
        Me.ps = ps
    End Sub

    Public Sub New()
        Me.New(New array_pointer(Of Byte)(), New map(Of array_pointer(Of Byte), Byte()))
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

    Public Function action(Of T)(Optional ByVal bytes_T As bytes_serializer(Of T) = Nothing) As T
        Dim r As T = Nothing
        If (+bytes_T).from_bytes(action(), r) Then
            Return r
        End If
        Return Nothing
    End Function

    Public Function action_is(ByVal b() As Byte) As Boolean
        Return has_action() AndAlso
               (memcmp(action(), b) = 0)
    End Function

    Public Function action_is(Of T)(ByVal k As T, Optional ByVal T_bytes As bytes_serializer(Of T) = Nothing) As Boolean
        Return action_is((+T_bytes).to_bytes(k))
    End Function

    Public Function foreach(ByVal v As Action(Of Byte(), Byte())) As Boolean
        If v Is Nothing Then
            Return False
        End If
        Dim it As map(Of array_pointer(Of Byte), Byte()).iterator = Nothing
        it = ps.begin()
        While it <> ps.end()
            v(+((+it).first), (+it).second)
            it += 1
        End While
        Return True
    End Function

    Public Function parameter(ByVal key() As Byte, ByRef v() As Byte) As Boolean
        If isemptyarray(key) Then
            Return False
        End If
        Dim it As map(Of array_pointer(Of Byte), Byte()).iterator = Nothing
        it = ps.find(make_array_pointer(key))
        If it = ps.end() Then
            Return False
        End If
        v = (+it).second
        Return True
    End Function

    Public Function parameter(Of KT)(ByVal k As KT,
                                     ByRef v() As Byte,
                                     Optional ByVal KT_bytes As bytes_serializer(Of KT) = Nothing) As Boolean
        Return parameter((+KT_bytes).to_bytes(k), v)
    End Function

    Public Function parameter(Of KT)(ByVal k As KT,
                                     Optional ByVal KT_bytes As bytes_serializer(Of KT) = Nothing) As Byte()
        Dim value() As Byte = Nothing
        If parameter(k, value, KT_bytes) Then
            Return value
        End If
        Return Nothing
    End Function

    Public Function parameter(Of VT)(ByVal key() As Byte,
                                     ByRef v As VT,
                                     Optional ByVal bytes_VT As bytes_serializer(Of VT) = Nothing) As Boolean
        Dim value() As Byte = Nothing
        Return parameter(key, value) AndAlso
               (+bytes_VT).from_bytes(value, v)
    End Function

    Public Function parameter(Of KT, VT)(ByVal k As KT,
                                         ByRef v As VT,
                                         Optional ByVal KT_bytes As bytes_serializer(Of KT) = Nothing,
                                         Optional ByVal bytes_VT As bytes_serializer(Of VT) = Nothing) As Boolean
        Dim value() As Byte = Nothing
        Return parameter((+KT_bytes).to_bytes(k), value) AndAlso
               (+bytes_VT).from_bytes(value, v)
    End Function

    Public Function parameter(Of KT, VT)(ByVal k As KT,
                                         Optional ByVal KT_bytes As bytes_serializer(Of KT) = Nothing,
                                         Optional ByVal bytes_VT As bytes_serializer(Of VT) = Nothing) As VT
        Dim v As VT = Nothing
        If parameter(k, v, KT_bytes, bytes_VT) Then
            Return v
        End If
        Return Nothing
    End Function

    Public Function parameter(ByVal key() As Byte, ByVal r As pointer(Of Byte())) As Boolean
        Dim v() As Byte = Nothing
        Return parameter(key, v) AndAlso
               eva(r, v)
    End Function

    Public Function parameter(Of VT)(ByVal key() As Byte,
                                     ByVal r As pointer(Of VT),
                                     Optional ByVal bytes_VT As bytes_serializer(Of VT) = Nothing) As Boolean
        Dim v As VT = Nothing
        Return parameter(key, v, bytes_VT) AndAlso
               eva(r, v)
    End Function

    Public Function parameter(Of KT)(ByVal k As KT,
                                     ByVal r As pointer(Of Byte()),
                                     Optional ByVal KT_bytes As bytes_serializer(Of KT) = Nothing) As Boolean
        Dim value() As Byte = Nothing
        Return parameter(k, value, KT_bytes) AndAlso
               eva(r, value)
    End Function

    Public Function parameter(Of KT, VT)(ByVal k As KT,
                                         ByVal r As pointer(Of VT),
                                         Optional ByVal KT_bytes As bytes_serializer(Of KT) = Nothing,
                                         Optional ByVal bytes_VT As bytes_serializer(Of VT) = Nothing) As Boolean
        Dim v As VT = Nothing
        Return parameter(k, v, KT_bytes, bytes_VT) AndAlso
               eva(r, v)
    End Function

    Public Function has_parameter(ByVal key() As Byte) As Boolean
        Return parameter(key, default_bytes)
    End Function

    Public Function has_parameter(Of KT)(ByVal k As KT,
                                         Optional ByVal KT_bytes As bytes_serializer(Of KT) = Nothing) As Boolean
        Return has_parameter((+KT_bytes).to_bytes(k))
    End Function

    Public Function parameter(ByVal key() As Byte) As Byte()
        Dim v() As Byte = Nothing
        If parameter(key, v) Then
            Return v
        End If
        Return Nothing
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

    Public Sub set_action(Of T)(ByVal i As T, Optional ByVal T_bytes As bytes_serializer(Of T) = Nothing)
        set_action_no_copy((+T_bytes).to_bytes(i))
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
                                    Optional ByVal KT_bytes As bytes_serializer(Of KT) = Nothing)
        set_parameter_no_copy((+KT_bytes).to_bytes(k), copy(v))
    End Sub

    Public Sub set_parameter(Of VT)(ByVal k() As Byte,
                                    ByVal v As VT,
                                    Optional ByVal VT_bytes As bytes_serializer(Of VT) = Nothing)
        set_parameter_no_copy(copy(k), (+VT_bytes).to_bytes(v))
    End Sub

    Public Sub set_parameter(Of KT, VT)(ByVal k As KT,
                                        ByVal v As VT,
                                        Optional ByVal KT_bytes As bytes_serializer(Of KT) = Nothing,
                                        Optional ByVal VT_bytes As bytes_serializer(Of VT) = Nothing)
        set_parameter_no_copy((+KT_bytes).to_bytes(k), (+VT_bytes).to_bytes(v))
    End Sub
End Class
