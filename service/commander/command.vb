
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.utils

' TODO: Remove. Basic types and collections should be able to be used directly with dev_T.
Partial Public NotInheritable Class command
    Private ReadOnly a As array_ref(Of Byte)
    Private ReadOnly ps As map(Of array_ref(Of Byte), Byte())

    '( cannot be the first character of a line in vb.net
    Public Shared Function [New]() As command
        Return New command()
    End Function

    Public Shared Function [New](Of T)(ByVal action As T) As command
        Return (New command()).attach(Of T)(action)
    End Function

    <copy_constructor>
    Protected Sub New(ByVal a As array_ref(Of Byte), ByVal ps As map(Of array_ref(Of Byte), Byte()))
        Me.a = a
        Me.ps = ps
    End Sub

    Public Sub New()
        Me.New(New array_ref(Of Byte)(), New map(Of array_ref(Of Byte), Byte()))
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

    Public Function action(Of T)() As T
        Dim r As T = Nothing
        If bytes_serializer.from_bytes(action(), r) Then
            Return r
        End If
        Return Nothing
    End Function

    Public Function action_is(ByVal b() As Byte) As Boolean
        Return has_action() AndAlso
               (memcmp(action(), b) = 0)
    End Function

    Public Function action_is(Of T)(ByVal k As T) As Boolean
        Return action_is(bytes_serializer.to_bytes(k))
    End Function

    Public Function foreach(ByVal v As Action(Of Byte(), Byte())) As Boolean
        If v Is Nothing Then
            Return False
        End If
        Dim it As map(Of array_ref(Of Byte), Byte()).iterator = Nothing
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
        Dim it As map(Of array_ref(Of Byte), Byte()).iterator = Nothing
        it = ps.find(array_ref.of(key))
        If it = ps.end() Then
            Return False
        End If
        v = (+it).second
        Return True
    End Function

    Public Function parameter(Of KT)(ByVal k As KT, ByRef v() As Byte) As Boolean
        Return parameter(bytes_serializer.to_bytes(k), v)
    End Function

    Public Function parameter(Of KT)(ByVal k As KT) As Byte()
        Dim value() As Byte = Nothing
        If parameter(k, value) Then
            Return value
        End If
        Return Nothing
    End Function

    Public Function parameter(Of VT)(ByVal key() As Byte, ByRef v As VT) As Boolean
        Dim value() As Byte = Nothing
        Return parameter(key, value) AndAlso
               bytes_serializer.from_bytes(value, v)
    End Function

    Public Function parameter(Of KT, VT)(ByVal k As KT, ByRef v As VT) As Boolean
        Dim value() As Byte = Nothing
        Return parameter(bytes_serializer.to_bytes(k), value) AndAlso
               bytes_serializer.from_bytes(value, v)
    End Function

    Public Function parameter(Of KT, VT)(ByVal k As KT) As VT
        Dim v As VT = Nothing
        If parameter(k, v) Then
            Return v
        End If
        Return Nothing
    End Function

    Public Function parameter(ByVal key() As Byte, ByVal r As ref(Of Byte())) As Boolean
        Dim v() As Byte = Nothing
        Return parameter(key, v) AndAlso
               eva(r, v)
    End Function

    Public Function parameter(Of VT)(ByVal key() As Byte, ByVal r As ref(Of VT)) As Boolean
        Dim v As VT = Nothing
        Return parameter(key, v) AndAlso
               eva(r, v)
    End Function

    Public Function parameter(Of KT)(ByVal k As KT, ByVal r As ref(Of Byte())) As Boolean
        Dim value() As Byte = Nothing
        Return parameter(k, value) AndAlso
               eva(r, value)
    End Function

    Public Function parameter(Of KT, VT)(ByVal k As KT, ByVal r As ref(Of VT)) As Boolean
        Dim v As VT = Nothing
        Return parameter(k, v) AndAlso
               eva(r, v)
    End Function

    Public Function has_parameter(ByVal key() As Byte) As Boolean
        Return parameter(key, default_bytes)
    End Function

    Public Function has_parameter(Of KT)(ByVal k As KT) As Boolean
        Return has_parameter(bytes_serializer.to_bytes(k))
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

    Public Sub set_action(Of T)(ByVal i As T)
        set_action_no_copy(bytes_serializer.to_bytes(i))
    End Sub

    'for bytes / uri
    Friend Sub set_parameter_no_copy(ByVal k() As Byte, ByVal v() As Byte)
        If Not isemptyarray(k) Then
            ps(array_ref.of(k)) = v
        End If
    End Sub

    Public Sub set_parameter(ByVal k() As Byte, ByVal v() As Byte)
        set_parameter_no_copy(copy(k), copy(v))
    End Sub

    Public Sub set_parameter(Of KT)(ByVal k As KT, ByVal v() As Byte)
        set_parameter_no_copy(bytes_serializer.to_bytes(k), copy(v))
    End Sub

    Public Sub set_parameter(Of VT)(ByVal k() As Byte, ByVal v As VT)
        set_parameter_no_copy(copy(k), bytes_serializer.to_bytes(v))
    End Sub

    Public Sub set_parameter(Of KT, VT)(ByVal k As KT, ByVal v As VT)
        set_parameter_no_copy(bytes_serializer.to_bytes(k), bytes_serializer.to_bytes(v))
    End Sub
End Class
