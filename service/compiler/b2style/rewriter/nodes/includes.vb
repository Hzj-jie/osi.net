
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.connector
Imports osi.root.delegates
Imports osi.root.formation
Imports osi.root.utils
Imports osi.service.automata
Imports osi.service.compiler.rewriters
Imports osi.service.resource

Partial Public NotInheritable Class b2style
    Private Shared include_folders As argument(Of vector(Of String))

    Private NotInheritable Class includes
        Public Shared Function file_parse(ByVal i As String, ByVal j As typed_node_writer) As Boolean
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

                Dim folder As String = Path.Combine(temp_folder, "b2style-inc")
                tar.gen.dump(b2style_lib.data, folder)
                r.emplace_back(folder)

                r.emplace_back(+b2style.include_folders)
                Return r
            End Function
        End Structure

        Public Structure ignore_include_error
            Implements func_t(Of Boolean)

            Public Function run() As Boolean Implements func_t(Of Boolean).run
                Return True
            End Function
        End Structure

        Private Sub New()
        End Sub
    End Class

    ' TODO: Consider to include bstyle headers into b2style.
    Private NotInheritable Class include_with_string
        Inherits code_gens(Of typed_node_writer).include_with_string(Of scope.includes_t.proxy,
                                                                        includes.folders,
                                                                        includes.ignore_include_error)

        Protected Overrides Function file_parse(ByVal s As String, ByVal o As typed_node_writer) As Boolean
            Return includes.file_parse(s, o)
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
        Inherits code_gens(Of typed_node_writer).include_with_file(Of scope.includes_t.proxy,
                                                                      includes.folders,
                                                                      includes.ignore_include_error)

        Protected Overrides Function file_parse(ByVal s As String, ByVal o As typed_node_writer) As Boolean
            Return includes.file_parse(s, o)
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
