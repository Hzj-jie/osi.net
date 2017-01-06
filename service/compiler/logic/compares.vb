
Imports osi.root.formation
Imports osi.service.interpreter.primitive

Namespace logic
    Public Class less
        Inherits compare

        Public Sub New(ByVal types As types, ByVal left As String, ByVal right As String, ByVal result As String)
            MyBase.New(types, left, right, result)
        End Sub

        Protected Overrides Sub export(ByVal left_ref As String,
                                       ByVal right_ref As String,
                                       ByVal result_ref As String,
                                       ByVal o As vector(Of String))
            o.emplace_back(instruction_builder.str(command.less, result_ref, left_ref, right_ref))
        End Sub
    End Class

    Public Class more
        Inherits compare

        Public Sub New(ByVal types As types, ByVal left As String, ByVal right As String, ByVal result As String)
            MyBase.New(types, left, right, result)
        End Sub

        Protected Overrides Sub export(ByVal left_ref As String,
                                       ByVal right_ref As String,
                                       ByVal result_ref As String,
                                       ByVal o As vector(Of String))
            o.emplace_back(instruction_builder.str(command.less, result_ref, right_ref, left_ref))
        End Sub
    End Class

    Public Class less_or_equal
        Inherits compare

        Public Sub New(ByVal types As types, ByVal left As String, ByVal right As String, ByVal result As String)
            MyBase.New(types, left, right, result)
        End Sub

        Protected Overrides Sub export(ByVal left_ref As String,
                                       ByVal right_ref As String,
                                       ByVal result_ref As String,
                                       ByVal o As vector(Of String))
            o.emplace_back(instruction_builder.str(command.leeq, result_ref, left_ref, right_ref))
        End Sub
    End Class

    Public Class more_or_equal
        Inherits compare

        Public Sub New(ByVal types As types, ByVal left As String, ByVal right As String, ByVal result As String)
            MyBase.New(types, left, right, result)
        End Sub

        Protected Overrides Sub export(ByVal left_ref As String,
                                       ByVal right_ref As String,
                                       ByVal result_ref As String,
                                       ByVal o As vector(Of String))
            o.emplace_back(instruction_builder.str(command.leeq, result_ref, right_ref, left_ref))
        End Sub
    End Class

    Public Class equal
        Inherits compare

        Public Sub New(ByVal types As types, ByVal left As String, ByVal right As String, ByVal result As String)
            MyBase.New(types, left, right, result)
        End Sub

        Protected Overrides Sub export(ByVal left_ref As String,
                                       ByVal right_ref As String,
                                       ByVal result_ref As String,
                                       ByVal o As vector(Of String))
            o.emplace_back(instruction_builder.str(command.equal, result_ref, right_ref, left_ref))
        End Sub
    End Class
End Namespace
