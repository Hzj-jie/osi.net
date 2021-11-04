
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
Imports osi.service.constructor
Imports osi.service.resource

Partial Public NotInheritable Class b2style
    Private Shared include_folders As argument(Of vector(Of String))
    Private Shared ignore_default_include As argument(Of Boolean)

    Public NotInheritable Class default_includes
        Private Shared ReadOnly folder As String = Path.Combine(temp_folder, "b2style-inc")

        Shared Sub New()
            assert(Not Directory.CreateDirectory(folder) Is Nothing)
            assert(b2style_lib.b2style_h.sync_export(Path.Combine(folder, "b2style.h"), True))
            assert(b2style_lib.b2style_operators_h.sync_export(Path.Combine(folder, "b2style_operators.h"), True))
            assert(b2style_lib.b2style_stdio_h.sync_export(Path.Combine(folder, "b2style_stdio.h"), True))
            assert(b2style_lib.b2style_ufloat_h.sync_export(Path.Combine(folder, "b2style_ufloat.h"), True))
            assert(b2style_lib.b2style_types_h.sync_export(Path.Combine(folder, "b2style_types.h"), True))
        End Sub

        Public NotInheritable Class parser
            Inherits __do(Of String, typed_node_writer, Boolean)

            Public Overrides Function at(ByRef i As String, ByRef j As typed_node_writer) As Boolean
                Return code_builder.current().build(i, j)
            End Function
        End Class

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

    Public NotInheritable Class include_with_string
        Inherits bstyle.include_with_string(Of typed_node_writer,
                                               default_includes.parser,
                                               default_includes.folders,
                                               default_includes.ignore_default_folder,
                                               default_includes.default_folder)
        Implements rewriter

        Private Shared ReadOnly instance As New include_with_string()

        Private Sub New()
        End Sub

        Public Shared Sub register(ByVal b As rewriters)
            assert(Not b Is Nothing)
            b.register(instance)
        End Sub

        Public Shadows Function build(ByVal n As typed_node,
                                      ByVal o As typed_node_writer) As Boolean Implements rewriter.build
            If Not MyBase.build(n, o) Then
                o.append(n)
            End If
            Return True
        End Function
    End Class

    Public NotInheritable Class include_with_file
        Inherits bstyle.include_with_file(Of typed_node_writer,
                                             default_includes.parser,
                                             default_includes.folders,
                                             default_includes.ignore_default_folder,
                                             default_includes.default_folder)
        Implements rewriter

        Private Shared ReadOnly instance As New include_with_file()

        Private Sub New()
        End Sub

        Public Shared Sub register(ByVal b As rewriters)
            assert(Not b Is Nothing)
            b.register(instance)
        End Sub

        Public Shadows Function build(ByVal n As typed_node,
                                      ByVal o As typed_node_writer) As Boolean Implements rewriter.build
            If Not MyBase.build(n, o) Then
                o.append(n)
            End If
            Return True
        End Function
    End Class
End Class
