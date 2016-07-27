
Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.lock
Imports osi.root.utils

Public Module _load
    <Extension()> Public Sub assert_load(ByVal c As configuration,
                                         ByVal wait_ms As Int64,
                                         ByVal ParamArray files() As String)
        assert(Not c Is Nothing)
        If Not isemptyarray(files) Then
            For i As Int32 = 0 To array_size(files) - 1
                c.preload(files(i))
            Next
            assert(timeslice_sleep_wait_until(Function() As Boolean
                                                  For i As Int32 = 0 To array_size(files) - 1
                                                      Dim t As config = Nothing
                                                      If Not c.get(files(i), t) Then
                                                          Return False
                                                      End If
                                                  Next
                                                  Return True
                                              End Function,
                                              wait_ms))
        End If
    End Sub

    <Extension()> Public Sub assert_load(ByVal c As configuration, ByVal ParamArray files() As String)
        assert_load(c, seconds_to_milliseconds(If(isdebugbuild(), 100, 5)) * array_size(files), files)
    End Sub

    <Extension()> Public Function unload(ByVal c As configuration, ByVal ParamArray files() As String) As Boolean
        If c Is Nothing Then
            Return False
        ElseIf Not isemptyarray(files) Then
            For i As Int32 = 0 To array_size(files) - 1
                c.unload(files(i))
            Next
        End If
        Return True
    End Function

    Public Sub assert_load(ByVal wait_ms As Int64, ByVal ParamArray files() As String)
        assert_load([default](), wait_ms, files)
    End Sub

    Public Sub assert_load(ByVal ParamArray files() As String)
        assert_load([default](), files)
    End Sub

    Public Function [default]() As configuration
        Return configuration.default
    End Function

    Public Function [default](ByVal file As String) As config
        Return [default]()(file)
    End Function
End Module
