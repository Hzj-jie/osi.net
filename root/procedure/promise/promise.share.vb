
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.lock

Partial Public NotInheritable Class promise
    Public Shared Function all(ByVal ParamArray promises() As promise) As promise
        If isemptyarray(promises) Then
            Return resolve(Nothing)
        Else
            Dim r As promise = Nothing
            r = New promise()
            Dim c As atomic_uint = Nothing
            c = New atomic_uint(array_size(promises))
            For i As Int32 = 0 To array_size_i(promises) - 1
                If promises(i) Is Nothing Then
                    r._reject(strcat("promises", i, " is nothing"))
                Else
                    promises(i).then(Sub(ByVal result As Object)
                                         If c.decrement() = uint32_0 Then
                                             r._resolve(Nothing)
                                         End If
                                     End Sub,
                                     Sub(ByVal reason As Object)
                                         r._reject(reason)
                                     End Sub)
                End If
            Next
            Return r
        End If
    End Function

    Public Shared Function race(ByVal ParamArray promises() As promise) As promise
        If isemptyarray(promises) Then
            Return resolve(Nothing)
        Else
            Dim r As promise = Nothing
            r = New promise()
            For i As Int32 = 0 To array_size_i(promises) - 1
                If promises(i) Is Nothing Then
                    r._reject(strcat("promises", i, " is nothing"))
                Else
                    promises(i).then(Sub(ByVal result As Object)
                                         r._resolve(result)
                                     End Sub,
                                     Sub(ByVal reason As Object)
                                         r._reject(reason)
                                     End Sub)
                End If
            Next
            Return r
        End If
    End Function

    Public Shared Function reject(ByVal reason As Object) As promise
        Return New promise(Sub(ByVal resolve As Action(Of Object), ByVal this_reject As Action(Of Object))
                               this_reject(reason)
                           End Sub)
    End Function

    Public Shared Function resolve(ByVal result As Object) As promise
        Return New promise(Sub(ByVal this_resolve As Action(Of Object), ByVal reject As Action(Of Object))
                               this_resolve(result)
                           End Sub)
    End Function
End Class
