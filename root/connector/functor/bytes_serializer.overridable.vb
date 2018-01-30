﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.delegates

Partial Public Class bytes_serializer(Of T)
    ' append - consume pair writes or reads serialized bytes as well as the boundary to or from the MemoryStream. So
    ' multiple objects with different types can be serialized to and deserialized from one MemoryStream.

    ' write - read pair writes or reads only serialized bytes to or from the MemoryStream. One object owns the
    ' MemoryStream. This pattern typically means no boundary and ReadToEnd().

    Protected Overridable Function append_to() As Func(Of T, MemoryStream, Boolean)
        Return default_functor.append_to()
    End Function

    Protected Overridable Function write_to() As Func(Of T, MemoryStream, Boolean)
        Return default_functor.write_to()
    End Function

    Protected Overridable Function consume_from() As _do_val_ref(Of MemoryStream, T, Boolean)
        Return default_functor.consume_from()
    End Function

    Protected Overridable Function read_from() As _do_val_ref(Of MemoryStream, T, Boolean)
        Return default_functor.read_from()
    End Function

    Private NotInheritable Class default_functor
        Public NotInheritable Class register
            Public Shared Sub append_to(ByVal f As Func(Of T, MemoryStream, Boolean))
                global_resolver(Of Func(Of T, MemoryStream, Boolean), consume_append).assert_first_register(f)
            End Sub

            Public Shared Sub write_to(ByVal f As Func(Of T, MemoryStream, Boolean))
                global_resolver(Of Func(Of T, MemoryStream, Boolean), read_write).assert_first_register(f)
            End Sub

            Public Shared Sub consume_from(ByVal f As _do_val_ref(Of MemoryStream, T, Boolean))
                global_resolver(Of _do_val_ref(Of MemoryStream, T, Boolean), consume_append).assert_first_register(f)
            End Sub

            Public Shared Sub read_from(ByVal f As _do_val_ref(Of MemoryStream, T, Boolean))
                global_resolver(Of _do_val_ref(Of MemoryStream, T, Boolean), read_write).assert_first_register(f)
            End Sub

            Private Sub New()
            End Sub
        End Class

        Shared Sub New()
            static_constructor(Of T).execute()
            delayed_init(Of bytes_serializer(Of T)).execute()
        End Sub

        Public Shared Function append_to() As Func(Of T, MemoryStream, Boolean)
            Return global_resolver(Of Func(Of T, MemoryStream, Boolean), consume_append).resolve_or_null()
        End Function

        Public Shared Function write_to() As Func(Of T, MemoryStream, Boolean)
            Return global_resolver(Of Func(Of T, MemoryStream, Boolean), read_write).resolve_or_null()
        End Function

        Public Shared Function consume_from() As _do_val_ref(Of MemoryStream, T, Boolean)
            Return global_resolver(Of _do_val_ref(Of MemoryStream, T, Boolean), consume_append).resolve_or_null()
        End Function

        Public Shared Function read_from() As _do_val_ref(Of MemoryStream, T, Boolean)
            Return global_resolver(Of _do_val_ref(Of MemoryStream, T, Boolean), read_write).resolve_or_null()
        End Function

        Private Interface consume_append
        End Interface

        Private Interface read_write
        End Interface

        Private Sub New()
        End Sub
    End Class
End Class