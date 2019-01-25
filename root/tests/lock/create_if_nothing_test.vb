
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Threading
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.lock
Imports osi.root.utt

Public NotInheritable Class create_if_nothing_test
    Inherits rinne_case_wrapper

    Public Sub New()
        MyBase.New(multithreading(repeat(New create_if_nothing_case(), 8), 32), 1024)
    End Sub

    Public Overrides Function reserved_processors() As Int16
        Return 1
    End Function

    Private NotInheritable Class create_if_nothing_case
        Inherits [case]

        Private Class C
            Public ReadOnly s As Int64

            Public Sub New()
                s = rnd_int64()
            End Sub
        End Class

        Private Const default_s As Int64 = min_int64
        Private i As C
        Private create_suc As Int32
        Private s As Int64

        Public Overrides Function prepare() As Boolean
            i = Nothing
            create_suc = 0
            s = default_s
            Return MyBase.prepare()
        End Function

        Public Overrides Function run() As Boolean
            If atomic.create_if_nothing(i) Then
                ' This operation does not require atomic, the s should be either default_s or i.s
                If s = default_s Then
                    atomic.eva(s, i.s)
                End If
                assertion.equal(s, i.s)
                assertion.equal(Interlocked.Increment(create_suc), 1)
            End If
            Return True
        End Function

        Public Overrides Function finish() As Boolean
            assertion.equal(create_suc, 1)
            assertion.not_equal(s, default_s)
            Return MyBase.finish()
        End Function
    End Class
End Class
