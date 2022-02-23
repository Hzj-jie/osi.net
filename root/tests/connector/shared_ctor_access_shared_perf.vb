
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.utt

' do not use cctor / shared new.
Public NotInheritable Class shared_ctor_access_shared_perf
    Inherits performance_comparison_case_wrapper

    Private Shared Function R(ByVal i As [case]) As [case]
        Return repeat(i, 100000000)
    End Function

    Public Sub New()
        MyBase.New(R(New no_cctor_case()), R(New cctor_case()))
    End Sub

    Protected Overrides Function average_rate_upper_bound(ByVal i As UInt32, ByVal j As UInt32) As Double
        Return loosen_bound({605, 1099}, i, j)
    End Function

    Private NotInheritable Class no_cctor
        Public Shared ReadOnly instance As String = "abc"
    End Class

    Private NotInheritable Class cctor
        Public Shared ReadOnly instance As String

        Shared Sub New()
            instance = "abc"
        End Sub
    End Class

    Private NotInheritable Class no_cctor_case
        Inherits [case]

        Public Overrides Function run() As Boolean
            Return assert(no_cctor.instance IsNot Nothing)
        End Function
    End Class

    Private NotInheritable Class cctor_case
        Inherits [case]

        Public Overrides Function run() As Boolean
            Return assert(cctor.instance IsNot Nothing)
        End Function
    End Class
End Class
