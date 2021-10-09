
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Reflection
Imports osi.root.connector

Partial Public NotInheritable Class case2
    ' Container of supportive information from a MemberInfo
    Private Class info
        Public ReadOnly name As String
        Public ReadOnly full_name As String
        Public ReadOnly repeat_times As UInt64
        Public ReadOnly thread_count As UInt32
        Public ReadOnly command_line_specified As Boolean
        Public ReadOnly flaky As Boolean
        Public ReadOnly has_reserved_processors As Boolean
        Public ReadOnly reserved_processors As Int16

        Private Sub New(ByVal name As String,
                        ByVal full_name As String,
                        ByVal repeat_times As UInt64,
                        ByVal thread_count As UInt32,
                        ByVal command_line_specified As Boolean,
                        ByVal flaky As Boolean,
                        ByVal has_reserved_processors As Boolean,
                        ByVal reserved_processors As Int16)
            Me.name = name
            Me.full_name = full_name
            Me.repeat_times = repeat_times
            Me.thread_count = thread_count
            Me.command_line_specified = command_line_specified
            Me.flaky = flaky
            Me.has_reserved_processors = has_reserved_processors
            Me.reserved_processors = reserved_processors
        End Sub

        Protected Sub New(ByVal info As info)
            Me.New(assert_which.of(info).is_not_null().name,
                   assert_which.of(info).is_not_null().full_name,
                   assert_which.of(info).is_not_null().repeat_times,
                   assert_which.of(info).is_not_null().thread_count,
                   assert_which.of(info).is_not_null().command_line_specified,
                   assert_which.of(info).is_not_null().flaky,
                   assert_which.of(info).is_not_null().has_reserved_processors,
                   assert_which.of(info).is_not_null().reserved_processors)
        End Sub

        Public Shared Function merge(ByVal i As info, ByVal j As info) As info
            assert(Not i Is Nothing)
            assert(Not j Is Nothing)
            Return New info("",
                            "",
                            i.repeat_times * j.repeat_times,
                            i.thread_count * j.thread_count,
                            i.command_line_specified OrElse j.command_line_specified,
                            i.flaky OrElse j.flaky,
                            i.has_reserved_processors OrElse j.has_reserved_processors,
                            If(i.has_reserved_processors, i.reserved_processors, j.reserved_processors))
        End Function

        Public Shared Function from(ByVal member As MemberInfo) As info
            assert(Not member Is Nothing)
            Dim repeat_times As UInt64 = 0
            Using code_block
                Dim attribute As attributes.repeat = Nothing
                If member.custom_attribute(attribute) Then
                    repeat_times = attribute.times
                Else
                    repeat_times = 1
                End If
            End Using

            Dim thread_count As UInt32 = 0
            Using code_block
                Dim attribute As attributes.multi_threading = Nothing
                If member.custom_attribute(attribute) Then
                    thread_count = attribute.thread_count
                Else
                    thread_count = 1
                End If
            End Using

            Dim command_line_specified As Boolean = False
            command_line_specified = member.has_custom_attribute(Of attributes.command_line_specified)()

            Dim flaky As Boolean = False
            flaky = member.has_custom_attribute(Of attributes.flaky)()

            Dim has_reserved_processors As Boolean = False
            Dim reserved_processors As Int16 = 0
            Using code_block
                Dim attribute As attributes.reserved_processors = Nothing
                If member.custom_attribute(attribute) Then
                    has_reserved_processors = True
                    reserved_processors = attribute.reserved_processors
                Else
                    has_reserved_processors = False
                    reserved_processors = 1
                End If
            End Using

            Return New info(member.Name(),
                            member.full_name(),
                            repeat_times,
                            thread_count,
                            command_line_specified,
                            flaky,
                            has_reserved_processors,
                            reserved_processors)
        End Function
    End Class

    ' info with a bool function
    Private Class function_info
        Inherits info

        Public ReadOnly f As Func(Of Object, Boolean)

        Private Sub New(ByVal f As Func(Of Object, Boolean), ByVal info As info)
            MyBase.New(info)
            assert(Not f Is Nothing)
            Me.f = f
        End Sub

        Protected Sub New(ByVal f As function_info)
            Me.New(assert_which.of(f).is_not_null().f, f)
        End Sub

        Public Shared Shadows Function from(ByVal method As MethodInfo) As function_info
            assert(Not method Is Nothing)
            Return New function_info(Function(ByVal obj As Object) As Boolean
                                         Return _object_extensions.is_null_or_true(method.Invoke(obj, Nothing))
                                     End Function,
                                     info.from(method))
        End Function
    End Class

    ' function_info with percentage
    Private Class random_function_info
        Inherits function_info

        Public ReadOnly percentage As Double

        Private Sub New(ByVal percentage As Double, ByVal f As function_info)
            MyBase.New(f)
            assert(percentage >= 0 AndAlso percentage <= 1)
            Me.percentage = percentage
        End Sub

        Public Shared Shadows Function from(ByVal method As MethodInfo) As random_function_info
            assert(Not method Is Nothing)
            Dim percentage As Double = 0
            Using code_block
                Dim attribute As attributes.random = Nothing
                assert(method.custom_attribute(attribute))
                percentage = attribute.percentage
            End Using

            Return New random_function_info(percentage, function_info.from(method))
        End Function
    End Class
End Class
