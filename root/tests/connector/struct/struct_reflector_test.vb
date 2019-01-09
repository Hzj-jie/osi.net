
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

    <test>
    Private Shared Sub struct_case()

    End Sub

    <test>
    Private Shared Sub readonly_variables_case()

    End Sub

    <test>
    Private Shared Sub should_ignore_non_public_variables()

    End Sub

    Private Sub New()
    End Sub
End Class
