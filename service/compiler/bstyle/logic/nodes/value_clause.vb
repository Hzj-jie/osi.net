
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
        Implements code_gen(Of writer)

        Private ReadOnly l As code_gens(Of writer)

        <inject_constructor>
        Public Sub New(ByVal b As code_gens(Of writer))
            assert(Not b Is Nothing)
            Me.l = b
        End Sub

        Private Function build(ByVal name As typed_node,
                               ByVal value As typed_node,
                               ByVal struct_copy As Func(Of vector(Of String), Boolean),
                               ByVal single_data_slot_copy As Func(Of String, Boolean),
                               ByVal o As writer) As Boolean
            assert(Not name Is Nothing)
            assert(Not value Is Nothing)
            assert(Not struct_copy Is Nothing)
            assert(Not single_data_slot_copy Is Nothing)
            assert(Not o Is Nothing)
            Dim type As String = Nothing
            Dim is_delegate As New ref(Of Boolean)()
            If Not scope.current().variables().resolve(name.children_word_str(), type, is_delegate) Then
                ' Emmmm, scope.variable should log the error already.
                Return False
            End If
            If +is_delegate Then
                If scope.current().functions().return_type_of(value.children_word_str(), Nothing) Then
                    ' Use address-of to copy a function address to the target.
                    Return builders.of_address_of(name.children_word_str(), value.children_word_str()).to(o)
                End If
                Return builders.of_copy(name.children_word_str(), value.children_word_str()).to(o)
            End If
            If Not l.of(value).build(o) Then
                Return False
            End If
            If scope.current().structs().defined(type) Then
                Using r As read_scoped(Of scope.value_target_t.target).ref = bstyle.value.read_target()
                    If Not (+r).type.Equals(type) Then
                        raise_error(error_type.user,
                                    "Type ",
                                    type,
                                    " of ",
                                    name.children_word_str(),
                                    " does not match the rvalue ",
                                    (+r).type)
                        Return False
                    End If
                    Return struct_copy((+r).names)
                End Using
            End If
            Using r As read_scoped(Of scope.value_target_t.target).ref(Of String) =
                           bstyle.value.read_target_single_data_slot()
                ' The type check of single-data-slot-target will be handled by logic.
                Return single_data_slot_copy(+r)
            End Using
        End Function

        Public Function build(ByVal name As typed_node, ByVal value As typed_node, ByVal o As writer) As Boolean
            assert(Not name Is Nothing)
            ' TODO: If the value on the right is a temporary value (rvalue), move can be used to reduce memory copy.
            Return build(name,
                         value,
                         Function(ByVal r As vector(Of String)) As Boolean
                             Return struct.copy(r, name.children_word_str(), o)
                         End Function,
                         Function(ByVal r As String) As Boolean
                             Return builders.of_copy(name.children_word_str(), r).to(o)
                         End Function,
                         o)
        End Function

        Public Function build(ByVal n As typed_node, ByVal o As writer) As Boolean Implements code_gen(Of writer).build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(n.child_count() = 3)
            If Not n.child(0).child().type_name.Equals("heap-name") Then
                Return build(n.child(0).child(), n.child(2), o)
            End If
            Return l.typed(Of heap_name).build(
                       n.child(0).child().child(2),
                       o,
                       Function(ByVal indexstr As String) As Boolean
                           Return build(n.child(0).child().child(0),
                                        n.child(2),
                                        Function(ByVal r As vector(Of String)) As Boolean
                                            Return struct.copy(r,
                                                               n.child(0).child().child(0).children_word_str(),
                                                               indexstr,
                                                               o)
                                        End Function,
                                        Function(ByVal r As String) As Boolean
                                            Return builders.of_copy(
                                                       variable.name_of(n.child(0).child().child(0).children_word_str(),
                                                                        indexstr),
                                                   r).to(o)
                                        End Function,
                                 o)
                       End Function)
        End Function
    End Class
End Class
