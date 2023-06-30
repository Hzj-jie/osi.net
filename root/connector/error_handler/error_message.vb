
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Text
Imports osi.root.constants

<global_init(global_init_level.log_and_counter_services)>
Public NotInheritable Class error_message
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

    Private Shared Sub init()
        If error_type_char.Length() <> error_type_count Then
            Console.Error().WriteLine("error_type_char length <> error_type count")
            assert_break()
        End If
        If error_type_defination.Length() <> error_type_count Then
            Console.Error().WriteLine("error_type_defination length <> error_type count")
            assert_break()
        End If
    End Sub

    <Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)>
    Public Shared Function p(ByVal err_type As error_type,
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

        Return errmsg.Replace(newline.incode(), character.newline)
    End Function

    Public Shared Function p(ByVal m() As Object) As String
        assert(Not m Is Nothing)
        ' shortcut
        If m.Length() = 1 AndAlso TypeOf m(0) Is String Then
            Return Convert.ToString(m(0))
        End If
        Dim s As New StringBuilder()
        process_obj_array(m, s)
        Return Convert.ToString(s)
    End Function

    Private Shared Sub process_obj_array(ByVal m() As Object, ByVal s As StringBuilder)
        For i As Int32 = 0 To array_size_i(m) - 1
            process_obj(m(i), s)
        Next
    End Sub

    Private Shared Sub process_obj(ByVal i As Object, ByVal s As StringBuilder)
        If i Is Nothing Then
            Return
        End If
        If process_as_obj_array(i, s) Then
            Return
        End If
        If process_as_error_type(i, s) Then
            Return
        End If
        If process_as_func_obj(i, s) Then
            Return
        End If
        s.Append(Convert.ToString(i))
    End Sub

    Private Shared Function process_as_obj_array(ByVal i As Object, ByVal s As StringBuilder) As Boolean
        Dim a() As Object = Nothing
        If Not direct_cast(i, a) Then
            Return False
        End If
        assert(Not a Is Nothing)
        process_obj_array(a, s)
        Return True
    End Function

    Private Shared Function process_as_error_type(ByVal i As Object, ByVal s As StringBuilder) As Boolean
        If Not TypeOf i Is error_type Then
            Return False
        End If
        s.Append(Convert.ToString(i)) _
         .Append(character.comma) _
         .Append(character.blank)
        Return True
    End Function

    Private Shared Function process_as_func_obj(ByVal i As Object, ByVal s As StringBuilder) As Boolean
        Dim f As Func(Of Object) = Nothing
        If Not direct_cast(i, f) Then
            Return False
        End If
        assert(Not f Is Nothing)
        process_obj(f(), s)
        Return True
    End Function

    Private Sub New()
    End Sub
End Class
