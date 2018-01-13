
' TODO: Remove
Public Interface icaster(Of In T1, T2)
    Function cast(ByVal i As T1, ByRef o As T2) As Boolean
End Interface

Public Class caster(Of T1, T2)
    Implements icaster(Of T1, T2)

    Public Function cast(ByVal i As T1, ByRef o As T2) As Boolean Implements icaster(Of T1, T2).cast
        Return osi.root.connector.cast(i, o)
    End Function
End Class

Public Class string_int_caster
    Implements icaster(Of String, Int32)

    Public Shared ReadOnly instance As string_int_caster

    Shared Sub New()
        instance = New string_int_caster()
    End Sub

    Private Sub New()
    End Sub

    Public Function cast(ByVal i As String, ByRef o As Int32) As Boolean Implements icaster(Of String, Int32).cast
        Return Int32.TryParse(i, o)
    End Function
End Class

Public Class string_double_caster
    Implements icaster(Of String, Double)

    Public Shared ReadOnly instance As string_double_caster

    Shared Sub New()
        instance = New string_double_caster()
    End Sub

    Private Sub New()
    End Sub

    Public Function cast(ByVal i As String, ByRef o As Double) As Boolean Implements icaster(Of String, Double).cast
        Return Double.TryParse(i, o)
    End Function
End Class

Public Class string_string_caster
    Implements icaster(Of String, String)

    Public Shared ReadOnly instance As string_string_caster

    Shared Sub New()
        instance = New string_string_caster()
    End Sub

    Public Function cast(ByVal i As String, ByRef o As String) As Boolean Implements icaster(Of String, String).cast
        o = i
        Return True
    End Function
End Class