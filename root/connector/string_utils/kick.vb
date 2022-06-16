
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Text
Imports System.Runtime.CompilerServices
Imports osi.root.constants

Public Module _kick
    <Extension()> Public Function kick_between(ByRef content As String,
                                               ByVal left As String,
                                               ByVal right As String,
                                               Optional ByVal case_sensitive As Boolean = True,
                                               Optional ByVal recursive As Boolean = True) As String
        If content.null_or_empty() Then
            Return content
        Else
            Dim l As UInt32 = 0
            l = strlen(content)
            assert(l > 0)
            If left.null_or_empty() Then
                left = content(0)
            End If
            If right.null_or_empty() Then
                right = content(CInt(l - uint32_1))
            End If

            Dim bracket As Int32 = 0
            Dim rtn As StringBuilder = Nothing
            Dim leftlen As UInt32 = 0
            Dim rightlen As UInt32 = 0
            leftlen = strlen(left)
            rightlen = strlen(right)
            rtn = New StringBuilder(CInt(strlen(content)))
            For i As UInt32 = 0 To l - uint32_1
                If strsame(content, i, left, uint32_0, leftlen, case_sensitive) Then
                    If recursive Then
                        bracket += 1
                    Else
                        assert(bracket <= 1)
                        If bracket = 0 Then
                            bracket = 1
                        End If
                    End If
                    i += leftlen - uint32_1
                ElseIf strsame(content, i, right, uint32_0, rightlen, case_sensitive) AndAlso bracket > 0 Then
                    bracket -= 1
                    i += rightlen - uint32_1
                ElseIf bracket = 0 Then
                    rtn.Append(content(CInt(i)))
                End If
            Next

            content = Convert.ToString(rtn)
            Return content
        End If
    End Function
End Module
