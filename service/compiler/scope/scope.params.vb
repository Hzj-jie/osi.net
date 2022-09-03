
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.delegates
Imports osi.root.formation
Imports builders = osi.service.compiler.logic.builders

Partial Public Class scope(Of WRITER As {lazy_list_writer, New},
                              __BUILDER As func_t(Of String, WRITER, Boolean),
                              __CODE_GENS As func_t(Of code_gens(Of WRITER)),
                              T As scope(Of WRITER, __BUILDER, __CODE_GENS, T))
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
