
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.delegates

Partial Public Class scope(Of WRITER As {lazy_list_writer, New},
                              __BUILDER As func_t(Of String, WRITER, Boolean),
                              __CODE_GENS As func_t(Of code_gens(Of WRITER)),
                              T As scope(Of WRITER, __BUILDER, __CODE_GENS, T))
    ' Inject any code before the current root-type.
    Public NotInheritable Class root_type_injector_t
        Private i As WRITER

        Public Sub _new(ByVal o As WRITER)
            assert(Not o Is Nothing)
            i = New WRITER()
            o.append(i)
        End Sub

        Public Function current() As WRITER
            assert(Not i Is Nothing)
            Return i
        End Function
    End Class
End Class