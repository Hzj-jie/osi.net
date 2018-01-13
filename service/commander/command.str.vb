
#If RETIRED
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.connector
Imports osi.root.constants

<global_init(default_global_init_level.functor)>
Friend Module _command_str
    Private Const separator As Char = character.newline
    Private ReadOnly separators() As Char = {separator}

    Sub New()
        string_serializer.register(Function(ByVal i As StringReader, ByRef o As command) As Boolean
                                       assert(Not i Is Nothing)
                                       Dim s As String = Nothing
                                       s = i.ReadToEnd()
                                       If String.IsNullOrEmpty(s) Then
                                           Return False
                                       End If
                                       Dim ss() As String = Nothing
                                       ss = s.Split(separators)
                                       If (array_size(ss) And 1) <> 1 Then
                                           Return False
                                       End If
                                       o = New command()
                                       o.set_action_no_copy(decode(ss(0)))
                                       For j As Int32 = 1 To array_size_i(ss) - 1 Step 2
                                           o.set_parameter_no_copy(decode(ss(j)), decode(ss(j + 1)))
                                       Next
                                       Return True
                                   End Function)
        string_serializer.register(Sub(ByVal i As command, ByVal o As StringWriter)
                                       If i Is Nothing Then
                                           Return
                                       End If

                                       assert(Not o Is Nothing)
                                       o.Write(encode(i.action()))
                                       o.Write(separator)
                                       i.foreach(Sub(x, y)
                                                     o.Write(separator)
                                                     o.Write(encode(x))
                                                     o.Write(separator)
                                                     o.Write(encode(y))
                                                 End Sub)
                                   End Sub)
    End Sub

    Private Function encode(ByVal i() As Byte) As String
        If isemptyarray(i) Then
            Return Nothing
        Else
            Return Convert.ToBase64String(i)
        End If
    End Function

    Private Function decode(ByVal i As String) As Byte()
        If String.IsNullOrEmpty(i) Then
            Return Nothing
        Else
            Return Convert.FromBase64String(i)
        End If
    End Function
End Module
#End If
