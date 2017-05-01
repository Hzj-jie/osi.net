
Imports osi.root.connector
Imports osi.root.template

'usually i should use compare function directly, but to make the compare a little bit quicker
Public Interface icomparer(Of T)
    Function equal(ByVal x As T, ByVal y As T) As Boolean
    Function less(ByVal x As T, ByVal y As T) As Boolean
    Function less_or_equal(ByVal x As T, ByVal y As T) As Boolean
    Function compare(ByVal x As T, ByVal y As T) As Int32
End Interface

Public Class comparer(Of T)
    Implements icomparer(Of T)

    Public Shared ReadOnly instance As icomparer(Of T)

    Shared Sub New()
        instance = New comparer(Of T)()
    End Sub

    Protected Sub New()
    End Sub

    Public Overridable Function equal(ByVal x As T, ByVal y As T) As Boolean Implements icomparer(Of T).equal
        Return compare(x, y) = 0
    End Function

    Public Overridable Function less(ByVal x As T, ByVal y As T) As Boolean Implements icomparer(Of T).less
        Return compare(x, y) < 0
    End Function

    Public Overridable Function less_or_equal(ByVal x As T, ByVal y As T) As Boolean _
                                             Implements icomparer(Of T).less_or_equal
        Return less(x, y) OrElse equal(x, y)
    End Function

    Public Overridable Function compare(ByVal x As T, ByVal y As T) As Int32 Implements icomparer(Of T).compare
        If less(x, y) Then
            Return -1
        ElseIf equal(x, y) Then
            Return 0
        Else
            Return 1
        End If
    End Function
End Class

Public Class int_comparer
    Inherits comparer(Of Int32)

    Public Shared Shadows ReadOnly instance As int_comparer

    Shared Sub New()
        instance = New int_comparer()
    End Sub

    Private Sub New()
    End Sub

    Public Overrides Function equal(ByVal x As Int32, ByVal y As Int32) As Boolean
        Return x = y
    End Function

    Public Overrides Function less(ByVal x As Int32, ByVal y As Int32) As Boolean
        Return x < y
    End Function

    Public Overrides Function less_or_equal(ByVal x As Int32, ByVal y As Int32) As Boolean
        Return x <= y
    End Function
End Class

Public Class int64_comparer
    Inherits comparer(Of Int64)

    Public Shared Shadows ReadOnly instance As int64_comparer

    Shared Sub New()
        instance = New int64_comparer()
    End Sub

    Private Sub New()
    End Sub

    Public Overrides Function equal(ByVal x As Int64, ByVal y As Int64) As Boolean
        Return x = y
    End Function

    Public Overrides Function less(ByVal x As Int64, ByVal y As Int64) As Boolean
        Return x < y
    End Function

    Public Overrides Function less_or_equal(ByVal x As Int64, ByVal y As Int64) As Boolean
        Return x <= y
    End Function
End Class

Public Class double_comparer
    Inherits comparer(Of Double)

    Public Shared Shadows ReadOnly instance As double_comparer

    Shared Sub New()
        instance = New double_comparer()
    End Sub

    Private Sub New()
    End Sub

    Public Overrides Function equal(ByVal x As Double, ByVal y As Double) As Boolean
        Return x = y
    End Function

    Public Overrides Function less(ByVal x As Double, ByVal y As Double) As Boolean
        Return x < y
    End Function

    Public Overrides Function less_or_equal(ByVal x As Double, ByVal y As Double) As Boolean
        Return x <= y
    End Function
End Class

Public Class string_comparer(Of case_sensitive As _boolean)
    Inherits comparer(Of String)

    Public Shared Shadows ReadOnly instance As string_comparer(Of case_sensitive)
    Private Shared ReadOnly cc As Boolean

    Shared Sub New()
        cc = +(alloc(Of case_sensitive)())
        instance = New string_comparer(Of case_sensitive)()
    End Sub

    Protected Sub New()
    End Sub

    Public Overrides Function equal(ByVal x As String, ByVal y As String) As Boolean
        Return strsame(x, y, cc)
    End Function

    Public Overrides Function less(ByVal x As String, ByVal y As String) As Boolean
        Return strcmp(x, y, cc) < 0
    End Function

    Public Overrides Function less_or_equal(ByVal x As String, ByVal y As String) As Boolean
        Return strcmp(x, y, cc) <= 0
    End Function
End Class

Public Class string_comparer
    Inherits string_comparer(Of _false)
End Class

Public Class string_case_sensitive_comparer
    Inherits string_comparer(Of _true)
End Class

Public Class bytes_comparer
    Inherits comparer(Of Byte())

    Public Shared Shadows ReadOnly instance As bytes_comparer

    Shared Sub New()
        instance = New bytes_comparer()
    End Sub

    Private Sub New()
    End Sub

    Public Overrides Function equal(ByVal x() As Byte, ByVal y() As Byte) As Boolean
        Return memcmp(x, y) = 0
    End Function

    Public Overrides Function less(ByVal x() As Byte, ByVal y() As Byte) As Boolean
        Return memcmp(x, y) < 0
    End Function

    Public Overrides Function less_or_equal(ByVal x() As Byte, ByVal y() As Byte) As Boolean
        Return memcmp(x, y) <= 0
    End Function
End Class
