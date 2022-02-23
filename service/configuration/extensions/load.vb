
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.lock

Public Module _load
    <Extension()> Public Function load(ByVal c As configuration,
                                       ByVal wait_ms As Int64,
                                       ByVal ParamArray files() As String) As Boolean
        assert(c IsNot Nothing)
        If isemptyarray(files) Then
            Return True
        End If
        For i As Int32 = 0 To array_size_i(files) - 1
            c.preload(files(i))
        Next
        Return timeslice_sleep_wait_until(Function() As Boolean
                                              For i As Int32 = 0 To array_size_i(files) - 1
                                                  Dim t As config = Nothing
                                                  If Not c.get(files(i), t) Then
                                                      Return False
                                                  End If
                                              Next
                                              Return True
                                          End Function,
                                          wait_ms)
    End Function

    <Extension()> Public Function load(ByVal c As configuration, ByVal ParamArray files() As String) As Boolean
        Return load(c, seconds_to_milliseconds(10 * If(isdebugbuild(), 10, 1)) * array_size(files), files)
    End Function

    <Extension()> Public Sub assert_load(ByVal c As configuration,
                                         ByVal wait_ms As Int64,
                                         ByVal ParamArray files() As String)
        assert(load(c, wait_ms, files))
    End Sub

    <Extension()> Public Sub assert_load(ByVal c As configuration, ByVal ParamArray files() As String)
        assert(load(c, files))
    End Sub

    <Extension()> Public Function unload(ByVal c As configuration, ByVal ParamArray files() As String) As Boolean
        If c Is Nothing Then
            Return False
        Else
            For i As Int32 = 0 To array_size_i(files) - 1
                c.unload(files(i))
            Next
            Return True
        End If
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
