
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Reflection
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

Partial Public NotInheritable Class case2
    Private Shared Function apply_repeat(ByVal i As function_info) As Func(Of Object, Boolean)
        assert(Not i Is Nothing)
        assert(i.repeat_times >= uint64_1)
        Return Function(ByVal obj As Object) As Boolean
                   For j As UInt64 = 0 To i.repeat_times - uint64_1
                       If Not i.f(obj) Then
                           Return False
                       End If
                   Next
                   Return True
               End Function
    End Function

    ' Apply repeat only, for prepare and finish.
    Private Shared Function chain(ByVal vs As vector(Of function_info)) As Func(Of Object, Boolean)
        If vs.null_or_empty() Then
            Return Nothing
        Else
            Dim fs() As Func(Of Object, Boolean) = Nothing
            ReDim fs(CInt(vs.size() - uint32_1))
            For i As UInt32 = 0 To vs.size() - uint32_1
                fs(CInt(i)) = apply_repeat(vs(i))
            Next
            Return Function(ByVal obj As Object) As Boolean
                       For i As Int32 = 0 To array_size_i(fs) - 1
                           If Not fs(i)(obj) Then
                               Return False
                           End If
                       Next
                       Return True
                   End Function
        End If
    End Function

    Private Shared Function parse_functions(Of AT As Attribute, FI As function_info) _
                                           (ByVal t As Type, ByVal from As Func(Of MethodInfo, FI)) As vector(Of FI)
        assert(Not t Is Nothing)
        assert(Not from Is Nothing)
        Dim ms() As MethodInfo = Nothing
        ms = t.GetMethods(binding_flags.all_method)
        If isemptyarray(ms) Then
            Return Nothing
        End If

        Dim vs As vector(Of FI) = Nothing
        vs = New vector(Of FI)()
        For i As Int32 = 0 To array_size_i(ms) - 1
            If ms(i).has_custom_attribute(Of AT) Then
                vs.emplace_back(from(ms(i)))
            End If
        Next
        Return vs
    End Function

    Private Shared Function parse_functions(Of AT As Attribute)(ByVal t As Type) As vector(Of function_info)
        Return parse_functions(Of AT, function_info)(t, AddressOf function_info.from)
    End Function

    Private Shared Function parse_chained_function(Of AT As Attribute) _
                                                          (ByVal t As Type) As Func(Of Object, Boolean)
        Return chain(parse_functions(Of AT)(t))
    End Function

    Private Shared Function parse_prepare(ByVal t As Type) As Func(Of Object, Boolean)
        Return parse_chained_function(Of attributes.prepare)(t)
    End Function

    Private Shared Function parse_finish(ByVal t As Type) As Func(Of Object, Boolean)
        Return parse_chained_function(Of attributes.finish)(t)
    End Function

    Private Shared Function parse_tests(ByVal t As Type) As vector(Of function_info)
        Return parse_functions(Of attributes.test)(t)
    End Function

    Private Shared Function parse_randoms(ByVal t As Type) As vector(Of random_function_info)
        Return parse_functions(Of attributes.random, random_function_info)(t, AddressOf random_function_info.from)
    End Function
End Class
