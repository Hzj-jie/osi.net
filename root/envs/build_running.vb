
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Public Module _build_running
    Public ReadOnly build As String = Function() As String
                                          If isdebugbuild() Then
                                              Return "debugbuild"
                                          End If
                                          If isreleasebuild() Then
                                              Return "releasebuild"
                                          End If
                                          Return "unknownbuild"
                                      End Function()
    Public ReadOnly running_mode As String = Function() As String
                                                 If isdebugmode() Then
                                                     Return "debugmode"
                                                 End If
                                                 Return "normalmode"
                                             End Function()
End Module
