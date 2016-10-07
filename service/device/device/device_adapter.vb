
Imports osi.root.connector
Imports osi.service.argument
Imports osi.service.selector

Public NotInheritable Class device_adapter
    Private Sub New()
    End Sub

    Public Shared Function [New](Of IT, OT) _
                                (ByVal input As idevice(Of IT), ByVal c As Func(Of IT, OT)) As device_adapter(Of IT, OT)
        assert(Not input Is Nothing)
        assert(Not c Is Nothing)
        Return [New](input, c(input.get()))
    End Function

    Public Shared Function [New](Of IT, OT) _
                                (ByVal input As idevice(Of IT), ByVal output As OT) As device_adapter(Of IT, OT)
        Return New device_adapter(Of IT, OT)(input, output)
    End Function

    Public Shared Function [New](Of IT, OT) _
                                (ByVal input As idevice(Of async_getter(Of IT)), ByVal c As Func(Of IT, OT)) _
                                As device_adapter(Of async_getter(Of IT), async_getter(Of OT))
        assert(Not input Is Nothing)
        assert(Not c Is Nothing)
        Return [New](Of IT, OT)(input, async_getter_adapter.new_async_getter(input.get(), c))
    End Function

    Public Shared Function [New](Of IT, OT) _
                                (ByVal input As idevice(Of async_getter(Of IT)), ByVal output As async_getter(Of OT)) _
                                As device_adapter(Of async_getter(Of IT), async_getter(Of OT))
        Return New device_adapter(Of async_getter(Of IT), async_getter(Of OT))(input, output)
    End Function

    Public Shared Function wrap(Of T)(ByVal v As var, ByVal i As idevice(Of T), ByRef o As idevice(Of T)) As Boolean
        If i Is Nothing Then
            Return False
        Else
            Dim w As T = Nothing
            If wrapper.wrap(v, i.get(), w) Then
                o = [New](i, w)
                Return True
            Else
                o = i
                Return True
            End If
        End If
    End Function
End Class

Public Class device_adapter(Of IT, OT)
    Inherits device(Of OT)

    Public ReadOnly input As idevice(Of IT)
    Private ReadOnly id As String

    Public Sub New(ByVal input As idevice(Of IT), ByVal output As OT)
        MyBase.New(output)
        assert(Not input Is Nothing)
        Me.input = input
        Me.id = strcat(input.identity(), "_", type_info(Of OT).name, "_adapter")
    End Sub

    Protected NotOverridable Overrides Function identity(ByVal c As OT) As String
        Return id
    End Function

    Protected NotOverridable Overrides Sub close(ByVal c As OT)
        input.close()
    End Sub

    Protected NotOverridable Overrides Function validate(ByVal c As OT) As Boolean
        Return input.is_valid()
    End Function

    Protected NotOverridable Overrides Function close_when_finalize() As Boolean
        Return False
    End Function
End Class
