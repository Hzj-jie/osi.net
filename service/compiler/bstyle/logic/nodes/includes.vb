
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.delegates
Imports osi.root.formation
Imports osi.root.utils
Imports osi.service.resource

Partial Public NotInheritable Class bstyle
    Private Shared include_folders As argument(Of vector(Of String))
    Private Shared ignore_default_include As argument(Of Boolean)

    Public NotInheritable Class includes
        Public Shared Function file_parse(ByVal i As String, ByVal j As logic_writer) As Boolean
            If i Is Nothing Then
                ' The file has been included already.
                Return True
            End If
            Return parse_wrapper.with_current_file(i, Function(ByVal s As String) As Boolean
                                                          Return code_builder.build(s, j)
                                                      End Function)
        End Function

        Public Structure folders
            Implements func_t(Of vector(Of String))

            Public Function run() As vector(Of String) Implements func_t(Of vector(Of String)).run
                Dim r As New vector(Of String)()

                Dim folder As String = Path.Combine(temp_folder, "bstyle-inc")
                tar.gen.dump(bstyle_lib.data, folder)
                r.emplace_back(folder)

                r.emplace_back(+bstyle.include_folders)
                Return r
            End Function
        End Structure

        Public Structure ignore_include_error
            Implements func_t(Of Boolean)

            Public Function run() As Boolean Implements func_t(Of Boolean).run
                Return False
            End Function
        End Structure

        Private Sub New()
        End Sub
    End Class

    Public NotInheritable Class include_with_string
        Inherits code_gens(Of logic_writer).include_with_string(Of scope.includes_t.proxy,
                                                                   includes.folders,
                                                                   includes.ignore_include_error)

        Protected Overrides Function file_parse(ByVal s As String, ByVal o As logic_writer) As Boolean
            Return includes.file_parse(s, o)
        End Function
    End Class

    Public NotInheritable Class include_with_file
        Inherits code_gens(Of logic_writer).include_with_file(Of scope.includes_t.proxy,
                                                                 includes.folders,
                                                                 includes.ignore_include_error)

        Protected Overrides Function file_parse(ByVal s As String, ByVal o As logic_writer) As Boolean
            Return includes.file_parse(s, o)
        End Function
    End Class
End Class
