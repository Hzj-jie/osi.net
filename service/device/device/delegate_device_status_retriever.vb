
Imports osi.root.connector
Imports osi.root.template

Public NotInheritable Class delegate_device_status_retriever
    Public Shared Function [New](Of T)(Optional ByVal validator As Func(Of T, Boolean) = Nothing,
                                       Optional ByVal closer As Action(Of T) = Nothing,
                                       Optional ByVal identifier As Func(Of T, String) = Nothing,
                                       Optional ByVal checker As Action(Of T) = Nothing) _
                                      As delegate_device_status_retriever(Of T)
        Return New delegate_device_status_retriever(Of T)(validator, closer, identifier, checker)
    End Function

    Private Sub New()
    End Sub
End Class
Public Class delegate_device_status_retriever(Of T)
    Private ReadOnly validator As Func(Of T, Boolean)
    Private ReadOnly closer As Action(Of T)
    Private ReadOnly identifier As Func(Of T, String)
    Private ReadOnly checker As Action(Of T)

    Public Sub New(Optional ByVal validator As Func(Of T, Boolean) = Nothing,
                   Optional ByVal closer As Action(Of T) = Nothing,
                   Optional ByVal identifier As Func(Of T, String) = Nothing,
                   Optional ByVal checker As Action(Of T) = Nothing)
        Me.validator = validator
        Me.closer = closer
        Me.identifier = identifier
        Me.checker = checker
    End Sub

    Public Function validate(ByVal c As T) As Boolean
        Return validator Is Nothing OrElse validator(c)
    End Function

    Public Sub close(ByVal c As T)
        If closer IsNot Nothing Then
            closer(c)
        End If
    End Sub

    Public Function identity(ByVal c As T) As String
        Return If(identifier Is Nothing, Convert.ToString(c), identifier(c))
    End Function

    Public Sub check(ByVal c As T)
        If checker IsNot Nothing Then
            checker(c)
        End If
    End Sub
End Class

Public Class delegate_device_status_retriever(Of T,
                                                 _VALIDATOR As __do(Of T, Boolean),
                                                 _CLOSER As __void(Of T),
                                                 _IDENTIFIER As __do(Of T, String),
                                                 _CHECKER As __void(Of T))
    Inherits delegate_device_status_retriever(Of T)

    Public Shared ReadOnly instance As delegate_device_status_retriever(Of T, 
                                                                           _VALIDATOR, 
                                                                           _CLOSER, 
                                                                           _IDENTIFIER, 
                                                                           _CHECKER)

    Private Shared ReadOnly validator As Func(Of T, Boolean)
    Private Shared ReadOnly closer As Action(Of T)
    Private Shared ReadOnly identifier As Func(Of T, String)
    Private Shared ReadOnly checker As Action(Of T)

    Shared Sub New()
        Dim a As _VALIDATOR = Nothing
        a = alloc(Of _VALIDATOR)()
        validator = Function(x As T) As Boolean
                        Return a(x)
                    End Function
        Dim b As _CLOSER = Nothing
        b = alloc(Of _CLOSER)()
        closer = Sub(x As T)
                     b.invoke(x)
                 End Sub
        Dim c As _IDENTIFIER = Nothing
        c = alloc(Of _IDENTIFIER)()
        identifier = Function(x As T) As String
                         Return c(x)
                     End Function
        Dim d As _CHECKER = Nothing
        d = alloc(Of _CHECKER)()
        checker = Sub(x As T)
                      d.invoke(x)
                  End Sub

        instance = New delegate_device_status_retriever(Of T, _VALIDATOR, _CLOSER, _IDENTIFIER, _CHECKER)()
    End Sub

    Private Sub New()
        MyBase.New(validator, closer, identifier)
    End Sub
End Class
