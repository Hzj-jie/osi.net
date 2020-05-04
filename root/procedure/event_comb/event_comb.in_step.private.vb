
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants

Partial Public Class event_comb
    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Function valid_working_step(ByVal i As Int32) As Boolean
        Return i < ds_len AndAlso i >= 0
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Function valid_step(ByVal i As Int32) As Boolean
        Return valid_working_step(i) OrElse i = end_step OrElse i = not_started_step
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Function assert_step_is_valid() As Boolean
        assert_in_lock()
        Return assert(valid_step([step]), "event ", callstack(), ":", [step])
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Function debug_assert_valid_step(ByVal i As Int32) As Boolean
        Return debug_assert(valid_working_step(i) OrElse i = end_step OrElse i = not_started_step)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Function in_step(ByVal exp As Int32) As Boolean
        assert_in_lock()
        Return assert_step_is_valid() AndAlso ([step] = exp)
    End Function

    'the step is in end_step, but may still have pendings
    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Function in_end_step() As Boolean
        Return in_step(end_step)
    End Function

    'the callback resume process is ready to be started, but it may or may not happen
    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Function _callback_resume_ready() As Boolean
        Return in_end_step() AndAlso not_pending()
    End Function

    'no pendings, the resume process has been finished,
    'since the resume will happen right after end_ticks has been set
    'if not being called within the lock, which usually happens in another sync thread,
    'do not guarantee the chained event_comb has been resumed already.
    'but for the event_comb waiting for this event_comb, it's surely safe.
    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Function _end() As Boolean
        assert_in_lock()
        Return end_ticks() <> npos AndAlso
               assert(end_result_raw().notunknown()) AndAlso
               assert(_callback_resume_ready())
    End Function

    'no pendings, but the resume process has not been started
    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Function _ending() As Boolean
        Return _callback_resume_ready() AndAlso Not _end()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Function _not_started() As Boolean
        Return in_step(not_started_step)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Function _started() As Boolean
        Return Not _not_started()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Function _working() As Boolean
        Return assert_step_is_valid() AndAlso valid_working_step([step])
    End Function
End Class
