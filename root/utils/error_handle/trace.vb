
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.envs

Public Module _trace
    Private ReadOnly enabled As Boolean = enable_trace OrElse enable_detail_trace

    <Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)>
    Public Sub trace(ByVal ParamArray msg() As Object)
        If enabled Then
            trace(1, msg)
        End If
    End Sub

    <Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)>
    Public Sub trace(ByVal additional_jump As Int32, ByVal ParamArray msg() As Object)
        If Not enabled Then
            Return
        End If
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
    End Sub
End Module
