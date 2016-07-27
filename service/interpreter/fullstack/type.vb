
Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.utils

Namespace fullstack
    Public Class type
        Public Shared ReadOnly bool As type
        Public Shared ReadOnly int As type
        Public Shared ReadOnly float As type
        Public Shared ReadOnly [char] As type
        Public Shared ReadOnly [string] As type
        Public Shared ReadOnly var As type

        Public Enum def As Int32
            bool = 0
            int
            float
            [char]
            [string]
            var

            struct
        End Enum

        Public ReadOnly definition As def
        Public ReadOnly ctypes() As type

        Shared Sub New()
            bool = New type(def.bool)
            int = New type(def.int)
            float = New type(def.float)
            [char] = New type(def.char)
            [string] = New type(def.string)
            var = New type(def.var)
        End Sub

        Public Sub New(ByVal ctypes() As type)
            assert(Not isemptyarray(ctypes))
            Me.definition = def.struct
            Me.ctypes = ctypes
        End Sub

        Private Sub New(ByVal definition As def)
            assert(definition <> def.struct)
            Me.definition = definition
        End Sub

        Public Function ctype_count() As Int32
            Return array_size(ctypes)
        End Function
    End Class

    Public Module _type
        <Extension()> Public Function is_bool(ByVal i As type) As Boolean
            assert(Not i Is Nothing)
            Return i.definition = type.def.bool
        End Function

        <Extension()> Public Function is_int(ByVal i As type) As Boolean
            assert(Not i Is Nothing)
            Return i.definition = type.def.int
        End Function

        <Extension()> Public Function is_float(ByVal i As type) As Boolean
            assert(Not i Is Nothing)
            Return i.definition = type.def.float
        End Function

        <Extension()> Public Function is_char(ByVal i As type) As Boolean
            assert(Not i Is Nothing)
            Return i.definition = type.def.char
        End Function

        <Extension()> Public Function is_string(ByVal i As type) As Boolean
            assert(Not i Is Nothing)
            Return i.definition = type.def.string
        End Function

        <Extension()> Public Function is_var(ByVal i As type) As Boolean
            assert(Not i Is Nothing)
            Return i.definition = type.def.var
        End Function

        <Extension()> Public Function is_struct(ByVal i As type) As Boolean
            assert(Not i Is Nothing)
            Return i.definition = type.def.struct
        End Function

        <Extension()> Public Function is_type(ByVal this As type, ByVal that As type) As Boolean
            assert(Not this Is Nothing)
            assert(Not that Is Nothing)
            If object_compare(this, that) = 0 Then
                Return True
            ElseIf this.definition = that.definition Then
                If this.is_struct() Then
                    If this.ctype_count() <> that.ctype_count() Then
                        Return False
                    Else
                        For i As Int32 = 0 To this.ctype_count() - 1
                            If Not is_type(this.ctypes(i), that.ctypes(i)) Then
                                Return False
                            End If
                        Next
                        Return True
                    End If
                Else
                    Return True
                End If
            Else
                Return False
            End If
        End Function
    End Module
End Namespace
