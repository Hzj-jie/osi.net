
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.template
Imports osi.service.compiler.rewriters

Partial Public NotInheritable Class b2style
    Public NotInheritable Class parser
        Inherits __do(Of String, typed_node_writer, Boolean)

        Public Overrides Function at(ByRef i As String, ByRef j As typed_node_writer) As Boolean
            Return code_builder.build(i, j)
        End Function
    End Class

    Public NotInheritable Class file_parser
        Inherits __do(Of String, typed_node_writer, Boolean)

        Public Overrides Function at(ByRef i As String, ByRef j As typed_node_writer) As Boolean
            If i Is Nothing Then
                ' The file has been included already.
                Return True
            End If
            Dim o As typed_node_writer = j
            Return parse_wrapper.with_current_file(i, Function(ByVal s As String) As Boolean
                                                          Return code_builder.build(s, o)
                                                      End Function)
        End Function
    End Class
End Class
