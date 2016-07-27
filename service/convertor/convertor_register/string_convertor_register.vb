
Imports osi.root.constants
Imports osi.root.delegates
Imports osi.root.connector

Public Class string_convertor_register(Of T, PROTECTOR)
    Private Class RT
        Inherits convertor_register(Of String, T, PROTECTOR)

        Private Sub New()
        End Sub
    End Class

    Private Class RF
        Inherits convertor_register(Of T, String, PROTECTOR)

        Private Sub New()
        End Sub
    End Class

    Public Shared Function bind(ByVal f1 As _do_val_ref_ref(Of String, UInt32, T, Boolean),
                                ByVal f2 As _do_val_val_val_ref(Of String, UInt32, UInt32, T, Boolean),
                                ByVal t1 As _do_val_ref(Of T, String, Boolean)) As Boolean
        Return RT.bind(f1) AndAlso
               RT.bind(f2) AndAlso
               RF.bind(t1)
    End Function

    Public Shared Sub assert_bind(ByVal f1 As _do_val_ref_ref(Of String, UInt32, T, Boolean),
                                  ByVal f2 As _do_val_val_val_ref(Of String, UInt32, UInt32, T, Boolean),
                                  ByVal t1 As _do_val_ref(Of T, String, Boolean))
        assert(bind(f1, f2, t1))
    End Sub

    Protected Sub New()
    End Sub
End Class

Public Class string_convertor_register(Of T)
    Inherits string_convertor_register(Of T, string_conversion_binder_protector)

    Private Sub New()
    End Sub
End Class

Public Class uri_convertor_register(Of T)
    Inherits string_convertor_register(Of T, uri_conversion_binder_protector)

    Private Sub New()
    End Sub
End Class
