﻿
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
Imports osi.service.constructor
Imports osi.service.resource

Partial Public NotInheritable Class bstyle
    Private Shared include_folders As argument(Of vector(Of String))
    Private Shared ignore_default_include As argument(Of Boolean)

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

        Public NotInheritable Class parser
            Inherits __do(Of String, writer, Boolean)

            Public Overrides Function at(ByRef i As String, ByRef j As writer) As Boolean
                Return bstyle.internal_parse(i, j)
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
        Inherits include_with_string(Of writer,
                                        default_includes.parser,
                                        default_includes.folders,
                                        default_includes.ignore_default_folder,
                                        default_includes.default_folder)
        Implements logic_gen

        <inject_constructor>
        Public Sub New(ByVal i As logic_gens)
        End Sub

        Public Shared Sub register(ByVal b As logic_gens)
            assert(Not b Is Nothing)
            b.register(Of include_with_string)()
        End Sub

        Public Shadows Function build(ByVal n As typed_node, ByVal o As writer) As Boolean Implements logic_gen.build
            Return MyBase.build(n, o)
        End Function
    End Class

    Public NotInheritable Class include_with_file
        Inherits include_with_file(Of writer,
                                      default_includes.parser,
                                      default_includes.folders,
                                      default_includes.ignore_default_folder,
                                      default_includes.default_folder)
        Implements logic_gen

        <inject_constructor>
        Public Sub New(ByVal i As logic_gens)
        End Sub

        Public Shared Sub register(ByVal b As logic_gens)
            assert(Not b Is Nothing)
            b.register(Of include_with_file)()
        End Sub

        Public Shadows Function build(ByVal n As typed_node, ByVal o As writer) As Boolean Implements logic_gen.build
            Return MyBase.build(n, o)
        End Function
    End Class

    Public NotInheritable Class include
        Inherits logic_gen_wrapper
        Implements logic_gen

        <inject_constructor>
        Public Sub New(ByVal i As logic_gens)
            MyBase.New(i)
        End Sub

        Public Shared Sub register(ByVal b As logic_gens)
            assert(Not b Is Nothing)
            b.register(Of include)()
        End Sub

        Public Function build(ByVal n As typed_node, ByVal o As writer) As Boolean Implements logic_gen.build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            Return l.of(n.child()).build(o)
        End Function
    End Class
End Class