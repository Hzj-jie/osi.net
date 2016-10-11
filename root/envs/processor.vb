
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.template

Public Class processor_count
    Inherits _int64

    Protected Overrides Function at() As Int64
        Return Environment.ProcessorCount()
    End Function
End Class

Public Module _processor
    Public ReadOnly single_cpu As Boolean
    Public ReadOnly cpu_address_width As Int32
    Public ReadOnly x64_cpu As Boolean
    Public ReadOnly x32_cpu As Boolean

    Sub New()
        single_cpu = (Environment.ProcessorCount() = 1)
        cpu_address_width = IntPtr.Size() * bit_count_in_byte
        x32_cpu = (cpu_address_width = 32)
        x64_cpu = (cpu_address_width = 64)

        If env_bool(env_keys("report", "processor")) Then
            raise_error("single_cpu = ",
                          single_cpu,
                          ", processor architecture x32 = ",
                          x32_cpu,
                          ", x64 = ",
                          x64_cpu)
        End If
    End Sub
End Module
