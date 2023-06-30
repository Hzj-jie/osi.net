
Option Explicit On
Option Infer Off
Option Strict On

Public NotInheritable Class func_t
    Public Shared Function [of](Of T)(ByVal i As T) As Func(Of T)
        Return Function() As T
                   Return i
               End Function
    End Function

    Private Sub New()
    End Sub
End Class

Public Interface func_t(Of RT)
    Function run() As RT
End Interface

Public Interface func_t(Of T1, RT)
    Function run(ByVal i As T1) As RT
End Interface

Public Interface func_t(Of T1, T2, RT)
    Function run(ByVal i As T1, ByVal j As T2) As RT
End Interface

Public Interface func_t(Of T1, T2, T3, RT)
    Function run(ByVal i As T1, ByVal j As T2, ByVal k As T3) As RT
End Interface

Public Interface action_t
    Sub run()
End Interface

Public Interface action_t(Of T)
    Sub run(ByVal i As T)
End Interface

Public Interface action_t(Of T1, T2)
    Sub run(ByVal i As T1, ByVal j As T2)
End Interface

Public Interface action_t(Of T1, T2, T3)
    Sub run(ByVal i As T1, ByVal j As T2, ByVal k As T3)
End Interface
