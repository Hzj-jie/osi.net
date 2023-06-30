
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utils
Imports osi.service.storage

'MustInherit for utt
Public MustInherit Class temp_drive_istrkeyvt_case
    Inherits istrkeyvt_case

    Private Const data_dir_base As String = "T:\"
    Private Shared ReadOnly temp_dir As String
    Protected ReadOnly data_dir As String
    Private ReadOnly valid As Boolean

    Shared Sub New()
        temp_dir = Path.Combine(data_dir_base, "temp")
        void_(Sub()
                  Directory.Delete(temp_dir, True)
              End Sub)
        void_(Sub()
                  Directory.CreateDirectory(temp_dir)
              End Sub)
    End Sub

    Protected Sub New(ByVal i As iistrkeyvt_case)
        MyBase.New(i)
        data_dir = Path.Combine(temp_dir, guid_str())
        valid = Directory.Exists(data_dir_base)
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
