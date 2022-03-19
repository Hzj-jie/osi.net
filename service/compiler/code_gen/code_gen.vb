
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.service.automata
Imports osi.service.constructor

Public Interface code_gen(Of WRITER As New)
    Function build(ByVal n As typed_node, ByVal o As WRITER) As Boolean
End Interface

Partial Public NotInheritable Class code_gens(Of WRITER As New)
    Private ReadOnly m As New unordered_map(Of String, code_gen(Of WRITER))()

    Private Shared Function code_gen_name(Of T As code_gen(Of WRITER))() As String
        Return GetType(T).Name().TrimStart("_"c).Replace("_"c, "-"c)
    End Function

    Public Sub register(ByVal s As String, ByVal b As code_gen(Of WRITER))
        assert(Not s.null_or_whitespace())
        assert(Not b Is Nothing)
        assert(m.emplace(s, b).second(), s)
    End Sub

    Public Sub register(Of T As code_gen(Of WRITER))(ByVal b As T)
        register(code_gen_name(Of T)(), b)
    End Sub

    Public Sub register(Of T As code_gen(Of WRITER))(ByVal name As String)
        register(name, inject_constructor(Of T).invoke(Me))
    End Sub

    Public Sub register(Of T As code_gen(Of WRITER))()
        register(Of T)(code_gen_name(Of T)())
    End Sub

    Private Function code_gen_of(ByVal name As String) As code_gen(Of WRITER)
        Dim it As unordered_map(Of String, code_gen(Of WRITER)).iterator = m.find(name)
        assert(it <> m.end(), "Cannot find code_gen of ", name)
        Return (+it).second
    End Function

    Private Function code_gen_of(ByVal n As typed_node) As code_gen(Of WRITER)
        assert(Not n Is Nothing)
        Return code_gen_of(n.type_name)
    End Function

    ' Limit the use of this function, prefer code_gen_proxy.dump if possible.
    Public Function typed(Of T As code_gen(Of WRITER))() As T
        Return direct_cast(Of T)(code_gen_of(code_gen_name(Of T)()))
    End Function

    Public Function [of](ByVal n As typed_node) As code_gen_proxy
        Return New code_gen_proxy(code_gen_of(n), n)
    End Function

    Public Function of_all_children(ByVal n As typed_node) As code_gen_all_children_proxy
        Return New code_gen_all_children_proxy(Me, n)
    End Function

    Public Structure code_gen_proxy
        Private ReadOnly b As code_gen(Of WRITER)
        Private ReadOnly n As typed_node

        Public Sub New(ByVal b As code_gen(Of WRITER), ByVal n As typed_node)
            assert(Not b Is Nothing)
            assert(Not n Is Nothing)
            Me.b = b
            Me.n = n
        End Sub

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function build(ByVal o As WRITER) As Boolean
            Return b.build(n, o)
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function build() As Boolean
            Return build(Nothing)
        End Function

        ' TODO: Consider to avoid using ToString(), use lazy_list_writer.str() instead.
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

        Public Sub New(ByVal l As code_gens(Of WRITER), ByVal n As typed_node)
            assert(Not l Is Nothing)
            assert(Not n Is Nothing)
            Me.l = l
            Me.n = n
        End Sub

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

        Public Function dump(ByRef o As vector(Of String)) As Boolean
            o.renew()
            Dim i As UInt32 = 0
            While i < n.child_count()
                Dim s As String = Nothing
                If Not l.of(n.child(i)).dump(s) Then
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

