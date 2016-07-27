
'global_init is helping to decouple with stopwatch / event / bind
Public Enum global_init_level As Byte
    foundamental = 0
    debugging
    log_and_counter_services
    threading_and_procedure
    services
    server_services
    productions

    max = max_uint8
    other = max
    all = max
End Enum

'follow .net naming rules, so it will be able to use global_init as attribute name
Public Class global_initAttribute
    Inherits Attribute

    Private Const default_init_once As Boolean = True
    Private Const default_level As Byte = 0
    Public Shared ReadOnly type As Type

    Shared Sub New()
        type = GetType(global_initAttribute)
    End Sub

    Public ReadOnly init_once As Boolean
    Public ReadOnly level As Byte

    Public Sub New(ByVal init_once As Boolean, ByVal level As Byte)
        Me.init_once = init_once
        Me.level = level
    End Sub

    Public Sub New(ByVal level As Byte, ByVal init_once As Boolean)
        Me.New(init_once, level)
    End Sub

    Public Sub New(ByVal init_once As Boolean)
        Me.New(init_once, default_level)
    End Sub

    Public Sub New(ByVal level As Byte)
        Me.New(default_init_once, level)
    End Sub

    Public Sub New()
        Me.New(default_init_once, default_level)
    End Sub
End Class
