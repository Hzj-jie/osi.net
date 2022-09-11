
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.delegates
Imports osi.root.formation
Imports osi.service.compiler.logic

Partial Public NotInheritable Class bstyle
    Partial Public NotInheritable Class value
        Implements code_gen(Of logic_writer)

        Private Shared Sub define_single_data_slot_temp_target(ByVal type As String,
                                                               ByVal name As String,
                                                               ByVal o As logic_writer)
            assert(Not o Is Nothing)
            type = scope.current().type_alias()(type)
            assert(Not scope.current().structs().types().defined(type))
            assert(Not scope.current().variables().try_resolve(name, Nothing))
            assert(scope.current().variables().define(type, name))
            assert(builders.of_define(name, type).to(o))
        End Sub

        Public Shared Function with_single_data_slot_temp_target(ByVal type As String,
                                                                 ByVal o As logic_writer) As String
            Dim value_name As String = scope.current().temp_logic_name().variable()
            define_single_data_slot_temp_target(type, value_name, o)
            scope.current().value_target().with_value(type, value_name)
            Return value_name
        End Function

        Public Structure with_single_data_slot_temp_target_t
            Implements func_t(Of String, logic_writer, String)

            Public Function run(ByVal i As String, ByVal j As logic_writer) As String _
                    Implements func_t(Of String, logic_writer, String).run
                Return with_single_data_slot_temp_target(i, j)
            End Function
        End Structure

        Public Shared Function with_temp_target(ByVal type As String, ByVal o As logic_writer) As vector(Of String)
            assert(Not o Is Nothing)
            Dim params As scope.struct_def = Nothing
            assert(scope.current().structs().resolve(type, scope.current().temp_logic_name().variable(), params))
            assert(Not params Is Nothing)
            params.primitives.
                   foreach(Sub(ByVal p As builders.parameter)
                               assert(Not p Is Nothing)
                               define_single_data_slot_temp_target(p.type, p.name, o)
                           End Sub)
            Return scope.current().value_target().with_value(type, params.primitives())
        End Function
    End Class
End Class
