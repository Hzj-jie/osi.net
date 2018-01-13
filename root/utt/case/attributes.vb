
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants

Public NotInheritable Class attributes
    ' Define a test case for a class or a method.
    ' When describing class, it indicates the class contains test cases.
    ' When describing a method, it indicates the method as a test case.
    ' Each test case will be executed in a new "test" class instance.
    <AttributeUsage(AttributeTargets.Class Or AttributeTargets.Method, AllowMultiple:=False, Inherited:=True)> _
    Public NotInheritable Class test
        Inherits attribute
    End Class

    ' Define the repeat times of a class or a method.
    <AttributeUsage(AttributeTargets.Class Or AttributeTargets.Method, AllowMultiple:=False, Inherited:=True)> _
    Public NotInheritable Class repeat
        Inherits attribute

        Public ReadOnly times As UInt64

        Public Sub New(ByVal times As UInt64)
            Me.New(times, times)
        End Sub

        Public Sub New(ByVal debug_times As UInt64, ByVal release_times As UInt64)
            Dim times As UInt64 = 0
            If isdebugbuild() Then
                times = debug_times
            Else
                times = release_times
            End If
            assert(times > 1)
            Me.times = times
        End Sub
    End Class

    ' Define a prepare method for the test class.
    ' The function will be executed for each test case in the test class.
    ' If multiple methods are described with "prepare" attribute, they will be executed one-by-one, but the order is
    ' unexpected.
    <AttributeUsage(AttributeTargets.Method, AllowMultiple:=False, Inherited:=True)> _
    Public NotInheritable Class prepare
        Inherits attribute
    End Class

    ' Define a finish method for the test class.
    ' The function will be executed after each test case in the test class.
    ' If multiple methods are described with "finish" attribute, they will be executed one-by-one, but the order is
    ' unexpected.
    <AttributeUsage(AttributeTargets.Method, AllowMultiple:=False, Inherited:=True)> _
    Public NotInheritable Class finish
        Inherits attribute
    End Class

    ' Define a method as random-run target.
    <AttributeUsage(AttributeTargets.Method, AllowMultiple:=False, Inherited:=True)> _
    Public NotInheritable Class random
        Inherits attribute

        Public ReadOnly percentage As Double

        Public Sub New(ByVal percentage As Double)
            assert(percentage >= 0 AndAlso percentage <= 1)
            Me.percentage = percentage
        End Sub
    End Class

    ' Define the reserved processor count of a test case or test class.
    <AttributeUsage(AttributeTargets.Class Or AttributeTargets.Method, AllowMultiple:=False, Inherited:=True)>
    Public NotInheritable Class reserved_processors
        Inherits Attribute

        Public ReadOnly reserved_processors As Int16

        Public Sub New(ByVal reserved_processors As Int16)
            Me.reserved_processors = reserved_processors
        End Sub
    End Class

    ' Define a test case to be executed only when command line specified.
    <AttributeUsage(AttributeTargets.Class Or AttributeTargets.Method, AllowMultiple:=False, Inherited:=True)> _
    Public NotInheritable Class command_line_specified
        Inherits Attribute
    End Class

    ' Define a flaky test case
    <AttributeUsage(AttributeTargets.Class Or AttributeTargets.Method, AllowMultiple:=False, Inherited:=True)>
    Public NotInheritable Class flaky
        Inherits Attribute
    End Class

    <AttributeUsage(AttributeTargets.Class Or AttributeTargets.Method, AllowMultiple:=False, Inherited:=True)>
    Public NotInheritable Class multi_threading
        Inherits Attribute

        Public ReadOnly thread_count As UInt32

        Public Sub New(ByVal thread_count As UInt32)
            assert(thread_count <= max_int32)
            assert(multithreading_case_wrapper.valid_thread_count(thread_count))
            Me.thread_count = thread_count
        End Sub
    End Class

    Private Sub New()
    End Sub
End Class
