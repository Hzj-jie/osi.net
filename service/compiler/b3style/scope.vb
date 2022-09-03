﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.service.constructor

Partial Public NotInheritable Class b3style
    Partial Public NotInheritable Class scope
        Inherits scope(Of logic_writer, code_builder_proxy, code_gens_proxy, scope)

        <inject_constructor>
        Public Sub New(ByVal parent As scope)
            MyBase.New(parent)
        End Sub

        Public Sub New()
            Me.New(Nothing)
        End Sub
    End Class
End Class