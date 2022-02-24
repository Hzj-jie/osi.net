
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector

Public Module _event_comb_extensions
    <Extension()> Public Function success_finished(ByVal this As event_comb) As Boolean
        Return Not this Is Nothing AndAlso this.end() AndAlso this.end_result()
    End Function

    <Extension()> Public Function cancel(ByVal ecs() As event_comb) As Boolean
        For i As Int32 = 0 To array_size_i(ecs) - 1
            If ecs(i) Is Nothing Then
                Return False
            End If
            ecs(i).cancel()
        Next
        Return True
    End Function

    <Extension()> Public Function [end](ByVal ecs() As event_comb) As Boolean
        For i As Int32 = 0 To array_size_i(ecs) - 1
            If ecs(i) Is Nothing OrElse Not ecs(i).end() Then
                Return False
            End If
        Next
        Return True
    End Function

    <Extension()> Public Function end_result(ByVal ecs() As event_comb) As Boolean
        For i As Int32 = 0 To array_size_i(ecs) - 1
            If ecs(i) Is Nothing OrElse Not ecs(i).end_result() Then
                Return False
            End If
        Next
        Return True
    End Function

    <Extension()> Public Function any_end_result(ByVal ecs() As event_comb) As Boolean
        Dim r As Boolean = False
        For i As Int32 = 0 To array_size_i(ecs) - 1
            If ecs(i) Is Nothing Then
                Return False
            ElseIf ecs(i).end_result() Then
                r = True
            End If
        Next
        Return r
    End Function

    <Extension()> Public Function end_result_or_null(ByVal ec As event_comb) As Boolean
        Return ec Is Nothing OrElse ec.end_result()
    End Function

    <Extension()> Public Function end_result_or_null(ByVal ecs() As event_comb) As Boolean
        For i As Int32 = 0 To array_size_i(ecs) - 1
            If Not ecs(i) Is Nothing AndAlso Not ecs(i).end_result() Then
                Return False
            End If
        Next
        Return True
    End Function

    <Extension()> Public Function any_end_result_or_null(ByVal ecs() As event_comb) As Boolean
        For i As Int32 = 0 To array_size_i(ecs) - 1
            If Not ecs(i) Is Nothing AndAlso ecs(i).end_result() Then
                Return True
            End If
        Next
        Return False
    End Function

    Friend Function create(ByVal precondition As Func(Of Boolean),
                           ByVal ctor As Func(Of event_comb)) As event_comb
        assert(Not precondition Is Nothing)
        assert(Not ctor Is Nothing)
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  If precondition() Then
                                      ec = ctor()
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  Else
                                      Return False
                                  End If
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function
End Module
