
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports osi.root.constants

Public Module _member_info
    <Extension()> Public Function full_name(ByVal this As MemberInfo) As String
        If this Is Nothing Then
            Return Nothing
        End If
        If this.DeclaringType() Is Nothing OrElse this.GetType().inherit(GetType(Type)) Then
            Return Convert.ToString(this)
        End If
        Return strcat(this.DeclaringType().FullName(),
                      character.dot,
                      character.left_brace,
                      this,
                      character.right_brace)
    End Function
End Module
