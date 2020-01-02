
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.template
Imports osi.service.automata
Imports osi.service.compiler.logic

Partial Public NotInheritable Class cstyle
    Partial Public NotInheritable Class value
        Inherits logic_gen_wrapper
        Implements logic_gen

        Public Shared Function current_target() As String
            Return instance_stack(Of annotated_ref(Of value, String)).current().v
        End Function

        Private Shared Sub pop_current_target()
            instance_stack(Of annotated_ref(Of value, String)).pop()
        End Sub

        Public Shared Function with_current_target(ByVal v As String) As IDisposable
            set_current_target(v)
            Return deferring.to(AddressOf pop_current_target)
        End Function

        Private Shared Sub set_current_target(ByVal v As String)
            assert(Not v.null_or_whitespace())
            instance_stack(Of annotated_ref(Of value, String)).push(annotated_ref(Of value).with(v))
        End Sub

        Public Function with_current_target(ByVal n As typed_node) As IDisposable
            set_current_target(n)
            Return deferring.to(AddressOf pop_current_target)
        End Function

        Private Sub set_current_target(ByVal n As typed_node)
            assert(Not n Is Nothing)
            assert(strsame(type_name(n), "name"))
            set_current_target(n.word().str())
        End Sub

        Public NotInheritable Class target
            Implements IDisposable

            Public ReadOnly value_name As String

            Public Sub New(ByVal value_name As String)
                assert(Not value_name.null_or_whitespace())
                Me.value_name = value_name
            End Sub

            Public Sub Dispose() Implements IDisposable.Dispose
                pop_current_target()
            End Sub
        End Class

        Public Function with_value_target(ByVal n As typed_node, ByVal o As writer) As target
            Return with_value_target(n, types.biguint, o)
        End Function

        ' TODO: Check return type of value.
        Public Function with_value_target(ByVal n As typed_node, ByVal type As String, ByVal o As writer) As target
            assert(Not type.null_or_whitespace())
            assert(Not o Is Nothing)
            Dim value_name As String = Nothing
            value_name = set_value_target(n)
            builders.of_define(value_name, type).to(o)
            Return New target(value_name)
        End Function

        Private Function set_value_target(ByVal n As typed_node) As String
            assert(Not n Is Nothing)
            assert(strsame(type_name(n), "value"))
            Dim r As String = Nothing
            r = strcat("raw_value_@", n.word_start(), "-", n.word_end())
            set_current_target(r)
            Return r
        End Function
    End Class
End Class
