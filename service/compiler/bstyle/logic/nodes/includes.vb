
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.delegates
Imports osi.root.formation
Imports osi.root.template
Imports osi.root.utils
Imports osi.service.compiler.logic
Imports osi.service.resource

Partial Public NotInheritable Class bstyle
    Private Shared include_folders As argument(Of vector(Of String))
    Private Shared ignore_default_include As argument(Of Boolean)

    Public NotInheritable Class file_parser
        Inherits __do(Of String, logic_writer, Boolean)

        Public Shared ReadOnly instance As New file_parser()

        Private Sub New()
        End Sub

        Public Overrides Function at(ByRef i As String, ByRef j As logic_writer) As Boolean
            If i Is Nothing Then
                ' The file has been included already.
                Return True
            End If
            Dim o As logic_writer = j
            Return parse_wrapper.with_current_file(i, Function(ByVal s As String) As Boolean
                                                          Return code_builder.build(s, o)
                                                      End Function)
        End Function
    End Class

    Public NotInheritable Class default_includes
        Private Shared ReadOnly folder As String = create_inc_folder()

        Private Shared Function create_inc_folder() As String
            Dim folder As String = Path.Combine(temp_folder, "bstyle-inc")
            tar.gen.dump(bstyle_lib.data, folder)
            Return folder
        End Function

        Public NotInheritable Class folders
            Inherits __do(Of vector(Of String))

            Protected Overrides Function at() As vector(Of String)
                Return +include_folders
            End Function
        End Class

        Public NotInheritable Class ignore_default_folder
            Inherits __do(Of Boolean)

            Protected Overrides Function at() As Boolean
                Return ignore_default_include Or False
            End Function
        End Class

        Public NotInheritable Class default_folder
            Inherits __do(Of String)

            Protected Overrides Function at() As String
                Return folder
            End Function
        End Class

        Private Sub New()
        End Sub
    End Class

    Public NotInheritable Class should_include_t
        Inherits __do(Of String, Boolean)

        Public Overrides Function at(ByRef k As String) As Boolean
            Return scope.current().includes().should_include(k)
        End Function
    End Class

    Private NotInheritable Class include_with_string
        Inherits code_gens(Of logic_writer).include_with_string(Of default_includes.folders,
                                                                   default_includes.ignore_default_folder,
                                                                   default_includes.default_folder,
                                                                   _false,
                                                                   should_include_t)
        Public Sub New()
            MyBase.New(file_parser.instance)
        End Sub
    End Class

    Private NotInheritable Class include_with_file
        Inherits code_gens(Of logic_writer).include_with_file(Of default_includes.folders,
                                                                 default_includes.ignore_default_folder,
                                                                 default_includes.default_folder,
                                                                 _false,
                                                                 should_include_t)
        Public Sub New()
            MyBase.New(file_parser.instance)
        End Sub
    End Class
End Class
