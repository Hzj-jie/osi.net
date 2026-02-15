
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.delegates
Imports osi.root.template

Public Module _concurrency_runner
    Private concurrency As argument(Of UInt32)

    Public NotInheritable Class concurrency_t
        Inherits _int64

        Protected Overrides Function at() As Int64
            Return concurrency Or uint32_2
        End Function
    End Class

    Public ReadOnly concurrency_runner As New concurrency_runner(Of concurrency_t)()
End Module