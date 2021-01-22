
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.utt

' do not use cctor / shared new.
Public NotInheritable Class static_constructor_perf
    Inherits performance_comparison_case_wrapper

    Private Shared Function R(ByVal i As [case]) As [case]
        Return repeat(i, 100000000)
    End Function

    Public Sub New()
        MyBase.New(R(New cctor_case()), R(New shared_variable_case()))
    End Sub

    Protected Overrides Function average_rate_upper_bound(ByVal i As UInt32, ByVal j As UInt32) As Double
        Return loosen_bound({35, 497}, i, j)
    End Function

    Private NotInheritable Class cctor
        Public Shared ReadOnly x As String

        Shared Sub New()
            x = "abc"
        End Sub

        Public Shared Sub init()
        End Sub
    End Class

    Private NotInheritable Class shared_variable
        Public Shared ReadOnly x As String = "abc"

        Public Shared Sub init()
        End Sub
    End Class

    Private NotInheritable Class cctor_case
        Inherits [case]

        Public Overrides Function run() As Boolean
            cctor.init()
            Return True
        End Function
    End Class

    Private NotInheritable Class shared_variable_case
        Inherits [case]

        Public Overrides Function run() As Boolean
            static_constructor(Of shared_variable).execute()
            shared_variable.init()
            Return True
        End Function
    End Class
End Class
