
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.automata
Imports osi.service.compiler.logic

Partial Public NotInheritable Class bstyle
    Partial Public NotInheritable Class value
        Inherits logic_gen_wrapper
        Implements logic_gen

        Private ReadOnly read_targets As read_scoped(Of String)
        Private ReadOnly write_targets As read_scoped(Of write_target_ref)
        Private ReadOnly defined_temp_targets As [set](Of String)

        Public Function read_target() As read_scoped(Of String).ref
            assert(read_targets.size() > 0)
            Return read_targets.pop()
        End Function

        Public Function write_target() As read_scoped(Of write_target_ref).ref
            assert(write_targets.size() > 0)
            Dim r As read_scoped(Of write_target_ref).ref = Nothing
            r = write_targets.pop()
            read_targets.push((+r).name)
            Return r
        End Function

        Public NotInheritable Class write_target_ref
            Public ReadOnly name As String
            Private ReadOnly type As type_ref

            Public Sub New(ByVal name As String, ByVal type As type_ref)
                assert(Not name.null_or_whitespace())
                assert(Not type Is Nothing)
                Me.name = name
                Me.type = type
            End Sub

            Public Sub set_type(ByVal type As String)
                Me.type.type = type
            End Sub
        End Class

        Public NotInheritable Class type_ref
            Inherits builders.type_ref

            Private ReadOnly ta As type_alias
            Public type As String

            Public Sub New(ByVal ta As type_alias)
                assert(Not ta Is Nothing)
                Me.ta = ta
            End Sub

            Public Overrides Function ToString() As String
                assert(Not type.null_or_whitespace())
                Return ta(type)
            End Function
        End Class

        Private Sub with_temp_target(ByVal ta As type_alias, ByVal n As typed_node, ByVal o As writer)
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            Dim value_name As String = Nothing
            value_name = strcat("raw_value_@", n.word_start(), "-", n.word_end())
            Dim type As type_ref = Nothing
            type = New type_ref(ta)
            If defined_temp_targets.find(value_name) = defined_temp_targets.end() Then
                builders.of_define(value_name, type).to(o)
                defined_temp_targets.emplace(value_name)
            End If
            write_targets.push(New write_target_ref(value_name, type))
        End Sub
    End Class
End Class
