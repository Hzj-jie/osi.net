
Imports System.Runtime.CompilerServices
Imports osi.root.connector

Namespace turing
    Public Class type
        Public Shared ReadOnly [byte] As type
        Public Shared ReadOnly bool As type
        Public Shared ReadOnly int As type
        Public Shared ReadOnly float As type
        Public Shared ReadOnly [char] As type
        Public Shared ReadOnly [string] As type

        Public Enum def As Int32
            [byte] = 0
            bool
            int
            float
            [char]
            [string]
        End Enum

        Public ReadOnly definition As def

        Shared Sub New()
            [byte] = New type(def.byte)
            bool = New type(def.bool)
            int = New type(def.int)
            float = New type(def.float)
            [char] = New type(def.char)
            [string] = New type(def.string)
        End Sub

        Private Sub New(ByVal definition As def)
            Me.definition = definition
        End Sub
    End Class

    Public Module _type
        <Extension()> Public Function is_byte(ByVal i As type) As Boolean
            assert(Not i Is Nothing)
            Return i.definition = type.def.byte
        End Function

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

        <Extension()> Public Function match(ByVal this As type, ByVal that As type) As Boolean
            assert(Not this Is Nothing)
            assert(Not that Is Nothing)
            Return this.definition = that.definition
        End Function

        <Extension()> Public Function match(ByVal i As type, ByVal value As Object) As Boolean
            assert(Not i Is Nothing)
            Return (i.is_byte() AndAlso TypeOf value Is Byte) OrElse
                   (i.is_bool() AndAlso TypeOf value Is Boolean) OrElse
                   (i.is_int() AndAlso TypeOf value Is Int32) OrElse
                   (i.is_float() AndAlso TypeOf value Is Double) OrElse
                   (i.is_char() AndAlso TypeOf value Is Char) OrElse
                   (i.is_string() AndAlso TypeOf value Is String)
        End Function
    End Module
End Namespace
