
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
    Public NotInheritable Class class_def
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
            inherit_non_existing_funcs(other)
            Return Me
        End Function

        Private Function forward_to(ByVal other As class_def, ByVal f As function_def) As String
            assert(Not other Is Nothing)
            assert(Not f Is Nothing)
            Dim content As New StringBuilder()
            content.Append("reinterpret_cast(this,").
                    Append(other.name.in_global_namespace()).
                    Append(");")
            ' TODO: A better way to check the return type.
            If Not f.return_type.name().Equals("void") Then
                content.Append("return ")
            End If
            content.Append(f.name().in_global_namespace()).
                    Append("(this")
            For i As Int32 = 2 To CInt(f.signature.size()) - 1
                content.Append(",").
                        Append("i").
                        Append(i - 2)
            Next
            content.Append(");")
            Return content.ToString()
        End Function

        Private Sub inherit_non_existing_funcs(ByVal other As class_def)
            assert(Not other Is Nothing)
            _funcs.emplace_back(other.funcs().
                                      except(funcs()).
                                      map(Function(ByVal f As function_def) As function_def
                                              assert(Not f Is Nothing)
                                              Dim content As New StringBuilder()
                                              content.Append(f.return_type.in_global_namespace()).
                                                      Append(" ").
                                                      Append(f.name().in_global_namespace()).
                                                      Append("(").
                                                      Append(name.in_global_namespace()).
                                                      Append("& this")
                                              For i As Int32 = 2 To CInt(f.signature.size()) - 1
                                                  content.Append(",").
                                                          Append(f.signature(CUInt(i)).in_global_namespace()).
                                                          Append("i").
                                                          Append(i - 2)
                                              Next
                                              content.Append("){").
                                                      Append(forward_to(other, f)).
                                                      Append("}")
                                              Return f.with_content(content.ToString())
                                          End Function).
                                      collect(Of vector(Of function_def))())
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
                      If node.type_name.Equals("virtual-function") Then
                          Return tuple.of(node.child(1), function_def.type_t.virtual)
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
                          If node.child(1).input().Equals("construct") Then
                              has_constructor = True
                          ElseIf node.child(1).input().Equals("destruct") Then
                              has_destructor = True
                          End If
                          ' No namespace is necessary, the first parameter contains namespace.
                          Dim o As New StringBuilder()
                          o.Append(node.child(0).input()).
                            Append(" ").
                            Append(_namespace.with_global_namespace(node.child(1).input())).
                            Append("(").
                            Append(name.name()).
                            Append("& this")
                          ' With parameter list.
                          If node.child_count() = 6 Then
                              o.Append(", ").
                                Append(node.child(3).input())
                          End If
                          o.Append(")").
                            Append(node.last_child().input()).
                            AppendLine() ' beautiful output.
                          Dim signature As New vector(Of name_with_namespace)()
                          signature.emplace_back(function_def.name_of(node.child(1).input()))
                          If node.child_count() = 6 Then
                              For i As UInt32 = 0 To node.child(3).child_count() - uint32_1
                                  Dim p As typed_node = node.child(3).child(i)
                                  If i < node.child(3).child_count() - uint32_1 Then
                                      p = p.child(0)
                                  End If
                                  assert(p.type_name.Equals("param"))
                                  signature.emplace_back(function_def.type_of(p.child(0).input_without_ignored()))
                              Next
                          End If
                          with_func(New function_def(function_def.type_of(node.child(0).input_without_ignored()),
                                                     signature,
                                                     t.second(),
                                                     o.ToString()))
                      End Sub)
            If Not has_constructor Then
                with_func(New function_def(function_def.type_of("void"),
                                           function_def.name_of("construct"),
                                           function_def.type_t.pure,
                                           New StringBuilder().Append("void ").
                                                               Append(_namespace.with_global_namespace("construct")).
                                                               Append("(").
                                                               Append(name.name()).
                                                               Append("& this){}").ToString()))
            End If
            If Not has_destructor Then
                with_func(New function_def(function_def.type_of("void"),
                                           function_def.name_of("destruct"),
                                           function_def.type_t.pure,
                                           New StringBuilder().Append("void ").
                                                               Append(_namespace.with_global_namespace("destruct")).
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
                       collect(Of unordered_map(Of String, UInt32))()
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
                        collect(Of unordered_map(Of function_def, UInt32))()
            If Not c.empty() Then
                raise_error(error_type.user, "Duplicate function in ", name, ": ", c)
                Return False
            End If
            Return True
        End Function

        Public Function check() As Boolean
            Return check_vars_duplicate() AndAlso check_funcs_duplicate()
        End Function

        Public NotInheritable Class function_def
            Implements IEquatable(Of function_def)

            Public Enum type_t
                pure
                virtual
                override
            End Enum

            Public ReadOnly return_type As name_with_namespace
            Public ReadOnly signature As vector(Of name_with_namespace)
            Public ReadOnly type As type_t
            Public ReadOnly content As String

            Public Function name() As name_with_namespace
                Return signature(0)
            End Function

            Public Shared Function name_of(ByVal name As String) As name_with_namespace
                Return name_with_namespace.of_global_namespace(name)
            End Function

            Public Shared Function type_of(ByVal type As String) As name_with_namespace
                Return name_with_namespace.of(type)
            End Function

            Public Sub New(ByVal return_type As name_with_namespace,
                           ByVal signature As vector(Of name_with_namespace),
                           ByVal type As type_t,
                           ByVal content As String)
                assert(Not signature.null_or_empty())
                assert(Not content.null_or_whitespace())
                Me.return_type = return_type
                Me.signature = signature
                Me.type = type
                Me.content = content
            End Sub

            Public Sub New(ByVal return_type As name_with_namespace,
                           ByVal name As name_with_namespace,
                           ByVal type As type_t,
                           ByVal content As String)
                Me.New(return_type, vector.emplace_of(name), type, content)
            End Sub

            Public Function with_content(ByVal content As String) As function_def
                Return New function_def(return_type, signature, type, content)
            End Function

            Public Overrides Function Equals(ByVal obj As Object) As Boolean
                Return Equals(direct_cast(Of function_def)(obj, False))
            End Function

            Public Overloads Function Equals(ByVal other As function_def) As Boolean _
                                            Implements IEquatable(Of function_def).Equals
                If other Is Nothing Then
                    Return False
                End If
                Return signature.Equals(other.signature)
            End Function

            Public Overrides Function GetHashCode() As Int32
                Return signature.GetHashCode()
            End Function

            Public Overrides Function ToString() As String
                Return strcat("{", signature, ": ", return_type, "}")
            End Function
        End Class
    End Class
End Class
