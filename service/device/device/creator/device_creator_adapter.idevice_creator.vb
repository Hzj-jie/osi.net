
Imports osi.root.connector

Partial Public Class device_creator_adapter(Of IT, OT)
    Private Class for_idevice_creator
        Implements idevice_creator(Of OT)

        Private ReadOnly i As idevice_creator(Of IT)
        Private ReadOnly c As Func(Of IT, OT)

        Public Sub New(ByVal i As idevice_creator(Of IT), ByVal c As Func(Of IT, OT))
            assert(i IsNot Nothing)
            assert(c IsNot Nothing)
            Me.i = i
            Me.c = c
        End Sub

        Public Function create(ByRef o As idevice(Of OT)) As Boolean Implements idevice_creator(Of OT).create
            Dim it As idevice(Of IT) = Nothing
            If i.create(it) Then
                o = device_adapter.[New](Of IT, OT)(it, c)  ' VS 2010
                Return True
            Else
                Return False
            End If
        End Function
    End Class
End Class
