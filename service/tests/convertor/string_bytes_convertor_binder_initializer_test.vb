
#If RETIRED
Imports osi.root.constants
Imports osi.root.delegates
Imports osi.root.connector
Imports osi.root.utt

Public Class string_bytes_convertor_binder_initializer_test
    Inherits [case]

    Private Shared Function assert_has_value(Of T As Class)() As Boolean
        assert_true(DirectCast(Nothing, binder(Of T, bytes_conversion_binder_protector)).has_value())
        assert_true(DirectCast(Nothing, binder(Of T, string_conversion_binder_protector)).has_value())
        Return True
    End Function

    Private Shared Function assert_has_not_value(Of T As Class)() As Boolean
        assert_false(DirectCast(Nothing, binder(Of T, bytes_conversion_binder_protector)).has_value())
        assert_false(DirectCast(Nothing, binder(Of T, string_conversion_binder_protector)).has_value())
        Return True
    End Function

    Public Overrides Function run() As Boolean
        Return assert_has_value(Of _do_val_ref_val_ref(Of String, UInt32, Byte(), UInt32, Boolean))() AndAlso
               assert_has_value(Of _do_val_ref_val(Of String, UInt32, Byte(), Boolean))() AndAlso
               assert_has_value(Of _do_val_ref_ref(Of String, UInt32, Byte(), Boolean))() AndAlso
               assert_has_value(Of _do_val_ref(Of String, UInt32, Byte()))() AndAlso
               assert_has_value(Of  _
                   _do_val_val_val_val_ref(Of String, UInt32, UInt32, Byte(), UInt32, Boolean))() AndAlso
               assert_has_value(Of Func(Of String, UInt32, UInt32, Byte(), Boolean))() AndAlso
               assert_has_value(Of _do_val_val_val_ref(Of String, UInt32, Byte(), UInt32, Boolean))() AndAlso
               assert_has_value(Of Func(Of String, UInt32, Byte(), Boolean))() AndAlso
               assert_has_value(Of _do_val_val_ref(Of String, Byte(), UInt32, Boolean))() AndAlso
               assert_has_value(Of Func(Of String, Byte(), Boolean))() AndAlso
               assert_has_value(Of _do_val_val_val_ref(Of String, UInt32, UInt32, Byte(), Boolean))() AndAlso
               assert_has_value(Of _do_val_val_ref(Of String, UInt32, Byte(), Boolean))() AndAlso
               assert_has_value(Of _do_val_ref(Of String, Byte(), Boolean))() AndAlso
               assert_has_value(Of Func(Of String, Byte()))() AndAlso
 _
               assert_has_not_value(Of _do_val_ref_val_ref(Of Byte(), UInt32, String, UInt32, Boolean))() AndAlso
               assert_has_not_value(Of _do_val_ref_val(Of Byte(), UInt32, String, Boolean))() AndAlso
               assert_has_value(Of _do_val_ref_ref(Of Byte(), UInt32, String, Boolean))() AndAlso
               assert_has_value(Of _do_val_ref(Of Byte(), UInt32, String))() AndAlso
               assert_has_not_value(Of  _
                   _do_val_val_val_val_ref(Of Byte(), UInt32, UInt32, String, UInt32, Boolean))() AndAlso
               assert_has_not_value(Of Func(Of Byte(), UInt32, UInt32, String, Boolean))() AndAlso
               assert_has_not_value(Of _do_val_val_val_ref(Of Byte(), UInt32, String, UInt32, Boolean))() AndAlso
               assert_has_not_value(Of Func(Of Byte(), UInt32, String, Boolean))() AndAlso
               assert_has_not_value(Of _do_val_val_ref(Of Byte(), String, UInt32, Boolean))() AndAlso
               assert_has_not_value(Of Func(Of Byte(), String, Boolean))() AndAlso
               assert_has_value(Of _do_val_val_val_ref(Of Byte(), UInt32, UInt32, String, Boolean))() AndAlso
               assert_has_value(Of _do_val_val_ref(Of Byte(), UInt32, String, Boolean))() AndAlso
               assert_has_value(Of _do_val_ref(Of Byte(), String, Boolean))() AndAlso
               assert_has_value(Of Func(Of Byte(), String))()
    End Function
End Class
#End If
