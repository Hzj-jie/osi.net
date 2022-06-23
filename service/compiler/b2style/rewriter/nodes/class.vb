
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Text
Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.automata
Imports osi.service.compiler.logic
Imports osi.service.compiler.rewriters

Partial Public NotInheritable Class b2style
    Private NotInheritable Class _class
        Inherits code_gens(Of typed_node_writer).reparser
        Implements code_gen(Of typed_node_writer), template.name_node, template.name

        Public Sub New()
            MyBase.New(parser.instance)
        End Sub

        Private Function name_node_of(ByVal n As typed_node) As typed_node Implements template.name_node.of
            assert(Not n Is Nothing)
            Return n.child(1)
        End Function

        Private Function [of](ByVal n As typed_node) As String Implements template.name.of
            Return template.name_of(template.name_node_of(n), template.type_param_count(n))
        End Function

        Protected Overrides Function dump(ByVal n As typed_node, ByRef s As String) As Boolean
            Dim o As New StringBuilder()
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(n.child_count() >= 5)
            Dim class_name As String = n.child(1).input()
            Dim cd As New class_def(class_name)
            bstyle.struct.parse_struct_body(n).foreach(AddressOf cd.with_var)
            cd.with_funcs(n)
            Dim classes As scope.class_proxy = scope.current().classes()
            If n.child(2).type_name.Equals("class-inheritance") AndAlso
               Not code_gens().of_all_children(n.child(2).child(1)).
                     dump().
                     stream().
                     with_index().
                     map(Function(ByVal t As tuple(Of UInt32, String)) As Boolean
                             Dim bcd As class_def = Nothing
                             If Not classes.resolve(t.second(), bcd) Then
                                 Return False
                             End If
                             cd.inherit_from(bcd).
                                with_var(bstyle.struct.create_id(t.second()))
                             Return True
                         End Function).
                     aggregate(bool_stream.aggregators.all_true) Then
                Return False
            End If
            If Not scope.current().classes().define(class_name, cd) Then
                Return False
            End If
            ' Append struct-body back into the structure.
            o.Append("struct ").
              Append(class_name).
              Append("{")
            cd.vars().
               foreach(Sub(ByVal var As builders.parameter)
                           assert(Not var Is Nothing)
                           o.Append(var.type).
                             Append(" ").
                             Append(var.name).
                             Append(";")
                       End Sub)
            o.Append("};")
            cd.funcs().
               foreach(Sub(ByVal f As class_def.function_def)
                           assert(Not f Is Nothing)
                           o.Append(f.content)
                       End Sub)
            cd.temps().
               foreach(Sub(ByVal f As tuple(Of String, class_def.function_def))
                           ' TODO: Avoid generating source code, directly define templates to allow including type
                           ' names.
                           assert(Not f.first().null_or_whitespace())
                           assert(Not f.second() Is Nothing)
                           o.Append(f.first() + " " + f.second().content)
                       End Sub)
            s = o.ToString()
            Return True
        End Function
    End Class
End Class
