
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.connector
Imports osi.root.constants

<Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable")>
Public Class application_info_writer
    Private ReadOnly w As streamwriter_auto_disposer

    Protected Sub New(ByVal folder As String, ByVal file As String, ByVal extension As String)
        assert(Not folder.null_or_empty())
        assert(Not extension.null_or_empty())
        assert(Not strstartwith(extension, filesystem.extension_prefix))
        If file.null_or_empty() Then
            file = strcat(envs.deploys.application_info_output_filename(), filesystem.extension_prefix, extension)
        End If
        If Not Path.IsPathRooted(file) Then
            file = Path.Combine(folder, file)
        End If
        w = New streamwriter_auto_disposer(file)
        application_lifetime_binder.instance.insert(w)
    End Sub

    Protected Function writer() As StreamWriter
        Return w.get()
    End Function
End Class
