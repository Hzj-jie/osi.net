
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.compiler.rewriters

Public NotInheritable Class typed_node_writer_code_gens_registrar
    Inherits lazy_list_writer_code_gens_registrar(Of typed_node_writer, typed_node_writer_code_gens_registrar)

    Public Function with_of_leaf_nodes(ByVal ParamArray names() As String) As typed_node_writer_code_gens_registrar
        Return [with](streams.of(names).
                      map(Function(ByVal name As String) As Action(Of code_gens(Of typed_node_writer))
                              Return code_gen.of_leaf_node(name)
                          End Function).
                      to_array())
    End Function
End Class

Public NotInheritable Class lazy_list_writer_code_gens_registrar
    Inherits lazy_list_writer_code_gens_registrar(Of lazy_list_writer, lazy_list_writer_code_gens_registrar)
End Class

Public Class lazy_list_writer_code_gens_registrar(Of WRITER As {lazy_list_writer, New},
                                                       RT As lazy_list_writer_code_gens_registrar(Of WRITER, RT))
    Inherits code_gens_registrar(Of WRITER, RT)

    Public Function with_of_names(ByVal ParamArray names() As String) As RT
        Return [with](streams.of(names).
                      map(Function(ByVal name As String) As Action(Of code_gens(Of WRITER))
                              Return code_gen.of_name(Of WRITER)(name)
                          End Function).
                      to_array())
    End Function
End Class

Public NotInheritable Class code_gens_registrar(Of WRITER As New)
    Inherits code_gens_registrar(Of WRITER, code_gens_registrar(Of WRITER))
End Class

Public Class code_gens_registrar(Of WRITER As New, RT As code_gens_registrar(Of WRITER, RT))
    Private ReadOnly v As New vector(Of Action(Of code_gens(Of WRITER)))()

    Private Function self() As RT
        Return direct_cast(Of RT)(Me)
    End Function

    Public Function [with](ByVal actions() As Action(Of code_gens(Of WRITER))) As RT
        assert(Not actions Is Nothing)
        For Each a As Action(Of code_gens(Of WRITER)) In actions
            v.emplace_back(a)
        Next
        Return self()
    End Function

    Public Function [with](ByVal a As Action(Of code_gens(Of WRITER))) As RT
        Return [with]({a})
    End Function

    ' Keep tracking of the code_gen without really using it.
    Public Function without(Of T As New)() As RT
        Return self()
    End Function

    Public Function [with](Of T As New)() As RT
        Return [with](Sub(ByVal b As code_gens(Of WRITER))
                          assert(Not b Is Nothing)
                          b.register(code_gens(Of WRITER).code_gen_name(Of T)(), New T())
                      End Sub)
    End Function

    Public Function with_of_only_childs(ByVal ParamArray names() As String) As RT
        Return [with](streams.of(names).
                              map(Function(ByVal name As String) As Action(Of code_gens(Of WRITER))
                                      Return code_gen.of_only_child(Of WRITER)(name)
                                  End Function).
                              to_array())
    End Function

    Public Function with_of_all_childrens(ByVal ParamArray names() As String) As RT
        Return [with](streams.of(names).
                      map(Function(ByVal name As String) As Action(Of code_gens(Of WRITER))
                              Return code_gen.of_all_children(Of WRITER)(name)
                          End Function).
                      to_array())
    End Function

    Public Shared Widening Operator CType(ByVal this As code_gens_registrar(Of WRITER, RT)) _
                                         As vector(Of Action(Of code_gens(Of WRITER)))
        assert(Not this Is Nothing)
        Return this.v
    End Operator
End Class
