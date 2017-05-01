
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.utils

Public Class config
    Public Shared ReadOnly [default] As config

    Shared Sub New()
        [default] = New config()
    End Sub

    Public argument_separator As String = character.blank
    Public switcher_prefix As String = character.minus_sign + character.left_slash
    Public full_swither_prefix As String = character.tilde
    Public arg_prefix As String = character.minus_sign + character.minus_sign
    Public arg_key_value_separator As String = character.equal_sign
    Public case_sensitive As Boolean = True

    Public Function create_arg(Of T1, T2)(ByVal key As T1, ByVal value As T2) As String
        Return strcat(arg_prefix, key, arg_key_value_separator, value)
    End Function

    Public Function create_switcher(Of T)(ByVal key As T) As String
        Return strcat(strleft(switcher_prefix, 1), strleft(Convert.ToString(key), 1))
    End Function

    Public Function create_full_switcher(Of T)(ByVal key As T) As String
        Return strcat(full_swither_prefix, key)
    End Function

    Public Function merge_args(ByVal ParamArray kvs() As String) As String
        Return strjoin(argument_separator, kvs)
    End Function

    Private Function is_not_any_prefix(ByVal c As Char) As Boolean
        Return strindexof(switcher_prefix, c, case_sensitive) = npos AndAlso
               strindexof(arg_prefix, c, case_sensitive) = npos AndAlso
               strindexof(full_swither_prefix, c, case_sensitive) = npos
    End Function

    Private Function is_switcher_prefix(ByVal i As String) As Boolean
        Return Not String.IsNullOrEmpty(i) AndAlso
               Not String.IsNullOrEmpty(switcher_prefix) AndAlso
               strlen(i) > 1 AndAlso
               strindexof(switcher_prefix, i(0), case_sensitive) <> npos AndAlso
               is_not_any_prefix(i(1))
    End Function

    Private Function is_full_switcher_prefix(ByVal i As String) As Boolean
        Return Not String.IsNullOrEmpty(i) AndAlso
               Not String.IsNullOrEmpty(full_swither_prefix) AndAlso
               strlen(i) > 1 AndAlso
               strindexof(full_swither_prefix, i(0), case_sensitive) <> npos AndAlso
               is_not_any_prefix(i(1))
    End Function

    Private Function is_arg_prefix(ByVal i As String) As Boolean
        Return Not String.IsNullOrEmpty(i) AndAlso
               Not String.IsNullOrEmpty(arg_prefix) AndAlso
               strlen(i) > strlen(arg_prefix) AndAlso
               strsame(i, arg_prefix, strlen(arg_prefix)) AndAlso
               is_not_any_prefix(i(strlen(arg_prefix)))
    End Function

    Friend Function is_switcher(ByVal i As String, ByRef o As String) As Boolean
        If is_switcher_prefix(i) Then
            o = strmid(i, 1)
            If Not case_sensitive Then
                strtolower(o)
            End If
            Return True
        Else
            Return False
        End If
    End Function

    Friend Function is_full_switcher(ByVal i As String, ByRef o As String) As Boolean
        If is_full_switcher_prefix(i) Then
            o = strmid(i, 1)
            If Not case_sensitive Then
                strtolower(o)
            End If
            Return True
        Else
            Return False
        End If
    End Function

    Friend Function is_arg(ByVal i As String, ByRef k As String, ByRef v As String) As Boolean
        If is_arg_prefix(i) Then
            If strsep(strmid(i, strlen(arg_prefix)), k, v, arg_key_value_separator, case_sensitive) Then
                If Not case_sensitive Then
                    strtolower(k)
                End If
                Return True
            Else
                Return False
            End If
        Else
            Return False
        End If
    End Function
End Class
