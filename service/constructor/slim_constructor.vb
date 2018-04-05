
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.delegates
Imports osi.root.formation

' A container of constructors of RETURN_TYPE. It selects the first constructor which can create an instance by matching
' the KEY_TYPE and returning true.
Public Class slim_constructor(Of KEY_TYPE, PARAMETER, RETURN_TYPE)
    Private ReadOnly cs As vector(Of pair(Of KEY_TYPE, _do_val_ref(Of PARAMETER, RETURN_TYPE, Boolean)))

    Public Sub New()
        cs = New vector(Of pair(Of KEY_TYPE, _do_val_ref(Of PARAMETER, RETURN_TYPE, Boolean)))()
    End Sub

    Public Function [New](ByVal key As KEY_TYPE, ByVal p As PARAMETER, ByRef o As RETURN_TYPE) As Boolean
        Dim i As UInt32 = 0
        While i < cs.size()
            If equal(key, cs(i).first) Then
                If cs(i).second(p, o) Then
                    Return True
                End If
            End If
            i += uint32_1
        End While
        Return False
    End Function

    Public Function [New](ByVal key As KEY_TYPE, ByVal p As PARAMETER) As RETURN_TYPE
        Dim o As RETURN_TYPE = Nothing
        assert([New](key, p, o))
        Return o
    End Function

    Public Sub register(ByVal key As KEY_TYPE, ByVal c As _do_val_ref(Of PARAMETER, RETURN_TYPE, Boolean))
        assert(Not c Is Nothing)
        cs.emplace_back(emplace_make_pair(key, c))
    End Sub

    Public Sub register(ByVal key As KEY_TYPE, ByVal c As Func(Of PARAMETER, RETURN_TYPE))
        assert(Not c Is Nothing)
        register(key,
                 Function(ByVal p As PARAMETER, ByRef o As RETURN_TYPE) As Boolean
                     o = c(p)
                     Return True
                 End Function)
    End Sub
End Class

Public Class slim_constructor(Of PARAMETER, RETURN_TYPE)
    Inherits slim_constructor(Of String, PARAMETER, RETURN_TYPE)
End Class

Public Class slim_constructor(Of RETURN_TYPE)
    Inherits slim_constructor(Of String, RETURN_TYPE)
End Class

' The implementation of slim_constructor without a KEY_TYPE. It goes through all the constructors and return the
' instance returned by the first constructor returning true.
Public Class keyless_slim_constructor(Of PARAMETER, RETURN_TYPE)
    Inherits slim_constructor(Of match_any, PARAMETER, RETURN_TYPE)

    Public Shadows Function [New](ByVal p As PARAMETER, ByRef o As RETURN_TYPE) As Boolean
        Return MyBase.[New](match_any.instance, p, o)
    End Function

    Public Shadows Function [New](ByVal p As PARAMETER) As RETURN_TYPE
        Dim o As RETURN_TYPE = Nothing
        assert([New](p, o))
        Return o
    End Function

    Public Shadows Sub register(ByVal c As _do_val_ref(Of PARAMETER, RETURN_TYPE, Boolean))
        MyBase.register(match_any.instance, c)
    End Sub

    Public Shadows Sub register(ByVal c As Func(Of PARAMETER, RETURN_TYPE))
        MyBase.register(match_any.instance, c)
    End Sub
End Class

Public Class keyless_slim_constructor(Of RETURN_TYPE)
    Inherits keyless_slim_constructor(Of String, RETURN_TYPE)
End Class
