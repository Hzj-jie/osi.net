
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.compiler.logic

Partial Public NotInheritable Class bstyle
    ' TODO: Remove in favor of builders.parameter.
    Public NotInheritable Class single_data_slot_variable
        Public ReadOnly type As String
        Public ReadOnly name As String

        Public Sub New(ByVal type As String, ByVal name As String)
            assert(Not type.null_or_whitespace())
            assert(Not name.null_or_whitespace())
            type = scope.current().type_alias()(type)
            assert(Not scope.current().structs().defined(type))

            Me.type = type
            Me.name = name
        End Sub

        Public Function with_index(ByVal index As String) As single_data_slot_variable
            Return New single_data_slot_variable(type, variable.name_of(name, index))
        End Function

        Public Shared Function to_builders_parameter(ByVal this As single_data_slot_variable) As builders.parameter
            assert(Not this Is Nothing)
            Return builders.parameter.no_ref(this.type, this.name)
        End Function

        Public Shared Function from_builders_parameter(ByVal this As builders.parameter) As single_data_slot_variable
            assert(Not this Is Nothing)
            Return New single_data_slot_variable(this.type, this.name)
        End Function
    End Class
End Class
