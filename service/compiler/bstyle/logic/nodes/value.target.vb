
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.automata
Imports osi.service.compiler.logic

Partial Public NotInheritable Class bstyle
    Partial Public NotInheritable Class value
        Inherits logic_gen_wrapper
        Implements logic_gen

        Private ReadOnly read_targets As New read_scoped(Of vector(Of String))()

        Public Function read_target_only() As read_scoped(Of vector(Of String)).ref(Of String)
            Return read_targets.pop(Function(ByVal x As vector(Of String)) As String
                                        assert(Not x Is Nothing)
                                        assert(x.size() = 1)
                                        Return x(0)
                                    End Function)
        End Function

        Public Function read_target() As read_scoped(Of vector(Of String)).ref
            Return read_targets.pop()
        End Function

        Private Sub with_internal_typed_temp_target(ByVal type As String,
                                                    ByVal name As String,
                                                    ByVal o As writer)
            assert(Not o Is Nothing)
            type = scope.current().type_alias()(type)
            Dim existing_type As String = Nothing
            If scope.current().variables().try_resolve(name, existing_type) Then
                assert(type.Equals(existing_type))
            Else
                assert(scope.current().variables().define(type, name))
                builders.of_define(name, type).to(o)
            End If
        End Sub

        Private Shared Function value_name(ByVal n As typed_node) As String
            Return strcat("raw_value_@",
                          code_builder.current().nested_build_level(),
                          "@",
                          n.word_start(),
                          "-",
                          n.word_end())
        End Function

        Public Function with_internal_typed_temp_target(ByVal type As String,
                                                        ByVal n As typed_node,
                                                        ByVal o As writer) As String
            Dim value_name As String = value.value_name(n)
            assert(Not scope.current().structs().resolve(type, value_name, Nothing))
            with_internal_typed_temp_target(type, value_name, o)
            read_targets.push(vector.of(value_name))
            Return value_name
        End Function

        Public Function with_temp_target(ByVal type As String,
                                         ByVal n As typed_node,
                                         ByVal o As writer) As vector(Of String)
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            Dim value_name As String = value.value_name(n)
            Dim params As vector(Of builders.parameter) = Nothing
            Dim vs As New vector(Of String)()
            If scope.current().structs().resolve(type, value_name, params) Then
                params.stream().foreach(Sub(ByVal p As builders.parameter)
                                            assert(Not p Is Nothing)
                                            with_internal_typed_temp_target(p.type, p.name, o)
                                            vs.emplace_back(p.name)
                                        End Sub)
            Else
                with_internal_typed_temp_target(type, value_name, o)
                vs.emplace_back(value_name)
            End If
            read_targets.push(vs)
            Return vs
        End Function
    End Class
End Class
