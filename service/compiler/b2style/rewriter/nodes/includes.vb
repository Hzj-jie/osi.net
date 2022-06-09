
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

    Public NotInheritable Class default_includes
        Private Shared ReadOnly folder As String = create_inc_folder()

        Private Shared Function create_inc_folder() As String
            Dim folder As String = Path.Combine(temp_folder, "b2style-inc")
            tar.gen.dump(b2style_lib.data, folder)
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

    ' TODO: Consider to include bstyle headers into b2style.
    Private NotInheritable Class include_with_string
        Inherits code_gens(Of typed_node_writer).include_with_string(Of default_includes.folders,
                                                                        default_includes.ignore_default_folder,
                                                                        default_includes.default_folder,
                                                                        _true,
                                                                        should_include_t)
        Public Shared ReadOnly instance As New include_with_string()

        Private Sub New()
            MyBase.new(file_parser.instance)
        End Sub

        ' Forward missing files to the bstyle.
        Protected Overrides Function handle_not_dumpable(ByVal n As typed_node, ByVal o As typed_node_writer) As Boolean
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            o.append(n)
            Return True
        End Function
    End Class

    Private NotInheritable Class include_with_file
        Inherits code_gens(Of typed_node_writer).include_with_file(Of default_includes.folders,
                                                                      default_includes.ignore_default_folder,
                                                                      default_includes.default_folder,
                                                                      _true,
                                                                      should_include_t)
        Public Shared ReadOnly instance As New include_with_file()

        Private Sub New()
            MyBase.New(file_parser.instance)
        End Sub

        ' Forward missing files to the bstyle.
        Protected Overrides Function handle_not_dumpable(ByVal n As typed_node, ByVal o As typed_node_writer) As Boolean
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            o.append(n)
            Return True
        End Function
    End Class
End Class
