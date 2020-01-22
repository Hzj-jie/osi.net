
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.automata
Imports osi.service.compiler.logic
Imports osi.service.constructor

Partial Public NotInheritable Class bstyle
    Public NotInheritable Class variable_name
        Inherits logic_gen_wrapper
        Implements logic_gen

        <inject_constructor>
        Public Sub New(ByVal b As logic_gens)
            MyBase.New(b)
        End Sub

        Public Shared Sub register(ByVal b As logic_gens)
            assert(Not b Is Nothing)
            b.register(Of variable_name)()
        End Sub

        Public Function build(ByVal n As typed_node, ByVal o As writer) As Boolean Implements logic_gen.build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(n.child_count() = 1)
            Using r As read_scoped(Of value.write_target_ref).ref = value.write_target()
                Dim variable_name As String = Nothing
                variable_name = n.child().word().str()
                With +r
                    .set_type(macros.type_of(variable_name))
                    builders.of_copy(.name, variable_name).to(o)
                End With
            End Using
            Return True
        End Function
    End Class
End Class
