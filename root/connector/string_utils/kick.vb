
Imports System.Text
Imports System.Runtime.CompilerServices
Imports osi.root.constants

Public Module _kick
    <Extension()> Public Function kick_between(ByRef content As String,
                                               ByVal left As String,
                                               ByVal right As String,
                                               Optional ByVal case_sensitive As Boolean = True) As String
        If String.IsNullOrEmpty(content) Then
            Return content
        Else
            Dim l As UInt32 = 0
            l = strlen(content)
            assert(l > 0)
            If String.IsNullOrEmpty(left) Then
                left = content(0)
            End If
            If String.IsNullOrEmpty(right) Then
                right = content(l - 1)
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
                    bracket += 1
                    i += leftlen - 1
                ElseIf strsame(content, i, right, uint32_0, rightlen, case_sensitive) AndAlso bracket > 0 Then
                    bracket -= 1
                    i += rightlen - 1
                ElseIf bracket = 0 Then
                    rtn.Append(content(i))
                End If
            Next

            content = Convert.ToString(rtn)
            Return content
        End If
    End Function
End Module
