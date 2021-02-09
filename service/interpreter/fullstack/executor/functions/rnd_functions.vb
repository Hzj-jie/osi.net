
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants
Imports osi.root.connector

Namespace fullstack.executor
    Public NotInheritable Class rnd_int_function
        Inherits [function]

        Public Sub New(ByVal input_count As Int32)
            MyBase.New(type.int,
                       If(input_count = 0,
                          Nothing,
                       If(input_count = 1,
                          New type() {type.int},
                       If(input_count = 2,
                          New type() {type.int, type.int},
                       assert_which.of(Of type())(Nothing).is_not_null()))))
        End Sub

        Protected Overrides Function execute(ByVal inputs() As Object) As Object
            assert(array_size(inputs) <= 2)
            Dim min As Int32 = 0
            Dim max As Int32 = 0
            min = min_int32
            max = max_int32
            If array_size(inputs) > 0 Then
#If DEBUG Then
                assert(TypeOf inputs(0) Is Int32)
#End If
                min = CInt(inputs(0))
                If array_size(inputs) > 1 Then
#If DEBUG Then
                    assert(TypeOf inputs(1) Is Int32)
#End If
                    max = CInt(inputs(1))
                End If
            End If
            Return rnd_int(min, max)
        End Function
    End Class
End Namespace
