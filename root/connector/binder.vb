
Imports System.Runtime.CompilerServices
Imports osi.root.constants

Public Module _binder
    <Extension()> Public Function has_value(Of T As Class, PROTECTOR) _
                                           (ByVal i As binder(Of T, PROTECTOR)) As Boolean
        Return (Not i Is Nothing AndAlso i.has_local_value()) OrElse
               binder(Of T, PROTECTOR).has_global_value()
    End Function
End Module

Public Class binder
    Private Shared ReadOnly suppress_rebind_global_value_error_binder As  _
                                binder(Of Func(Of Boolean), suppress_rebind_global_value_error_binder_protector)

    Shared Sub New()
        suppress_rebind_global_value_error_binder =
            New binder(Of Func(Of Boolean), suppress_rebind_global_value_error_binder_protector)()
    End Sub

    Protected Shared Function suppress_rebind_global_value_error() As Boolean
        Return suppress_rebind_global_value_error_binder.has_value() AndAlso
               (+suppress_rebind_global_value_error_binder)()
    End Function
End Class

Public Class binder(Of T As Class)
    Inherits binder(Of T, Object)
End Class

Public Class binder(Of T As Class, PROTECTOR)
    Inherits binder

    Private Shared ReadOnly is_protected As Boolean
#If DEBUG Then
    Private Shared _global_bind_callstack As String
#End If
    Private Shared _global As T
    Private _local As T

    Shared Sub New()
        is_protected = Not type_info(Of PROTECTOR).is_object
    End Sub

    Public Sub set_local(ByVal i As T)
        _local = i
    End Sub

    Public Shared Sub set_global(ByVal i As T)
        If Not suppress_rebind_global_value_error() Then
#If DEBUG Then
            assert(_global Is Nothing OrElse i Is Nothing,
                   "rebind ",
                   GetType(T).FullName(),
                   " with protector ",
                   GetType(PROTECTOR).Name(),
                   ", last binding callstack ",
                   _global_bind_callstack)
#Else
            assert(_global Is Nothing OrElse i Is Nothing)
#End If
        End If
        _global = i
#If DEBUG Then
        If i Is Nothing Then
            _global_bind_callstack = Nothing
        Else
            _global_bind_callstack = callstack()
        End If
#End If
    End Sub

    Public Shared Function [global]() As T
        If _global Is Nothing AndAlso is_protected Then
            Return binder(Of T).global()
        Else
            Return _global
        End If
    End Function

    Public Function local() As T
        Return _local
    End Function

    Public Shared Function has_global_value() As Boolean
        Return Not [global]() Is Nothing
    End Function

    Public Function has_local_value() As Boolean
        Return Not local() Is Nothing
    End Function

    Public Function [get]() As T
        If has_local_value() Then
            Return local()
        Else
            Return [global]()
        End If
    End Function

    Public Shared Operator +(ByVal this As binder(Of T, PROTECTOR)) As T
        If this Is Nothing Then
            Return [global]()
        Else
            Return this.get()
        End If
    End Operator
End Class
