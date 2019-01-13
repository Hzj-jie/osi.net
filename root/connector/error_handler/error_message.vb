
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Text
Imports osi.root.constants

Public Class error_message
    Public Const error_type_count As Int64 = error_type.last - error_type.first + 1
    Public Const error_type_char As String = "_aceiswuptod_"
    Public Shared ReadOnly error_type_defination() As String = {"_",
                                                                "application",
                                                                "critical",
                                                                "exclamation",
                                                                "information",
                                                                "system",
                                                                "warning",
                                                                "user",
                                                                "performance",
                                                                "trace",
                                                                "other",
                                                                "deprecated",
                                                                "_"}
    Public Const seperator As String = character.comma + character.blank

    Shared Sub New()
        If error_type_char.Length() <> error_type_count Then
            Console.Error().WriteLine("error_type_char length <> error_type count")
            assert_break()
        End If
        If error_type_defination.Length() <> error_type_count Then
            Console.Error().WriteLine("error_type_defination length <> error_type count")
            assert_break()
        End If
    End Sub

    <Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
    Public Shared Function P(ByVal err_type As error_type,
                             ByVal err_type_char As Char,
                             ByVal errmsg As String,
                             ByVal additional_jump As Int32) As String
        Dim prefix As String = Nothing
        If err_type <= error_type.first OrElse err_type >= error_type.last OrElse err_type = error_type.other Then
            If err_type_char = character.null Then
                prefix = character.x
            Else
                prefix = Char.ToLower(err_type_char)
            End If
        Else
            prefix = Char.ToLower(error_type_char(err_type))
        End If
        prefix = strcat(prefix, seperator, short_time(), seperator)
        errmsg = strcat(prefix, errmsg)
        If err_type <> error_type.information Then
            errmsg.append(seperator, backtrace(additional_jump + 1))
        End If

        'keep one \r\n
        strrplc(errmsg, newline.incode(), character.newline)

        Return errmsg
    End Function

    Public Shared Function P(ByVal m() As Object) As String
        assert(Not m Is Nothing)
        ' shortcut
        If m.Length() = 1 AndAlso TypeOf m(0) Is String Then
            Return Convert.ToString(m(0))
        End If
        Dim s As StringBuilder = Nothing
        s = New StringBuilder()
        For i As Int32 = 0 To m.Length() - 1
            If m(i) Is Nothing Then
                Continue For
            End If
            Dim a() As Object = Nothing
            If direct_cast(m(i), a) Then
                assert(Not a Is Nothing)
                If array_size(a) > 0 Then
                    s.Append(P(a))
                End If
            ElseIf TypeOf m(i) Is error_type Then
                s.Append(Convert.ToString(m(i))) _
                 .Append(character.comma) _
                 .Append(character.blank)
            Else
                assert(Not m(i) Is Nothing)
                s.Append(Convert.ToString(m(i)))
            End If
        Next
        Return Convert.ToString(s)
    End Function

    Private Sub New()
    End Sub
End Class
