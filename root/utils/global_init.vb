
Option Explicit On
Option Infer Off
Option Strict On

' TODO: Remove inited and times, move to connector.
Imports System.Reflection
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.lock
Imports global_init_attribute = osi.root.constants.global_initAttribute
Imports lock_t = osi.root.lock.slimlock.monitorlock

Public NotInheritable Class global_init
    Private Shared ReadOnly times As atomic_int
    Private Shared ReadOnly inited As [set](Of comparable_type)
    Private Shared initiating As lock_t

    Shared Sub New()
        times = New atomic_int()
        inited = New [set](Of comparable_type)()
    End Sub

    Public Shared Function init_times() As Int32
        Return (+times)
    End Function

    Private Shared Function is_global_init_type(ByVal t As Type, ByRef gi As global_init_attribute) As Boolean
        assert(Not t Is Nothing)
        Return t.custom_attribute(gi)
    End Function

    Private Shared Sub execute_init(ByVal t As Type)
        assert(Not t Is Nothing)
        assert(Not t.is_unspecified_generic_type())
        Dim m As invoker(Of Action) = Nothing
        invoker.of(m).
            with_type(t).
            with_binding_flags(binding_flags.static_all_method).
            with_name("init").
            with_suppress_error(True).
            build(m)
        Dim v As Action = Nothing
        If Not m Is Nothing AndAlso m.pre_bind(v) Then
            Try
                v()
            Catch ex As Exception
                raise_error(error_type.warning,
                            "failed to invoke init in type ",
                            t.FullName(),
                            ", ex ",
                            ex.Message())
            End Try
        Else
            Using scoped.atomic_bool(suppress.alloc_error)
                If t.allocate() Is Nothing Then
                    ' A public or private class should have an constructor.
                    ' A module may not have a contructor, but nothing else can be done.
                    ' t.allocate() returns null if the constructor threw an exception. But the static constructor should be
                    ' triggered already. Though the implementation may not be correct, it's not good to assert here.
                    raise_error(error_type.warning, "may fail to invoke any initialization functions in type ", t.FullName())
                End If
            End Using
        End If
    End Sub

    Private Shared Function not_initialized(ByVal t As Type) As Boolean
        Dim ct As comparable_type = Nothing
        ct = New comparable_type(t)
        SyncLock inited
            If inited.find(ct) = inited.end() Then
                assert(inited.insert(ct).second)
                Return True
            Else
                Return False
            End If
        End SyncLock
    End Function

    Public Shared Sub execute(Optional ByVal level As Byte = global_init_level.all,
                              Optional ByVal load_assemblies As Boolean = False)
        If load_assemblies Then
            AppDomain.CurrentDomain().load_all({envs.application_directory,
                                                Environment.CurrentDirectory()})
        End If
        times.increment()
        Dim its() As vector(Of Type) = Nothing
        ReDim its(level)
        For Each i As Assembly In AppDomain.CurrentDomain().GetAssemblies()
            assert(Not i Is Nothing)
            Try
                For Each j As Type In i.GetTypes()
                    Dim gi As global_init_attribute = Nothing
                    If is_global_init_type(j, gi) AndAlso
                       assert(Not gi Is Nothing) AndAlso
                       (Not gi.init_once OrElse
                        not_initialized(j)) AndAlso
                       gi.level <= level Then
                        If its(gi.level) Is Nothing Then
                            its(gi.level) = New vector(Of Type)()
                        End If
                        its(gi.level).emplace_back(j)
                    End If
                Next
            Catch ex As Exception
                raise_error("Failed to load types from assembly ", i, ", ex ", ex.details())
            End Try
        Next
        initiating.wait()
        For k As Int32 = 0 To level
            concurrency_runner.execute(AddressOf execute_init, +its(k))
        Next
        initiating.release()
    End Sub

    Private Sub New()
    End Sub
End Class
