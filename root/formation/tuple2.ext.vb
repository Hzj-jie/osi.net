
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices

Public Module _tuple2
    <Extension> Public Function first(Of T1, T2)(ByVal this As tuple(Of T1, T2)) As T1
        Return this._1()
    End Function

    <Extension> Public Function second(Of T1, T2)(ByVal this As tuple(Of T1, T2)) As T2
        Return this._2()
    End Function
End Module
