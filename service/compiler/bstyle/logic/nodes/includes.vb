
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

    Private NotInheritable Class includes
        Public Shared Function file_parse(ByVal i As String, ByVal j As logic_writer) As Boolean
            If i Is Nothing Then
                ' The file has been included already.
                Return True
            End If
            Return parse_wrapper.with_current_file(i, Function(ByVal s As String) As Boolean
                                                          Return code_builder.build(s, j)
                                                      End Function)
        End Function

        Public Shared ReadOnly folder As String = Function() As String
                                                      Dim folder As String = Path.Combine(temp_folder, "bstyle-inc")
                                                      tar.gen.dump(bstyle_lib.data, folder)
                                                      Return folder
                                                  End Function()

        Public Shared Function should_include(ByVal i As String) As Boolean
            Return scope.current().includes().should_include(i)
        End Function

        Public Shared Function include_folders() As vector(Of String)
            Return +bstyle.include_folders
        End Function

        Public Shared Function ignore_default_include() As Boolean
            Return bstyle.ignore_default_include Or False
        End Function

        Public Const ignore_include_error As Boolean = False

        Private Sub New()
        End Sub
    End Class

    Private NotInheritable Class include_with_string
        Inherits code_gens(Of logic_writer).include_with_string(Of scope.includes_t.proxy)

        Public Sub New()
            MyBase.New(includes.include_folders(),
                       includes.ignore_default_include(),
                       includes.folder,
                       includes.ignore_include_error)
        End Sub

        Protected Overrides Function file_parse(ByVal s As String, ByVal o As logic_writer) As Boolean
            Return includes.file_parse(s, o)
        End Function
    End Class

    Private NotInheritable Class include_with_file
        Inherits code_gens(Of logic_writer).include_with_file(Of scope.includes_t.proxy)

        Public Sub New()
            MyBase.New(includes.include_folders(),
                       includes.ignore_default_include(),
                       includes.folder,
                       includes.ignore_include_error)
        End Sub

        Protected Overrides Function file_parse(ByVal s As String, ByVal o As logic_writer) As Boolean
            Return includes.file_parse(s, o)
        End Function
    End Class
End Class
