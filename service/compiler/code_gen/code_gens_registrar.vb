
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.compiler.rewriters

Public NotInheritable Class typed_node_writer_code_gens_registrar
    Inherits code_gens_registrar(Of typed_node_writer, typed_node_writer_code_gens_registrar)

    Public Function with_of_leaf_nodes(ByVal ParamArray names() As String) As typed_node_writer_code_gens_registrar
        v.emplace_back(streams.of(names).
                       map(Function(ByVal name As String) As Action(Of code_gens(Of typed_node_writer))
                               Return code_gen.of_leaf_node(name)
                           End Function).
                       to_array())
        Return Me
    End Function
End Class

Public NotInheritable Class code_gens_registrar(Of WRITER As New)
    Inherits code_gens_registrar(Of WRITER, code_gens_registrar(Of WRITER))
End Class

Public Class code_gens_registrar(Of WRITER As New, RT As code_gens_registrar(Of WRITER, RT))
    Protected ReadOnly v As New vector(Of Action(Of code_gens(Of WRITER)))()

    Private Function this() As RT
        Return direct_cast(Of RT)(Me)
    End Function

    Public Function [with](ByVal a As Action(Of code_gens(Of WRITER))) As RT
        assert(Not a Is Nothing)
        v.emplace_back(a)
        Return this()
    End Function

    Public Function [with](Of T As code_gen(Of WRITER))(ByVal instance As T) As RT
        v.emplace_back(Sub(ByVal b As code_gens(Of WRITER))
                           assert(Not b Is Nothing)
                           b.register(instance)
                       End Sub)
        Return this()
    End Function

    Public Function [with](Of T As code_gen(Of WRITER))() As RT
        v.emplace_back(Sub(ByVal b As code_gens(Of WRITER))
                           assert(Not b Is Nothing)
                           b.register(Of T)()
                       End Sub)
        Return this()
    End Function

    Public Function with_of_only_childs(ByVal ParamArray names() As String) As RT
        v.emplace_back(streams.of(names).
                       map(Function(ByVal name As String) As Action(Of code_gens(Of WRITER))
                               Return code_gen.of_only_child(Of WRITER)(name)
                           End Function).
                       to_array())
        Return this()
    End Function

    Public Function with_of_all_childrens(ByVal ParamArray names() As String) As RT
        v.emplace_back(streams.of(names).
                       map(Function(ByVal name As String) As Action(Of code_gens(Of WRITER))
                               Return code_gen.of_all_children(Of WRITER)(name)
                           End Function).
                       to_array())
        Return this()
    End Function

    Public Shared Widening Operator CType(ByVal this As code_gens_registrar(Of WRITER, RT)) _
                                         As vector(Of Action(Of code_gens(Of WRITER)))
        assert(Not this Is Nothing)
        Return this.v
    End Operator
End Class
