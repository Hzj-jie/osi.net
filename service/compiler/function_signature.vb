
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports builders = osi.service.compiler.logic.builders

Public Class function_signature
    Inherits function_signature(Of builders.parameter_type)

    Public Sub New(ByVal name As String,
                   ByVal return_type As String,
                   ByVal parameters() As builders.parameter_type)
        MyBase.New(name, return_type, parameters)
    End Sub

    Public Sub New(ByVal name As String,
                   ByVal return_type As String,
                   ByVal parameters As const_array(Of builders.parameter_type))
        MyBase.New(name, return_type, parameters)
    End Sub
End Class

Public Class function_signature(Of T As builders.parameter_type)
    Public ReadOnly name As String
    Public ReadOnly return_type As String
    Public ReadOnly parameters As const_array(Of T)

    Public Sub New(ByVal name As String,
                   ByVal return_type As String,
                   ByVal parameters() As T)
        Me.New(name, return_type, const_array.of(parameters))
    End Sub

    Public Sub New(ByVal name As String,
                   ByVal return_type As String,
                   ByVal parameters As vector(Of T))
        Me.New(name, return_type, +parameters)
    End Sub

    Public Sub New(ByVal name As String,
                   ByVal return_type As String,
                   ByVal parameters As const_array(Of T))
        assert(Not name.null_or_whitespace())
        assert(Not return_type.null_or_whitespace())
        assert(Not parameters Is Nothing)
        Me.name = name
        Me.return_type = return_type
        Me.parameters = parameters
    End Sub

    Public NotOverridable Overrides Function ToString() As String
        Return strcat(return_type, " ", name, "(", parameters, ")")
    End Function
End Class

