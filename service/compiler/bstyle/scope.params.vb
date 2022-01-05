
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.formation
Imports osi.service.compiler.logic

Partial Public NotInheritable Class bstyle
    Partial Public NotInheritable Class scope
        Public NotInheritable Class params_t
            Private ReadOnly v As New vector(Of builders.parameter)()

            Public Sub pack(ByVal n As vector(Of builders.parameter))
                v.emplace_back(n)
            End Sub

            Public Function unpack() As vector(Of builders.parameter)
                Return vector.move(v)
            End Function
        End Class

        Public Function params() As params_t
            Return ps
        End Function
    End Class
End Class
