
Imports osi.root.connector
Imports osi.root.formation

Partial Public Class dispenser(Of DATA_T, ID_T)
    Public Interface accepter
        Function accept(ByVal remote As ID_T) As Boolean
        Sub received(ByVal data As DATA_T, ByVal remote As ID_T)
    End Interface

    Public MustInherit Class multiple_accepter
        Implements accepter

        Private ReadOnly sources As const_array(Of ID_T)

        Public Sub New(ByVal sources As const_array(Of ID_T))
            Me.sources = sources
        End Sub

        Protected Overridable Function match(ByVal remote As ID_T, ByVal source As ID_T) As Boolean
            Return compare(remote, source) = 0
        End Function

        Public Function accept(ByVal remote As ID_T) As Boolean Implements accepter.accept
            If sources.null_or_empty() Then
                Return True
            Else
                For i As Int32 = 0 To sources.size() - 1
                    If match(remote, sources(i)) Then
                        Return True
                    End If
                Next
                Return False
            End If
        End Function

        Public MustOverride Sub received(ByVal data As DATA_T, ByVal remote As ID_T) Implements accepter.received
    End Class
End Class
