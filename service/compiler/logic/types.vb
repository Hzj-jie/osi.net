
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

Namespace logic
    ' Defines primitive types, i.e. basic data types a language can handle, such as byte, int, byte array, etc. So the
    ' types instance won't be impacted by the source code or in another word, user input, of a language.
    Public NotInheritable Class types
        Public Shared ReadOnly [default] As New types()
        Public Const variable_type As String = "type*"
        Public Const heap_ptr_type As String = "type_ptr"
        Public Const zero_type As String = "type0"
        Private Const variable_size As UInt32 = max_int32
        Private Shared ReadOnly heap_size As UInt32 = sizeof_uint64
        Private Const zero_size As UInt32 = uint32_0
        Private ReadOnly sizes As New unordered_map(Of String, UInt32)

        Public Sub New()
            assert_define(variable_type, variable_size)
            assert_define(heap_ptr_type, heap_size)
            assert_define(zero_type, zero_size)
        End Sub

        Public Function define(ByVal type As String, ByVal size As UInt32) As Boolean
            assert(object_compare(Me, [default]) <> 0)
            assert(Not type.null_or_whitespace())
            If size = variable_size AndAlso Not strsame(type, variable_type) Then
                Return False
            End If
            If size = zero_size AndAlso Not strsame(type, zero_type) Then
                Return False
            End If
            If sizes.find(type) <> sizes.end() Then
                Return False
            End If
            sizes(type) = size
            Return True
        End Function

        Public Sub assert_define(ByVal type As String, ByVal size As UInt32)
            assert(define(type, size))
        End Sub

        Public Shared Function is_variable_size(ByVal size As UInt32) As Boolean
            Return size >= variable_size
        End Function

        Public Shared Function is_zero_size(ByVal size As UInt32) As Boolean
            Return size = uint32_0
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
