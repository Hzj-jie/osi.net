
'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with cycle_case.vbp ----------
'so change cycle_case.vbp instead of this file



'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with qless_case.vbp ----------
'so change qless_case.vbp instead of this file



Imports System.Threading
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.utils
Imports osi.root.connector
Imports osi.root.utt
Imports osi.root.lock

#If False Then
Imports osi.root.lock.slimlock
Friend Class cycle_1024_128_case(Of lock_t As islimlock)
#Else
Friend Class cycle_1024_128_case
#End If
    Inherits random_run_case

#If False Then
    Private ReadOnly q As cycle_1024_128(Of Int64, lock_t)
#Else
    Private ReadOnly q As cycle_1024_128(Of Int64)
#End If
    Private ReadOnly max_iteration_count As Int64
    Private s() As Boolean
    Private i As Int64 = 0
#If True
    Private l As slimlock.monitorlock
#End If

    Private Function validate() As Boolean
        Return max_iteration_count > 0
    End Function

    Private Sub write()
        Dim j As Int64 = 0
#If True Then
        l.wait()
        j = i
        i += 1
        If Not q.push(j) Then
            i -= 1
        End If
        l.release()
#Else
        j = Interlocked.Increment(i) - 1
        assert(Not validate() OrElse j < array_size(s))
        q.push(j)
#End If
    End Sub

    Private Sub read()
        assert_more_or_equal(q.size(), uint32_0)
        Dim j As Int64 = 0
        If q.pop(j) AndAlso validate() Then
            If assert_less(j, i) AndAlso
               assert_more_or_equal(j, 0) Then
                assert_false(s(j), j)
                s(j) = True
            End If
        End If
    End Sub

    Public Sub New(Optional ByVal max_iteration_count As Int64 = 0)
        assert(max_iteration_count >= 0)
#If False Then
        q = New cycle_1024_128(Of Int64, lock_t)()
#Else
        q = New cycle_1024_128(Of Int64)()
#End If
        Me.max_iteration_count = max_iteration_count
        insert_call(0.6, AddressOf read)
        insert_call(0.4, AddressOf write)
    End Sub

    Public Overrides Function prepare() As Boolean
        If MyBase.prepare() Then
            ReDim s(max_iteration_count - 1)
            i = 0
            Return True
        Else
            Return False
        End If
    End Function

    Public Overrides Function finish() As Boolean
        assert_more_or_equal(q.size(), uint32_0)
        If validate() Then
            While Not q.empty()
                read()
            End While
        Else
            q.clear()
        End If
        assert_equal(q.size(), uint32_0)
        If validate() Then
            For j As Int64 = 0 To i - 1
                assert_true(s(j), j)
            Next
        End If
        Erase s
        Return MyBase.finish()
    End Function
End Class
'finish qless_case.vbp --------
'finish cycle_case.vbp --------
