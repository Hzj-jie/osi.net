
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.service.automata
Imports builders = osi.service.compiler.logic.builders

Partial Public NotInheritable Class bstyle
    Private NotInheritable Class static_cast
        Implements code_gen(Of logic_writer)

        Private Function build(ByVal n As typed_node,
                               ByVal o As logic_writer) As Boolean Implements code_gen(Of logic_writer).build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(n.child_count() = 6)
            Dim name As String = scope.fully_qualified_variable_name.of(n.child(2))
            Dim type As String = scope.normalized_type.parameter_type_of(n.child(4)).full_type()
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
                Dim sdef As scope.struct_def = Nothing
                assert(scope.current().structs().resolve(type, name, sdef))
                assert(Not sdef Is Nothing)
                assert(sdef.primitive_count() > 0)
                Return sdef.for_each_primitive(Function(ByVal t As builders.parameter) As Boolean
                                                   Return value_declaration.declare_primitive_type(t.non_ref_type(),
                                                                                                   t.name,
                                                                                                   o)
                                               End Function) AndAlso
                       builders.start_scope(o).of(
                           Function() As Boolean
                               Dim offset_name As String = "@offset"
                               ' TODO: Using builders.of_define is sufficient.
                               Return value_declaration.declare_primitive_type(
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
                           End Function) AndAlso
                       builders.of_undefine(name).to(o) AndAlso
                       scope.current().variables().redefine(type, name)
            End If
            If scope.current().structs().variables().defined(name) Then
                ' Convert from struct ptr to type_ptr.
                Dim sdef As scope.struct_def = Nothing
                assert(scope.current().structs().variables().resolve(name, sdef))
                assert(Not sdef Is Nothing)
                Return builders.of_define(name, type).to(o) AndAlso
                       builders.of_copy(name, sdef.primitives().
                                              find_first().
                                              map(Function(ByVal x As builders.parameter) As String
                                                      assert(Not x Is Nothing)
                                                      Return x.name
                                                  End Function).
                                              or_assert()).to(o) AndAlso
                       sdef.for_each_primitive(Function(ByVal t As builders.parameter) As Boolean
                                                   Return builders.of_undefine(t.name).to(o) AndAlso
                                                          scope.current().variables().undefine(t.name)
                                               End Function) AndAlso
                       scope.current().variables().redefine(type, name)
            End If
            raise_error(error_type.user, "Unsupported static_cast ", name, " to ", type)
            Return False
        End Function
    End Class
End Class
