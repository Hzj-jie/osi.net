
Imports System.Threading
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.utils
Imports osi.root.delegates
Imports tp = System.Threading.ThreadPool

Public Class managed_threadpool
    Inherits threadpool

    Private Sub New(Optional ByVal thread_count As UInt32 = uint32_0)
        MyBase.New()
#If 0 Then
        If thread_count = 0 Then
            thread_count = default_thread_count
        End If
        Me.thread_count() = thread_count
#End If
    End Sub

    Public Overrides Property thread_count() As UInt32
        Get
            Dim ft As Int32 = 0
            tp.GetMaxThreads(ft, Nothing)
            If ft <= 0 Then
                Return 0
            Else
                Return ft
            End If
        End Get
        Set(ByVal value As UInt32)
            ' No effect for .net managed thread pool
#If 0 Then
            Dim bt As Int32 = 0
            tp.GetMaxThreads(Nothing, bt)
            tp.SetMaxThreads(value, bt)
            tp.GetMinThreads(Nothing, bt)
            tp.SetMinThreads(value, bt)
#End If
        End Set
    End Property

    Protected Overrides Function managed_threads() As Thread()
        Return Nothing
    End Function

    Protected Overrides Sub queue_job(ByVal wi As work_info)
        queue_in_managed_threadpool(AddressOf work_on, wi)
    End Sub

    Protected Overrides Function stoppable() As Boolean
        Return False
    End Function

    Public Shared ReadOnly [global] As managed_threadpool = Nothing

    Shared Sub New()
        [global] = New managed_threadpool()
    End Sub

    Public Shared Shadows Sub register()
        threadpool.register([global])
    End Sub
End Class
