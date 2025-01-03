
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.service.automata

Public Interface code_gen(Of WRITER As New)
    Function build(ByVal n As typed_node, ByVal o As WRITER) As Boolean
End Interface

Partial Public NotInheritable Class code_gens(Of WRITER As New)
    Private ReadOnly m As New unordered_map(Of String, Object)()

    Public Shared Function code_gen_name(Of T)() As String
        Return type_info(Of T).name_without_generic_arity.TrimStart("_"c).Replace("_"c, "-"c)
    End Function

    Public Sub register(ByVal s As String, ByVal b As Object)
        assert(Not s.null_or_whitespace())
        assert(Not b Is Nothing)
        assert(m.emplace(s, b).second(), s)
    End Sub

    ' Limit the use of this function, prefer code_gen_proxy.dump if possible.
    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Function typed(Of T)(ByVal name As String) As T
        Dim it As unordered_map(Of String, Object).iterator = m.find(name)
        assert(it <> m.end(), "Cannot find code_gen of ", name)
        Return direct_cast(Of T)((+it).second)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function typed(Of T)(ByVal n As typed_node) As T
        assert(Not n Is Nothing)
        Return typed(Of T)(n.type_name)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Function code_gen_of(ByVal name As String) As code_gen(Of WRITER)
        Return typed(Of code_gen(Of WRITER))(name)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function [of](ByVal n As typed_node) As code_gen_proxy
        assert(Not n Is Nothing)
        Return New code_gen_proxy(code_gen_of(n.type_name), n)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function of_all_children(ByVal n As typed_node) As code_gen_all_children_proxy
        Return New code_gen_all_children_proxy(Me, n)
    End Function

    Public Structure code_gen_proxy
        Private ReadOnly b As code_gen(Of WRITER)
        Private ReadOnly n As typed_node

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Sub New(ByVal b As code_gen(Of WRITER), ByVal n As typed_node)
            assert(Not b Is Nothing)
            assert(Not n Is Nothing)
            Me.b = b
            Me.n = n
        End Sub

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function build(ByVal o As WRITER) As Boolean
            If b.build(n, o) Then
                Return True
            End If
            ' Only log the failed nodes not same as its child node.
            If n.child_count() <> 1 Then
                raise_error(error_type.user, "Failed to build node ", n.input())
            End If
            Return False
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function build() As Boolean
            Return build(Nothing)
        End Function

        ' TODO: Consider to avoid using ToString(), use lazy_list_writer.str() instead.
        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function dump(ByRef o As String) As Boolean
            Dim w As New WRITER()
            If Not build(w) Then
                Return False
            End If
            o = w.ToString()
            Return True
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function dump() As String
            Dim r As String = Nothing
            assert(dump(r))
            Return r
        End Function
    End Structure

    Public NotInheritable Class code_gen_all_children_proxy
        Private ReadOnly l As code_gens(Of WRITER)
        Private ReadOnly n As typed_node

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Sub New(ByVal l As code_gens(Of WRITER), ByVal n As typed_node)
            assert(Not l Is Nothing)
            assert(Not n Is Nothing)
            Me.l = l
            Me.n = n
        End Sub

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function build(ByVal o As WRITER) As Boolean
            Dim i As UInt32 = 0
            While i < n.child_count()
                If Not l.of(n.child(i)).build(o) Then
                    Return False
                End If
                i += uint32_1
            End While
            Return True
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function dump(ByRef o As vector(Of String)) As Boolean
            o.renew()
            Dim i As UInt32 = 0
            While i < n.child_count()
                Dim s As String = Nothing
                If Not l.of(n.child(i)).dump(s) Then
                    raise_error(error_type.warning, "Failed to dump ", n.child(i).input())
                    Return False
                End If
                o.emplace_back(s)
                i += uint32_1
            End While
            Return True
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function dump() As vector(Of String)
            Dim r As vector(Of String) = Nothing
            assert(dump(r))
            Return r
        End Function
    End Class
End Class

