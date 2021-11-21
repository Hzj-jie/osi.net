
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utt
Imports osi.root.utt.attributes
Imports osi.service.compiler.logic

Namespace logic
    <test>
    Public NotInheritable Class variable_test
        <test>
        Private Shared Sub primitive_without_sizes()
            Dim s As New scope()
            Using defer.to(AddressOf s.end_scope)
                assertion.is_true(s.define_stack("abc", types.variable_type))
                Dim v As New vector(Of String)()
                Dim o As variable = Nothing
                assertion.is_true(variable.of("abc", v, o))
                assertion.equal(o.ToString(), "abs0")
                assertion.is_true(v.empty())
                assertion.equal(o.name, "abc")
                assertions.of(o.index).is_empty()
                assertions.of(o.size).is_empty()
                assertion.equal(o.type, types.variable_type)
            End Using
        End Sub

        <test>
        Private Shared Sub primitive()
            Dim s As New scope()
            Using defer.to(AddressOf s.end_scope)
                assertion.is_true(s.define_stack("abc", types.variable_type))
                Dim v As New vector(Of String)()
                Dim o As variable = Nothing
                assertion.is_true(variable.of(types.default, "abc", v, o))
                assertion.equal(o.ToString(), "abs0")
                assertion.is_true(v.empty())
                assertion.equal(o.name, "abc")
                assertions.of(o.index).is_empty()
                assertions.of(o.size).has_value(types.default(types.variable_type))
                assertion.equal(o.type, types.variable_type)
            End Using
        End Sub

        <test>
        Private Shared Sub heap_ptr_without_sizes()
            Dim s As New scope()
            Using defer.to(AddressOf s.end_scope)
                assertion.is_true(s.define_heap("abc", types.variable_type))
                Dim v As New vector(Of String)()
                Dim o As variable = Nothing
                assertion.is_true(variable.of("abc", v, o))
                assertion.equal(o.ToString(), "abs0")
                assertion.is_true(v.empty())
                assertion.equal(o.name, "abc")
                assertions.of(o.index).is_empty()
                assertions.of(o.size).is_empty()
                assertion.equal(o.type, types.heap_ptr_type)
            End Using
        End Sub

        <test>
        Private Shared Sub heap_ptr()
            Dim s As New scope()
            Using defer.to(AddressOf s.end_scope)
                assertion.is_true(s.define_heap("abc", types.variable_type))
                Dim v As New vector(Of String)()
                Dim o As variable = Nothing
                assertion.is_true(variable.of(types.default, "abc", v, o))
                assertion.equal(o.ToString(), "abs0")
                assertion.is_true(v.empty())
                assertion.equal(o.name, "abc")
                assertions.of(o.index).is_empty()
                assertions.of(o.size).has_value(types.default(types.heap_ptr_type))
                assertion.equal(o.type, types.heap_ptr_type)
            End Using
        End Sub

        <test>
        Private Shared Sub heap_without_sizes()
            Dim s As New scope()
            Using defer.to(AddressOf s.end_scope)
                assertion.is_true(s.define_heap("abc", types.variable_type))
                ' This is unlikely necessary, using ulong or big_uint are also OK.
                assertion.is_true(s.define_stack("i", types.heap_ptr_type))
                Dim v As New vector(Of String)()
                Dim o As variable = Nothing
                assertion.is_true(variable.of("abc[i]", v, o))
                assertion.equal(o.ToString(), "habs2")
                assertion.array_equal(+v, {
                    "push",
                    "add abs2 abs0 abs1"})
                assertion.not_equal(o.name, "abc")
                assertions.of(o.size).is_empty()
                assertion.equal(o.type, types.variable_type)

                assertions.of(o.index).has_value()
                assertion.equal((+o.index).name, "i")
                assertions.of((+o.index).index).is_empty()
                assertions.of((+o.index).size).is_empty()
                assertion.equal((+o.index).type, types.heap_ptr_type)
            End Using
        End Sub

        <test>
        Private Shared Sub heap()
            Dim s As New scope()
            Using defer.to(AddressOf s.end_scope)
                assertion.is_true(s.define_heap("abc", types.variable_type))
                assertion.is_true(s.define_stack("i", types.heap_ptr_type))
                Dim v As New vector(Of String)()
                Dim o As variable = Nothing
                assertion.is_true(variable.of(types.default, "abc[i]", v, o))
                assertion.equal(o.ToString(), "habs2")
                assertion.array_equal(+v, {
                    "push",
                    "add abs2 abs0 abs1"})
                assertion.not_equal(o.name, "abc")
                assertions.of(o.size).has_value(types.default(types.variable_type))
                assertion.equal(o.type, types.variable_type)

                assertions.of(o.index).has_value()
                assertion.equal((+o.index).name, "i")
                assertions.of((+o.index).index).is_empty()
                assertions.of((+o.index).size).has_value(sizeof_uint64)
                assertion.equal((+o.index).type, types.heap_ptr_type)
            End Using
        End Sub

        <test>
        Private Shared Sub nested_heap()
            Dim s As New scope()
            Using defer.to(AddressOf s.end_scope)
                assertion.is_true(s.define_heap("abc", types.variable_type))
                assertion.is_true(s.define_heap("def", types.variable_type))
                assertion.is_true(s.define_stack("i", types.heap_ptr_type))
                Dim v As New vector(Of String)()
                Dim o As variable = Nothing
                assertion.is_true(variable.of(types.default, "abc[def[i]]", v, o))
                assertion.equal(o.ToString(), "habs4")
                assertion.array_equal(+v, {
                    "push",
                    "add abs3 abs1 abs2",
                    "push",
                    "add abs4 abs0 habs3"})
                assertion.not_equal(o.name, "abc")
                assertions.of(o.size).has_value(types.default(types.variable_type))
                assertion.equal(o.type, types.variable_type)

                assertions.of(o.index).has_value()
                assertion.not_equal((+o.index).name, "def")
                assertion.equal((+o.index).type, types.variable_type)
                assertions.of((+o.index).size).has_value(types.default(types.variable_type))

                assertions.of((+o.index).index).has_value()
                assertion.equal((+(+o.index).index).name, "i")
                assertion.equal((+(+o.index).index).type, types.heap_ptr_type)
                assertions.of((+(+o.index).index).size).has_value(sizeof_uint64)
            End Using
        End Sub

        Private Sub New()
        End Sub
    End Class
End Namespace
