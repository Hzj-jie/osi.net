
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.service.automata
Imports osi.service.compiler.logic
Imports osi.service.constructor

Partial Public NotInheritable Class bstyle
    Partial Public NotInheritable Class value_list
        Inherits logic_gen_wrapper
        Implements logic_gen

        Private Shared ReadOnly rs As read_scoped(Of vector(Of String))

        Shared Sub New()
            rs = New read_scoped(Of vector(Of String))()
        End Sub

        Private Shared Sub with_current_targets(ByVal v As vector(Of String))
            rs.push(v)
        End Sub

        Public Shared Function current_targets() As read_scoped(Of vector(Of String)).ref
            Return rs.pop()
        End Function
    End Class
End Class
