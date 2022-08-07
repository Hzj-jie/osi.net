
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.delegates
Imports osi.root.formation
Imports osi.service.compiler.logic
Imports struct_def = osi.service.compiler.scope(Of osi.service.compiler.bstyle.scope).struct_def
Imports target = osi.service.compiler.bstyle.scope.value_target_t.target

Partial Public NotInheritable Class bstyle
    Partial Public NotInheritable Class value
        Implements code_gen(Of logic_writer)

        ' Type of the single data slot is handled by logic.
        Public Shared Function read_target_single_data_slot() As read_scoped(Of target).ref(Of String)
            Return scope.current().value_target().value(Function(ByVal x As target, ByRef o As String) As Boolean
                                                            assert(Not x Is Nothing)
                                                            If x.names.size() <> 1 Then
                                                                Return False
                                                            End If
                                                            o = x.names(0)
                                                            Return True
                                                        End Function)
        End Function

        Public Shared Function read_target() As read_scoped(Of target).ref
            Return scope.current().value_target().value()
        End Function

        Private Shared Sub define_single_data_slot_temp_target(ByVal type As String,
                                                               ByVal name As String,
                                                               ByVal o As logic_writer)
            assert(Not o Is Nothing)
            type = scope.current().type_alias()(type)
            assert(Not scope.current().structs().types().defined(type))
            Dim existing_type As String = Nothing
            If scope.current().variables().try_resolve(name, existing_type) Then
                assert(type.Equals(existing_type))
            Else
                assert(scope.current().variables().define(type, name))
                assert(builders.of_define(name, type).to(o))
            End If
        End Sub

        Public Shared Function with_single_data_slot_temp_target(ByVal type As String,
                                                                 ByVal o As logic_writer) As String
            Dim value_name As String = scope.current().temp_logic_name().variable()
            define_single_data_slot_temp_target(type, value_name, o)
            with_single_data_slot_target(type, value_name)
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
            Dim params As struct_def = Nothing
            assert(scope.current().structs().resolve(type, scope.current().temp_logic_name().variable(), params))
            assert(Not params Is Nothing)
            params.primitives.
                   foreach(Sub(ByVal p As builders.parameter)
                               assert(Not p Is Nothing)
                               define_single_data_slot_temp_target(p.type, p.name, o)
                           End Sub)
            Return with_target(type, params.primitives())
        End Function

        Public Shared Function with_target(ByVal type As String,
                                           ByVal ps As stream(Of builders.parameter)) As vector(Of String)
            assert(Not ps Is Nothing)
            Dim vs As vector(Of String) = ps.map(Function(ByVal p As builders.parameter) As String
                                                     assert(Not p Is Nothing)
                                                     assert(Not p.name.null_or_whitespace())
                                                     Return p.name
                                                 End Function).
                                             collect_to(Of vector(Of String))()
            scope.current().value_target().with_value(type, vs)
            Return vs
        End Function

        Public Shared Function with_single_data_slot_target(ByVal type As String, ByVal v As String) As String
            assert(Not v.null_or_whitespace())
            scope.current().value_target().with_value(type, vector.emplace_of(v))
            Return v
        End Function
    End Class
End Class
