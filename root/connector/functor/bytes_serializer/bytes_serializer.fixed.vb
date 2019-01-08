
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.delegates

Partial Public Class bytes_serializer(Of T)
    Public NotInheritable Class fixed
        Public Shared Sub register(ByVal append_to As Func(Of T, MemoryStream, Boolean),
                                   ByVal consume_from As _do_val_ref(Of MemoryStream, T, Boolean))
            default_functor.register.append_to(append_to)
            default_functor.register.write_to(append_to)
            default_functor.register.consume_from(consume_from)
            default_functor.register.read_from(consume_from)
        End Sub

        Private Sub New()
        End Sub
    End Class
End Class
