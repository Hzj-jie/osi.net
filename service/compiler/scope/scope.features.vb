
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.delegates
Imports osi.root.formation

Partial Public Class scope(Of WRITER As {lazy_list_writer, New},
                              __BUILDER As func_t(Of String, WRITER, Boolean),
                              __CODE_GENS As func_t(Of code_gens(Of WRITER)),
                              T As scope(Of WRITER, __BUILDER, __CODE_GENS, T))
    Private ReadOnly _features As lazier(Of features_t) = lazier.of(AddressOf Me.get_features)

    Private Function features() As features_t
        Return +_features
    End Function

    Protected Overridable Function get_features() As features_t
        Return New features_t()
    End Function

    Public Class features_t
        Public Overridable Function with_type_alias() As Boolean
            Return True
        End Function

        Public Overridable Function with_namespace() As Boolean
            Return True
        End Function
    End Class
End Class
