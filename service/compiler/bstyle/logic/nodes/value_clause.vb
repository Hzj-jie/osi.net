
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.service.automata
Imports osi.service.compiler.logic
Imports osi.service.constructor

Partial Public NotInheritable Class bstyle
    Public NotInheritable Class value_clause
        Inherits logic_gen_wrapper
        Implements logic_gen

        <inject_constructor>
        Public Sub New(ByVal b As logic_gens)
            MyBase.New(b)
        End Sub

        Public Shared Sub register(ByVal b As logic_gens)
            assert(Not b Is Nothing)
            b.register(Of value_clause)()
        End Sub

        Public Function build(ByVal name As typed_node,
                              ByVal value As typed_node,
                              ByVal struct_copy As Func(Of vector(Of String), Boolean),
                              ByVal single_data_slot_copy As Func(Of String, Boolean),
                              ByVal o As writer) As Boolean
            assert(Not name Is Nothing)
            assert(Not value Is Nothing)
            assert(Not struct_copy Is Nothing)
            assert(Not single_data_slot_copy Is Nothing)
            assert(Not o Is Nothing)
            If Not l.of(value).build(o) Then
                Return False
            End If
            Dim type As String = Nothing
            assert(scope.current().variables().resolve(name.word().str(), type))
            If scope.current().structs().defined(type) Then
                Using r As read_scoped(Of value.target).ref = l.typed_code_gen(Of value)().read_target()
                    If Not (+r).type.Equals(type) Then
                        raise_error(error_type.user,
                                    "Type ",
                                    type,
                                    " of ",
                                    name.word().str(),
                                    " does not match the rvalue ",
                                    (+r).type)
                        Return False
                    End If
                    Return struct_copy((+r).names)
                End Using
            Else
                Using r As read_scoped(Of value.target).ref(Of value.single_data_slot_target) =
                        l.typed_code_gen(Of value)().read_target_single_data_slot()
                    ' The type check of single-data-slot-target will be handled by logic.
                    Return single_data_slot_copy((+r).name)
                End Using
            End If
        End Function

        Public Function build(ByVal name As typed_node, ByVal value As typed_node, ByVal o As writer) As Boolean
            assert(Not name Is Nothing)
            assert(name.leaf())
            ' TODO: If the value on the right is a temporary value (rvalue), move can be used to reduce memory copy.
            Return build(name,
                         value,
                         Function(ByVal r As vector(Of String)) As Boolean
                             Return struct.copy(r, name.word().str(), o)
                         End Function,
                         Function(ByVal r As String) As Boolean
                             Return builders.of_copy(name.word().str(), r).to(o)
                         End Function,
                         o)
        End Function

        Public Function build(ByVal n As typed_node, ByVal o As writer) As Boolean Implements logic_gen.build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(n.child_count() = 3)
            assert(n.child(0).leaf())
            Return build(n.child(0), n.child(2), o)
        End Function
    End Class
End Class
