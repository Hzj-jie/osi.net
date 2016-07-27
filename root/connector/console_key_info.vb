
Imports System.Runtime.CompilerServices

Public Module _console_key_info
    <Extension()> Public Function alt(ByVal c As ConsoleKeyInfo) As Boolean
        Return (c.Modifiers() And ConsoleModifiers.Alt) <> 0
    End Function

    <Extension()> Public Function ctrl(ByVal c As ConsoleKeyInfo) As Boolean
        Return (c.Modifiers() And ConsoleModifiers.Control) <> 0
    End Function

    <Extension()> Public Function control(ByVal c As ConsoleKeyInfo) As Boolean
        Return ctrl(c)
    End Function

    <Extension()> Public Function shift(ByVal c As ConsoleKeyInfo) As Boolean
        Return (c.Modifiers() And ConsoleModifiers.Shift) <> 0
    End Function

    <Extension()> Public Function no_modifiers(ByVal c As ConsoleKeyInfo) As Boolean
        Return Not alt(c) AndAlso
               Not ctrl(c) AndAlso
               Not shift(c)
    End Function
End Module
