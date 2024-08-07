
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.automata
Imports builders = osi.service.compiler.logic.builders

Partial Public NotInheritable Class bstyle
    Private NotInheritable Class raw_variable_name
        Implements code_gen(Of logic_writer)

        Private Shared Function build(ByVal name As String,
                                      ByVal struct_handle As Func(Of String, stream(Of builders.parameter), Boolean),
                                      ByVal primitive_type_handle As Func(Of String, String, Boolean),
                                      ByVal o As logic_writer) As Boolean
            assert(Not name.null_or_whitespace())
            assert(Not struct_handle Is Nothing)
            assert(Not primitive_type_handle Is Nothing)
            assert(Not o Is Nothing)

            Dim type As String = Nothing
            If Not scope.current().variables().resolve(name, type) Then
                Return False
            End If
            Dim ps As scope.struct_def = Nothing
            If scope.current().structs().resolve(type, name, ps) Then
                Return struct_handle(type, ps.primitives())
            End If
            Return primitive_type_handle(type, name)
        End Function

        Public Shared Function build(ByVal n As typed_node,
                                     ByVal struct_handle As Func(Of String, stream(Of builders.parameter), Boolean),
                                     ByVal primitive_type_handle As Func(Of String, String, Boolean),
                                     ByVal o As logic_writer) As Boolean
            assert(Not n Is Nothing)
            Return build(n.input_without_ignored(), struct_handle, primitive_type_handle, o)
        End Function

        Public Shared Function build(ByVal name As String, ByVal o As logic_writer) As Boolean
            Return build(name,
                         Function(ByVal type As String, ByVal ps As stream(Of builders.parameter)) As Boolean
                             scope.current().value_target().with_value(type, ps)
                             Return True
                         End Function,
                         Function(ByVal type As String, ByVal source As String) As Boolean
                             scope.current().value_target().with_value(type, source)
                             Return True
                         End Function,
                         o)
        End Function

        Private Function build(ByVal n As typed_node,
                               ByVal o As logic_writer) As Boolean Implements code_gen(Of logic_writer).build
            assert(Not n Is Nothing)
            assert(n.child_count() = 1)
            Return build(n.child().input_without_ignored(), o)
        End Function
    End Class
End Class
