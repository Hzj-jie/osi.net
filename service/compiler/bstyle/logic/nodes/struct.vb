
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.service.automata
Imports osi.service.compiler.logic

Partial Public NotInheritable Class bstyle
    Public NotInheritable Class struct
        Implements logic_gen

        Private NotInheritable Class type_annotation
            Public ReadOnly type As String
            Public ReadOnly name As String

            Public Sub New(ByVal type As String, ByVal name As String)
                assert(Not type.null_or_whitespace())
                assert(Not name.null_or_whitespace())
                Me.type = type
                Me.name = name
            End Sub
        End Class

        Private Shared ReadOnly root_types As unordered_set(Of String) = unordered_set.of(
            types.zero_type,
            types.variable_type,
            code_types.biguint,
            code_types.bool,
            code_types.int,
            code_types.string,
            code_types.ufloat
        )
        Private ReadOnly s As New unordered_map(Of String, vector(Of type_annotation))()

        Public Shared Sub register(ByVal b As logic_gens, ByVal p As parameters_t)
            assert(Not b Is Nothing)
            b.register(New struct(p))
        End Sub

        Private ReadOnly ta As type_alias

        Private Sub New(ByVal p As parameters_t)
            assert(Not p Is Nothing)
            ta = p.type_alias
            assert(Not ta Is Nothing)
        End Sub

        Private Function resolve_type(ByVal type As String,
                                      ByVal name As String,
                                      ByVal o As vector(Of type_annotation)) As Boolean
            type = ta(type)
            Dim sub_type As vector(Of type_annotation) = Nothing
            If root_types.find(type) <> root_types.end() OrElse Not s.find(type, sub_type) Then
                o.emplace_back(New type_annotation(type, name))
                Return True
            End If
            assert(Not sub_type Is Nothing)
            Dim i As UInt32 = 0
            While i < sub_type.size()
                If Not resolve_type(sub_type(i).type, strcat(name, ".", sub_type(i).name), o) Then
                    Return False
                End If
                i += uint32_1
            End While
            Return True
        End Function

        ' TODO: Support value_definition
        Private Function resolve_type(ByVal c As typed_node, ByVal o As vector(Of type_annotation)) As Boolean
            assert(Not c Is Nothing)
            assert(Not o Is Nothing)
            assert(c.child_count() = 2)
            assert(c.child(0).child_count() = 2)
            Return resolve_type(c.child(0).child(0).word().str(), c.child(0).child(1).word().str(), o)
        End Function

        Public Function export(ByVal n As typed_node, ByVal o As writer) As Boolean
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            If n.child_count() > 2 Then
                ' TODO: Support value-definition
                Return False
            End If
            assert(n.child_count() = 2)
            Dim type As String = n.child(0).word().str()
            Dim v As vector(Of type_annotation) = Nothing
            If Not s.find(type, v) Then
                Return False
            End If
            assert(Not v Is Nothing)
            Dim i As UInt32 = 0
            While i < v.size()
                builders.of_define(strcat(n.child(1).word().str(), ".", v(i).name), v(i).type).to(o)
                i += uint32_1
            End While
            Return True
        End Function

        Public Function build(ByVal n As typed_node,
                              ByVal o As writer) As Boolean Implements code_gen(Of writer).build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(n.child_count() >= 5)
            Dim types As New vector(Of type_annotation)()
            For i As UInt32 = 3 To n.child_count() - CUInt(3)
                If Not resolve_type(n.child(i), types) Then
                    Return False
                End If
            Next
            Dim type_name As String = n.child(1).word().str()
            If s.emplace(type_name, types).second() Then
                Return True
            End If
            raise_error(error_type.user, "Struct type ", type_name, " has been defined already as: ", s(type_name))
            Return False
        End Function
    End Class
End Class
