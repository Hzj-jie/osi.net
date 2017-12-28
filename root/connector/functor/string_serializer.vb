
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants
Imports osi.root.delegates

Public Class string_serializer(Of T, PROTECTOR)
    Public Shared ReadOnly [default] As string_serializer(Of T, PROTECTOR)

    Public Shared Sub register(ByVal to_str As _do_val_ref(Of T, String, Boolean))
        binder(Of _do_val_ref(Of T, String, Boolean), PROTECTOR).set_global(to_str)
    End Sub

    Public Shared Sub register(ByVal from_str As _do_val_ref(Of String, T, Boolean))
        binder(Of _do_val_ref(Of String, T, Boolean), PROTECTOR).set_global(from_str)
    End Sub

    Public Overridable Function to_str(ByVal i As T, ByRef o As String) As Boolean
        assert(binder(Of _do_val_ref(Of T, String, Boolean), PROTECTOR).has_global())
        Return binder(Of _do_val_ref(Of T, String, Boolean), PROTECTOR).global()(i, o)
    End Function

    Public Overridable Function from_bytes(ByVal i As String, ByRef o As T) As Boolean
        assert(binder(Of _do_val_ref(Of String, T, Boolean), PROTECTOR).has_global())
        Return binder(Of _do_val_ref(Of String, T, Boolean), PROTECTOR).global()(i, o)
    End Function

    Protected Sub New()
    End Sub
End Class

Public Class string_serializer(Of T)
    Inherits string_serializer(Of T, string_conversion_binder_protector)

    Protected Sub New()
    End Sub
End Class

Public Class uri_serializer(Of T)
    Inherits string_serializer(Of T, uri_conversion_binder_protector)

    Protected Sub New()
    End Sub
End Class