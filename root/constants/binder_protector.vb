
' if a function is globally using, such as sizeof / operator+ / operator- / from_bytes / to_bytes
' there should be a global binder_protector, and use it everywhere
' if a function has a specific meaning in a class, such as flower.is_eos
' should use the class as binder_protector
' only use binder(Of T) in setters if there is no conflict, and always use binder(Of T, PROTECTOR) in consumers

Public NotInheritable Class binary_operator_add_protector
    Private Sub New()
    End Sub
End Class

Public NotInheritable Class binary_operator_minus_protector
    Private Sub New()
    End Sub
End Class

Public NotInheritable Class sizeof_binder_protector
    Private Sub New()
    End Sub
End Class

Public NotInheritable Class bytes_conversion_binder_protector
    Private Sub New()
    End Sub
End Class

Public NotInheritable Class uri_conversion_binder_protector
    Private Sub New()
    End Sub
End Class

Public NotInheritable Class string_conversion_binder_protector
    Private Sub New()
    End Sub
End Class

' compare is in connector, while suppress is in utils, it depends on atomic_bool, which is in lock
Public NotInheritable Class suppress_compare_error_binder_protector
    Private Sub New()
    End Sub
End Class

' binder is in connector, while suppress is in utils, it depends on atomic_bool, which is in lock
Public NotInheritable Class suppress_rebind_global_value_error_binder_protector
    Private Sub New()
    End Sub
End Class

' alloc is in connector, while suppress is in utils, it depends on atomic_bool, which is in lock
Public NotInheritable Class suppress_alloc_error_binder_protector
    Private Sub New()
    End Sub
End Class

Public NotInheritable Class disposer_binder_protector
    Private Sub New()
    End Sub
End Class