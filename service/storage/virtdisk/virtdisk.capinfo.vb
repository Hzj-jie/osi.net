
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.envs

Partial Public Class virtdisk
    Private Class capinfo
        Private ReadOnly di As storage.capinfo
        Private ReadOnly is_fs As Boolean
        Private ReadOnly is_ms As Boolean

        Public Sub New(ByVal i As Stream)
            is_fs = False
            is_ms = False
            di = Nothing
            If i Is Nothing Then
                raise_error(error_type.warning,
                            "do not get a valid stream instance for virtdisk, the capacity will always return 0")
            ElseIf TypeOf i Is FileStream Then
                is_fs = True
                di = New storage.capinfo(i.direct_cast_to(Of FileStream)().Name())
            ElseIf TypeOf i Is MemoryStream Then
                is_ms = True
            Else
                raise_error(error_type.warning,
                            "unsupported input stream type for virtdisk, the capacity will always return 0")
            End If
        End Sub

        Public Shared Operator +(ByVal i As capinfo) As UInt64
            If i Is Nothing Then
                Return 0
            ElseIf i.is_fs Then
                assert(Not i.di Is Nothing)
                Return i.di.capacity()
            ElseIf i.is_ms Then
                Return CULng(available_virtual_memory())
            Else
                Return 0
            End If
        End Operator
    End Class
End Class
