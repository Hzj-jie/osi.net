
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.envs

Public Module _trace
    Private ReadOnly enabled As Boolean

    Sub New()
        enabled = enable_trace OrElse enable_detail_trace
    End Sub

    <Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)>
    Public Sub trace(ByVal ParamArray msg() As Object)
        If enabled Then
            trace(1, msg)
        End If
    End Sub

    <Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)>
    Public Sub trace(ByVal additional_jump As Int32, ByVal ParamArray msg() As Object)
        If enabled Then
            If enable_detail_trace Then
                raise_error(error_type.trace,
                        character.null,
                        additional_jump + 1,
                        "thread id ",
                        current_thread_id(),
                        ", ",
                        msg,
                        ". Stack trace ",
                        callstack())
            Else
                raise_error(error_type.trace,
                        character.null,
                        additional_jump + 1,
                        "thread id ",
                        current_thread_id(),
                        ", ",
                        msg)
            End If
        End If
    End Sub
End Module
