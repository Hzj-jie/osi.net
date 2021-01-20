
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants

<global_init(global_init_level.foundamental)>
Friend NotInheritable Class rnd_register_internal_types
    Private Shared ReadOnly run_shared_sub_new As cctor_delegator = New cctor_delegator(
        Sub()
            rnd_register(Of Boolean).register(Function() rnd_bool())
            rnd_register(Of Byte).register(Function() rnd_uint8())
            rnd_register(Of SByte).register(Function() rnd_int8())
            rnd_register(Of UInt16).register(Function() rnd_uint16())
            rnd_register(Of Int16).register(Function() rnd_int16())
            rnd_register(Of UInt32).register(Function() rnd_uint())
            rnd_register(Of Int32).register(Function() rnd_int())
            rnd_register(Of UInt64).register(Function() rnd_uint64())
            rnd_register(Of Int64).register(Function() rnd_int64())
            rnd_register(Of Double).register(Function() rnd_double())
            rnd_register(Of String).register(Function() rnd_utf8_chars(rnd_int(100, 200)))
            rnd_register(Of Char).register(Function() rnd_char())
        End Sub)

    Private Sub New()
    End Sub

    Private Shared Sub init()
    End Sub
End Class

Public NotInheritable Class rnd_register(Of T)
    Private Shared c As Func(Of T)

    Public Shared Sub register(ByVal c As Func(Of T))
        assert(Not c Is Nothing)
        assert(rnd_register(Of T).c Is Nothing OrElse object_compare(rnd_register(Of T).c, c) = 0)
        rnd_register(Of T).c = c
    End Sub

    Public Shared Function defined() As Boolean
        Return Not c Is Nothing
    End Function

    Public Shared Function ref() As Func(Of T)
        Return c
    End Function

    Public Shared Function rnd() As T
        assert(Not c Is Nothing)
        Return c()
    End Function

    Private Sub New()
    End Sub
End Class
