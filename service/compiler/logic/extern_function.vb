
Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.interpreter.primitive

Namespace logic
    Public Class extern_function
        Implements exportable

        Private ReadOnly types As types
        Private ReadOnly functions As extern_functions
        Private ReadOnly function_name As String
        Private ReadOnly parameter As String
        Private ReadOnly result As String

        Public Sub New(ByVal types As types,
                       ByVal functions As extern_functions,
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
            If Not functions.find_extern_function(function_name, function_id) Then
                errors.extern_function_undefined(function_name)
                Return False
            End If

            Dim movc As move_const = Nothing
            movc = New move_const(types, result, unique_ptr.[New](New data_block(function_id)))
            If Not movc.export(scope, o) Then
                Return False
            End If

            Dim p As variable = Nothing
            If Not variable.[New](scope, parameter, p) Then
                Return False
            End If

            Dim r As variable = Nothing
            assert(variable.[New](scope, result, r))  ' Otherwise movc.export() should not succeed.

            o.emplace_back(instruction_builder.str(command.extern, r.ref, p.ref, r.ref))
            Return True
        End Function
    End Class
End Namespace
