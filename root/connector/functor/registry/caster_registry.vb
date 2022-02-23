
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Text
Imports osi.root.constants

<global_init(global_init_level.functor)>
Friend Module _caster_registry
    Sub New()
        caster.register(Function(ByVal i As String) As StringBuilder
                            assert(i IsNot Nothing)
                            Return New StringBuilder(i)
                        End Function)
        caster.register(Function(ByVal i As StringBuilder) As String
                            assert(i IsNot Nothing)
                            Return Convert.ToString(i)
                        End Function)
    End Sub

    Private Sub init()
    End Sub
End Module
