
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Text
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.delegates
Imports osi.root.formation
Imports osi.service.automata
Imports builders = osi.service.compiler.logic.builders

Partial Public Class scope(Of WRITER As {lazy_list_writer, New},
                              __BUILDER As func_t(Of String, WRITER, Boolean),
                              __CODE_GENS As func_t(Of code_gens(Of WRITER)),
                              T As scope(Of WRITER, __BUILDER, __CODE_GENS, T))
    Partial Public NotInheritable Class class_def
        Public Const construct As String = "construct"
        Public Const destruct As String = "destruct"
        Private ReadOnly name As name_with_namespace
        ' The type-name pair directly passes to bstyle/struct.
        Private ReadOnly _vars As New vector(Of builders.parameter)()
        Private ReadOnly _funcs As New vector(Of function_def)()
        Private ReadOnly _temps As New vector(Of tuple(Of String, function_def))()

        Public Sub New(ByVal name As String)
            Me.name = name_with_namespace.of(name)
        End Sub

        Public Function inherit_from(ByVal other As class_def) As class_def
            assert(Not other Is Nothing)
            _vars.emplace_back(other._vars)
            inherit_non_overrides(other)
            inherit_overrides(other)
            Return Me
        End Function

        Private Function forward_to(ByVal other As class_def, ByVal f As function_def) As function_def
            assert(Not other Is Nothing)
            assert(Not f Is Nothing)
            f = f.with_class(Me)
            scope(Of T).current().call_hierarchy().to(f.name().in_global_namespace())
            Return f.with_content(f.declaration() + "{" + f.forward_to(other) + "}")
        End Function

        Private Function forward_to(ByVal other As class_def) As Func(Of function_def, function_def)
            Return Function(ByVal f As function_def) As function_def
                       Return forward_to(other, f)
                   End Function
        End Function

        Private Function forward_with_temp_to(ByVal other As class_def) _
                         As Func(Of tuple(Of String, function_def), tuple(Of String, function_def))
            Return Function(ByVal p As tuple(Of String, function_def)) As tuple(Of String, function_def)
                       Return tuple.emplace_of(p.first, forward_to(other, p.second))
                   End Function
        End Function

        Private Shared Function filter_non_overrides(ByVal f As function_def) As Boolean
            assert(Not f Is Nothing)
            ' Never directly forward constructor and destructor.
            Return Not f.is_virtual() AndAlso
                   Not f.name().name().Equals(construct) AndAlso
                   Not f.name().name().Equals(destruct)
        End Function

        Private Sub inherit_non_overrides(ByVal other As class_def)
            assert(Not other Is Nothing)
            _funcs.emplace_back(other.funcs().
                                      filter(AddressOf filter_non_overrides).
                                      map(forward_to(other)).
                                      collect_to(Of vector(Of function_def))())
            _temps.emplace_back(other.temps().
                                      filter(Function(ByVal p As tuple(Of String, function_def)) As Boolean
                                                 Return filter_non_overrides(p.second)
                                             End Function).
                                      map(forward_with_temp_to(other)).
                                      collect_to(Of vector(Of tuple(Of String, function_def)))())
        End Sub

        Private Sub inherit_overrides(ByVal other As class_def)
            assert(Not other Is Nothing)
            _funcs.emplace_back(other.funcs().
                                      filter(Function(ByVal f As function_def) As Boolean
                                                 assert(Not f Is Nothing)
                                                 Return f.is_virtual()
                                             End Function).
                                      except(funcs().filter(Function(ByVal f As function_def) As Boolean
                                                                assert(Not f Is Nothing)
                                                                Return f.is_override()
                                                            End Function)).
                                      map(forward_to(other)).
                                      collect_to(Of vector(Of function_def))())
            _temps.emplace_back(other.temps().
                                      filter(Function(ByVal p As tuple(Of String, function_def)) As Boolean
                                                 assert(Not p.second Is Nothing)
                                                 Return p.second.is_virtual()
                                             End Function).
                                      except(temps().filter(
                                                 Function(ByVal p As tuple(Of String, function_def)) As Boolean
                                                     assert(Not p.second Is Nothing)
                                                     Return p.second.is_override()
                                                 End Function)).
                                      map(forward_with_temp_to(other)).
                                      collect_to(Of vector(Of tuple(Of String, function_def)))())
        End Sub

        Public Function vars() As stream(Of builders.parameter)
            Return _vars.stream()
        End Function

        Public Function funcs() As stream(Of function_def)
            Return _funcs.stream()
        End Function

        Public Function temps() As stream(Of tuple(Of String, function_def))
            Return _temps.stream()
        End Function

        Public Function with_var(ByVal p As builders.parameter) As class_def
            assert(Not p Is Nothing)
            assert(Not p.ref)
            _vars.emplace_back(p)
            Return Me
        End Function

        Private Function with_func(ByVal f As function_def) As class_def
            assert(Not f Is Nothing)
            _funcs.emplace_back(f)
            Return Me
        End Function

        Private Function parse_function(ByVal node As typed_node,
                                        ByVal type As function_def.type_t,
                                        ByVal ignored_types As unordered_set(Of String)) As function_def
            assert(Not node Is Nothing)
            Dim signature As New vector(Of name_with_namespace)()
            signature.emplace_back(function_def.name_of(node.child(1).input()))
            Dim param_names As New vector(Of String)()
            If node.child_count() = 6 Then
                For i As UInt32 = 0 To node.child(3).child_count() - uint32_1
                    Dim p As typed_node = node.child(3).child(i)
                    If i < node.child(3).child_count() - uint32_1 Then
                        p = p.child(0)
                    End If
                    assert(p.type_name.Equals("param"))
                    signature.emplace_back(function_def.type_of(p.child(0).input_without_ignored()))
                    param_names.emplace_back(p.child(1).input())
                Next
            End If
            Dim f As New function_def(Me,
                                      function_def.type_of(node.child(0).input_without_ignored()),
                                      signature,
                                      type,
                                      "// This content should never be used.")
            Return f.with_content(f.declaration(param_names, ignored_types) + node.last_child().input())
        End Function

        Private Shared Function parse_class_function(ByVal node As typed_node) _
                                As tuple(Of typed_node, function_def.type_t)
            assert(Not node Is Nothing)
            node = node.child()
            If node.type_name.Equals("overridable-function") Then
                Return tuple.of(node.child(1), function_def.type_t.overridable)
            End If
            If node.type_name.Equals("override-function") Then
                Return tuple.of(node.child(1), function_def.type_t.override)
            End If
            Return tuple.of(node, function_def.type_t.pure)
        End Function

        Public Function with_funcs(ByVal n As typed_node) As class_def
            assert(Not n Is Nothing)
            Dim has_constructor As Boolean = False
            Dim has_destructor As Boolean = False
            n.children_of("class-function").
              stream().
              map(AddressOf parse_class_function).
              foreach(Sub(ByVal t As tuple(Of typed_node, function_def.type_t))
                          Dim node As typed_node = t.first()
                          assert(Not node Is Nothing)
                          assert(node.child_count() = 5 OrElse node.child_count() = 6)
                          If node.child(1).input().Equals(construct) Then
                              has_constructor = True
                          ElseIf node.child(1).input().Equals(destruct) Then
                              has_destructor = True
                          End If
                          with_func(parse_function(node, t.second(), Nothing))
                      End Sub)
            If Not has_constructor Then
                with_func(New function_def(
                              Me,
                              function_def.type_of("void"),
                              function_def.name_of(construct),
                              function_def.type_t.pure,
                              New StringBuilder().
                                  Append("void ").
                                  Append(current_namespace_t.in_global_namespace(construct)).
                                  Append("(").
                                  Append(name.name()).
                                  Append("& this){}").ToString()))
            End If
            If Not has_destructor Then
                with_func(New function_def(
                              Me,
                              function_def.type_of("void"),
                              function_def.name_of(destruct),
                              function_def.type_t.pure,
                              New StringBuilder().
                                  Append("void ").
                                  Append(current_namespace_t.in_global_namespace(destruct)).
                                  Append("(").
                                  Append(name.name()).
                                  Append("& this){}").ToString()))
            End If

            n.children_of("class-template-function").
              stream().
              map(Function(ByVal node As typed_node) As tuple(Of typed_node, typed_node, function_def.type_t)
                      assert(Not node Is Nothing)
                      assert(node.child_count() = 2)
                      Dim f As tuple(Of typed_node, function_def.type_t) = parse_class_function(node.child(1))
                      Return tuple.emplace_of(node.child(0), f.first(), f.second())
                  End Function).
              foreach(Sub(ByVal t As tuple(Of typed_node, typed_node, function_def.type_t))
                          Dim template_types As unordered_set(Of String) =
                                  unordered_set.emplace_of(+code_gens().of_all_children(t._1().child(2)).dump())
                          _temps.emplace_back(tuple.of(t._1().input(), parse_function(t._2(), t._3(), template_types)))
                      End Sub)
            Return Me
        End Function

        Private Function check_vars_duplicate() As Boolean
            Dim c As unordered_map(Of String, UInt32) =
                vars().map(Function(ByVal p As builders.parameter) As String
                               assert(Not p Is Nothing)
                               Return p.name
                           End Function).
                       count().
                       filter(Function(ByVal t As tuple(Of String, UInt32)) As Boolean
                                  assert(t.second() <= 2 AndAlso t.second() > 0)
                                  Return t.second() > 1
                              End Function).
                       map(AddressOf tuple.to_first_const_pair).
                       collect_to(Of unordered_map(Of String, UInt32))()
            If Not c.empty() Then
                raise_error(error_type.user, "Duplicate variable in ", name, ": ", c)
                Return False
            End If
            Return True
        End Function

        Private Function check_funcs_duplicate() As Boolean
            Dim c As unordered_map(Of function_def, UInt32) =
                funcs().count().
                        filter(Function(ByVal t As tuple(Of function_def, UInt32)) As Boolean
                                   assert(t.second() <= 2 AndAlso t.second() > 0)
                                   Return t.second() > 1
                               End Function).
                        map(AddressOf tuple.to_first_const_pair).
                        collect_to(Of unordered_map(Of function_def, UInt32))()
            If Not c.empty() Then
                raise_error(error_type.user, "Duplicate function in ", name, ": ", c)
                Return False
            End If
            Return True
        End Function

        Public Function check() As Boolean
            Return check_vars_duplicate() AndAlso check_funcs_duplicate()
        End Function
    End Class
End Class
