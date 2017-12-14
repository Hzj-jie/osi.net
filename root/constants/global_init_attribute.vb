
Option Explicit On
Option Infer Off
Option Strict On

' global_init is helping to decouple the dependencies between stopwatch / event / bind, etc.
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

' Follow .net naming rules: using global_init as attribute name will be accepted. It also avoids conflict with
' osi.root.utils.global_init class.
Public NotInheritable Class global_initAttribute
    Inherits Attribute

    Private Const default_init_once As Boolean = True
    Private Const default_level As Byte = 0

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
