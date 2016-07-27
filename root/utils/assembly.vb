
Imports System.IO
Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports osi.root.constants
Imports osi.root.constants.filesystem
Imports osi.root.connector

Public Module _assembly
    <Extension()> Public Sub load_all(ByVal this As AppDomain,
                                      ByVal path As String,
                                      Optional ByVal file_pattern As String = Nothing)
        If this Is Nothing Then
            this = AppDomain.CurrentDomain()
        End If
        If String.IsNullOrEmpty(file_pattern) Then
            file_pattern = strcat(multi_pattern_matching_character,
                                  extension_prefix,
                                  extensions.dynamic_link_library)
        End If
        Dim files() As String = Nothing
        files = Directory.GetFiles(path, file_pattern)
        concurrency_runner.execute(Sub(file As String)
                                       Try
                                           this.Load(AssemblyName.GetAssemblyName(file))
                                       Catch ex As Exception
                                           raise_error(error_type.exclamation,
                                                       "failed to load assembly ",
                                                       file,
                                                       ", ex ",
                                                       ex.Message())
                                       End Try
                                   End Sub,
                                   files)
    End Sub

    <Extension()> Public Sub load_all(ByVal this As AppDomain,
                                      ByVal path As String,
                                      ByVal file_patterns() As String)
        If isemptyarray(file_patterns) Then
            load_all(this, path)
        Else
            For i As Int32 = 0 To array_size(file_patterns) - 1
                load_all(this, path, file_patterns(i))
            Next
        End If
    End Sub

    <Extension()> Public Sub load_all(ByVal this As AppDomain,
                                      ByVal paths() As String,
                                      Optional ByVal file_patterns() As String = Nothing)
        For i As Int32 = 0 To array_size(paths) - 1
            load_all(this, paths(i), file_patterns)
        Next
    End Sub
End Module
