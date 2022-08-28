
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Partial Public Class scope(Of T As scope(Of T))
    ' Inject any code before the current root-type.
    Public NotInheritable Class root_type_injector_t(Of WRITER As {lazy_list_writer, New})
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