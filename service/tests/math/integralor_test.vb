
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.utt
Imports osi.root.utt.attributes
Imports osi.service.math

<test>
Public NotInheritable Class integralor_test
    <test>
    Private Shared Sub case1()
        assertions.of(New integralor().
                            with_function(Function(ByVal i As Double) As Double
                                              Return i
                                          End Function).
                            with_start(0).
                            with_end(1).
                            with_incremental(0.01).calculate()).
                     in_range(0.5)
    End Sub

    <test>
    Private Shared Sub case2()
        assertions.of(New integralor().
                            with_function(Function(ByVal i As Double) As Double
                                              Return i * i
                                          End Function).
                            with_start(-1).
                            with_end(1).
                            with_incremental(0.01).calculate()).
                     in_range(0.666666)
    End Sub

    Private Sub New()
    End Sub
End Class
