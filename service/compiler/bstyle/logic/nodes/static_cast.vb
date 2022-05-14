
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.service.automata
Imports osi.service.compiler.logic

Partial Public NotInheritable Class bstyle
    Private NotInheritable Class static_cast
        Implements code_gen(Of logic_writer)

        Public Shared ReadOnly instance As New static_cast()

        Private Sub New()
        End Sub

        Public Function build(ByVal n As typed_node,
                              ByVal o As logic_writer) As Boolean Implements code_gen(Of logic_writer).build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(n.child_count() = 6)
            Dim name As String = n.child(2).input_without_ignored()
            Dim type As String = scope.current().type_alias()(n.child(4).input_without_ignored())
            If scope.current().structs().types().defined(type) AndAlso
               scope.current().structs().variables().defined(name) Then
                ' Convert from struct ptr to struct ptr.
                raise_error(error_type.user,
                            "Cannot static_cast a struct instance of ",
                            name,
                            " to another struct type ",
                            type,
                            ", convert it to ",
                            compiler.logic.scope.type_t.ptr_type,
                            " first. ",
                            "TODO: Implementing this ""shortcut"" is quite complicated since the struct fields need ",
                            "to be carefully considered to avoid conflicting with the target type.")
                Return False
            End If
            If scope.current().structs().types().defined(type) Then
                ' Convert from type_ptr to struct ptr.
                ' TODO: Try to avoid building the struct_def twice.
                assert(struct.define_in_stack(type, name, o))
                Dim sdef As struct_def = Nothing
                assert(scope.current().structs().resolve(type, name, sdef))
                assert(Not sdef Is Nothing)
                assert(sdef.primitive_count() > 0)
                Return builders.start_scope(o).of(
                    Function() As Boolean
                        Dim offset_name As String = "@offset"
                        Return value_declaration.declare_single_data_slot(
                                   compiler.logic.scope.type_t.ptr_type,
                                   offset_name,
                                   o) AndAlso
                               sdef.for_each_primitive(
                                   Function(ByVal t As builders.parameter) As Boolean
                                       ' TODO: Include bstyle/lib/const.h automatically.
                                       Return builders.of_add(t.name, offset_name, name).to(o) AndAlso
                                              builders.of_add(offset_name,
                                                              offset_name,
                                                              "@@prefixes@constants@ptr_offset").to(o)
                                   End Function)
                    End Function)
            ElseIf scope.current().structs().variables().defined(name) Then
                ' Convert from struct ptr to type_ptr.
                ' TODO: Implementation
            End If
            ' Convert from type_ptr to type_ptr
            Return scope.current().variables().redefine(type, name)
        End Function
    End Class
End Class
