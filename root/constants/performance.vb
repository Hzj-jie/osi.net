
Public Module _performance
    Public Class performanceCounterCategories
        Public Const process As String = "process"
        Public Const processor As String = "Processor"
        Public Const memory As String = "Memory"
    End Class

    Public Class performanceCounterCounters
        Public Const processorTime As String = "% Processor Time"
        Public Const availableMBytes As String = "Available MBytes"
        Public Const availableBytes As String = "Available Bytes"
        Public Const workingSet As String = "Working Set"
        Public Const virtualBytes As String = "Virtual Bytes"
    End Class
End Module
