
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
Imports osi.service.compiler.rewriters
Imports osi.service.constructor
Imports osi.service.resource

Partial Public NotInheritable Class b2style
    Private Shared includes As argument(Of vector(Of String))
    Private Shared ignore_default_includes As argument(Of Boolean)

    Private NotInheritable Class default_includes
        Public Shared ReadOnly folder As String = Path.Combine(temp_folder, "b2style-inc")

        Shared Sub New()
            assert(Not Directory.CreateDirectory(folder) Is Nothing)
            assert(b2style_lib.stdio_h.sync_export(Path.Combine(folder, "stdio.h"), True))
            assert(b2style_lib.cstdio.sync_export(Path.Combine(folder, "cstdio"), True))
        End Sub

        Private Sub New()
        End Sub
    End Class

    Public MustInherit Class include
        Private Shared Function include_file(ByVal p As String,
                                             ByVal s As String,
                                             ByVal o As typed_node_writer) As Boolean
            Dim f As String = Path.Combine(p, s)
            If File.Exists(f) Then
                Return no_fixes.parse(File.ReadAllText(f), o)
            End If
            Return False
        End Function

        Protected Shared Function include_file(ByVal s As String, ByVal o As typed_node_writer) As Boolean
            Dim i As UInt32 = 0
            While i < (+includes).size_or_0()
                If include_file((+includes)(i), o) Then
                    Return True
                End If
                i += uint32_1
            End While
            If Not (ignore_default_includes Or False) AndAlso include_file(default_includes.folder, s, o) Then
                Return True
            End If
            raise_error(error_type.user, "Cannot find or include file ", s)
            Return False
        End Function
    End Class

    Public NotInheritable Class include_with_string
        Inherits include
        Implements rewriter

        <inject_constructor>
        Public Sub New(ByVal i As rewriters)
        End Sub

        Public Shared Sub register(ByVal b As rewriters)
            assert(Not b Is Nothing)
            b.register(Of include_with_string)()
        End Sub

        Public Function build(ByVal n As typed_node,
                              ByVal o As typed_node_writer) As Boolean Implements code_gen(Of typed_node_writer).build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(n.child_count() = 2)
            Return include_file(n.child(1).word().str().Trim(character.quote).c_unescape(), o)
        End Function
    End Class

    Public NotInheritable Class include_with_file
        Inherits include
        Implements rewriter

        <inject_constructor>
        Public Sub New(ByVal i As rewriters)
        End Sub

        Public Shared Sub register(ByVal b As rewriters)
            assert(Not b Is Nothing)
            b.register(Of include_with_file)()
        End Sub

        Public Function build(ByVal n As typed_node,
                              ByVal o As typed_node_writer) As Boolean Implements code_gen(Of typed_node_writer).build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(n.child_count() = 2)
            Return include_file(n.child(1).word().str().Trim(character.left_angle_bracket,
                                                             character.right_angle_bracket),
                                o)
        End Function
    End Class
End Class
