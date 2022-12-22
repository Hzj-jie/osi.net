
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.template
Imports osi.root.utils
Imports osi.service.resource
Imports statements = osi.service.compiler.statements(Of osi.service.compiler.logic_writer)

Partial Public NotInheritable Class b3style
    Inherits logic_rule_wrapper(Of nlexer_rule_t, syntaxer_rule_t, prefixes_t, suffixes_t, logic_gens_t, scope)

    Private Shared ReadOnly folder As String = Path.Combine(temp_folder, "service/compiler/b3style")

    Public NotInheritable Class nlexer_rule_t
        Inherits __do(Of String)

        Private Shared ReadOnly file As String = Path.Combine(folder, "nlexer_rule.txt")

        Shared Sub New()
            static_constructor(Of b2style.nlexer_rule_t).execute()
            b3style_rules.nlexer_rule.sync_export(file)
        End Sub

        Protected Overrides Function at() As String
            Return file
        End Function
    End Class

    Public NotInheritable Class syntaxer_rule_t
        Inherits __do(Of String)

        Private Shared ReadOnly file As String = Path.Combine(folder, "syntaxer_rule.txt")

        Shared Sub New()
            static_constructor(Of b2style.syntaxer_rule_t).execute()
            b3style_rules.syntaxer_rule.sync_export(file)
        End Sub

        Protected Overrides Function at() As String
            Return file
        End Function
    End Class

    Public NotInheritable Class prefixes_t
        Inherits __do(Of vector(Of Action(Of statements)))

        Protected Overrides Function at() As vector(Of Action(Of statements))
            Return vector.emplace_of(Of Action(Of statements))(AddressOf bstyle.code_types.register)
        End Function
    End Class

    Public NotInheritable Class suffixes_t
        Inherits __do(Of vector(Of Action(Of statements)))

        Protected Overrides Function at() As vector(Of Action(Of statements))
            Return vector.emplace_of(Of Action(Of statements))(AddressOf bstyle.main.register,
                                                               AddressOf scope.call_hierarchy_t.calculator.register)
        End Function
    End Class

    Public NotInheritable Class logic_gens_t
        Inherits __do(Of vector(Of Action(Of code_gens(Of logic_writer))))

        Protected Overrides Function at() As vector(Of Action(Of code_gens(Of logic_writer)))
            Return New code_gens_registrar(Of logic_writer)().
                with(Of include_with_file)().
                with(Of include_with_string)().
                with(Of biguint)().
                with(Of bool)().
                with(Of _integer)().
                with(Of _string)().
                with(Of ufloat)().
                with(Of bstyle.logic)().
                with(scope.define_t.code_gens.ifndef_wrapped(AddressOf code_gen_of)).
                with(scope.define_t.code_gens.define()).
                with(Of bstyle.typedef)().
                with(Of bstyle.typedef_type_name)().
                with(Of bstyle.typedef_type_str)().
                with(Of bstyle.heap_declaration(Of code_builder_proxy, code_gens_proxy, scope))().
                with(Of bstyle.heap_name(Of code_builder_proxy, code_gens_proxy, scope))().
                with(Of bstyle.struct(Of code_builder_proxy, code_gens_proxy, scope))().
                with(Of bstyle.value_declaration(Of code_builder_proxy, code_gens_proxy, scope))().
                with(Of bstyle.value_definition(Of code_builder_proxy, code_gens_proxy, scope))().
                with(Of bstyle.value_clause(Of code_builder_proxy, code_gens_proxy, scope))().
                with(Of bstyle.function_call(Of code_builder_proxy, code_gens_proxy, scope))().
                with(Of bstyle.return_clause(Of code_builder_proxy, code_gens_proxy, scope))().
                with(Of bstyle.static_cast(Of code_builder_proxy, code_gens_proxy, scope))().
                with(Of bstyle.ignore_result_function_call(Of code_builder_proxy, code_gens_proxy, scope))().
                with(Of bstyle.value_list(Of code_builder_proxy, code_gens_proxy, scope))().
                without(Of bstyle.raw_variable_name(Of code_builder_proxy, code_gens_proxy, scope))().
                with(Of bstyle.kw_file)().
                with(Of bstyle.kw_func)().
                with(Of bstyle.kw_line)().
                with(Of bstyle.kw_statement)()
        End Function
    End Class
End Class