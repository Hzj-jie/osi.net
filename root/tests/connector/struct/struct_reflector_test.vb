
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utt
Imports osi.root.utt.attributes

<test>
Public NotInheritable Class struct_reflector_test
    Private NotInheritable Class c
        Public a As String
        Public b As vector(Of Int32)
        Public c As Boolean
    End Class

    <test>
    Private Shared Sub empty_class_case()
        Dim vs() As struct.variable = Nothing
        assertion.is_true(struct.disassemble(New c(), vs))
        assertion.equal(array_size_i(vs), 3)
        Dim o As c = Nothing
        assertion.is_true(struct.assemble(vs, o))
        assertion.is_null(o.a)
        assertion.is_null(o.b)
        assertion.is_false(o.c)
    End Sub

    <test>
    Private Shared Sub class_with_value_case()
        Dim e As c = Nothing
        e = New c()
        e.a = guid_str()
        e.b = vector.emplace_of(rnd_int(), rnd_int(), rnd_int())
        e.c = rnd_bool()
        Dim vs() As struct.variable = Nothing
        assertion.is_true(struct.disassemble(e, vs))
        assertion.equal(array_size_i(vs), 3)
        Dim o As c = Nothing
        assertion.is_true(struct.assemble(vs, o))
        assertion.equal(e.a, o.a)
        assertion.equal(e.b, o.b)
        assertion.equal(e.c, o.c)

        assertion.reference_equal(e.a, o.a)
        assertion.reference_equal(e.b, o.b)
    End Sub

    Private Structure s
        Public a As String
        Public b As vector(Of Int32)
        Public c As Boolean
    End Structure

    <test>
    Private Shared Sub struct_case()
        Dim e As s = Nothing
        e = New s()
        e.a = guid_str()
        e.b = vector.emplace_of(rnd_int(), rnd_int(), rnd_int())
        e.c = rnd_bool()
        Dim vs() As struct.variable = Nothing
        assertion.is_true(struct.disassemble(e, vs))
        assertion.equal(array_size_i(vs), 3)
        Dim o As s = Nothing
        assertion.is_true(struct.assemble(vs, o))
        assertion.equal(e.a, o.a)
        assertion.equal(e.b, o.b)
        assertion.equal(e.c, o.c)

        assertion.reference_equal(e.a, o.a)
        assertion.reference_equal(e.b, o.b)
    End Sub

    Private NotInheritable Class c2
        Public ReadOnly a As String

        Public Sub New()
            a = guid_str()
        End Sub
    End Class

    <test>
    Private Shared Sub readonly_variables_case()
        Dim a As c2 = Nothing
        a = New c2()
        Dim vs() As struct.variable = Nothing
        assertion.is_true(struct.disassemble(a, vs))
        assertion.equal(array_size_i(vs), 1)
        Dim b As c2 = Nothing
        assertion.is_true(struct.assemble(vs, b))
        assertion.equal(a.a, b.a)
        assertion.reference_equal(a.a, b.a)
    End Sub

    Private NotInheritable Class c3
        Private a As String

        Public Sub New()
            a = guid_str()
        End Sub

        Public Function g() As String
            Return a
        End Function
    End Class

    <test>
    Private Shared Sub should_ignore_non_public_variables()
        Dim a As c3 = Nothing
        a = New c3()
        Dim vs() As struct.variable = Nothing
        assertion.is_true(struct.disassemble(a, vs))
        assertion.equal(array_size_i(vs), 0)
        Dim b As c3 = Nothing
        assertion.is_true(struct.assemble(vs, b))
        assertion.not_equal(a.g(), b.g())
        assertion.not_reference_equal(a.g(), b.g())
    End Sub

    Private Sub New()
    End Sub
End Class
