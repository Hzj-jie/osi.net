
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.lock
Imports osi.root.utt

Public MustInherit Class atomic_uint_test
    Inherits atom_test

    Protected MustInherit Class atomic_uint_case
        Inherits atom_case

        Private ReadOnly a As atomic_uint

        Public MustOverride Function initial_value() As UInt32

        <Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")>
        Public Sub New()
            a = New atomic_uint(initial_value())
        End Sub

        Public Overrides Function run() As Boolean
            a.increment()
            Return True
        End Function

        Public Function result() As UInt32
            Return a.get()
        End Function
    End Class

    Protected MustOverride Overrides Function create_case() As atom_case

    Protected Overrides Sub validate(ByVal ac As atom_case)
        assert(Not ac Is Nothing)
        assert(TypeOf ac Is atomic_uint_case)
        assertion.is_true(ac.direct_cast_to(Of atomic_uint_case)().result() >= 0)
        assertion.equal(CLng(ac.direct_cast_to(Of atomic_uint_case)().result()),
                     round * thread_count + ac.direct_cast_to(Of atomic_uint_case)().initial_value())
    End Sub
End Class

Public Class atomic_uint_0_test
    Inherits atomic_uint_test

    Private Class atomic_uint_0_case
        Inherits atomic_uint_case

        Public Overrides Function initial_value() As UInt32
            Return 0
        End Function
    End Class

    Protected Overrides Function create_case() As atom_case
        Return New atomic_uint_0_case()
    End Function
End Class

Public Class atomic_uint_min_int32_test
    Inherits atomic_uint_test

    Private Class atomic_uint_min_int32_case
        Inherits atomic_uint_case

        Public Overrides Function initial_value() As UInt32
            Return int32_uint32(min_int32)
        End Function
    End Class

    Protected Overrides Function create_case() As atom_case
        Return New atomic_uint_min_int32_case()
    End Function
End Class

Public Class atomic_uint_max_int32_test
    Inherits atomic_uint_test

    Private Class atomic_uint_max_int32_case
        Inherits atomic_uint_case

        Public Overrides Function initial_value() As UInt32
            Return int32_uint32(max_int32)
        End Function
    End Class

    Protected Overrides Function create_case() As atom_case
        Return New atomic_uint_max_int32_case()
    End Function
End Class
