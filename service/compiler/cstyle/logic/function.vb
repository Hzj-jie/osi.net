
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.template
Imports osi.service.automata

Public NotInheritable Class [function]
    Inherits builder_wrapper
    Implements builder

    Public Sub New(ByVal b As builders, ByVal lp As lang_parser)
        MyBase.New(b, lp)
    End Sub

    Public NotInheritable Class function_ref
        Private ReadOnly n As typed_node

        Public Sub New(ByVal n As typed_node)
            assert(Not n Is Nothing)
            Me.n = n
        End Sub

        Public Shared Function [of](ByVal n As typed_node) As function_ref
            Return New function_ref(n)
        End Function

        Public Function allow_return_value() As Boolean
            Return Not strsame(n.word(0).str(), "void")
        End Function
    End Class

    Public Shared Function current() As function_ref
        Return New function_ref(instance_stack(Of annotated_ref(Of [function], typed_node)).current().v)
    End Function

    Public Function build(ByVal n As typed_node, ByVal o As writer) As Boolean Implements builder.build
        assert(Not n Is Nothing)
        assert(Not o Is Nothing)
        instance_stack(Of annotated_ref(Of [function], typed_node)).push(annotated_ref(Of [function]).with(n))
        o.append("callee")
        If Not b.of(n.child(1)).build(o) Then
            o.err("@function name ", n.child(1))
            Return False
        End If
        Dim has_paramlist As Boolean = False
        has_paramlist = strsame(lp.type_name(n.child(3)), "paramlist")
        If has_paramlist Then
            If Not b.of(n.child(3)).build(o) Then
                o.err("@function paramlist ", n.child(3))
                Return False
            End If
        End If
        Dim gi As UInt32 = 0
        gi = CUInt(If(has_paramlist, 5, 4))
        If Not b.of(n.child(gi)).build(o) Then
            o.err("@function multi-sentence-paragraph ", n.child(gi))
            Return False
        End If
        instance_stack(Of annotated_ref(Of [function], typed_node)).pop()
        Return True
    End Function
End Class
