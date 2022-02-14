
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
            Return Me
        End Function

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
              map(Function(ByVal node As typed_node) As typed_node
                      assert(Not node Is Nothing)
                      node = node.child()
                      If node.type_name.Equals("virtual-function") OrElse
                         node.type_name.Equals("override-function") Then
                          Return node.child(1)
                      End If
                      Return node
                  End Function).
              foreach(Sub(ByVal node As typed_node)
                          assert(Not node Is Nothing)
                          assert(node.child_count() = 5 OrElse node.child_count() = 6)
                          If node.child(1).input().Equals("construct") Then
                              has_constructor = True
                          ElseIf node.child(1).input().Equals("destruct") Then
                              has_destructor = True
                          End If
                          ' No namespace is necessary, the first parameter contains namespace.
                          Dim o As New stringbuilder()
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
                      End Sub)
            Return Me
        End Function

        Private Function check_vars_duplicate() As Boolean
            Dim c As unordered_map(Of String, UInt32) =
                _vars.stream().
                      map(Function(ByVal p As builders.parameter) As String
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

        Public Function check() As Boolean
            Return check_vars_duplicate()
        End Function

        Public NotInheritable Class function_def
            Public Enum type_t
                pure
                virtual
                override
            End Enum

            Public ReadOnly signature As vector(Of name_with_namespace)
            Public ReadOnly type As type_t
            Public ReadOnly content As String

            Public Sub New(ByVal signature As vector(Of name_with_namespace),
                           ByVal type As type_t,
                           ByVal content As String)
                assert(Not signature.null_or_empty())
                assert(Not content.null_or_whitespace())
                Me.signature = signature
                Me.type = type
                Me.content = content
            End Sub
        End Class
    End Class
End Class
