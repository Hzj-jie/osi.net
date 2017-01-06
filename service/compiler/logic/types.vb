
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation

Namespace logic
    ' Defines primitive types, i.e. basic data types a language can handle, such as byte, int, byte array, etc. So the
    ' types instance won't be impacted by the source code or in another word, user input, of a language.
    Public Class types
        Private Const variable_size As UInt32 = max_uint32
        Private ReadOnly sizes As map(Of String, UInt32)

        Public Sub New()
            sizes = New map(Of String, UInt32)()
        End Sub

        Public Sub define(ByVal type As String, ByVal size As UInt32)
            assert(Not String.IsNullOrEmpty(type))
            assert(sizes.find(type) = sizes.end())
            sizes(type) = size
        End Sub

        Public Sub define_variable_size(ByVal type As String)
            define(type, variable_size)
        End Sub

        Public Function is_size(ByVal type As String, ByVal size As UInt32, ByRef o As Boolean) As Boolean
            Dim exp_size As UInt32 = 0
            If retrieve(type, exp_size) Then
                o = (size = exp_size)
                Return True
            Else
                Return False
            End If
        End Function

        Public Function is_size(ByVal type As String, ByVal size As UInt32) As Boolean
            Dim o As Boolean = False
            assert(is_size(type, size, o))
            Return o
        End Function

        Public Function is_variable_size(ByVal type As String, ByRef o As Boolean) As Boolean
            Return is_size(type, variable_size, o)
        End Function

        Public Function is_variable_size(ByVal type As String) As Boolean
            Dim o As Boolean = False
            assert(is_variable_size(type, o))
            Return o
        End Function

        Public Shared Function is_variable_size(ByVal size As UInt32) As Boolean
            Return size = variable_size
        End Function

        Public Shared Function is_size_or_variable(ByVal size As UInt32, ByVal exp_size As UInt32) As Boolean
            Return size = exp_size OrElse is_variable_size(size)
        End Function

        Public Function retrieve(ByVal type As String, ByRef size As UInt32) As Boolean
            Return sizes.find(type, size)
        End Function

        Public Function is_assignable(ByVal [from] As String, ByVal [to] As String) As Boolean
            Dim from_size As UInt32 = 0
            Dim to_size As UInt32 = 0
            Return retrieve([from], from_size) AndAlso
                   retrieve([to], to_size) AndAlso
                   is_size_or_variable(to_size, from_size)
        End Function

        Public Function is_assignable(ByVal [to] As String, ByVal size As UInt32) As Boolean
            Dim to_size As UInt32 = 0
            Return retrieve([to], to_size) AndAlso
                   is_size_or_variable(to_size, size)
        End Function
    End Class
End Namespace
