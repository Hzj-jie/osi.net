
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants

Public NotInheritable Class free_list(Of T)
    Private ReadOnly v As vector(Of T)
    Private ReadOnly f As unordered_set(Of UInt32)

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub New()
        v = New vector(Of T)()
        f = New unordered_set(Of UInt32)()
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub clear()
        v.clear()
        v.shrink_to_fit()
        f.clear()
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function free_pool_size() As UInt32
        Return f.size()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function pool_size() As UInt32
        Return v.size()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function emplace(ByVal d As T) As UInt32
        Dim r As UInt32 = uint32_0
        If f.empty() Then
            v.emplace_back(d)
            r = v.size() - uint32_1
        Else
            r = +(f.begin())
            f.erase(f.begin())
            v.replace(r, d)
        End If
        Return r
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function push(ByVal d As T) As UInt32
        Return emplace(copy_no_error(d))
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub [erase](ByVal p As UInt32)
        v.replace(p, [default](Of T).null)
        f.emplace(p)
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function size() As UInt32
        Dim r As UInt32 = uint32_0
        Dim x As Int64 = CLng(v.size()) - f.size()
        assert(x >= 0)
        assert(x <= max_uint32)
        r = CUInt(x)
        Return r
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function empty() As Boolean
        Return size() = uint32_0
    End Function

    Default Public ReadOnly Property at(ByVal p As UInt32) As T
        <MethodImpl(method_impl_options.aggressive_inlining)>
        Get
#If Not Performance Then
            assert(f.find(p) = f.end())
#End If
            Return v(p)
        End Get
    End Property

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function has(ByVal p As UInt32) As Boolean
        Return v.available_index(p) AndAlso f.find(p) = f.end()
    End Function
End Class
