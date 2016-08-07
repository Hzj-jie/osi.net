
'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with iqless_threadpool.vbp ----------
'so change iqless_threadpool.vbp instead of this file



Imports System.DateTime
Imports System.Threading
Imports osi.root.constants
Imports osi.root.template
Imports osi.root.envs
Imports osi.root.formation
Imports osi.root.connector
Imports osi.root.lock
Imports osi.root.utils
Imports counter = osi.root.utils.counter

Public NotInheritable Class slimqless2_threadpool
    Inherits threadpool

    Private Const low_pri_rate As Double = 0
    Private Shared ReadOnly wait_time_ms As Int32 = sixteen_timeslice_length_ms
    <ThreadStatic()> Private Shared managed_thread As Boolean
    Private ReadOnly q As slimqless2(Of work_info) = Nothing
    Private ReadOnly threads As vector(Of Thread) = Nothing
    'AutoResetEvent is pretty slow
    Private ReadOnly are As AutoResetEvent = Nothing
    ' Private it As Int32 = 0

    Shared Sub New()
        assert(managed_thread = False)
    End Sub

    Public Shared Function in_managed_thread() As Boolean
        Return managed_thread
    End Function

    Public Overrides Function wait_job(Optional ByVal ms As Int64 = npos) As Boolean
        'Interlocked.Increment(it)
        If ms >= 0 Then
            If busy_wait Then
                Dim start_ms As Int64 = 0
                start_ms = nowadays.milliseconds()
                wait_when(Function() Not (are.WaitOne(0) OrElse stopping() OrElse nowadays.milliseconds() > start_ms + ms))
                Return are.WaitOne(0)
            Else
                Return are.WaitOne(ms)
            End If
        Else
            If busy_wait Then
                wait_when(Function() Not (are.WaitOne(0) OrElse stopping()))
            Else
                assert(are.WaitOne() OrElse stopping())
            End If
            Return True
        End If
        'are.WaitOne(wait_time_ms)
        'sleep(wait_time_ms)
        'Interlocked.Decrement(it)
    End Function

    Private Sub set_job()
        'If working_threads() = 0 Then
        '    are.Set()
        'End If
        'If it > 0 Then
        '    are.Set()
        'End If
        are.Set()
    End Sub

    Protected Overrides Sub queue_job(ByVal wi As work_info)
        assert(Not wi Is Nothing)
#If False Then
        assert(q.emplace(wi))
#Else
        q.emplace(wi)
#End If
        set_job()
    End Sub

    Public Overrides Function execute_job() As Boolean
        Dim wi As work_info = Nothing
        If q.pop(wi) Then
            work_on(wi)
            Return True
        Else
            Return False
        End If
    End Function

    Private Function typedname(ByVal s As String) As String
        Return strcat(TYPE_NAME, s)
    End Function

    Private Function upper_typedname(ByVal s As String) As String
        Return strtoupper(typedname(s))
    End Function

    Public Overrides Property thread_count() As UInt32
        Get
            Return threads.size()
        End Get
        Set(ByVal value As UInt32)
            If value = 0 Then
                value = default_thread_count
            End If
            If threads.size() > value Then
                [stop]()
                reset_stop_signal()
            End If
            While threads.size() < value
                Dim id As UInt32 = 0
                id = threads.size()
                threads.emplace_back(New Thread(Sub()
                                                    assert(processor_affinity = npos OrElse
                                                           (processor_affinity >= 0 AndAlso
                                                            processor_affinity < Environment.ProcessorCount()))
                                                    If processor_affinity <> npos Then
                                                        loop_set_thread_affinity(CUInt(processor_affinity) +
                                                                                 queue_runner.thread_count +
                                                                                 id)
                                                    End If
                                                    managed_thread = True
                                                    worker()
                                                End Sub))
                Dim t As Thread = Nothing
                t = threads.back()
                If low_pri_rate > 0 Then
                    If threads.size() <= value / (1 + low_pri_rate) Then
                        t.Name() = upper_typedname("_workthread_hi_pri")
                        t.Priority() = ThreadPriority.Highest
                    Else
                        t.Name() = upper_typedname("_workthread_low_pri")
                        t.Priority() = ThreadPriority.Lowest
                    End If
                Else
                    t.Name() = upper_typedname("_workthread")
                End If
                t.Start()
            End While
        End Set
    End Property

    Public Sub New(Optional ByVal thread_count As UInt32 = 0)
        MyBase.New()
        q = New slimqless2(Of work_info)()
        threads = New vector(Of Thread)()
        are = New AutoResetEvent(False)
        Me.thread_count() = thread_count
    End Sub

    Protected Overrides Function managed_threads() As Thread()
        Return +threads
    End Function

    Protected Overrides Sub Finalize()
        are.Close()
        MyBase.Finalize()
    End Sub

    ' Consumers should use registry.vb
    Friend Shared Shadows Sub register()
        threadpool.register(New slimqless2_threadpool())
    End Sub
End Class
'finish iqless_threadpool.vbp --------
