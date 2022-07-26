
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.connector
Imports osi.root.delegates
Imports osi.root.formation
Imports osi.root.template
Imports osi.root.utils
Imports osi.service.automata
Imports osi.service.compiler.rewriters
Imports osi.service.resource

Partial Public NotInheritable Class b2style
    Private Shared include_folders As argument(Of vector(Of String))
    Private Shared ignore_default_include As argument(Of Boolean)

    Private NotInheritable Class includes
        Public Shared Function file_parse(ByVal i As String, ByVal j As typed_node_writer) As Boolean
            If i Is Nothing Then
                ' The file has been included already.
                Return True
            End If
            Dim o As typed_node_writer = j
            Return parse_wrapper.with_current_file(i, Function(ByVal s As String) As Boolean
                                                          Return code_builder.build(s, o)
                                                      End Function)
        End Function

        Public Shared ReadOnly folder As String = Function() As String
                                                      Dim folder As String = Path.Combine(temp_folder, "b2style-inc")
                                                      tar.gen.dump(b2style_lib.data, folder)
                                                      Return folder
                                                  End Function()

        Public Shared Function should_include(ByVal i As String) As Boolean
            Return scope.current().includes().should_include(i)
        End Function

        Public Shared Function include_folders() As vector(Of String)
            Return +b2style.include_folders
        End Function

        Public Shared Function ignore_default_include() As Boolean
            Return b2style.ignore_default_include Or False
        End Function

        Public Const ignore_include_error As Boolean = True

        Private Sub New()
        End Sub
    End Class

    Public NotInheritable Class should_include_t
        Inherits __do(Of String, Boolean)

        Public Overrides Function at(ByRef k As String) As Boolean
            Return scope.current().includes().should_include(k)
        End Function
    End Class

    ' TODO: Consider to include bstyle headers into b2style.
    Private NotInheritable Class include_with_string
        Inherits code_gens(Of typed_node_writer).include_with_string

        Public Sub New()
            MyBase.New(includes.include_folders(),
                       includes.ignore_default_include(),
                       includes.folder,
                       includes.ignore_include_error)
        End Sub

        Protected Overrides Function file_parse(ByVal s As String, ByVal o As typed_node_writer) As Boolean
            Return includes.file_parse(s, o)
        End Function

        Protected Overrides Function should_include(ByVal s As String) As Boolean
            Return includes.should_include(s)
        End Function

        ' Forward missing files to the bstyle.
        Protected Overrides Function handle_not_dumpable(ByVal n As typed_node, ByVal o As typed_node_writer) As Boolean
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            o.append(n)
            Return True
        End Function
    End Class

    Private NotInheritable Class include_with_file
        Inherits code_gens(Of typed_node_writer).include_with_file

        Public Sub New()
            MyBase.New(includes.include_folders(),
                       includes.ignore_default_include(),
                       includes.folder,
                       includes.ignore_include_error)
        End Sub

        Protected Overrides Function file_parse(ByVal s As String, ByVal o As typed_node_writer) As Boolean
            Return includes.file_parse(s, o)
        End Function

        Protected Overrides Function should_include(ByVal s As String) As Boolean
            Return includes.should_include(s)
        End Function

        ' Forward missing files to the bstyle.
        Protected Overrides Function handle_not_dumpable(ByVal n As typed_node, ByVal o As typed_node_writer) As Boolean
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            o.append(n)
            Return True
        End Function
    End Class
End Class
