
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.envs
Imports osi.root.formation
Imports osi.service.interpreter.primitive

Namespace logic
    Public NotInheritable Class instruction_gen_wrapper
        Implements instruction_gen

        Private Shared ReadOnly debug_logic_instruction As Boolean = env_bool(env_keys("debug", "logic", "instruction"))

        Private ReadOnly v As vector(Of String)
        Private ReadOnly start As UInt32
        Private ReadOnly [end] As UInt32
        Private ReadOnly e As instruction_gen

        Private Sub New(ByVal v As vector(Of String),
                        ByVal start As UInt32,
                        ByVal [end] As UInt32,
                        ByVal e As instruction_gen)
            assert(v IsNot Nothing)
            assert(start >= 0 AndAlso [end] > start AndAlso [end] <= v.size())
            assert(e IsNot Nothing)
            Me.v = New vector(Of String)()
            assert(Me.v.emplace_back(+v, start, [end] - start))
            Me.e = e
        End Sub

        Public Shared Function maybe_wrap(ByVal v As vector(Of String),
                                          ByVal start As UInt32,
                                          ByVal [end] As UInt32,
                                          ByVal e As instruction_gen) As instruction_gen
            If debug_logic_instruction OrElse isdebugmode() Then
                Return New instruction_gen_wrapper(v, start, [end], e)
            End If
            Return e
        End Function

        Public Function build(ByVal o As vector(Of String)) As Boolean Implements instruction_gen.build
            assert(o IsNot Nothing)
            Dim start As UInt32 = o.size()
            If Not e.build(o) Then
                Return False
            End If
            If o.size() > start Then
                Dim comment As String = v.str()
                ' The size of o cannot be changed.
                o(start) = strcat(o(start), character.tab, comment_builder.str(">>>", comment))
                If o.size() - uint32_1 > start Then
                    o(o.size() - uint32_1) = strcat(o(o.size() - uint32_1),
                                                    character.tab, comment_builder.str("<<<", comment))
                End If
            End If
            Return True
        End Function
    End Class
End Namespace
