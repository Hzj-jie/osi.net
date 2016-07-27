
Imports System.Runtime.CompilerServices

Public Module _delegate2
    Public Function delegate_is_pinning_object(ByVal d As [Delegate], ByVal o As Object) As Boolean
        Return Not d Is Nothing AndAlso
               Not o Is Nothing AndAlso
               Not d.Target() Is Nothing AndAlso
               (object_compare(d.Target(), o) = 0)
    End Function

    <Extension()> Public Function is_pinning_object(ByVal d As [Delegate], ByVal o As Object) As Boolean
        Return delegate_is_pinning_object(d, o)
    End Function
End Module
