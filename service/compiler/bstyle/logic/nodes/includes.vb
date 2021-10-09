
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.delegates
Imports osi.root.formation
Imports osi.root.utils
Imports osi.service.automata
Imports osi.service.compiler.logic
Imports osi.service.constructor
Imports osi.service.resource

Partial Public NotInheritable Class bstyle
    Private Shared include_folders As argument(Of vector(Of String))
    Private Shared ignore_default_include As argument(Of Boolean)

    Private NotInheritable Class default_includes
        Public Shared ReadOnly folder As String = Path.Combine(temp_folder, "bstyle-inc")

        Shared Sub New()
            assert(Not Directory.CreateDirectory(folder) Is Nothing)
            assert(bstyle_lib.stdio_h.sync_export(Path.Combine(folder, "stdio.h"), True))
            assert(bstyle_lib.cstdio.sync_export(Path.Combine(folder, "cstdio"), True))
            assert(bstyle_lib.bstyle_h.sync_export(Path.Combine(folder, "bstyle.h"), True))
            assert(bstyle_lib.bstyle_types_h.sync_export(Path.Combine(folder, "bstyle_types.h"), True))
            assert(bstyle_lib.bstyle_constants_h.sync_export(Path.Combine(folder, "bstyle_constants.h"), True))
        End Sub

        Private Sub New()
        End Sub
    End Class

    Public MustInherit Class includes
        Private Shared Function include_file(ByVal p As String,
                                             ByVal s As String,
                                             ByVal o As writer) As Boolean
            Dim f As String = Path.Combine(p, s)
            If File.Exists(f) Then
                Return bstyle.internal_parse(File.ReadAllText(f), o)
            End If
            Return False
        End Function

        Protected Shared Function include_file(ByVal s As String, ByVal o As writer) As Boolean
            Dim i As UInt32 = 0
            While i < (+include_folders).size_or_0()
                If include_file((+include_folders)(i), o) Then
                    Return True
                End If
                i += uint32_1
            End While
            If Not (ignore_default_include Or False) AndAlso include_file(default_includes.folder, s, o) Then
                Return True
            End If
            raise_error(error_type.user, "Cannot find or include file ", s)
            Return False
        End Function
    End Class

    Public NotInheritable Class include_with_string
        Inherits includes
        Implements logic_gen

        <inject_constructor>
        Public Sub New(ByVal i As logic_gens)
        End Sub

        Public Shared Sub register(ByVal b As logic_gens)
            assert(Not b Is Nothing)
            b.register(Of include_with_string)()
        End Sub

        Public Function build(ByVal n As typed_node, ByVal o As writer) As Boolean Implements logic_gen.build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(n.child_count() = 2)
            Return include_file(n.child(1).word().str().Trim(character.quote).c_unescape(), o)
        End Function
    End Class

    Public NotInheritable Class include_with_file
        Inherits includes
        Implements logic_gen

        Private Const kw_include As String = "#include"

        <inject_constructor>
        Public Sub New(ByVal i As logic_gens)
        End Sub

        Public Shared Sub register(ByVal b As logic_gens)
            assert(Not b Is Nothing)
            b.register(Of include_with_file)()
        End Sub

        Public Function build(ByVal n As typed_node, ByVal o As writer) As Boolean Implements logic_gen.build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(n.leaf())
            Dim file As String = n.word().str()
            assert(file.StartsWith(kw_include))
            file = file.Substring(kw_include.Length()).Trim()
            assert(file.StartsWith(character.left_angle_bracket))
            assert(file.EndsWith(character.right_angle_bracket))
            Return include_file(file.Substring(1, file.Length() - 2), o)
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
