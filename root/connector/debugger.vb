
Imports System.Diagnostics
Imports osi.root.constants

Public Module _debugger
    Public Function attach_debugger() As Boolean
        If Debugger.IsAttached() Then
            Debugger.Break()
            Return True
        Else
            'for mono
            Try
                Return Debugger.Launch()
            Catch
            End Try
            Return False
        End If
    End Function

    Public Sub debugpause(Optional ByVal show_message As Boolean = True)
        If isdebugmode() Then
            pause(show_message)
        End If
    End Sub

    Public Sub pause(Optional ByVal show_message As Boolean = True)
        If show_message Then
            rewrite_console_error("press any key to continue")
        End If

        Try
            'redirect input
#If Not (PocketPC OrElse Smartphone) Then
            Console.ReadKey()
#Else
            Console.In.Read()
#End If
        Catch
        End Try
    End Sub
End Module
