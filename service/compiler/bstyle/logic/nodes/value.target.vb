
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

        Private Shared ReadOnly read_targets As read_scoped(Of String)
        Private Shared ReadOnly write_targets As read_scoped(Of String)

        Shared Sub New()
            read_targets = New read_scoped(Of String)()
            write_targets = New read_scoped(Of String)()
        End Sub

        Public Shared Function read_target() As read_scoped(Of String).ref
            assert(read_targets.size() > 0)
            Return read_targets.pop()
        End Function

        Public Shared Function write_target() As read_scoped(Of String).ref
            assert(write_targets.size() > 0)
            Dim r As read_scoped(Of String).ref = Nothing
            r = write_targets.pop()
            read_targets.push(+r)
            Return r
        End Function

        ' TODO: Check return type of value.
        Public Shared Sub with_temp_target(ByVal l As logic_gens, ByVal n As typed_node, ByVal o As writer)
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            Dim value_name As String = Nothing
            value_name = strcat("raw_value_@", n.word_start(), "-", n.word_end())
            l.define_variable(value_name, types.string, o)
            write_targets.push(value_name)
        End Sub
    End Class
End Class
