
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.formation
Imports osi.root.utils

Partial Public NotInheritable Class tar
    Public NotInheritable Class selector
        Public root As String = Environment.CurrentDirectory()
        Public include_sub_folders As Boolean = True
        Public pattern As String = "*"

        Public Function absolute() As vector(Of String)
            Return vector.emplace_of(Directory.GetFiles(root,
                                                        pattern,
                                                        If(include_sub_folders,
                                                           SearchOption.AllDirectories,
                                                           SearchOption.TopDirectoryOnly)))
        End Function

        Public Function relative() As vector(Of String)
            Return absolute().stream().
                              map(Function(ByVal s As String) As String
                                      Return pather.default.relative_path(root, s)
                                  End Function).
                              collect(Of vector(Of String))()
        End Function
    End Class
End Class
