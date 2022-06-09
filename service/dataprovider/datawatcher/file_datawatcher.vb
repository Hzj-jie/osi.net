
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utils
Imports osi.service.dataprovider.constants.trigger_datawatcher

Public Class file_datawatcher
    Inherits size_time_datawatcher

    Private ReadOnly filename As String

    Private Sub New(ByVal filename As String, ByVal interval_ms As Int64)
        MyBase.New(interval_ms)
        assert(Not filename.null_or_empty())
        assert(Path.IsPathRooted(filename))
        Me.filename = filename
    End Sub

    Public Shared Function generate(ByVal filename As String,
                                    Optional ByVal interval_ms As Int64 = default_interval_ms) As idatawatcher
        assert_fullpath(filename)
        Return collection.generate(strcat("file-datawatcher://", filename, "?interval_ms=", interval_ms),
                                   Function() New file_datawatcher(filename, interval_ms))
    End Function

    Protected Overrides Function info(ByVal sz As ref(Of UInt64), ByVal tm As ref(Of UInt64)) As event_comb
        Dim suc As Boolean = False
        Return New event_comb(Function() As Boolean
                                  Return waitfor(Sub()
                                                     If File.Exists(filename) Then
                                                         Dim fi As FileInfo = Nothing
                                                         fi = New FileInfo(filename)
                                                         suc = eva(sz, CULng(fi.Length())) AndAlso
                                                               eva(tm, CULng(fi.LastWriteTime().Ticks()))
                                                     Else
                                                         suc = False
                                                     End If
                                                 End Sub) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  Return suc AndAlso
                                         goto_end()
                              End Function)
    End Function
End Class
