
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.automata
Imports osi.service.compiler.logic

Partial Public NotInheritable Class bstyle
    Partial Public NotInheritable Class value
        Inherits logic_gen_wrapper
        Implements logic_gen

        Private ReadOnly read_targets As New read_scoped(Of String)()

        Public Function read_target() As read_scoped(Of String).ref
            assert(read_targets.size() > 0)
            Return read_targets.pop()
        End Function

        Public Function with_temp_target(ByVal type As String, ByVal n As typed_node, ByVal o As writer) As String
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            Dim value_name As String = strcat("raw_value_@",
                                              code_builder.current().nested_build_level(),
                                              "@",
                                              n.word_start(),
                                              "-",
                                              n.word_end())
            type = scope.current().type_alias()(type)
            Dim existing_type As String = Nothing
            If scope.current().variables().try_resolve(value_name, existing_type) Then
                assert(type.Equals(existing_type))
            Else
                assert(scope.current().variables().define(type, value_name))
                builders.of_define(value_name, type).to(o)
            End If
            read_targets.push(value_name)
            Return value_name
        End Function
    End Class
End Class
