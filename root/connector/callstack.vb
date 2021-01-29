
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Diagnostics
Imports System.Reflection
Imports System.Text
Imports osi.root.constants

Public Module _callstack
    ' Test only
    Public Function in_shared_constructor_of(ByVal stack_trace As String, ByVal t As Type) As Boolean
        If t Is Nothing Then
            Return False
        Else
            Return strcontains(stack_trace, strcat("   at ", strrplc(t.FullName(), "+", "."), "..cctor()"))
        End If
    End Function

    Public Function in_shared_constructor_of(ByVal t As Type) As Boolean
        Return in_shared_constructor_of(callstack(), t)
    End Function

    Public Function in_shared_constructor_of(Of T)() As Boolean
        Return in_shared_constructor_of(GetType(T))
    End Function

    Public Function callstack(Optional ByVal remove_blanks As Boolean = False,
                              Optional ByVal separator As Char = character.colon) As String
        Dim rtn As String = Environment.StackTrace()
        If remove_blanks Then
            rtn = rtn.Replace("   ", empty_string) _
                     .Replace(newline.incode(), separator)
        End If
        Return rtn
    End Function

    Private Function build_stack_trace(ByVal s As StackFrame) As String
        assert(Not s Is Nothing)
        If s.GetMethod() Is Nothing Then
            Return "##NO_MANAGED_METHOD##"
        End If
        Dim r As StringBuilder = Nothing
        r = New StringBuilder()
        If isdebugmode() Then
            If String.IsNullOrEmpty(s.GetFileName()) Then
                r.Append("##MISSING_PDB##")
            Else
                r.Append(s.GetFileName())
                r.Append(character.left_bracket)
                r.Append(Convert.ToString(s.GetFileLineNumber()))
                r.Append(character.right_bracket)
            End If
            r.Append(character.colon)
        End If
        r.Append(s.GetMethod().Module().Name())
        r.Append(character.dot)
        r.Append(s.GetMethod().DeclaringType().FullName())
        r.Append(character.dot)
        r.Append(s.GetMethod().Name())
        Return Convert.ToString(r)
    End Function

    <Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)>
    Public Function backtrace(Optional ByVal additional_jump As Int32 = 1) As String
#If Not (PocketPC OrElse Smartphone) Then
        Dim callstack As StackFrame = Nothing
        Dim jump As Int32 = 0
        jump = additional_jump + 1
        If isdebugmode() Then
            callstack = New StackFrame(jump, True)
        Else
            callstack = New StackFrame(jump, False)
        End If
        Return build_stack_trace(callstack)
#Else
        Return "##CANNOTTRACE##"
#End If
    End Function

    Private Function should_ignore(ByVal m As MethodBase, ByVal ignore As String) As Boolean
        Return strcontains(m.Module().Name(), ignore, False) OrElse
               strcontains(m.DeclaringType().FullName(), ignore, False) OrElse
               strcontains(m.Name(), ignore, False)
    End Function

    Private Function should_ignore(ByVal m As MethodBase, ByVal ignores() As String) As Boolean
        For i As Int32 = 0 To array_size_i(ignores) - 1
            If should_ignore(m, ignores(i)) Then
                Return True
            End If
        Next
        Return False
    End Function

    <Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)>
    Public Function backtrace(ByVal ParamArray ignores() As String) As String
        Dim i As Int32 = 0
        While True
            'jump myself
            i += 1
            Dim s As StackFrame = Nothing
            If isdebugmode() Then
                s = New StackFrame(i, True)
            Else
                s = New StackFrame(i, False)
            End If
            Dim m As MethodBase = Nothing
            m = s.GetMethod()
            If m Is Nothing Then
                Exit While
            End If
            If should_ignore(m, GetType(_callstack).FullName()) OrElse
               should_ignore(m, ignores) Then
                Continue While
            End If
            Return build_stack_trace(s)
        End While

        Return "##NO_MATCH##"
    End Function

    <Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)>
    Public Function backtrace(Of ignore)() As String
        Return backtrace(template_array(Of ignore, String, type_info.full_name_func).vs)
    End Function

    <Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)>
    Public Function backtrace(Of ignore1, ignore2)() As String
        Return backtrace(template_array(Of ignore1, ignore2, String, type_info.full_name_func).vs)
    End Function

    <Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)>
    Public Function backtrace(Of ignore1, ignore2, ignore3)() As String
        Return backtrace(template_array(Of ignore1, ignore2, ignore3, String, type_info.full_name_func).vs)
    End Function

    <Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)>
    Public Function backtrace(Of ignore1, ignore2, ignore3, ignore4)() As String
        Return backtrace(template_array(Of ignore1, ignore2, ignore3, ignore4, String, type_info.full_name_func).vs)
    End Function
End Module
