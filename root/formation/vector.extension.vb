
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Text
Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants

Public Module vector_extension
    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function renew(Of T)(ByRef v As vector(Of T)) As vector(Of T)
        If v Is Nothing Then
            v = New vector(Of T)()
        Else
            v.clear()
        End If
        Return v
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function null_or_empty(Of T)(ByVal i As vector(Of T)) As Boolean
        Return i Is Nothing OrElse i.empty()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function size_or_0(Of T)(ByVal i As vector(Of T)) As UInt32
        If i Is Nothing Then
            Return uint32_0
        End If
        Return i.size()
    End Function

    <Extension()> Public Function except(Of T As IComparable(Of T)) _
                                        (ByVal this As vector(Of T),
                                         ByVal that As vector(Of T)) As vector(Of T)
        If this.null_or_empty() Then
            Return Nothing
        End If
        If that.null_or_empty() Then
            Return copy(this)
        End If
        Dim s As [set](Of T) = Nothing
        s = New [set](Of T)()
        For i As UInt32 = 0 To that.size() - uint32_1
            s.insert(that(i))
        Next

        Dim rtn As vector(Of T) = Nothing
        rtn = New vector(Of T)()
        For i As UInt32 = 0 To this.size() - uint32_1
            If s.find(this(i)) = s.end() Then
                rtn.push_back(this(i))
            End If
        Next

        Return rtn
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function modget(Of T)(ByVal v As vector(Of T), ByVal i As Int64) As T
        If v.null_or_empty() Then
            Return Nothing
        End If
        Return v(CUInt(Math.Abs(i Mod v.size())))
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function take(Of T)(ByVal v As vector(Of T), ByVal i As Int32, ByRef o As T) As Boolean
        If available_index(v, i) Then
            assert(i >= 0)
            o = v(CUInt(i))
            Return True
        End If
        Return False
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function take(Of T)(ByVal v As vector(Of T), ByVal i As UInt32, ByRef o As T) As Boolean
        If available_index(v, i) Then
            o = v(i)
            Return True
        End If
        Return False
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function take(Of T)(ByVal v As vector(Of T), ByVal i As Int64, ByRef o As T) As Boolean
        If available_index(v, i) Then
            assert(i >= 0)
            assert(i <= max_uint32)
            o = v(CUInt(i))
            Return True
        End If
        Return False
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function take(Of T)(ByVal v As vector(Of T), ByVal i As UInt64, ByRef o As T) As Boolean
        If available_index(v, i) Then
            assert(i <= max_uint32)
            o = v(CUInt(i))
            Return True
        End If
        Return False
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function available_index(Of T)(ByVal v As vector(Of T), ByVal i As Int32) As Boolean
        Return Not v Is Nothing AndAlso i >= 0 AndAlso i < v.size()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function available_index(Of T)(ByVal v As vector(Of T), ByVal i As UInt32) As Boolean
        Return Not v Is Nothing AndAlso i < v.size()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function available_index(Of T)(ByVal v As vector(Of T), ByVal i As Int64) As Boolean
        Return Not v Is Nothing AndAlso i >= 0 AndAlso i < v.size()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function available_index(Of T)(ByVal v As vector(Of T), ByVal i As UInt64) As Boolean
        Return Not v Is Nothing AndAlso i < v.size()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function [erase](Of T)(ByVal v As vector(Of T), ByVal i As Int32) As Boolean
        If i < 0 Then
            Return False
        End If
        v.[erase](CUInt(i))
        Return True
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function [erase](Of T)(ByVal v As vector(Of T), ByVal d As T) As Boolean
        Return [erase](v, v.find(d))
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function push_back(Of T)(ByVal d As vector(Of T), ByVal s As vector(Of T)) As Boolean
        If d Is Nothing OrElse s.null_or_empty() Then
            Return False
        End If
        Return assert(d.push_back(s.data(), 0, s.size()))
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function emplace_back(Of T)(ByVal d As vector(Of T), ByVal s As vector(Of T)) As Boolean
        If d Is Nothing OrElse s.null_or_empty() Then
            Return False
        End If
        Return assert(d.emplace_back(s.data(), 0, s.size()))
    End Function

    <Extension()> Public Sub fill(Of T)(ByVal v As vector(Of T), ByRef d() As T)
        If v Is Nothing Then
            Return
        End If
        If v.empty() Then
            ReDim d(-1)
        Else
            If array_size(d) <> v.size() Then
                ReDim d(CInt(v.size() - uint32_1))
            End If
            For i As UInt32 = 0 To v.size() - uint32_1
                copy(d(CInt(i)), v(i))
            Next
        End If
    End Sub

    <Extension()> Public Function find(Of T)(ByVal v As vector(Of T), ByVal k As T) As Int32
        If v.null_or_empty() Then
            Return npos
        End If
        For i As UInt32 = 0 To v.size() - uint32_1
            If compare(k, v(i)) = 0 Then
                Return CInt(i)
            End If
        Next
        Return npos
    End Function

    <Extension()> Public Function str(Of T)(ByVal v As vector(Of T),
                                            ByVal f As Func(Of T, String),
                                            ByVal separator As String) As String
        assert(Not f Is Nothing)
        If v Is Nothing Then
            Return Nothing
        End If
        If v.empty() Then
            Return String.Empty
        End If
        Dim r As StringBuilder = Nothing
        r = New StringBuilder()
        For i As UInt32 = 0 To v.size() - uint32_1
            If i > 0 Then
                r.Append(separator)
            End If
            r.Append(f(v(i)))
        Next
        Return Convert.ToString(r)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function str(Of T)(ByVal v As vector(Of T), ByVal separator As String) As String
        Return str(v, AddressOf Convert.ToString, separator)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function str(Of T)(ByVal v As vector(Of T)) As String
        Return str(v, Nothing)
    End Function

    ' TODO: Remove, use stream().map() instead.
    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function map(Of T, R)(ByVal v As vector(Of T), ByVal f As Func(Of T, R)) As vector(Of R)
        If v Is Nothing Then
            Return Nothing
        End If
        assert(Not f Is Nothing)
        Dim o As vector(Of R) = Nothing
        o = New vector(Of R)(v.size())
        Dim j As UInt32 = 0
        While j < v.size()
            o.push_back(f(v(j)))
            j += uint32_1
        End While
        Return o
    End Function

    <Extension()> Public Function stream(Of T)(ByVal v As vector(Of T)) As stream(Of T)
        Return New stream(Of T).container(Of vector(Of T))(v)
    End Function
End Module

Public NotInheritable Class vector
    Private Shared Function create(Of T)(ByVal vs() As T, ByVal require_copy As Boolean) As vector(Of T)
        Dim r As vector(Of T) = Nothing
        r = New vector(Of T)()
        If require_copy Then
            assert(r.push_back(vs))
        Else
            assert(r.emplace_back(vs))
        End If
        Return r
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function [of](Of T)(ByVal ParamArray vs() As T) As vector(Of T)
        Return create(vs, True)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function emplace_of(Of T)(ByVal ParamArray vs() As T) As vector(Of T)
        Return create(vs, False)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function repeat_of(Of T)(ByVal v As T, ByVal size As UInt32) As vector(Of T)
        Dim r As vector(Of T) = Nothing
        r = New vector(Of T)()
        Dim i As UInt32 = 0
        While i < size
            r.push_back(v)
            i += uint32_1
        End While
        Return r
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function matrix_of(Of T)(ByVal v As T,
                                           ByVal height As UInt32,
                                           ByVal width As UInt32) As vector(Of vector(Of T))
        Return repeat_of(repeat_of(v, width), height)
    End Function

    Private Sub New()
    End Sub
End Class
