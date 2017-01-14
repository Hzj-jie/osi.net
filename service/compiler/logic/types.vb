
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation

Namespace logic
    ' Defines primitive types, i.e. basic data types a language can handle, such as byte, int, byte array, etc. So the
    ' types instance won't be impacted by the source code or in another word, user input, of a language.
    Public Class types
        Public Const variable_type As String = "*"
        Private Const variable_size As UInt32 = max_uint32
        Private ReadOnly sizes As map(Of String, UInt32)

        Public Sub New()
            sizes = New map(Of String, UInt32)()
            define_variable_size(variable_type)
        End Sub

        Public Sub define(ByVal type As String, ByVal size As UInt32)
            assert(Not String.IsNullOrEmpty(type))
            assert(sizes.find(type) = sizes.end())
            sizes(type) = size
        End Sub

        Public Sub define_variable_size(ByVal type As String)
            define(type, variable_size)
        End Sub

        Public Shared Function is_variable_size(ByVal size As UInt32) As Boolean
            Return size = variable_size
        End Function

        Public Shared Function is_size_or_variable(ByVal size As UInt32, ByVal exp_size As UInt32) As Boolean
            Return size = exp_size OrElse is_variable_size(size)
        End Function

        Public Function retrieve(ByVal type As String, ByRef size As UInt32) As Boolean
            Return sizes.find(type, size)
        End Function

        Default Public Property D(ByVal type As String) As UInt32
            Get
                Dim o As UInt32 = 0
                assert(retrieve(type, o))
                Return o
            End Get
            Set(ByVal value As UInt32)
                define(type, value)
            End Set
        End Property
    End Class
End Namespace
