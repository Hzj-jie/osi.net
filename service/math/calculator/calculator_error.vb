
Imports System.Runtime.CompilerServices

Public Module _calculator_error
    <Extension()> Public Function has_error(ByVal this As calculator_error) As Boolean
        Return this IsNot Nothing AndAlso
               (this.divide_by_zero OrElse
                this.imaginary_number OrElse
                this.overflow OrElse
                this.operand_mismatch OrElse
                this.bracket_mismatch)
    End Function

    <Extension()> Public Function has_no_error(ByVal this As calculator_error) As Boolean
        Return Not has_error(this)
    End Function

    <Extension()> Public Function str(ByVal this As calculator_error) As String
        If this Is Nothing Then
            Return Nothing
        ElseIf this.divide_by_zero Then
            Return "divide by zero"
        ElseIf this.imaginary_number Then
            Return "imaginary number"
        ElseIf this.overflow Then
            Return "overflow"
        ElseIf this.operand_mismatch Then
            Return "operand mismatch"
        ElseIf this.bracket_mismatch Then
            Return "bracket mismatch"
        Else
            Return Nothing
        End If
    End Function
End Module

Public Class calculator_error
    Public ReadOnly divide_by_zero As Boolean
    Public ReadOnly imaginary_number As Boolean
    Public ReadOnly overflow As Boolean
    Public ReadOnly operand_mismatch As Boolean
    Public ReadOnly bracket_mismatch As Boolean

    Private Sub New(Optional ByVal divide_by_zero As Boolean = False,
                    Optional ByVal imaginary_number As Boolean = False,
                    Optional ByVal overflow As Boolean = False,
                    Optional ByVal operand_mismatch As Boolean = False,
                    Optional ByVal bracket_mismatch As Boolean = False)
        Me.divide_by_zero = divide_by_zero
        Me.imaginary_number = imaginary_number
        Me.overflow = overflow
        Me.operand_mismatch = operand_mismatch
        Me.bracket_mismatch = bracket_mismatch
    End Sub

    Public Shared ReadOnly divide_by_zero_error As calculator_error
    Public Shared ReadOnly imaginary_number_error As calculator_error
    Public Shared ReadOnly overflow_error As calculator_error
    Public Shared ReadOnly operand_mismatch_error As calculator_error
    Public Shared ReadOnly bracket_mismatch_error As calculator_error

    Shared Sub New()
        divide_by_zero_error = New calculator_error(divide_by_zero:=True)
        imaginary_number_error = New calculator_error(imaginary_number:=True)
        overflow_error = New calculator_error(overflow:=True)
        operand_mismatch_error = New calculator_error(operand_mismatch:=True)
        bracket_mismatch_error = New calculator_error(bracket_mismatch:=True)
    End Sub
End Class
