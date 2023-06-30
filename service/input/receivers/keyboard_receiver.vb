
Imports osi.root.connector
Imports osi.root.utils
Imports osi.root.constants
Imports osi.root.procedure

Public MustInherit Class keyboard_receiver
    Inherits status_receiver(Of keyboard_status)

    Protected Sub New(ByVal rs As keyboard_status)
        MyBase.New(rs)
    End Sub

    Protected Sub New()
        Me.New(New keyboard_status())
    End Sub

    Protected NotOverridable Overrides Function check_case(ByVal c As [case]) As Boolean
        Return MyBase.check_case(c) AndAlso
               c.valid_keyboard_case()
    End Function

    Protected MustOverride Overloads Function handle(ByVal action As action,
                                                     ByVal kc As Int32,
                                                     ByRef ec As event_comb) As Boolean

    Protected NotOverridable Overrides Function handle(ByVal c As [case],
                                                       ByRef ec As event_comb) As Boolean
        assert(Not c Is Nothing)
        Dim kc As Int32 = 0
        If c.keyboard_code(kc) Then
            Return handle(c.action, kc, ec)
        Else
            Return False
        End If
    End Function
End Class
