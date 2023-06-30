
Imports osi.root.connector
Imports osi.root.utt

' union_int has a on-par performance with unchecked functions. So the unchecked int32_uint32, etc are removed from math.
Public Class int_uint_perf
    Inherits performance_comparison_case_wrapper

    Private Shared Function r(ByVal c As [case]) As [case]
        Return repeat(c, 100000000)
    End Function

    Public Sub New()
        MyBase.New(r(New c_case()), r(New m_case()))
    End Sub

    Private Class c_case
        Inherits [case]

        Public Overrides Function run() As Boolean
            int32_uint32(rnd_int())
            uint32_int32(rnd_uint())
            Return True
        End Function
    End Class

    Private Class m_case
        Inherits [case]

        Private Shared Function c(ByVal i As Int32) As UInt32
            Return i
        End Function

        Private Shared Function c(ByVal i As UInt32) As Int32
            Return i
        End Function

        Public Overrides Function run() As Boolean
            c(rnd_int())
            c(rnd_uint())
            Return True
        End Function
    End Class
End Class
