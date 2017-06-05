
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Text
Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants

Public Module vector_extension
    <Extension()> Public Sub renew(Of T)(ByRef v As vector(Of T))
        If v Is Nothing Then
            v = New vector(Of T)()
        Else
            v.clear()
        End If
    End Sub

    <Extension()> Public Function null_or_empty(Of T)(ByVal i As vector(Of T)) As Boolean
        Return i Is Nothing OrElse i.empty()
    End Function

    <Extension()> Public Function size_or_0(Of T)(ByVal i As vector(Of T)) As UInt32
        If i Is Nothing Then
            Return uint32_0
        Else
            Return i.size()
        End If
    End Function

    <Extension()> Public Function except(Of T As IComparable(Of T)) _
                                        (ByVal this As vector(Of T),
                                         ByVal that As vector(Of T)) As vector(Of T)
        If this.null_or_empty() Then
            Return Nothing
        ElseIf that.null_or_empty() Then
            Return copy(this)
        Else
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
        End If
    End Function

    <Extension()> Public Function modget(Of T)(ByVal v As vector(Of T), ByVal i As Int64) As T
        If v.null_or_empty() Then
            Return Nothing
        Else
            Return v(CUInt(Math.Abs(i Mod v.size())))
        End If
    End Function

    <Extension()> Public Function take(Of T)(ByVal v As vector(Of T), ByVal i As Int32, ByRef o As T) As Boolean
        If available_index(v, i) Then
            assert(i >= 0)
            o = v(CUInt(i))
            Return True
        Else
            Return False
        End If
    End Function

    <Extension()> Public Function take(Of T)(ByVal v As vector(Of T), ByVal i As UInt32, ByRef o As T) As Boolean
        If available_index(v, i) Then
            o = v(i)
            Return True
        Else
            Return False
        End If
    End Function

    <Extension()> Public Function take(Of T)(ByVal v As vector(Of T), ByVal i As Int64, ByRef o As T) As Boolean
        If available_index(v, i) Then
            assert(i >= 0)
            assert(i <= max_uint32)
            o = v(CUInt(i))
            Return True
        Else
            Return False
        End If
    End Function

    <Extension()> Public Function take(Of T)(ByVal v As vector(Of T), ByVal i As UInt64, ByRef o As T) As Boolean
        If available_index(v, i) Then
            assert(i <= max_uint32)
            o = v(CUInt(i))
            Return True
        Else
            Return False
        End If
    End Function

    <Extension()> Public Function available_index(Of T)(ByVal v As vector(Of T), ByVal i As Int32) As Boolean
        Return Not v Is Nothing AndAlso i >= 0 AndAlso i < v.size()
    End Function

    <Extension()> Public Function available_index(Of T)(ByVal v As vector(Of T), ByVal i As UInt32) As Boolean
        Return Not v Is Nothing AndAlso i < v.size()
    End Function

    <Extension()> Public Function available_index(Of T)(ByVal v As vector(Of T), ByVal i As Int64) As Boolean
        Return Not v Is Nothing AndAlso i >= 0 AndAlso i < v.size()
    End Function

    <Extension()> Public Function available_index(Of T)(ByVal v As vector(Of T), ByVal i As UInt64) As Boolean
        Return Not v Is Nothing AndAlso i < v.size()
    End Function

    <Extension()> Public Function [erase](Of T)(ByVal v As vector(Of T), ByVal i As Int32) As Boolean
        If i < 0 Then
            Return False
        Else
            v.[erase](CUInt(i))
            Return True
        End If
    End Function

    <Extension()> Public Function [erase](Of T)(ByVal v As vector(Of T), ByVal d As T) As Boolean
        Return [erase](v, v.find(d))
    End Function

    <Extension()> Public Function push_back(Of T)(ByVal d As vector(Of T), ByVal s As vector(Of T)) As Boolean
        If d Is Nothing OrElse s.null_or_empty() Then
            Return False
        Else
            Return assert(d.push_back(s.data(), 0, s.size()))
        End If
    End Function

    <Extension()> Public Function emplace_back(Of T)(ByVal d As vector(Of T), ByVal s As vector(Of T)) As Boolean
        If d Is Nothing OrElse s.null_or_empty() Then
            Return False
        Else
            Return assert(d.emplace_back(s.data(), 0, s.size()))
        End If
    End Function

    <Extension()> Public Sub fill(Of T)(ByVal v As vector(Of T), ByRef d() As T)
        If Not v Is Nothing Then
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
        End If
    End Sub

    <Extension()> Public Function find(Of T)(ByVal v As vector(Of T), ByVal k As T) As Int32
        If v.null_or_empty() Then
            Return npos
        Else
            For i As UInt32 = 0 To v.size() - uint32_1
                If compare(k, v(i)) = 0 Then
                    Return CInt(i)
                End If
            Next
            Return npos
        End If
    End Function

    <Extension()> Public Function ToString(Of T)(ByVal v As vector(Of T), ByVal separator As String) As String
        If v Is Nothing Then
            Return Nothing
        ElseIf v.empty() Then
            Return String.Empty
        Else
            Dim r As StringBuilder = Nothing
            r = New StringBuilder()
            For i As UInt32 = 0 To v.size() - uint32_1
                If i > 0 Then
                    r.Append(separator)
                End If
                r.Append(v(i))
            Next
            Return Convert.ToString(r)
        End If
    End Function

    <Extension()> Public Function str(Of T)(ByVal v As vector(Of T), ByVal separator As String) As String
        Return ToString(v, separator)
    End Function

    <Extension()> Public Function str(Of T)(ByVal v As vector(Of T)) As String
        Return ToString(v, Nothing)
    End Function

    <Extension()> Public Function from(Of T)(ByRef v As vector(Of T), ByVal ParamArray vs() As T) As Boolean
        v.renew()
        Return v.push_back(vs)
    End Function
End Module
