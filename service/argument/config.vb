
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants

Public Class config
    Implements ICloneable, ICloneable(Of config)

    Public Shared ReadOnly [default] As config

    Shared Sub New()
        [default] = New config()
    End Sub

    Public argument_separator As String = character.blank
    Public switcher_prefix As String = character.minus_sign + character.left_slash
    Public full_switcher_prefix As String = character.tilde
    Public arg_prefix As String = character.minus_sign + character.minus_sign
    Public arg_key_value_separator As String = character.equal_sign
    Public case_sensitive As Boolean = True

    Public Sub New()
    End Sub

    <copy_constructor>
    Protected Sub New(ByVal argument_separator As String,
                      ByVal switcher_prefix As String,
                      ByVal full_switcher_prefix As String,
                      ByVal arg_prefix As String,
                      ByVal arg_key_value_separator As String,
                      ByVal case_sensitive As Boolean)
        Me.argument_separator = argument_separator
        Me.switcher_prefix = switcher_prefix
        Me.full_switcher_prefix = Me.full_switcher_prefix
        Me.arg_prefix = arg_prefix
        Me.arg_key_value_separator = arg_key_value_separator
        Me.case_sensitive = case_sensitive
    End Sub

    Public Function create_arg(Of T1, T2)(ByVal key As T1, ByVal value As T2) As String
        Return strcat(arg_prefix, key, arg_key_value_separator, value)
    End Function

    Public Function create_switcher(Of T)(ByVal key As T) As String
        Return strcat(strleft(switcher_prefix, 1), strleft(Convert.ToString(key), 1))
    End Function

    Public Function create_full_switcher(Of T)(ByVal key As T) As String
        Return strcat(full_switcher_prefix, key)
    End Function

    Public Function merge_args(ByVal ParamArray kvs() As String) As String
        Return strjoin(argument_separator, kvs)
    End Function

    Private Function is_not_any_prefix(ByVal c As Char) As Boolean
        Return strindexof(switcher_prefix, c, case_sensitive) = npos AndAlso
               strindexof(arg_prefix, c, case_sensitive) = npos AndAlso
               strindexof(full_switcher_prefix, c, case_sensitive) = npos
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
               Not String.IsNullOrEmpty(full_switcher_prefix) AndAlso
               strlen(i) > 1 AndAlso
               strindexof(full_switcher_prefix, i(0), case_sensitive) <> npos AndAlso
               is_not_any_prefix(i(1))
    End Function

    Private Function is_arg_prefix(ByVal i As String) As Boolean
        Return Not String.IsNullOrEmpty(i) AndAlso
               Not String.IsNullOrEmpty(arg_prefix) AndAlso
               strlen(i) > strlen(arg_prefix) AndAlso
               strsame(i, arg_prefix, strlen(arg_prefix)) AndAlso
               is_not_any_prefix(i(strlen_i(arg_prefix)))
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

    Public Function Clone() As Object Implements ICloneable.Clone
        Return cloneT()
    End Function

    Public Function CloneT() As config Implements ICloneable(Of config).Clone
        Return clone(Of config)()
    End Function

    Public Function clone(Of R As config)() As R
        Return copy_constructor(Of R).invoke(argument_separator,
                                             switcher_prefix,
                                             full_switcher_prefix,
                                             arg_prefix,
                                             arg_key_value_separator,
                                             case_sensitive)
    End Function
End Class
