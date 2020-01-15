
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.service.compiler.logic

Public Interface logic_gen
    Inherits code_gen(Of writer)
End Interface

Public MustInherit Class logic_gen_wrapper
    Inherits code_gen_wrapper(Of writer)

    Protected Sub New(ByVal l As logic_gens)
        MyBase.New(l)
    End Sub

    Protected Function logic_gen_of(Of T As logic_gen)() As T
        Return code_gen_of(Of T)()
    End Function
End Class

Public NotInheritable Class logic_gens
    Inherits code_gens(Of writer)
End Class

