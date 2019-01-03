
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.utils
Imports osi.root.utt

Public Class zipgen_test
    Inherits [case]

    Public Sub New()
        If isemptyarray(data) OrElse isemptyarray(zipdata) Then
            static_constructor.execute(GetType(_data))
            static_constructor.execute(GetType(_zipdata))
        End If
    End Sub

    Public Overrides Function run() As Boolean
        If assertion.equal(array_size(data), CUInt(1349206)) AndAlso
           assertion.equal(array_size(data), array_size(zipdata)) Then
            For i As UInt32 = 0 To array_size(data) - 1
                assertion.equal(data(i), zipdata(i))
            Next
        End If
        Return True
    End Function

    Protected Overrides Sub Finalize()
        Dim v As valuer(Of Byte()) = Nothing
        v = New valuer(Of Byte())(GetType(_data), binding_flags.static_public, "data")
        v.set(Nothing)
        v = New valuer(Of Byte())(GetType(_zipdata), binding_flags.static_public, "zipdata")
        v.set(Nothing)
        MyBase.Finalize()
    End Sub
End Class
