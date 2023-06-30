
Option Explicit On
Option Infer Off
Option Strict On

#Const cached_alloc = True

Public Module _alloc
    Public Function use_cached_alloc() As Boolean
#If cached_alloc Then
        Return True
#Else
        Return False
#End If
    End Function

    Public Function alloc(Of T)() As T
#If cached_alloc Then
        assert(Not type_info(Of T).dominated_constructor() Is Nothing)
        Return type_info(Of T).dominated_constructor()()
#Else
        Try
            'T As New
            Return Activator.CreateInstance(Of T)()
        Catch
            'there is not a constructor without parameters
            Return GetType(T).allocate()
        End Try
#End If
    End Function
End Module
