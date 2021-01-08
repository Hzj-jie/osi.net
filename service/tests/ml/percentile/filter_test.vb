
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utt
Imports osi.root.utt.attributes
Imports p = osi.service.ml.percentile

Namespace percentile
    <test>
    Public NotInheritable Class filter_test
        Private Shared Function samples(ByVal lower As UInt32, ByVal upper As UInt32) _
                                    As vector(Of tuple(Of String, UInt32))
            Return streams.range(lower, upper).
                           map(Function(ByVal i As Int32) As tuple(Of String, UInt32)
                                   Return tuple.of(strcat("""", i.ToString(), """"), CUInt(i))
                               End Function).
                           collect(Of vector(Of tuple(Of String, UInt32)))()
        End Function

        Private Shared Function samples() As vector(Of tuple(Of String, UInt32))
            Return samples(0, 100)
        End Function

        <test>
        Private Shared Sub ascent()
            Dim v As vector(Of tuple(Of String, UInt32)) = samples()
            assertion.equal(v.stream().filter(p.ascent.filter(v, 0.1)).collect(Of vector(Of tuple(Of String, UInt32))),
                            samples(0, 10))
        End Sub

        <test>
        Private Shared Sub desent()
            Dim v As vector(Of tuple(Of String, UInt32)) = samples()
            assertion.equal(v.stream().filter(p.descent.filter(v, 0.1)).collect(Of vector(Of tuple(Of String, UInt32))),
                            samples(90, 100))
        End Sub

        Private Sub New()
        End Sub
    End Class
End Namespace
