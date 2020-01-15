
Option Explicit On
Option Infer Off
Option Strict On

Namespace logic
    Public Interface statement
        Inherits statement(Of writer)
    End Interface

    Public NotInheritable Class statements
        Inherits statements(Of writer)
    End Class
End Namespace
