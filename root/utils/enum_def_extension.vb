
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.formation

Public Module _enum_def_extension
    Private NotInheritable Class enum_definition2(Of T)
        Public Shared ReadOnly strings As lazier(Of String()) =
            lazier.of(Function() As String()
                          Dim i As Int32 = 0
                          Dim r(enum_def(Of T).count_i() - 1) As String
                          enum_def(Of T).foreach(Sub(ByVal x As T, ByVal s As String)
                                                     r(i) = s
                                                     i += 1
                                                 End Sub)
                          Return r
                      End Function)
        Public Shared ReadOnly string_pairs As lazier(Of pair(Of T, String)()) =
            lazier.of(Function() As pair(Of T, String)()
                          Dim i As Int32 = 0
                          Dim r(enum_def(Of T).count_i() - 1) As pair(Of T, String)
                          enum_def(Of T).foreach(Sub(ByVal x As T, ByVal s As String)
                                                     r(i) = pair.emplace_of(x, s)
                                                     i += 1
                                                 End Sub)
                          Return r
                      End Function)
        Public Shared ReadOnly string_T As lazier(Of unordered_map(Of String, T)) =
            lazier.of(Function() As unordered_map(Of String, T)
                          Dim r As unordered_map(Of String, T) = Nothing
                          r = New unordered_map(Of String, T)()
                          For i As Int32 = 0 To array_size_i(+string_pairs) - 1
                              assert(r.emplace(strtolower((+string_pairs)(i).second),
                                                          (+string_pairs)(i).first).second)
                              r.emplace((+string_pairs)(i).second, (+string_pairs)(i).first)
                          Next
                          Return r
                      End Function)
    End Class

    <Extension()>
    Public Function string_pairs(Of T)(ByVal this As enum_definition(Of T)) As pair(Of T, String)()
        Return (+enum_definition2(Of T).string_pairs).deep_clone()
    End Function

    <Extension()>
    Public Function strings(Of T)(ByVal this As enum_definition(Of T)) As String()
        Return (+enum_definition2(Of T).strings).shallow_clone()
    End Function

    <Extension()>
    Public Function string_map(Of T)(ByVal this As enum_definition(Of T)) As unordered_map(Of String, T)
        Return (+enum_definition2(Of T).string_T).CloneT()
    End Function
End Module
