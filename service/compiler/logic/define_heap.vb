
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.interpreter.primitive

Partial Public NotInheritable Class logic
    ' Define an array with @name, @type and @size
    Public NotInheritable Class _define_heap
        Implements instruction_gen

        Private ReadOnly name As String
        Private ReadOnly type As String
        Private ReadOnly size As String

        Public Sub New(ByVal name As String,
                       ByVal type As String,
                       ByVal size As String)
            assert(Not name.null_or_whitespace())
            assert(Not type.null_or_whitespace())
            assert(Not size.null_or_whitespace())
            Me.name = name
            Me.type = type
            Me.size = size
        End Sub

        Public Function build(ByVal o As vector(Of String)) As Boolean Implements instruction_gen.build
            assert(Not o Is Nothing)
            Dim size As variable = Nothing
            If Not variable.of(Me.size, o, size) Then
                Return False
            End If
            If Not scope.current().variables().define(name, type) Then
                Return False
            End If
            o.emplace_back(command_str(command.push))
            o.emplace_back(instruction_builder.str(command.alloc, "rel0", size))
            scope.current().when_end_scope(Sub()
                                               If scope.current().variables().defined_in_current_scope(name) Then
                                                   ' The heap ptr has not been removed.
                                                   o.emplace_back(instruction_builder.str(command.dealloc, "rel0"))
                                               End If
                                               o.emplace_back(command_str(command.pop))
                                           End Sub)
            Return True
        End Function
    End Class
End Class
