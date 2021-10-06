
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.interpreter.primitive

Namespace logic
    Public NotInheritable Class interrupt
        Implements exportable

        Private ReadOnly types As types
        Private ReadOnly functions As interrupts
        Private ReadOnly function_name As String
        Private ReadOnly parameter As String
        Private ReadOnly result As String

        Public Sub New(ByVal types As types,
                       ByVal functions As interrupts,
                       ByVal function_name As String,
                       ByVal parameter As String,
                       ByVal result As String)
            assert(Not types Is Nothing)
            assert(Not functions Is Nothing)
            assert(Not String.IsNullOrEmpty(function_name))
            assert(Not String.IsNullOrEmpty(parameter))
            assert(Not String.IsNullOrEmpty(result))
            Me.types = types
            Me.functions = functions
            Me.function_name = function_name
            Me.parameter = parameter
            Me.result = result
        End Sub

        Public Function export(ByVal scope As scope,
                               ByVal o As vector(Of String)) As Boolean Implements exportable.export
            assert(Not scope Is Nothing)
            assert(Not o Is Nothing)
            Dim function_id As UInt32 = 0
            If Not functions.of(function_name, function_id) Then
                errors.interrupt_undefined(function_name)
                Return False
            End If

            Dim cpc As copy_const = Nothing
            cpc = New copy_const(types, result, unique_ptr.[New](New data_block(function_id)))
            If Not cpc.export(scope, o) Then
                Return False
            End If

            Dim p As variable = Nothing
            If Not variable.of_stack(scope, parameter, p) Then
                Return False
            End If

            Dim r As variable = Nothing
            assert(variable.of_stack(scope, result, r))  ' Otherwise cpc.export() should not succeed.

            o.emplace_back(instruction_builder.str(command.int, r, p, r))
            Return True
        End Function
    End Class
End Namespace
