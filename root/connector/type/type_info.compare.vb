
Option Explicit On
Option Infer Off
Option Strict On

Public Class type_info_operators
    ' T1 == T2 or T1 is a subclass or an implementation of T2.
    Public NotInheritable Class [is]
        Inherits type_info_operators

        Private Sub New()
        End Sub
    End Class

    ' T1 == T2
    Public NotInheritable Class equal
        Inherits type_info_operators

        Private Sub New()
        End Sub
    End Class

    ' T1 inherits T2
    Public NotInheritable Class inherit
        Inherits type_info_operators

        Private Sub New()
        End Sub
    End Class

    ' T1 implements T2
    Public NotInheritable Class implement
        Inherits type_info_operators

        Private Sub New()
        End Sub
    End Class

    ' T1 is an interface and it inherits T2
    Public NotInheritable Class interface_inherit
        Inherits type_info_operators

        Private Sub New()
        End Sub
    End Class

    Private Sub New()
    End Sub
End Class

Public NotInheritable Class type_info(Of T1, _OP As type_info_operators, T2)
    Public Shared ReadOnly v As Boolean

    Shared Sub New()
        If GetType(_OP).Equals(GetType(type_info_operators.[is])) Then
            v = GetType(T1).is(GetType(T2))
        ElseIf GetType(_OP).Equals(GetType(type_info_operators.equal)) Then
            v = GetType(T1).Equals(GetType(T2))
        ElseIf GetType(_OP).Equals(GetType(type_info_operators.inherit)) Then
            v = GetType(T1).inherit(GetType(T2))
        ElseIf GetType(_OP).Equals(GetType(type_info_operators.implement)) Then
            v = GetType(T1).implement(GetType(T2))
        ElseIf GetType(_OP).Equals(GetType(type_info_operators.interface_inherit)) Then
            v = GetType(T1).interface_inherit(GetType(T2))
        Else
            assert(False)
        End If
    End Sub

    Private Sub New()
    End Sub
End Class
