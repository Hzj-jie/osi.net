
#If RETIRED
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.delegates

Public Class bytes_convertor_register(Of T)
    Private Class RT
        Inherits convertor_register(Of Byte(), T, bytes_conversion_binder_protector)

        Private Sub New()
        End Sub
    End Class

    Private Class RF
        Inherits convertor_register(Of T, Byte(), bytes_conversion_binder_protector)

        Private Sub New()
        End Sub
    End Class

    Public Shared Function bind(ByVal f1 As _do_val_ref_ref(Of Byte(), UInt32, T, Boolean),
                                ByVal f2 As _do_val_val_val_ref(Of Byte(), UInt32, UInt32, T, Boolean),
                                ByVal t1 As _do_val_val_ref(Of T, Byte(), UInt32, Boolean),
                                ByVal t2 As _do_val_ref(Of T, Byte(), Boolean)) As Boolean
        Return RT.bind(f1) AndAlso
               RT.bind(f2) AndAlso
               RF.bind(t1) AndAlso
               RF.bind(t2)
    End Function

    Public Shared Sub assert_bind(ByVal f1 As _do_val_ref_ref(Of Byte(), UInt32, T, Boolean),
                                  ByVal f2 As _do_val_val_val_ref(Of Byte(), UInt32, UInt32, T, Boolean),
                                  ByVal t1 As _do_val_val_ref(Of T, Byte(), UInt32, Boolean),
                                  ByVal t2 As _do_val_ref(Of T, Byte(), Boolean))
        assert(bind(f1, f2, t1, t2))
    End Sub

    Private Sub New()
    End Sub
End Class
#End If
