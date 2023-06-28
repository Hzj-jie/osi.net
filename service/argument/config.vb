
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants

Public NotInheritable Class config
    Implements ICloneable, ICloneable(Of config)

    Public Shared Function [default]() As config
        Return New config()
    End Function

    Public argument_separator As String = character.blank
    Public switcher_prefix As String = character.minus_sign + character.left_slash
    Public full_switcher_prefix As String = character.tilde
    Public arg_prefix As String = character.minus_sign + character.minus_sign
    Public arg_key_value_separator As String = character.equal_sign
    Public case_sensitive As Boolean = True

    ' Anything should start from [default]() then CloneT().
    Private Sub New()
    End Sub

    Private Sub New(ByVal argument_separator As String,
                    ByVal switcher_prefix As String,
                    ByVal full_switcher_prefix As String,
                    ByVal arg_prefix As String,
                    ByVal arg_key_value_separator As String,
                    ByVal case_sensitive As Boolean)
        Me.argument_separator = argument_separator
        Me.switcher_prefix = switcher_prefix
        Me.full_switcher_prefix = full_switcher_prefix
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

    Private Function is_switcher_prefix(ByVal i As String) As Boolean
        Return Not i.null_or_empty() AndAlso
               Not switcher_prefix.null_or_empty() AndAlso
               strlen(i) > 1 AndAlso
               strindexof(switcher_prefix, i(0), case_sensitive) <> npos AndAlso
               Not is_arg_prefix(i)
    End Function

    Private Function is_full_switcher_prefix(ByVal i As String) As Boolean
        Return Not i.null_or_empty() AndAlso
               Not full_switcher_prefix.null_or_empty() AndAlso
               strlen(i) > 1 AndAlso
               strindexof(full_switcher_prefix, i(0), case_sensitive) <> npos AndAlso
               Not is_arg_prefix(i)
    End Function

    Private Function is_arg_prefix(ByVal i As String) As Boolean
        Return Not i.null_or_empty() AndAlso
               Not arg_prefix.null_or_empty() AndAlso
               strlen(i) > strlen(arg_prefix) AndAlso
               strsame(i, arg_prefix, strlen(arg_prefix))
    End Function

    Friend Function is_switcher(ByVal i As String, ByRef o As String) As Boolean
        If Not is_switcher_prefix(i) Then
            Return False
        End If
        o = strmid(i, 1)
        If Not case_sensitive Then
            strtolower(o)
        End If
        Return True
    End Function

    Friend Function is_full_switcher(ByVal i As String, ByRef o As String) As Boolean
        If Not is_full_switcher_prefix(i) Then
            Return False
        End If
        o = strmid(i, 1)
        If Not case_sensitive Then
            strtolower(o)
        End If
        Return True
    End Function

    Friend Function is_arg(ByVal i As String, ByRef k As String, ByRef v As String) As Boolean
        If Not is_arg_prefix(i) Then
            Return False
        End If
        i = i.strmid(arg_prefix.strlen())
        If Not strsep(i, k, v, arg_key_value_separator, case_sensitive) Then
            k = i
            v = Nothing
        End If
        If Not case_sensitive Then
            strtolower(k)
        End If
        Return True
    End Function

    Public Function Clone() As Object Implements ICloneable.Clone
        Return CloneT()
    End Function

    Public Function CloneT() As config Implements ICloneable(Of config).Clone
        Return New config(argument_separator,
                          switcher_prefix,
                          full_switcher_prefix,
                          arg_prefix,
                          arg_key_value_separator,
                          case_sensitive)
    End Function
End Class

