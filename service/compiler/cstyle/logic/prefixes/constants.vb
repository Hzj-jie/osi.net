
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.service.compiler.logic

Partial Public NotInheritable Class cstyle
    Public NotInheritable Class constants
        Implements prefix

        Public Shared ReadOnly int_1 As String = unique_name("int_1")

        Private Shared ReadOnly v As vector(Of def)

        Shared Sub New()
            v = vector.of(
                New def("int", int_1, "i1")
            )
        End Sub

        Private Shared Function unique_name(ByVal name As String) As String
            Return "@@prefixes@constants@" + name
        End Function

        Private NotInheritable Class def
            Public ReadOnly type As String
            Public ReadOnly name As String
            Public ReadOnly value As String

            Public Sub New(ByVal type As String, ByVal name As String, ByVal value As String)
                assert(Not type.null_or_whitespace())
                assert(Not name.null_or_whitespace())
                assert(Not value.null_or_whitespace())
                Me.type = type
                Me.name = name
                Me.value = value
            End Sub
        End Class

        Public Shared Sub register()
            prefixes.register(New constants())
        End Sub

        Public Sub export(ByVal o As writer) Implements prefix.export
            Dim i As UInt32 = 0
            While i < v.size()
                o.append("define", v(i).name, v(i).type)
                o.append("copy_const", v(i).name, v(i).value)
                i += uint32_1
            End While
        End Sub

        Private Sub New()
        End Sub
    End Class
End Class
