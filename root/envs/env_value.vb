
Imports System.Collections
Imports osi.root.constants
Imports osi.root.connector

Public Module _env_value
    Public Function env_value(ByVal str() As String, ByRef value As String) As Boolean
        If str Is Nothing OrElse str.Length() = 0 Then
            Return False
        Else
            Dim envs As IDictionary = Nothing
            envs = Environment.GetEnvironmentVariables()
            If Not envs.null_or_empty() Then
                For i As Int64 = 0 To str.Length() - 1
                    If envs.Contains(str(i)) Then
                        value = envs(str(i))
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
        If String.IsNullOrEmpty(i) Then
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

Public Class scoped_environments
    Implements IDisposable

    Private ReadOnly envs() As String

    Public Sub New(ByVal envs(,) As String)
        assert(envs.GetLength(1) >= 2)
        ReDim Me.envs(array_size(envs) - 1)
        For i As Int32 = 0 To array_size(envs) - 1
            Me.envs(i) = envs(i, 0)
            assert(set_env(envs(i, 0), envs(i, 1)))
        Next
    End Sub

    Public Sub New(ByVal envs()() As String)
        ReDim Me.envs(array_size(envs) - 1)
        For i As Int32 = 0 To array_size(envs) - 1
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

#Region "IDisposable Support"
    Private disposedValue As Boolean

    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                For i As Int32 = 0 To array_size(envs) - 1
                    assert(set_env(envs(i), Nothing))
                Next
            End If
        End If
        Me.disposedValue = True
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class