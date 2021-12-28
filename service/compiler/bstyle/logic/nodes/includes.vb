
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
Imports osi.service.compiler.logic
Imports osi.service.resource

Partial Public NotInheritable Class bstyle
    Private Shared include_folders As argument(Of vector(Of String))
    Private Shared ignore_default_include As argument(Of Boolean)

    Public NotInheritable Class parser
        Inherits __do(Of String, writer, Boolean)

        Public Overrides Function at(ByRef i As String, ByRef j As writer) As Boolean
            Return code_builder.current().build(i, j)
        End Function
    End Class

    Public NotInheritable Class default_includes
        Private Shared ReadOnly folder As String = Path.Combine(temp_folder, "bstyle-inc")

        Shared Sub New()
            assert(Not Directory.CreateDirectory(folder) Is Nothing)
            assert(bstyle_lib.stdio_h.sync_export(Path.Combine(folder, "stdio.h"), True))
            assert(bstyle_lib.cstdio.sync_export(Path.Combine(folder, "cstdio"), True))
            assert(bstyle_lib.bstyle_h.sync_export(Path.Combine(folder, "bstyle.h"), True))
            assert(bstyle_lib.bstyle_types_h.sync_export(Path.Combine(folder, "bstyle_types.h"), True))
            assert(bstyle_lib.bstyle_constants_h.sync_export(Path.Combine(folder, "bstyle_constants.h"), True))
        End Sub

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
        Inherits code_gens(Of writer).include_with_string(Of parser,
                                                             default_includes.folders,
                                                             default_includes.ignore_default_folder,
                                                             default_includes.default_folder,
                                                             _false)
        Private Shared ReadOnly instance As New include_with_string()

        Private Sub New()
        End Sub

        Public Shared Sub register(ByVal b As code_gens(Of writer))
            assert(Not b Is Nothing)
            b.register(instance)
        End Sub
    End Class

    Public NotInheritable Class include_with_file
        Inherits code_gens(Of writer).include_with_file(Of parser,
                                                           default_includes.folders,
                                                           default_includes.ignore_default_folder,
                                                           default_includes.default_folder,
                                                           _false)
        Private Shared ReadOnly instance As New include_with_file()

        Private Sub New()
        End Sub

        Public Shared Sub register(ByVal b As code_gens(Of writer))
            assert(Not b Is Nothing)
            b.register(instance)
        End Sub
    End Class
End Class
