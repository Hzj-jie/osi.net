
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.template

Public Class processor_count
    Inherits _int64

    Protected Overrides Function at() As Int64
        Return Environment.ProcessorCount()
    End Function
End Class

<global_init(global_init_level.max)>
Public Module _processor
    Public ReadOnly single_cpu As Boolean = (Environment.ProcessorCount() = 1)
    Public ReadOnly cpu_address_width As Int32 = IntPtr.Size() * bit_count_in_byte
    Public ReadOnly x64_cpu As Boolean = (cpu_address_width = 32)
    Public ReadOnly x32_cpu As Boolean = (cpu_address_width = 64)

    Private Sub init()
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
