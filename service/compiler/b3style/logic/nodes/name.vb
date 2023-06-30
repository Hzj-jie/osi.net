
Option Explicit On
Option Infer On
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.automata
Imports builders = osi.service.compiler.logic.builders

Partial Public NotInheritable Class b3style
    Private NotInheritable Class name
        Implements code_gen(Of logic_writer)

        Public Function build(ByVal n As typed_node,
                              ByVal o As logic_writer) As Boolean Implements code_gen(Of logic_writer).build
            ' TODO: This is a temporary solution to make bstyle work in b3style.
            assert(Not n Is Nothing)
            Return raw_variable_name.build(n,
                                           Function(ByVal type As String,
                                                    ByVal ps As stream(Of builders.parameter)) As Boolean
                                               scope.current().value_target().with_value(type, ps)
                                               Return True
                                           End Function,
                                           Function(ByVal type As String,
                                                    ByVal source As String) As Boolean
                                               scope.current().value_target().with_value(type, source)
                                               Return True
                                           End Function,
                                           o)
        End Function
    End Class
End Class
