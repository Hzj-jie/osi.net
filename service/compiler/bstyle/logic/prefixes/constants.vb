
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.service.compiler.logic
Imports osi.service.interpreter.primitive

Partial Public NotInheritable Class bstyle
    Public NotInheritable Class constants
        Implements statement

        Public Shared ReadOnly int_1 As String = unique_name("int_1")

        Private Shared ReadOnly v As vector(Of def)
        Private ReadOnly l As logic_gens

        Shared Sub New()
            v = vector.of(
                New def("int", int_1, New data_block(1))
            )
        End Sub

        Private Shared Function unique_name(ByVal name As String) As String
            Return "@@prefixes@constants@" + name
        End Function

        Private NotInheritable Class def
            Public ReadOnly type As String
            Public ReadOnly name As String
            Public ReadOnly value As data_block

            Public Sub New(ByVal type As String, ByVal name As String, ByVal value As data_block)
                assert(Not type.null_or_whitespace())
                assert(Not name.null_or_whitespace())
                assert(Not value Is Nothing)
                Me.type = type
                Me.name = name
                Me.value = value
            End Sub
        End Class

        Public Shared Sub register(ByVal p As statements, ByVal l As logic_gens)
            assert(Not p Is Nothing)
            p.register(New constants(l))
        End Sub

        Public Sub export(ByVal o As writer) Implements statement.export
            Dim i As UInt32 = 0
            While i < v.size()
                l.define_variable(v(i).name, v(i).type, o)
                builders.of_copy_const(v(i).name, v(i).value).to(o)
                i += uint32_1
            End While
        End Sub

        Private Sub New(ByVal l As logic_gens)
            assert(Not l Is Nothing)
            Me.l = l
        End Sub
    End Class
End Class
