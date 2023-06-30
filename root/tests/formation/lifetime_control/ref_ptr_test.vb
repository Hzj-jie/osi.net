
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.lock
Imports osi.root.utt

Public NotInheritable Class ref_ptr_test
    Inherits multithreading_case_wrapper

    Private Const repeat_count As Int64 = 32
    Private Const thread_count As Int16 = 64

    Public Sub New()
        MyBase.New(repeat(New ref_ptr_case(), repeat_count), thread_count)
    End Sub

    Public Overrides Function reserved_processors() As Int16
        Return 1
    End Function

    <Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable")>
    Private NotInheritable Class ref_ptr_case
        Inherits [case]

        Private i As ref_ptr(Of atomic_int)

        Public Overrides Function prepare() As Boolean
            If MyBase.prepare() Then
                i = New ref_ptr(Of atomic_int)(New atomic_int(),
                                               disposer:=Sub(x As atomic_int)
                                                             If assertion.is_not_null(x) AndAlso
                                                                assertion.equal(object_compare(x, i.get()), 0) Then
                                                                 x.increment()
                                                             End If
                                                         End Sub)
                Return True
            Else
                Return False
            End If
        End Function

        Public Overrides Function run() As Boolean
            If rnd_bool() Then
                sleep(rnd_int(0, 100))
            End If
            i.ref()
            If rnd_bool() Then
                sleep(rnd_int(0, 100))
            End If
            i.unref()
            Return True
        End Function

        Public Overrides Function finish() As Boolean
            assertion.equal(i.ref(), uint32_2)
            assertion.equal(i.unref(), uint32_1)
            assertion.equal(i.unref(), uint32_0)
            assertion.equal(++i, 1)
            Return MyBase.finish()
        End Function
    End Class
End Class
