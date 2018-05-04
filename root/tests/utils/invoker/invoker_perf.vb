
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.utils
Imports osi.root.utt

' A pre_binding invoker (Func or Action object) has on-par performance with native function call.
Public Class invoker_perf
    Inherits performance_comparison_case_wrapper

    Public Sub New()
        MyBase.New(r(New invoker_static_case()),
                   r(New invoker_static_pre_bind_case()),
                   r(New call_static_case()),
                   r(New invoker_object_case()),
                   r(New invoker_object_pre_bind_case()),
                   r(New call_object_case()))
    End Sub

    Protected Overrides Function min_rate_table() As Double(,)
        If isdebugbuild() Then
            Return {{0, 20, 28, -1, -1, -1},
                    {-1, 0, 2, -1, -1, -1},
                    {-1, -1, 0, -1, -1, -1},
                    {-1, -1, -1, 0, 18, 20},
                    {-1, -1, -1, -1, 0, 1.5},
                    {-1, -1, -1, -1, -1, 0}}
        Else
            Return {{0, 25, 45, -1, -1, -1},
                    {-1, 0, 2.5, -1, -1, -1},
                    {-1, -1, 0, -1, -1, -1},
                    {-1, -1, -1, 0, 25, 40},
                    {-1, -1, -1, -1, 0, 2},
                    {-1, -1, -1, -1, -1, 0}}
        End If
    End Function

    Private Shared Function r(ByVal c As [case]) As [case]
        Return repeat(c, 10000000)
    End Function

    <Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)>
    Private Shared Function f1() As Int32
        Dim x As Int32 = 0
        x += 1
        Return 0
    End Function

    <Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)>
    Private Shared Function f2(ByVal i As Int32) As Int32
        Dim x As Int32 = 0
        x += 1
        Return i
    End Function

    <Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)>
    Private Shared Sub g1()
        Dim x As Int32 = 0
        x += 1
    End Sub

    <Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)>
    Private Shared Sub g2(ByVal i As Int32)
        Dim x As Int32 = 0
        x += 1
    End Sub

    Private Class test_class
        <Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)>
        Public Function f1() As Int32
            Dim x As Int32 = 0
            x += 1
            Return 0
        End Function

        <Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)>
        Public Function f2(ByVal i As Int32) As Int32
            Dim x As Int32 = 0
            x += 1
            Return i
        End Function

        <Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)>
        Public Sub g1()
            Dim x As Int32 = 0
            x += 1
        End Sub

        <Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)>
        Public Sub g2(ByVal i As Int32)
            Dim x As Int32 = 0
            x += 1
        End Sub
    End Class

    Private Class invoker_static_case
        Inherits [case]

        Protected ReadOnly i As invoker(Of Func(Of Int32))
        Protected ReadOnly j As invoker(Of Func(Of Int32, Int32))
        Protected ReadOnly k As invoker(Of Action)
        Protected ReadOnly l As invoker(Of Action(Of Int32))

        Public Sub New()
            i = invoker.of(i).
                    with_type(Of invoker_perf)().
                    with_binding_flags(binding_flags.static_private_method).
                    with_name("f1").
                    build()
            assert(i.pre_binding())
            j = invoker.of(j).
                    with_type(Of invoker_perf)().
                    with_binding_flags(binding_flags.static_private_method).
                    with_name("f2").
                    build()
            assert(j.pre_binding())
            k = invoker.of(k).
                    with_type(Of invoker_perf).
                    with_binding_flags(binding_flags.static_private_method).
                    with_name("g1").
                    build()
            assert(k.pre_binding())
            l = invoker.of(l).
                    with_type(Of invoker_perf).
                    with_binding_flags(binding_flags.static_private_method).
                    with_name("g2").
                    build()
            assert(l.pre_binding())
        End Sub

        Public Overrides Function run() As Boolean
            i.get()()
            j.get()(0)
            k.get()()
            l.get()(0)
            Return True
        End Function
    End Class

    Private Class invoker_static_pre_bind_case
        Inherits invoker_static_case

        Private Shadows ReadOnly i As Func(Of Int32)
        Private Shadows ReadOnly j As Func(Of Int32, Int32)
        Private Shadows ReadOnly k As Action
        Private Shadows ReadOnly l As Action(Of Int32)

        Public Sub New()
            MyBase.New()
            Me.i = +MyBase.i
            Me.j = +MyBase.j
            Me.k = +MyBase.k
            Me.l = +MyBase.l
        End Sub

        Public Overrides Function run() As Boolean
            i()
            j(0)
            k()
            l(0)
            Return True
        End Function
    End Class

    Private Class call_static_case
        Inherits [case]

        Public Overrides Function run() As Boolean
            f1()
            f2(0)
            g1()
            g2(0)
            Return True
        End Function
    End Class

    Private Class invoker_object_case
        Inherits [case]

        Protected ReadOnly i As invoker(Of Func(Of Int32))
        Protected ReadOnly j As invoker(Of Func(Of Int32, Int32))
        Protected ReadOnly k As invoker(Of Action)
        Protected ReadOnly l As invoker(Of Action(Of Int32))

        Public Sub New()
            Dim o As test_class = Nothing
            o = New test_class()
            i = invoker.of(i).
                    with_object(o).
                    with_binding_flags(binding_flags.instance_public_method).
                    with_name("f1").
                    build()
            assert(i.pre_binding())
            j = invoker.of(j).
                    with_object(o).
                    with_binding_flags(binding_flags.instance_public_method).
                    with_name("f2").
                    build()
            assert(j.pre_binding())
            k = invoker.of(k).
                    with_object(o).
                    with_binding_flags(binding_flags.instance_public_method).
                    with_name("g1").
                    build()
            assert(k.pre_binding())
            l = invoker.of(l).
                    with_object(o).
                    with_binding_flags(binding_flags.instance_public_method).
                    with_name("g2").
                    build()
            assert(l.pre_binding())
        End Sub

        Public Overrides Function run() As Boolean
            i.get()()
            j.get()(0)
            k.get()()
            l.get()(0)
            Return True
        End Function
    End Class

    Private Class invoker_object_pre_bind_case
        Inherits invoker_object_case

        Private Shadows ReadOnly i As Func(Of Int32)
        Private Shadows ReadOnly j As Func(Of Int32, Int32)
        Private Shadows ReadOnly k As Action
        Private Shadows ReadOnly l As Action(Of Int32)

        Public Sub New()
            Me.i = +MyBase.i
            Me.j = +MyBase.j
            Me.k = +MyBase.k
            Me.l = +MyBase.l
        End Sub

        Public Overrides Function run() As Boolean
            i()
            j(0)
            k()
            l(0)
            Return True
        End Function
    End Class

    Private Class call_object_case
        Inherits [case]

        Private ReadOnly o As test_class

        Public Sub New()
            o = New test_class()
        End Sub

        Public Overrides Function run() As Boolean
            o.f1()
            o.f2(0)
            o.g1()
            o.g2(0)
            Return True
        End Function
    End Class
End Class
