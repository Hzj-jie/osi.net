
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.compiler.logic

Partial Public Class scope(Of _ACCESSOR As accessor, T As scope(Of _ACCESSOR, T))
    Public NotInheritable Class params_t
        Private ReadOnly v As New vector(Of builders.parameter)()

        Public Sub pack(ByVal n As stream(Of builders.parameter))
            assert(Not n Is Nothing)
            pack(n.to_array())
        End Sub

        Public Sub pack(ByVal n() As builders.parameter)
            assert(Not n Is Nothing)
            v.emplace_back(n)
        End Sub

        Public Function unpack() As vector(Of builders.parameter)
            Return vector.move(v)
        End Function
    End Class
End Class
