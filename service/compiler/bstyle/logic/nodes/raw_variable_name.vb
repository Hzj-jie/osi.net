
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
                                      ByVal handle As Func(Of String, stream(Of builders.parameter), Boolean),
                                      ByVal o As logic_writer) As Boolean
            assert(Not name.null_or_whitespace())
            assert(Not handle Is Nothing)
            assert(Not o Is Nothing)

            Dim type As String = Nothing
            If Not scope.current().variables().resolve(name, type) Then
                Return False
            End If
            Dim ps As scope.struct_def = Nothing
            If Not scope.current().structs().resolve(type, name, ps) Then
                ps = scope.struct_def.of_primitive(type, name)
            End If
            Return handle(type, ps.primitives())
        End Function

        Public Shared Function build(ByVal n As typed_node,
                                     ByVal handle As Func(Of String, stream(Of builders.parameter), Boolean),
                                     ByVal o As logic_writer) As Boolean
            assert(Not n Is Nothing)
            Return build(scope.variable_name.of(n), handle, o)
        End Function

        Public Shared Function build(ByVal name As String, ByVal o As logic_writer) As Boolean
            Return build(name,
                         Function(ByVal type As String, ByVal ps As stream(Of builders.parameter)) As Boolean
                             scope.current().value_target().with_value(type, ps)
                             Return True
                         End Function,
                         o)
        End Function

        Private Function build(ByVal n As typed_node,
                               ByVal o As logic_writer) As Boolean Implements code_gen(Of logic_writer).build
            assert(Not n Is Nothing)
            assert(n.child_count() = 1)
            Return build(scope.variable_name.of(n.child()), o)
        End Function
    End Class
End Class
