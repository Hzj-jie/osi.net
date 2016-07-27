
#If PENDING Then
Public Class sequentiallock
    Private Shared Sub amuwait()
        sleep(Rnd(0, 10, True))
    End Sub

    Private Shared Sub amuthread()
        assert(Not lock Is Nothing, "lock is nothing before amuthread.")

        'too waste time, so change testsize to 1024
        Dim testsize As Int64 = 1024
        If isreleasebuild() Then
            'add threadcount in release build, so decease the testsize to 4 times only
            testsize <<= 2
        End If

        Dim i As Int64
        For i = 0 To testsize - 1
            Dim tokengot As Int32
            If Rnd(0, 2, True) = 0 Then
                lock.wait(tokengot)
                raiseError("lock.wait got token " + Convert.ToString(tokengot))
                amuwait()
                raiseError("lock release with tokengot " + Convert.ToString(tokengot))
                lock.release(tokengot)
            Else
                lock.wait()
                raiseError("lock.wait without tokengot.")
                amuwait()
                raiseError("lock.release without tokengot.")
                lock.release()
            End If
        Next
    End Sub

    Private Shared lock As sequentiallock = Nothing

    Public Shared Sub automaticalUnittest()
        Dim threadcount As Int64 = 16
        If isreleasebuild() Then
            threadcount <<= 2 '64 threads in release build
        End If
        lock = New sequentiallock()

        raiseError("********** sequentialLock Automatical Unittest ***********", errorHandle.errorType.application)
        Dim threads(threadcount - 1) As Thread
        Dim i As Int64
        For i = 0 To threadcount - 1
            threads(i) = startThread(AddressOf amuthread)
        Next

        For i = 0 To threadcount - 1
            assert(Not threads(i) Is Nothing, "thread(" + Convert.ToString(i) + ") is nothing.")
            threads(i).Join()
        Next
        raiseError("******* finish sequentialLock Automatical Unittest *******", errorHandle.errorType.application)
    End Sub
End Class
#End If
