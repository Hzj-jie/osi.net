
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.utt

' do not use cctor / shared new.
Public NotInheritable Class shared_ctor_perf
    Inherits performance_comparison_case_wrapper

    Private Shared Function R(ByVal i As [case]) As [case]
        Return repeat(i, 100000000)
    End Function

    Public Sub New()
        MyBase.New(R(New no_cctor_no_shared_case()),
                   R(New cctor_no_shared_case()),
                   R(New no_cctor_shared_case()),
                   R(New cctor_shared_case()))
    End Sub

    Protected Overrides Function average_rate_upper_bound(ByVal i As UInt32, ByVal j As UInt32) As Double
        Return loosen_bound({25, 93, 25, 95}, i, j)
    End Function

    Private NotInheritable Class no_cctor_no_shared
    End Class

    Private NotInheritable Class cctor_no_shared
        Shared Sub New()
            alloc(Of no_cctor_no_shared)()
        End Sub
    End Class

    Private NotInheritable Class no_cctor_shared
        Private Shared ReadOnly x As no_cctor_no_shared = alloc(Of no_cctor_no_shared)()
    End Class

    Private NotInheritable Class cctor_shared
        Private Shared ReadOnly x As no_cctor_no_shared = alloc(Of no_cctor_no_shared)()

        Shared Sub New()
            alloc(Of no_cctor_no_shared)()
        End Sub
    End Class

    Private NotInheritable Class no_cctor_no_shared_case
        Inherits [case]

        Public Overrides Function run() As Boolean
            Dim x As no_cctor_no_shared = New no_cctor_no_shared()
            Return x IsNot Nothing
        End Function
    End Class

    Private NotInheritable Class cctor_no_shared_case
        Inherits [case]

        Public Overrides Function run() As Boolean
            Dim x As cctor_no_shared = New cctor_no_shared()
            Return x IsNot Nothing
        End Function
    End Class

    Private NotInheritable Class no_cctor_shared_case
        Inherits [case]

        Public Overrides Function run() As Boolean
            Dim x As no_cctor_shared = New no_cctor_shared()
            Return x IsNot Nothing
        End Function
    End Class

    Private NotInheritable Class cctor_shared_case
        Inherits [case]

        Public Overrides Function run() As Boolean
            Dim x As cctor_shared = New cctor_shared()
            Return x IsNot Nothing
        End Function
    End Class
End Class
