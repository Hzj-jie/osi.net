
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Reflection
Imports osi.root.connector
Imports osi.root.constants

Partial Public NotInheritable Class case2
    ' Container of supportive information from a MemberInfo
    Private Class info
        Public ReadOnly name As String
        Public ReadOnly full_name As String
        Public ReadOnly repeat_times As UInt64
        Public ReadOnly command_line_specified As Boolean
        Public ReadOnly has_reserved_processors As Boolean
        Public ReadOnly reserved_processors As Int16

        Private Sub New(ByVal name As String,
                        ByVal full_name As String,
                        ByVal repeat_times As UInt64,
                        ByVal command_line_specified As Boolean,
                        ByVal has_reserved_processors As Boolean,
                        ByVal reserved_processors As Int16)
            Me.name = name
            Me.full_name = full_name
            Me.repeat_times = repeat_times
            Me.command_line_specified = command_line_specified
            Me.has_reserved_processors = has_reserved_processors
            Me.reserved_processors = reserved_processors
        End Sub

        Protected Sub New(ByVal info As info)
            Me.New(assert_not_nothing_return(info).name,
                   assert_not_nothing_return(info).full_name,
                   assert_not_nothing_return(info).repeat_times,
                   assert_not_nothing_return(info).command_line_specified,
                   assert_not_nothing_return(info).has_reserved_processors,
                   assert_not_nothing_return(info).reserved_processors)
        End Sub

        Public Shared Function from(ByVal member As MemberInfo) As info
            assert(Not member Is Nothing)
            Dim repeat_times As UInt64 = 0
            Using code_block
                Dim attribute As attributes.repeat = Nothing
                If member.custom_attribute(attribute) Then
                    assert(attribute.times > uint64_1)
                    repeat_times = attribute.times
                Else
                    repeat_times = 1
                End If
            End Using

            Dim command_line_specified As Boolean = False
            command_line_specified = member.has_custom_attribute(Of attributes.command_line_specified)()

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
                            command_line_specified,
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
            Me.New(assert_not_nothing_return(f).f, f)
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
