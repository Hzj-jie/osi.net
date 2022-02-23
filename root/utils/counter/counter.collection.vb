
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.lock

Namespace counter
    Public Module _counter_collection
        Private ReadOnly v As New vector(Of counter_record)()
        Private l As duallock

        Friend Function counter(ByVal index As Int64) As counter_record
            l.reader_wait()
            Try
                Dim o As counter_record = Nothing
                If v.take(index, o) Then
                    Return o
                Else
                    Return Nothing
                End If
            Finally
                l.reader_release()
            End Try
        End Function

        Public Function snapshot(ByVal index As Int64) As snapshot
            Dim cr As counter_record = Nothing
            cr = counter(index)
            If cr Is Nothing Then
                Return Nothing
            Else
                Return cr.snapshot()
            End If
        End Function

        Public Function count() As Int64
            Return l.reader_locked(Function() v.size())
        End Function

        Friend Function insert(ByVal cr As counter_record) As Int64
            assert(cr IsNot Nothing)
            Return l.writer_locked(Function() As Int64
                                       Dim rtn As Int64 = 0
                                       rtn = v.size()
                                       v.push_back(cr)
                                       Return rtn
                                   End Function)
        End Function

        Friend Sub foreach(ByVal d As Action(Of counter_record))
            assert(d IsNot Nothing)
            l.reader_locked(Sub()
                                v.stream().foreach(d)
                            End Sub)
        End Sub

        Friend Sub workon(ByVal i As Int64, ByVal d As Action(Of counter_record))
            assert(d IsNot Nothing)
            assert(i <= max_uint32)
            assert(i >= 0)
            l.reader_locked(Function() As Boolean
                                d(counter(i))
                                Return True
                            End Function)
        End Sub
    End Module
End Namespace
