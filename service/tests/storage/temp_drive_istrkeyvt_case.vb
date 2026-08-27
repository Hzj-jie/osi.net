
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.connector
Imports osi.root.envs
Imports osi.root.procedure
Imports osi.root.utils
Imports osi.service.storage

'MustInherit for utt
Public MustInherit Class temp_drive_istrkeyvt_case
    Inherits istrkeyvt_case

    Private Shared ReadOnly data_dir_base As String =
        If(os.is_windows, "T:\", If(os.is_nix, "/dev/shm", Nothing))
    Private Shared ReadOnly temp_dir As String
    Private Shared ReadOnly valid As Boolean
    Protected ReadOnly data_dir As String

    Shared Sub New()
        If Not data_dir_base Is Nothing AndAlso Directory.Exists(data_dir_base) Then
            temp_dir = Path.Combine(data_dir_base, "temp", guid_str())
            void_(Sub()
                      If Directory.Exists(temp_dir) Then
                          Directory.Delete(temp_dir, True)
                      End If
                  End Sub)
            void_(Sub()
                      Directory.CreateDirectory(temp_dir)
                  End Sub)
            valid = Directory.Exists(temp_dir)
        End If
    End Sub

    Protected Sub New(ByVal i As iistrkeyvt_case)
        MyBase.New(i)
        If valid Then
            data_dir = Path.Combine(temp_dir, guid_str())
        End If
    End Sub

    Protected Sub New()
        Me.New(New default_istrkeyvt_case())
    End Sub

    Protected Overridable Function create_valid_istrkeyvt() As istrkeyvt
        assert(False)
        Return Nothing
    End Function

    Protected Overridable Function create_valid_istrkeyvt(ByVal p As ref(Of istrkeyvt)) As event_comb
        Return New event_comb(Function() As Boolean
                                  Return eva(p, create_valid_istrkeyvt()) AndAlso
                                         goto_end()
                              End Function)
    End Function

    Protected NotOverridable Overrides Function fulfill_precondition() As Boolean
        Return valid
    End Function

    Protected NotOverridable Overrides Function create_istrkeyvt(ByVal p As ref(Of istrkeyvt)) As event_comb
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  assert(valid)
                                  ec = create_valid_istrkeyvt(p)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function

    Protected NotOverridable Overrides Function clean_up() As event_comb
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  ec = MyBase.clean_up()
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If valid Then
                                      Return waitfor(Sub()
                                                         raise_error("clean up temp data_dir ", data_dir)
                                                         Directory.Delete(data_dir, True)
                                                     End Sub) AndAlso
                                             ec.end_result() AndAlso
                                             goto_end()
                                  Else
                                      Return ec.end_result() AndAlso
                                             goto_end()
                                  End If
                              End Function)
    End Function
End Class
