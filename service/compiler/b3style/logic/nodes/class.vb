
'The following code is generated by /osi/root/codegen/sed/sed.exe
'by replacing
'  typed_node_writer
'into
'  logic_writer
'from the input file Console.In
'Do not edit it manually.

'The following code is generated by /osi/root/codegen/sed/sed.exe
'by replacing
'  Class b2style
'into
'  Class b3style
'from the input file ..\..\..\b2style\rewriter\nodes\class.vb
'Do not edit it manually.

Option Explicit On
Option Infer Off
Option Strict On

Imports System.Text
Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.automata
Imports osi.service.compiler.rewriters
Imports builders = osi.service.compiler.logic.builders


Partial Public NotInheritable Class b3style
    Private NotInheritable Class _class
        Inherits code_gens(Of logic_writer).reparser
        Implements code_gen(Of logic_writer), scope.template_t.name_node, scope.template_t.name

        Private Function name_node_of(ByVal n As typed_node, ByRef o As typed_node) As Boolean _
                                     Implements scope.template_t.name_node.of
            assert(Not n Is Nothing)
            o = n.child(1)
            Return True
        End Function

        Private Function name_of(ByVal n As typed_node, ByRef o As String) As Boolean _
                                Implements scope.template_t.name.of
            o = scope.template_t.name_of(n)
            Return True
        End Function

        Protected Overrides Function parse(ByVal s As String, ByVal o As logic_writer) As Boolean
            Return code_builder.build(s, o)
        End Function

        Protected Overrides Function dump(ByVal n As typed_node, ByRef s As String) As Boolean
            assert(Not n Is Nothing)
            assert(n.child_count() >= 5)
            Dim class_name As String = n.child(1).input()
            Dim cd As New scope.class_def(class_name)
            bstyle.struct.parse_struct_body(n).foreach(AddressOf cd.with_var)
            cd.with_funcs(n)
            If n.child(2).type_name.Equals("class-inheritance") AndAlso
               Not code_gens().of_all_children(n.child(2).child(1)).
                     dump().
                     stream().
                     with_index().
                     map(Function(ByVal t As tuple(Of UInt32, String)) As Boolean
                             Dim bcd As scope.class_def = Nothing
                             If Not scope.current().classes().resolve(t.second(), bcd) Then
                                 Return False
                             End If
                             cd.inherit_from(bcd).
                                with_var(scope.struct_t.create_type_id(t.second()))
                             Return True
                         End Function).
                     aggregate(bool_stream.aggregators.all_true) Then
                Return False
            End If
            If Not scope.current().classes().define(class_name, cd) Then
                Return False
            End If
            Dim o As New StringBuilder()
            ' Append struct-body back into the structure.
            o.Append("struct ").
              Append(class_name).
              Append("{")
            cd.vars().
               foreach(Sub(ByVal var As builders.parameter)
                           assert(Not var Is Nothing)
                           o.Append(var.non_ref_type()).
                             Append(" ").
                             Append(var.name).
                             Append(";")
                       End Sub)
            o.Append("};")
            cd.funcs().
               foreach(Sub(ByVal f As scope.class_def.function_def)
                           assert(Not f Is Nothing)
                           o.Append(f.content)
                       End Sub)
            cd.temps().
               foreach(Sub(ByVal f As tuple(Of String, scope.class_def.function_def))
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
