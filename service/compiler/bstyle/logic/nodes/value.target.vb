
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

        Private Shared ReadOnly ws As write_scoped(Of String)

        Shared Sub New()
            ws = New write_scoped(Of String)()
        End Sub

        Public Shared Function current_target() As String
            Return ws.current()
        End Function

        Public Shared Function with_current_target(ByVal v As String) As IDisposable
            assert(Not v.null_or_whitespace())
            Return ws.push(v)
        End Function

        Public Function with_current_target(ByVal n As typed_node) As IDisposable
            assert(Not n Is Nothing)
            assert(strsame(n.type_name, "name"))
            Return ws.push(n.word().str())
        End Function

        Public Function with_value_target(ByVal n As typed_node, ByVal o As writer) As write_scoped(Of String).ref
            Return with_value_target(n, types.biguint, o)
        End Function

        ' TODO: Check return type of value.
        Public Function with_value_target(ByVal n As typed_node,
                                          ByVal type As String,
                                          ByVal o As writer) As write_scoped(Of String).ref
            assert(Not n Is Nothing)
            assert(strsame(n.type_name, "value"))
            assert(Not type.null_or_whitespace())
            assert(Not o Is Nothing)
            Dim value_name As String = Nothing
            value_name = strcat("raw_value_@", n.word_start(), "-", n.word_end())
            builders.of_define(value_name, type).to(o)
            Return ws.push(value_name)
        End Function
    End Class
End Class
