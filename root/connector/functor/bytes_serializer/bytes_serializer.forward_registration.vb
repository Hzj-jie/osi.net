
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.delegates

Partial Public Class bytes_serializer(Of T)
    Public NotInheritable Class forward_registration
        Private Shared Function c(Of OT)(ByVal f As Func(Of OT, MemoryStream, Boolean)) _
                                        As Func(Of T, MemoryStream, Boolean)
            assert(Not f Is Nothing)
            Return Function(ByVal i As T, ByVal o As MemoryStream) As Boolean
                       Dim r As OT = Nothing
                       r = cast(Of OT)(i)
                       Return f(r, o)
                   End Function
        End Function

        Private Shared Function c(Of OT)(ByVal f As _do_val_ref(Of MemoryStream, OT, Boolean)) _
                                        As _do_val_ref(Of MemoryStream, T, Boolean)
            assert(Not f Is Nothing)
            Return Function(ByVal i As MemoryStream, ByRef o As T) As Boolean
                       Dim r As OT = Nothing
                       If Not f(i, r) Then
                           Return False
                       End If

                       o = cast(Of T, OT)(r)
                       Return True
                   End Function
        End Function

        Public Shared Sub from(Of OT)()
            delayed_init(Of bytes_serializer(Of T)).register(
                Sub()
                    default_functor.register.append_to(c(bytes_serializer(Of OT).default_functor.append_to()))
                    default_functor.register.write_to(c(bytes_serializer(Of OT).default_functor.write_to()))
                    default_functor.register.consume_from(c(bytes_serializer(Of OT).default_functor.consume_from()))
                    default_functor.register.read_from(c(bytes_serializer(Of OT).default_functor.read_from()))
                End Sub)
        End Sub

        Private Sub New()
        End Sub
    End Class
End Class
