
#If RETIRED Then
Imports osi.root.constants
Imports osi.root.delegates
Imports osi.root.connector
Imports osi.root.utt

Public Class bytes_bytes_convertor_binder_initializer_test
    Inherits [case]

    Private Shared Function assert_has_value(Of T As Class)() As Boolean
        assert_true(DirectCast(Nothing, binder(Of T, bytes_conversion_binder_protector)).has_value())
        Return True
    End Function

    Public Overrides Function run() As Boolean
        Return assert_has_value(Of _do_val_ref_val_ref(Of Byte(), UInt32, Byte(), UInt32, Boolean))() AndAlso
               assert_has_value(Of _do_val_ref_val(Of Byte(), UInt32, Byte(), Boolean))() AndAlso
               assert_has_value(Of _do_val_ref_ref(Of Byte(), UInt32, Byte(), Boolean))() AndAlso
               assert_has_value(Of _do_val_ref(Of Byte(), UInt32, Byte()))() AndAlso
               assert_has_value(Of  _
                   _do_val_val_val_val_ref(Of Byte(), UInt32, UInt32, Byte(), UInt32, Boolean))() AndAlso
               assert_has_value(Of Func(Of Byte(), UInt32, UInt32, Byte(), Boolean))() AndAlso
               assert_has_value(Of _do_val_val_val_ref(Of Byte(), UInt32, Byte(), UInt32, Boolean))() AndAlso
               assert_has_value(Of Func(Of Byte(), UInt32, Byte(), Boolean))() AndAlso
               assert_has_value(Of _do_val_val_ref(Of Byte(), Byte(), UInt32, Boolean))() AndAlso
               assert_has_value(Of Func(Of Byte(), Byte(), Boolean))() AndAlso
               assert_has_value(Of _do_val_val_val_ref(Of Byte(), UInt32, UInt32, Byte(), Boolean))() AndAlso
               assert_has_value(Of _do_val_val_ref(Of Byte(), UInt32, Byte(), Boolean))() AndAlso
               assert_has_value(Of _do_val_ref(Of Byte(), Byte(), Boolean))() AndAlso
               assert_has_value(Of Func(Of Byte(), Byte()))()
    End Function
End Class
#End If
