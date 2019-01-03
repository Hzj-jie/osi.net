
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.utt
Imports osi.root.utt.attributes

<test>
Public NotInheritable Class type_resolver_test
    <test>
    Private Shared Sub register_and_resolve()
        Dim r As type_resolver(Of Int32) = Nothing
        r = New type_resolver(Of Int32)()
        r.assert_first_register(GetType(Int32), 1)
        r.assert_first_register(GetType(Boolean), 2)
        r.assert_first_register(GetType(String), 3)
        r.register(GetType(Int32), 4)

        Dim o As Int32 = 0
        assertion.is_true(r.registered(GetType(Int32)))
        assertion.is_true(r.from_type(GetType(Int32), o))
        assertion.equal(o, 4)
        assertion.is_true(r.registered(GetType(Boolean)))
        assertion.is_true(r.from_type(GetType(Boolean), o))
        assertion.equal(o, 2)
        assertion.is_true(r.registered(GetType(String)))
        assertion.is_true(r.from_type(GetType(String), o))
        assertion.equal(o, 3)

        assertion.is_false(r.registered(GetType(Int16)))
        assertion.is_false(r.from_type(GetType(Int16), o))
        assertion.is_false(r.registered(GetType(Object)))
        assertion.is_false(r.from_type(GetType(Object), o))
    End Sub

    <test>
    Private Shared Sub resolve_from_base()
        Dim r As type_resolver(Of Int32) = Nothing
        r = New type_resolver(Of Int32)()
        r.assert_first_register(GetType(base), 1)

        Dim o As Int32 = 0
        assertion.is_true(r.from_type_or_base(GetType(derived), o))
        assertion.equal(o, 1)
    End Sub

    <test>
    Private Shared Sub resolve_from_object()
        Dim r As type_resolver(Of Int32) = Nothing
        r = New type_resolver(Of Int32)()

        assertion.is_false(r.from_type_or_base(GetType(Object), 0))

        r.assert_first_register(GetType(Object), 1)
        Dim o As Int32 = 0
        assertion.is_true(r.from_type_or_base(GetType(Int32), o))
        assertion.equal(o, 1)
    End Sub

    <test>
    Private Shared Sub resolve_from_interfaces()
        Dim r As type_resolver(Of Int32) = Nothing
        r = New type_resolver(Of Int32)()
        r.assert_first_register(GetType(int), 1)
        r.assert_first_register(GetType(base), 2)

        Dim o As Int32 = 0
        assertion.is_true(r.from_type_or_interfaces(GetType(base), o))
        assertion.equal(o, 2)

        assertion.is_true(r.from_type_or_interfaces(GetType(derived), o))
        assertion.equal(o, 1)

        r.assert_unregister(GetType(int))
        assertion.is_false(r.from_type_or_interfaces(GetType(derived), o))
        r.assert_first_register(GetType(int2), 3)
        assertion.is_true(r.from_type_or_interfaces(GetType(derived), o))
        assertion.equal(o, 3)
    End Sub

    <test>
    Private Shared Sub resolve_without_interfaces()
        Dim r As type_resolver(Of Int32) = Nothing
        r = New type_resolver(Of Int32)()
        r.assert_first_register(GetType(Int32), 1)

        assertion.is_false(r.from_interfaces(GetType(Int32), 0))
        Dim o As Int32 = 0
        assertion.is_true(r.from_type_or_interfaces(GetType(Int32), o))
        assertion.equal(o, 1)
    End Sub

    Private Sub New()
    End Sub

    Private Interface int
    End Interface

    Private Interface int2
    End Interface

    Private Class base
        Implements int
    End Class

    Private Class derived
        Inherits base
        Implements int2
    End Class
End Class
