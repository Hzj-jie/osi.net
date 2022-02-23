
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Collections.Generic
Imports System.Runtime.CompilerServices
Imports System.Text
Imports osi.root.constants
Imports osi.root.delegates

Public Module _strcat
    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Function strncat(ByRef dest As String,
                             ByVal src As String,
                             ByVal count As Int64,
                             ByVal impl As _do_ref_val(Of String, String, String)) As String
        Dim len As Int64 = 0
        len = strlen(src)
        If len > 1 Then
            While count > 0
                impl(dest, src)
                count -= 1
            End While
        Else
            impl(dest, New String(src(0), CInt(count)))
        End If
        Return dest
    End Function

    'the binding of strcat will be changed
    'evil
    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Function strcat(ByRef dest As String, ByVal src As String) As String
        dest += src
        Return dest
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Function strrcat(ByRef dest As String, ByVal src As String) As String
        dest = src + dest
        Return dest
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function strncat(ByRef dest As String, ByVal src As String, ByVal count As Int64) As String
        Return strncat(dest, src, count, AddressOf strcat)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function strrncat(ByRef dest As String, ByVal src As String, ByVal count As Int64) As String
        Return strncat(dest, src, count, AddressOf strrcat)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Function strfillImpl(ByRef dest As String, ByVal src As String, ByVal length As Int64,
                                 ByVal impl As _do_ref_val_val(Of String, String, Int64, String)) As String
        Dim required_times As Int64 = 0
        required_times = length - strlen(dest)
        If required_times <> 0 Then
            Dim srclen As Int64 = 0
            srclen = strlen(src)
            assert(srclen > 0, "src is nothing or emptystring, cannot fill to required length.")
            assert(required_times >= 0, "dest length is already over length.")
            assert(required_times / srclen = required_times \ srclen, "requiredTimes is not an integer.")
            required_times \= srclen
            impl(dest, src, required_times)
        End If
        Return dest
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function strfill(ByRef dest As String, ByVal src As String, ByVal length As Int64) As String
        Return strfillImpl(dest, src, length, AddressOf strncat)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function strrfill(ByRef dest As String, ByVal src As String, ByVal length As Int64) As String
        Return strfillImpl(dest, src, length, AddressOf strrncat)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function strcat_hint(ByVal len As UInt32, ByVal p As IEnumerable(Of Object)) As String
        Dim s As StringBuilder = Nothing
        If p IsNot Nothing Then
            s = New StringBuilder(CInt(len))
            For Each x As Object In p
                s.Append(Convert.ToString(x))
            Next
        End If
        Return Convert.ToString(s)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function strcat(ByVal p As IEnumerable(Of Object)) As String
        Return strcat_hint(stringbuilder_default_capacity, p)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function strcat_hint(ByVal len As UInt32, ByVal ParamArray p() As Object) As String
        Return strcat_hint(len, DirectCast(p, IEnumerable(Of Object)))
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function strcat(ByVal ParamArray p() As Object) As String
        Return strcat_hint(stringbuilder_default_capacity, p)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function strcat_hint(Of T)(ByVal len As UInt32, ByVal p As IEnumerable(Of T)) As String
        Dim s As StringBuilder = Nothing
        If p IsNot Nothing Then
            s = New StringBuilder(CInt(len))
            For Each x As T In p
                s.Append(x)
            Next
        End If
        Return Convert.ToString(s)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function strcat(Of T)(ByVal p As IEnumerable(Of T)) As String
        Return strcat_hint(stringbuilder_default_capacity, p)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function strcat_hint(ByVal len As UInt32, ByVal p As IEnumerable(Of String)) As String
        Dim s As StringBuilder = Nothing
        If p IsNot Nothing Then
            s = New StringBuilder(CInt(len))
            For Each x As String In p
                s.Append(x)
            Next
        End If
        Return Convert.ToString(s)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function strcat(ByVal p As IEnumerable(Of String)) As String
        Return strcat_hint(stringbuilder_default_capacity, p)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function strcat_hint(ByVal len As UInt32, ByVal ParamArray p() As String) As String
        Return strcat_hint(len, DirectCast(p, IEnumerable(Of String)))
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function strcat(ByVal ParamArray p() As String) As String
        Return strcat_hint(stringbuilder_default_capacity, p)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function strcat_hint(ByVal len As UInt32, ByVal p As IEnumerable(Of Char)) As String
        Dim s As StringBuilder = Nothing
        If p IsNot Nothing Then
            s = New StringBuilder(CInt(len))
            For Each x As Char In p
                s.Append(x)
            Next
        End If
        Return Convert.ToString(s)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function strcat(ByVal p As IEnumerable(Of Char)) As String
        Return strcat_hint(stringbuilder_default_capacity, p)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function strcat_hint(ByVal len As UInt32, ByVal ParamArray p() As Char) As String
        Return strcat_hint(len, DirectCast(p, IEnumerable(Of Char)))
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function strcat(ByVal ParamArray p() As Char) As String
        Return strcat_hint(stringbuilder_default_capacity, p)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function append(ByRef this As String, ByVal ParamArray p() As Object) As String
        Dim s As String = Nothing
        s = strcat(p)
        Return strcat(this, s)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function strjoin(ByVal vs() As String,
                                          ByVal sep As String,
                                          ByVal start As Int32,
                                          ByVal count As Int32) As String
        Return strjoin(sep, vs, start, count)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function strjoin(ByVal vs() As String,
                                          ByVal sep As String,
                                          ByVal start As Int32) As String
        Return strjoin(sep, vs, start)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function strjoin(ByVal vs() As String,
                                          ByVal sep As String) As String
        Return strjoin(sep, vs)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function strjoin(Of T)(ByVal vs() As T,
                                                ByVal sep As String) As String
        Return strjoin(sep, vs)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function strjoin(ByVal sep As String,
                            ByVal vs() As String,
                            ByVal start As Int32,
                            ByVal count As Int32) As String
        If start < 0 OrElse count < 0 OrElse isemptyarray(vs) Then
            Return Nothing
        End If
        count = min(count, vs.array_size_i() - start)
        Return String.Join(sep, vs, start, count)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function strjoin(ByVal sep As String,
                            ByVal vs() As String,
                            ByVal start As Int32) As String
        Return strjoin(sep, vs, start, max_int32)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function strjoin(ByVal sep As String, ByVal ParamArray vs() As String) As String
        Return strjoin(sep, vs, 0, vs.array_size_i())
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function strjoin(Of T)(ByVal sep As String,
                                                ByVal vs As IEnumerable(Of T)) As String
        Dim s As StringBuilder = Nothing
        If vs IsNot Nothing Then
            s = New StringBuilder()
            For Each x As T In vs
                If strlen(s) > 0 Then
                    s.Append(sep)
                End If
                s.Append(x)
            Next
        End If
        Return Convert.ToString(s)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    <Extension()> Public Function str_repeat(ByRef s As String, ByVal c As UInt32) As String
        If c = 0 Then
            Return ""
        End If
        If c = 1 Then
            Return s
        End If
        If String.IsNullOrEmpty(s) Then
            Return s
        End If
        Dim r As StringBuilder = Nothing
        r = New StringBuilder(s.strlen_i() * CInt(c))
        While c > 0
            r.Append(s)
            c -= uint32_1
        End While
        s = r.ToString()
        Return s
    End Function
End Module
