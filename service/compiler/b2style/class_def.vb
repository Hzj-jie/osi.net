
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Text
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.service.automata
Imports osi.service.compiler.logic

Partial Public NotInheritable Class b2style
    Partial Public NotInheritable Class class_def
        Private Const construct As String = "construct"
        Private Const destruct As String = "destruct"
        Private ReadOnly name As name_with_namespace
        ' The type-name pair directly passes to bstyle/struct.
        Private ReadOnly _vars As New vector(Of builders.parameter)()
        Private ReadOnly _funcs As New vector(Of function_def)()

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

        Private Function forward_to(ByVal other As class_def) As Func(Of function_def, function_def)
            assert(Not other Is Nothing)
            Return Function(ByVal f As function_def) As function_def
                       assert(Not f Is Nothing)
                       f = f.with_class(Me)
                       scope.current().call_hierarchy().to(f.name().in_global_namespace())
                       Return f.with_content(f.declaration() + "{" + f.forward_to(other) + "}")
                   End Function
        End Function

        Private Sub inherit_non_overrides(ByVal other As class_def)
            assert(Not other Is Nothing)
            _funcs.emplace_back(other.funcs().
                                      filter(Function(ByVal f As function_def) As Boolean
                                                 assert(Not f Is Nothing)
                                                 ' Never directly forward constructor and destructor.
                                                 Return Not f.is_virtual() AndAlso
                                                        Not f.name().name().Equals(construct) AndAlso
                                                        Not f.name().name().Equals(destruct)
                                             End Function).
                                      map(forward_to(other)).
                                      collect_to(Of vector(Of function_def))())
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
        End Sub

        Public Function vars() As stream(Of builders.parameter)
            Return _vars.stream()
        End Function

        Public Function funcs() As stream(Of function_def)
            Return _funcs.stream()
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

        Public Function with_funcs(ByVal n As typed_node) As class_def
            assert(Not n Is Nothing)
            Dim has_constructor As Boolean = False
            Dim has_destructor As Boolean = False
            n.children_of("class-function").
              stream().
              map(Function(ByVal node As typed_node) As tuple(Of typed_node, function_def.type_t)
                      assert(Not node Is Nothing)
                      node = node.child()
                      If node.type_name.Equals("overridable-function") Then
                          Return tuple.of(node.child(1), function_def.type_t.overridable)
                      End If
                      If node.type_name.Equals("override-function") Then
                          Return tuple.of(node.child(1), function_def.type_t.override)
                      End If
                      Return tuple.of(node, function_def.type_t.pure)
                  End Function).
              foreach(Sub(ByVal t As tuple(Of typed_node, function_def.type_t))
                          Dim node As typed_node = t.first()
                          assert(Not node Is Nothing)
                          assert(node.child_count() = 5 OrElse node.child_count() = 6)
                          If node.child(1).input().Equals(construct) Then
                              has_constructor = True
                          ElseIf node.child(1).input().Equals(destruct) Then
                              has_destructor = True
                          End If
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
                                                    t.second(),
                                                    "// This content should never be used.")
                          with_func(f.with_content(f.declaration(param_names) + node.last_child().input()))
                      End Sub)
            If Not has_constructor Then
                with_func(New function_def(Me,
                                           function_def.type_of("void"),
                                           function_def.name_of(construct),
                                           function_def.type_t.pure,
                                           New StringBuilder().Append("void ").
                                                               Append(_namespace.with_global_namespace(construct)).
                                                               Append("(").
                                                               Append(name.name()).
                                                               Append("& this){}").ToString()))
            End If
            If Not has_destructor Then
                with_func(New function_def(Me,
                                           function_def.type_of("void"),
                                           function_def.name_of(destruct),
                                           function_def.type_t.pure,
                                           New StringBuilder().Append("void ").
                                                               Append(_namespace.with_global_namespace(destruct)).
                                                               Append("(").
                                                               Append(name.name()).
                                                               Append("& this){}").ToString()))
            End If
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
