
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.delegates
Imports osi.root.formation

Public NotInheritable Class once_action
    Private ReadOnly v As one_off(Of Action)

    Public Shared Function [New](ByVal v As Action) As once_action
        Return New once_action(v)
    End Function

    Public Shared Function [New](Of T)(ByVal v As void(Of T)) As once_action(Of T)
        Return New once_action(Of T)(v)
    End Function

    Public Sub New(ByVal v As Action)
        assert(v IsNot Nothing)
        Me.v = one_off.[New](v)
    End Sub

    Public Shared Widening Operator CType(ByVal this As once_action) As Boolean
        Return this IsNot Nothing AndAlso this.has()
    End Operator

    Public Shared Operator Not(ByVal this As once_action) As Boolean
        Return this Is Nothing OrElse Not this.has()
    End Operator

    Public Function has() As Boolean
        Return v.has()
    End Function

    Public Function run() As Boolean
        Dim a As Action = Nothing
        a = +v
        If a Is Nothing Then
            Return False
        End If
        void_(a)
        Return True
    End Function
End Class

Public NotInheritable Class once_action(Of T)
    Private ReadOnly v As one_off(Of void(Of T))

    Public Sub New(ByVal v As void(Of T))
        assert(v IsNot Nothing)
        Me.v = one_off.[New](v)
    End Sub

    Public Shared Widening Operator CType(ByVal this As once_action(Of T)) As Boolean
        Return this IsNot Nothing AndAlso this.has()
    End Operator

    Public Shared Operator Not(ByVal this As once_action(Of T)) As Boolean
        Return this Is Nothing OrElse Not this.has()
    End Operator

    Public Function has() As Boolean
        Return v.has()
    End Function

    Public Function run(ByRef p As T) As Boolean
        Dim a As void(Of T) = Nothing
        a = +v
        If a Is Nothing Then
            Return False
        End If
        void_(a, p)
        Return True
    End Function

    Public Function run() As Boolean
        Return run([default](Of T).null)
    End Function
End Class
