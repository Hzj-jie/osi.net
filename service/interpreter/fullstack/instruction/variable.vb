
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.utils

Namespace fullstack.instruction
    Public Class variable
        Implements instruction

        Public Enum def As Byte
            bool = 0
            int
            float
            [char]
            [string]
            var
        End Enum

        Public ReadOnly definition As def
        Public ReadOnly value As Object

        Private Sub New(ByVal definition As def, ByVal value As Object)
            Me.definition = definition
            Me.value = value
        End Sub

        Public Function execute(ByRef ip As UInt32) As Boolean Implements instruction.execute
            Return False
        End Function

        Public Shared Function parse(ByVal bc() As Byte,
                                     ByRef pos As UInt32,
                                     ByRef o As variable) As Boolean
            pos += 1
            If array_size(bc) <= pos Then
                Return False
            Else
                Dim type_def As Byte = 0
                type_def = bc(pos - 1)
                Dim b As Boolean = False
                Dim i As Int32 = 0
                Dim f As Double = 0
                Dim c As Char = character.null
                Dim s As String = Nothing
                If type_def = def.bool AndAlso
                   bytes_bool(bc, b, pos) Then
                    o = New variable(def.bool, b)
                ElseIf type_def = def.char AndAlso
                       bytes_char(bc, c, pos) Then
                    o = New variable(def.char, c)
                ElseIf type_def = def.float AndAlso
                       bytes_double(bc, f, pos) Then
                    o = New variable(def.float, f)
                ElseIf type_def = def.int AndAlso
                       bytes_int32(bc, i, pos) Then
                    o = New variable(def.int, i)
                ElseIf type_def = def.string AndAlso
                       bytes_int32(bc, i, pos) AndAlso
                       bytes_str(bc, pos, i, s) Then
                    pos += i
                    o = New variable(def.string, s)
                Else
                    Return False
                End If
                Return True
            End If
        End Function
    End Class
End Namespace
