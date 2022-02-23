
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.service.convertor

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

Public Module _case
    <Extension()> Public Function mode_or_void(ByVal this As [case]) As mode
        Return If(this Is Nothing, mode.VOID, this.mode)
    End Function

    <Extension()> Public Function action_or_void(ByVal this As [case]) As action
        Return If(this Is Nothing, action.VOID, this.action)
    End Function

    <Extension()> Public Function meta_or_null(ByVal this As [case]) As Byte()
        Return If(this Is Nothing, Nothing, this.meta)
    End Function
End Module

Public NotInheritable Class [case]
    Implements ICloneable, ICloneable(Of [case])

    Public ReadOnly mode As mode
    Public ReadOnly action As action
    Public ReadOnly meta() As Byte

    Shared Sub New()
        bytes_serializer(Of mode).forward_registration.from(Of Int32)()
        bytes_serializer(Of action).forward_registration.from(Of Int32)()
        bytes_serializer.fixed.register(Function(ByVal i As [case], ByVal o As MemoryStream) As Boolean
                                            assert(o IsNot Nothing)
                                            Return bytes_serializer.append_to(i.mode_or_void(), o) AndAlso
                                                   bytes_serializer.append_to(i.action_or_void(), o) AndAlso
                                                   bytes_serializer.append_to(i.meta_or_null(), o)
                                        End Function,
                                        Function(ByVal i As MemoryStream, ByRef o As [case]) As Boolean
                                            Dim m As mode = Nothing
                                            Dim a As action = Nothing
                                            Dim b() As Byte = Nothing
                                            If bytes_serializer.consume_from(i, m) AndAlso
                                               bytes_serializer.consume_from(i, a) AndAlso
                                               bytes_serializer.consume_from(i, b) Then
                                                o = New [case](m, a, b, True)
                                            End If
                                            Return False
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

    Public Function CloneT() As [case] Implements ICloneable(Of [case]).Clone
        Return New [case](mode, action, meta)
    End Function

    Public Function Clone() As Object Implements ICloneable.Clone
        Return CloneT()
    End Function

    Public Overrides Function ToString() As String
        Return strcat("case[", mode, ", ", action, ", ", meta.to_string(), "]")
    End Function
End Class
