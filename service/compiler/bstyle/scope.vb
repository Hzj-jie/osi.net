﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.service.constructor

Partial Public NotInheritable Class bstyle
    Partial Public NotInheritable Class scope
        Inherits scope(Of scope)

        Private ReadOnly d As defines_t
        Private ReadOnly ta As New type_alias_t()
        Private ReadOnly s As New struct_t()
        Private ReadOnly v As New variable_t()
        Private ReadOnly f As function_t
        Private ReadOnly fc As function_call_t
        Private cf As current_function_t

        <inject_constructor>
        Public Sub New(ByVal parent As scope)
            MyBase.New(parent)
        End Sub

        Public Sub New()
            Me.New(Nothing)
            d = New defines_t()
            f = New function_t()
            fc = New function_call_t()
        End Sub
    End Class

    Public NotInheritable Class scope_wrapper
        Inherits scope_wrapper(Of scope)

        Public Sub New()
            MyBase.New(bstyle.scope.current())
        End Sub
    End Class
End Class
