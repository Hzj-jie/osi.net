
Imports System.Threading
Imports osi.root.lock
Imports osi.root.utt

Public Class create_if_nothing_test
    Inherits rinne_case_wrapper

    Public Sub New()
        MyBase.New(multithreading(repeat(New create_if_nothing_case(), 1024), 32), 1024)
    End Sub

    Public Overrides Function reserved_processors() As Int16
        Return 1
    End Function

    Private Class create_if_nothing_case
        Inherits [case]

        Private Class C
        End Class

        Private i As C
        Private create_suc As Int32

        Public Overrides Function prepare() As Boolean
            i = Nothing
            create_suc = 0
            Return MyBase.prepare()
        End Function

        Public Overrides Function run() As Boolean
            If atomic.create_if_nothing(i) Then
                assertion.equal(Interlocked.Increment(create_suc), 1)
            End If
            Return True
        End Function

        Public Overrides Function finish() As Boolean
            assertion.equal(create_suc, 1)
            Return MyBase.finish()
        End Function
    End Class
End Class
