
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants

Partial Public NotInheritable Class bstyle
    Partial Public NotInheritable Class scope
        Public NotInheritable Class temp_logic_name_t
            Private id As UInt32 = 0

            Public Function variable() As String
                id += uint32_1
                Return "temp_value_@" + id.ToString()
            End Function
        End Class

        Public Function temp_logic_name() As temp_logic_name_t
            Return from_root(Function(ByVal i As scope) As temp_logic_name_t
                                 assert(Not i Is Nothing)
                                 Return i.t
                             End Function)
        End Function
    End Class
End Class
