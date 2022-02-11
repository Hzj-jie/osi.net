
Option Explicit On
Option Infer Off
Option Strict On

Public Interface idelegate
    Function valid() As Boolean
End Interface

Public Interface iparameter_action(Of PARA_T)
    Inherits idelegate
    Sub run(ByVal i As PARA_T)
End Interface

Public Interface iaction
    Inherits idelegate
    Sub run()
End Interface

Public Interface iaction(Of T)
    Inherits idelegate
    Sub run(ByVal v As T)
End Interface

Public Interface iaction(Of T1, T2)
    Inherits idelegate
    Sub run(ByVal v1 As T1, ByVal v2 As T2)
End Interface

Public Interface iaction(Of T1, T2, T3)
    Inherits idelegate
    Sub run(ByVal v1 As T1, ByVal v2 As T2, ByVal v3 As T3)
End Interface

Public Interface ifunc(Of RT)
    Function run() As RT
End Interface

Public Interface ifunc(Of T, RT)
    Function run(ByRef i As T) As RT
End Interface
