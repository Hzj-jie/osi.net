
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

    Public MustInherit Class includes
        Inherits code_gens(Of logic_writer).includes(Of scope.includes_t.proxy, folders)
        Public Shared Shadows Function parse(ByVal i As String, ByVal j As logic_writer) As Boolean
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

        Private Sub New()
        End Sub
    End Class

    Private NotInheritable Class include_with_string
        Inherits code_gens(Of logic_writer).include_with_string(Of scope.includes_t.proxy, includes.folders)

        Protected Overrides Function file_parse(ByVal s As String, ByVal o As logic_writer) As Boolean
            Return includes.parse(s, o)
        End Function
    End Class

    Private NotInheritable Class include_with_file
        Inherits code_gens(Of logic_writer).include_with_file(Of scope.includes_t.proxy, includes.folders)

        Protected Overrides Function file_parse(ByVal s As String, ByVal o As logic_writer) As Boolean
            Return includes.parse(s, o)
        End Function
    End Class
End Class
