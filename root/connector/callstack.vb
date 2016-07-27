
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

    Public Function callstack(Optional ByVal removeblanks As Boolean = False,
                              Optional ByVal separator As Char = character.colon) As String
#If Not PocketPC AndAlso Not Smartphone Then
        Dim rtn As String = Nothing
        rtn = Environment.StackTrace()
        If removeblanks Then
            rtn = rtn.Replace("   ", empty_string) _
                     .Replace(newline.incode(), separator)
        End If
        Return rtn
#Else
        Return "##CANNOTTRACE##"
#End If
    End Function

    Private Function build_stack_trace(ByVal s As StackFrame) As String
        assert(Not s Is Nothing)
        assert(Not s.GetMethod() Is Nothing)
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

    <Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
    Public Function callingcode(Optional ByVal additionalJump As Int32 = 0) As String
#If Not (PocketPC OrElse Smartphone) Then
        Dim callstack As StackFrame = Nothing
        Dim jump As Int32 = 0
        jump = additionalJump + 1
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

    <Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
    Public Function callingcode(ByVal ParamArray ignores() As String) As String
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
            Dim j As Int32 = 0
            For j = 0 To array_size(ignores) - 1
                If strcontains(m.Module().Name(), ignores(j), False) OrElse
                   strcontains(m.DeclaringType().FullName(), ignores(j), False) OrElse
                   strcontains(m.Name(), ignores(j), False) Then
                    Exit For
                End If
            Next
            If j = array_size(ignores) Then
                Return build_stack_trace(s)
            End If
        End While

        Return "##NO_MATCH##"
    End Function

    <Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
    Public Function invokingcode(Optional ByVal additionalJump As Int32 = 0) As String
        Return callingcode(additionalJump + 1)
    End Function

    Public Function invoke_method_name(ByVal invoke As [Delegate]) As String
#If PocketPC OrElse Smartphone Then
        Return "#CANNOT_GET_INVOKE_NAME#"
#Else
        If Not invoke Is Nothing Then
            Try
                Return invoke.Method().Name()
            Catch
            End Try
        End If
        Return "#CANNOT_GET_INVOKE_NAME#"
#End If
    End Function

    Public Function invoke_method_identity(ByVal invoke As [Delegate]) As String
        If Not invoke Is Nothing Then
            Try
                Dim rtn As StringBuilder = Nothing
                rtn = New StringBuilder()
                If Not invoke.Target() Is Nothing Then
                    rtn.Append(invoke.Target().GetType().FullName()) _
                       .Append(character.colon)
                End If
                rtn.Append(invoke.Method().DeclaringType().FullName()) _
                   .Append(character.dot) _
                   .Append(invoke.Method().Name())
                If isdebugmode() Then
                    rtn.Append(character.at) _
                       .Append(invoke.Method().Module().FullyQualifiedName())
                End If
                Return Convert.ToString(rtn)
            Catch
            End Try
        End If

        Return "#CANNOT_GET_INVOKE_IDENTITY#"
    End Function
End Module
