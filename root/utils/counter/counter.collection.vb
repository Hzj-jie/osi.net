
Imports osi.root.lock
Imports osi.root.formation
Imports osi.root.delegates
Imports osi.root.constants
Imports osi.root.connector

Namespace counter
    Public Module _counter_collection
        Private ReadOnly v As vector(Of counter_record) = Nothing
        Private l As duallock

        Sub New()
            v = New vector(Of counter_record)()
        End Sub

        Friend Function counter(ByVal index As Int64) As counter_record
            l.reader_wait()
            Try
                If v.available_index(index) Then
                    Return v(index)
                Else
                    Return Nothing
                End If
            Finally
                l.reader_release()
            End Try
        End Function

        Public Function counter(ByVal index As Int64,
                                ByRef name As String,
                                Optional ByRef count As Int64? = Nothing,
                                Optional ByRef average As Int64? = Nothing,
                                Optional ByRef lastAverage As Int64? = Nothing,
                                Optional ByRef rate As Int64? = Nothing,
                                Optional ByRef lastRate As Int64? = Nothing) As Boolean
            Dim cr As counter_record = Nothing
            cr = counter(index)
            Return Not cr Is Nothing AndAlso cr.value(name, count, average, lastAverage, rate, lastRate)
        End Function

        Public Function count() As Int64
            Return l.reader_locked(Function() v.size())
        End Function

        Friend Function insert(ByVal cr As counter_record) As Int64
            assert(Not cr Is Nothing)
            Return l.writer_locked(Function() As Int64
                                       Dim rtn As Int64 = 0
                                       rtn = v.size()
                                       v.push_back(cr)
                                       Return rtn
                                   End Function)
        End Function

        Friend Sub foreach(ByVal d As void(Of counter_record))
            assert(Not d Is Nothing)
            l.reader_locked(Sub()
                                assert(utils.foreach(AddressOf v.foreach, d))
                            End Sub)
        End Sub

        Friend Sub workon(ByVal i As Int64, ByVal d As void(Of counter_record))
            assert(Not d Is Nothing)
            l.reader_locked(Sub() d(v(i)))
        End Sub
    End Module
End Namespace
