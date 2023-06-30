
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Collections
Imports osi.root.connector
Imports osi.root.constants

Public Module _env_value
    Public Function env_value(ByVal str() As String, ByRef value As String) As Boolean
        If isemptyarray(str) Then
            Return False
        Else
            Dim envs As IDictionary = Nothing
            envs = Environment.GetEnvironmentVariables()
            If Not envs.null_or_empty() Then
                For i As Int32 = 0 To array_size_i(str) - 1
                    If envs.Contains(str(i)) Then
                        value = Convert.ToString(envs(str(i)))
                        raise_error("environment variable ", str(i), " = ", value)
                        Return True
                    End If
                Next
            End If

            Return False
        End If
    End Function

    Public Function env_bool(ByVal envs() As String) As Boolean
        Return env_value(envs, default_string)
    End Function

    Public Function env_value(ByVal str() As String, ByRef o As Int32) As Boolean
        Dim s As String = Nothing
        Return env_value(str, s) AndAlso
               Int32.TryParse(s, o)
    End Function

    Public Function env_value(ByVal str() As String, ByRef o As Int64) As Boolean
        Dim s As String = Nothing
        Return env_value(str, s) AndAlso
               Int64.TryParse(s, o)
    End Function

    Public Function env_value(ByVal i As String, ByRef o As String) As Boolean
        If i.null_or_empty() Then
            Return False
        Else
            o = Environment.GetEnvironmentVariable(i)
            Return Not o Is Nothing
        End If
    End Function

    Public Function env_bool(ByVal i As String) As Boolean
        Return env_value(i, default_string)
    End Function

    Public Function env_value(ByVal i As String, ByRef o As Int32) As Boolean
        Dim s As String = Nothing
        Return env_value(i, s) AndAlso
               Int32.TryParse(s, o)
    End Function

    Public Function env_value(ByVal i As String, ByRef o As Int64) As Boolean
        Dim s As String = Nothing
        Return env_value(i, s) AndAlso
               Int64.TryParse(s, o)
    End Function

    Public Function set_env(ByVal name As String, ByVal value As String) As Boolean
        Try
            Environment.SetEnvironmentVariable(name, value)
            Return True
        Catch
            Return False
        End Try
    End Function

    Public Function erase_env(ByVal name As String) As Boolean
        Return set_env(name, Nothing)
    End Function
End Module

Public NotInheritable Class scoped_environments
    Implements IDisposable

    Private ReadOnly envs() As String

    Public Sub New(ByVal envs(,) As String)
        assert(envs.GetLength(1) >= 2)
        ReDim Me.envs(array_size_i(envs) - 1)
        For i As Int32 = 0 To array_size_i(envs) - 1
            Me.envs(i) = envs(i, 0)
            assert(set_env(envs(i, 0), envs(i, 1)))
        Next
    End Sub

    Public Sub New(ByVal envs()() As String)
        ReDim Me.envs(array_size_i(envs) - 1)
        For i As Int32 = 0 To array_size_i(envs) - 1
            assert(array_size(envs(i)) >= 2)
            Me.envs(i) = envs(i)(0)
            assert(set_env(envs(i)(0), envs(i)(1)))
        Next
    End Sub

    Public Sub New(ByVal name As String, ByVal value As String)
        ReDim Me.envs(0)
        Me.envs(0) = name
        assert(set_env(name, value))
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        For i As Int32 = 0 To array_size_i(envs) - 1
            assert(set_env(envs(i), Nothing))
        Next
        GC.SuppressFinalize(Me)
    End Sub

    Protected Overrides Sub Finalize()
        Dispose()
        GC.KeepAlive(Me)
        MyBase.Finalize()
    End Sub
End Class