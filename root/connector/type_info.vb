
Public Class type_info(Of T)
    ' Do not add type, refer to gettype_perf test, since this is a template class, cache GetType(T) result cannot
    ' provide benefit.
    ' Public Shared ReadOnly type As Type
    Public Shared ReadOnly fullname As String
    Public Shared ReadOnly name As String

    ' => GetType(T) Is GetType(Object)
    Public Shared ReadOnly is_object As Boolean
    ' => GetType(T).IsValueType
    Public Shared ReadOnly is_valuetype As Boolean

    Shared Sub New()
        ' type = GetType(T)
        fullname = GetType(T).FullName()
        name = GetType(T).Name()
        is_object = (GetType(T) Is GetType(Object))
        is_valuetype = GetType(T).IsValueType()
    End Sub

    Private Sub New()
    End Sub
End Class
