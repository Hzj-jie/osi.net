
Imports osi.root.lock
Imports osi.root.connector
Imports osi.root.template
Imports osi.service.selector

Public MustInherit Class device(Of T)
    Inherits device
    Implements idevice(Of T)

    Public Shared Function empty() As idevice(Of T)
        Return New delegate_device(Of T)(Nothing,
                                         validator:=Function(x As T) As Boolean
                                                        Return False
                                                    End Function,
                                         identifier:=Function(x As T) As String
                                                         Return strcat("empty-", type_info(Of T).name, "-device")
                                                     End Function)
    End Function

    Private ReadOnly c As T

    Public Sub New(ByVal c As T)
        MyBase.New()
        Me.c = c
    End Sub

    Public Function [get]() As T Implements idevice(Of T).get
        Return c
    End Function

    Protected MustOverride Function validate(ByVal c As T) As Boolean
    Protected MustOverride Overloads Sub close(ByVal c As T)

    Protected Overridable Overloads Function identity(ByVal c As T) As String
        Return Convert.ToString(c)
    End Function

    Protected Overridable Overloads Sub check(ByVal c As T)
    End Sub

    Protected NotOverridable Overrides Sub close()
        close(c)
    End Sub

    Public NotOverridable Overrides Function identity() As String
        Return identity(c)
    End Function

    Protected NotOverridable Overrides Sub check()
        check(c)
    End Sub

    Protected NotOverridable Overrides Function is_valid() As Boolean
        Return validate(c)
    End Function

    Public Shared Operator +(ByVal this As device(Of T)) As T
        Return If(this Is Nothing, Nothing, this.get())
    End Operator
End Class
