
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utils
Imports osi.service.convertor
Imports osi.service.iosys

Public Enum mode
    VOID

    keyboard

    COUNT
End Enum

Public Enum action
    VOID

    down
    press
    up

    COUNT
End Enum

Public NotInheritable Class [case]
    Implements ICloneable

    Public ReadOnly mode As mode
    Public ReadOnly action As action
    Public ReadOnly meta() As Byte

    Shared Sub New()
        'TODO a better solution without copy byte()
        bytes_convertor_register(Of [case]).assert_bind(
            Function(i() As Byte, ByRef offset As UInt32, ByRef o As [case]) As Boolean
                Dim b() As Byte = Nothing
                Return bytes_bytes_ref(i, b, offset) AndAlso
                       from_bytes(b, o)
            End Function,
            Function(i() As Byte, ii As UInt32, il As UInt32, ByRef o As [case]) As Boolean
                Dim b() As Byte = Nothing
                Return bytes_bytes_ref(i, ii, il, b) AndAlso
                       from_bytes(b, o)
            End Function,
            Function(i As [case], o() As Byte, ByRef offset As UInt32) As Boolean
                Dim b() As Byte = Nothing
                Return to_bytes(i, b) AndAlso
                       bytes_bytes_val(b, o, offset)
            End Function,
            Function(i As [case], ByRef o() As Byte) As Boolean
                Return to_bytes(i, o)
            End Function)
    End Sub

    Private Sub New(ByVal mode As mode,
                    ByVal action As action,
                    ByVal meta() As Byte,
                    ByVal internal As Boolean)
        Me.mode = mode
        Me.action = action
        Me.meta = meta
    End Sub

    Public Sub New(ByVal mode As mode, ByVal action As action, ByVal meta() As Byte)
        Me.New(mode, action, copy(meta), True)
    End Sub

    Public Function Clone() As Object Implements ICloneable.Clone
        Return New [case](mode, action, meta)
    End Function

    Public Overrides Function ToString() As String
        Return strcat("case[", mode, ", ", action, ", ", meta.to_string(), "]")
    End Function

    Public Shared Function to_bytes(ByVal i As [case], ByRef o() As Byte) As Boolean
        If i Is Nothing Then
            Return False
        Else
            Dim v As vector(Of Byte()) = Nothing
            v = New vector(Of Byte())()
            v.emplace_back(CInt(i.mode).to_bytes())
            v.emplace_back(CInt(i.action).to_bytes())
            v.emplace_back(i.meta)
            o = v.to_bytes()
            Return True
        End If
    End Function

    Public Shared Function from_bytes(ByVal i() As Byte, ByRef o As [case]) As Boolean
        Dim v As vector(Of Byte()) = Nothing
        v = i.to_vector_bytes()
        If v Is Nothing OrElse Not v.size() = 3 Then
            Return False
        Else
            Dim mi As Int32 = 0
            Dim ai As Int32 = 0
            Dim meta() As Byte = Nothing
            If bytes_int32(v(0), mi) AndAlso
               mi >= mode.VOID AndAlso
               mi < mode.COUNT AndAlso
               bytes_int32(v(1), ai) AndAlso
               ai >= action.VOID AndAlso
               ai < action.COUNT Then
                meta = v(2)
                o = New [case](mi, ai, meta, True)
                Return True
            Else
                Return False
            End If
        End If
    End Function
End Class
