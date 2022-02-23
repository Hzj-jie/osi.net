
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.interpreter.primitive

Namespace logic
    Public NotInheritable Class _interrupt
        Implements instruction_gen

        Private ReadOnly function_name As String
        Private ReadOnly parameter As String
        Private ReadOnly result As String

        Public Sub New(ByVal function_name As String,
                       ByVal parameter As String,
                       ByVal result As String)
            assert(Not String.IsNullOrEmpty(function_name))
            assert(Not String.IsNullOrEmpty(parameter))
            assert(Not String.IsNullOrEmpty(result))
            Me.function_name = function_name
            Me.parameter = parameter
            Me.result = result
        End Sub

        Public Function build(ByVal o As vector(Of String)) As Boolean Implements instruction_gen.build
            assert(o IsNot Nothing)
            Dim function_id As UInt32 = 0
            If Not scope.current().functions().of(function_name, function_id) Then
                errors.interrupt_undefined(function_name)
                Return False
            End If

            Dim cpc As New _copy_const(result, New data_block(function_id))
            If Not cpc.build(o) Then
                Return False
            End If

            Dim p As variable = Nothing
            If Not variable.of(parameter, o, p) Then
                Return False
            End If

            Dim r As variable = Nothing
            assert(variable.of(result, o, r))  ' Otherwise cpc.export() should not succeed.

            o.emplace_back(instruction_builder.str(command.int, r, p, r))
            Return True
        End Function
    End Class
End Namespace
