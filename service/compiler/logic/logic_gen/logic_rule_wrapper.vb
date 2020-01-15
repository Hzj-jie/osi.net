
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.template
Imports osi.service.automata
Imports osi.service.compiler.logic
Imports osi.service.interpreter.primitive
Imports exportable = osi.service.compiler.logic.exportable

Public NotInheritable Class logic_rule_wrapper
    Public ReadOnly type_alias As type_alias

    Public Sub New()
        type_alias = New type_alias()
    End Sub
End Class

Public Class logic_rule_wrapper(Of _nlexer_rule As __do(Of Byte()),
                                   _syntaxer_rule As __do(Of Byte()),
                                   _prefixes As __do(Of vector(Of Action(Of statements, logic_rule_wrapper))),
                                   _suffixes As __do(Of vector(Of Action(Of statements, logic_rule_wrapper))),
                                   _logic_gens As __do(Of vector(Of Action(Of logic_gens, logic_rule_wrapper))))
    Inherits rule_wrapper(Of _nlexer_rule, _syntaxer_rule)

    Private Shared ReadOnly w As logic_rule_wrapper
    Private Shared ReadOnly l As logic_gens
    Private Shared ReadOnly p As statements
    Private Shared ReadOnly s As statements

    Shared Sub New()
        w = New logic_rule_wrapper()
        l = New logic_gens()
        p = New statements()
        s = New statements()
        init_logic_gens()
        init_prefixes()
        init_suffixes()
    End Sub

    ' Used by vector.of
    Protected Shared Function registerer(ByVal i As Action(Of statements, logic_rule_wrapper)) _
                                  As Action(Of statements, logic_rule_wrapper)
        assert(Not i Is Nothing)
        Return i
    End Function

    ' Used by vector.of
    Protected Shared Function registerer(ByVal i As Action(Of logic_gens, logic_rule_wrapper)) _
                                  As Action(Of logic_gens, logic_rule_wrapper)
        assert(Not i Is Nothing)
        Return i
    End Function

    Private Shared Function ignore_parameters(Of T)(ByVal i As Action(Of T)) As Action(Of T, logic_rule_wrapper)
        assert(Not i Is Nothing)
        Return Sub(ByVal x As T, ByVal y As logic_rule_wrapper)
                   i(x)
               End Sub
    End Function

    Protected Shared Function ignore_parameters(ByVal i As Action(Of statements)) _
                                  As Action(Of statements, logic_rule_wrapper)
        Return ignore_parameters(Of statements)(i)
    End Function

    Protected Shared Function ignore_parameters(ByVal i As Action(Of logic_gens)) _
                                  As Action(Of logic_gens, logic_rule_wrapper)
        Return ignore_parameters(Of logic_gens)(i)
    End Function

    Public Shared Function build(ByVal root As typed_node, ByVal o As writer) As Boolean
        assert(Not root Is Nothing)
        assert(Not o Is Nothing)
        assert(root.type = typed_node.ROOT_TYPE)
        assert(strsame(root.type_name, typed_node.ROOT_TYPE_NAME))
        If root.leaf() Then
            Return False
        End If
        p.export(o)
        assert(root.child_count() > 0)
        For i As UInt32 = 0 To root.child_count() - uint32_1
            l.of(root.child(i)).build(o)
        Next
        s.export(o)
        Return True
    End Function

    Public Shared Function parse(ByVal input As String, ByVal o As writer) As Boolean
        assert(Not input.null_or_whitespace())
        assert(Not o Is Nothing)
        Dim r As typed_node = Nothing
        If Not nlp().parse(input, root:=r) Then
            Return False
        End If
        assert(Not r Is Nothing)
        Return build(r, o)
    End Function

    Public Shared Function parse(ByVal input As String, ByRef o As String) As Boolean
        Dim w As writer = Nothing
        w = New writer()
        If Not parse(input, w) Then
            Return False
        End If
        o = w.dump()
        Return True
    End Function

    Public Shared Function with_functions(ByVal functions As interrupts) As parse_wrapper
        Return New parse_wrapper(functions)
    End Function

    Public NotInheritable Class parse_wrapper
        Private ReadOnly functions As interrupts

        Public Sub New(ByVal functions As interrupts)
            assert(Not functions Is Nothing)
            Me.functions = functions
        End Sub

        Public Function parse(ByVal input As String, ByRef e As executor) As Boolean
            e = New simulator(functions)
            If parse(input, direct_cast(Of interpreter.primitive.exportable)(e)) Then
                Return True
            End If
            e = Nothing
            Return False
        End Function

        Public Function parse(ByVal input As String, ByVal e As interpreter.primitive.exportable) As Boolean
            assert(Not e Is Nothing)
            Dim o As writer = Nothing
            o = New writer()
            If Not logic_rule_wrapper(Of _nlexer_rule, _syntaxer_rule, _prefixes, _suffixes, _logic_gens).
                parse(input, o) Then
                Return False
            End If
            Dim es As vector(Of exportable) = Nothing
            es = New vector(Of exportable)()
            If Not o.dump(functions, es) Then
                Return False
            End If
            Return e.import(+es)
        End Function
    End Class

    Private Shared Sub init_logic_gens()
        Dim v As vector(Of Action(Of logic_gens, logic_rule_wrapper)) = Nothing
        v = +alloc(Of _logic_gens)()
        Dim i As UInt32 = 0
        While i < v.size()
            v(i)(l, w)
            i += uint32_1
        End While
    End Sub

    Private Shared Sub init_statements(Of T As __do(Of vector(Of Action(Of statements, logic_rule_wrapper)))) _
                                      (ByVal p As statements)
        Dim v As vector(Of Action(Of statements, logic_rule_wrapper)) = Nothing
        v = +alloc(Of T)()
        Dim i As UInt32 = 0
        While i < v.size()
            v(i)(p, w)
            i += uint32_1
        End While
    End Sub

    Private Shared Sub init_prefixes()
        init_statements(Of _prefixes)(p)
    End Sub

    Private Shared Sub init_suffixes()
        init_statements(Of _suffixes)(s)
    End Sub

    Protected Sub New()
    End Sub
End Class
