﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.service.constructor
Imports osi.service.interpreter.primitive

Namespace logic
    Partial Public NotInheritable Class scope
        Inherits scope(Of scope)

        Private ReadOnly v As New variable_t(Me)

        <inject_constructor>
        Public Sub New(ByVal parent As scope)
            MyBase.New(parent)
        End Sub

        Public Sub New()
            Me.New(Nothing)
        End Sub
    End Class

    Public NotInheritable Class scope_wrapper
        Inherits scope_wrapper(Of scope)

        Private ReadOnly o As vector(Of String)

        Public Sub New(ByVal o As vector(Of String))
            MyBase.New(logic.scope.current())
            assert(Not o Is Nothing)
            Me.o = o
        End Sub

        Protected Overrides Sub when_dispose()
            new_scope.variables().
                      heap().
                      stream().
                      foreach(Sub(ByVal name As String)
                                  Dim v As String = Nothing
                                  assert(new_scope.variables().export(name).data_ref.export(v))
                                  o.emplace_back(instruction_builder.str(command.dealloc, v))
                              End Sub)
            Dim i As UInt32 = 0
            While i < new_scope.variables().size()
                o.emplace_back(instruction_builder.str(command.pop))
                i += uint32_1
            End While
            MyBase.when_dispose()
        End Sub
    End Class
End Namespace
