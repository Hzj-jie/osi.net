
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.constants

Public NotInheritable Class ref
    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function [of](Of T As Structure)(ByVal i As T) As ref(Of T)
        Dim r As ref(Of T) = Nothing
        r = New ref(Of T)()
        r.p = i
        Return r
    End Function

    Private Sub New()
    End Sub
End Class

'according to generic_perf test,
'the performance to directly access the p should be on-par with other kind of wrapper logic
' This class is to convert a value type into a reference type, so one value type instance can be passed into a lambda
' expression, which is useful when using slimlock / lock / singleentry / forks, etc with event_comb.
' This class is intended to be a simple class, if comparing / cloning are required, use pointer.
Public NotInheritable Class ref(Of T As Structure)
    Public p As T
End Class
