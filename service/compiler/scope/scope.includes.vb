
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.delegates
Imports osi.root.formation

Partial Public Class scope(Of WRITER As {lazy_list_writer, New},
                              __BUILDER As func_t(Of String, WRITER, Boolean),
                              __CODE_GENS As func_t(Of code_gens(Of WRITER)),
                              T As scope(Of WRITER, __BUILDER, __CODE_GENS, T))
    Public NotInheritable Class includes_t
        Private ReadOnly _included As New unordered_set(Of String)()

        Public Function should_include(ByVal f As String) As Boolean
            Return _included.emplace(f).second()
        End Function

        Public Structure proxy
            Implements func_t(Of String, Boolean)

            Public Function run(ByVal f As String) As Boolean Implements func_t(Of String, Boolean).run
                Return current().includes().should_include(f)
            End Function
        End Structure
    End Class
End Class
