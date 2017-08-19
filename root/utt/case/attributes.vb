
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

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
    <AttributeUsage(AttributeTargets.Class, AllowMultiple:=False, Inherited:=True)> _
    Public NotInheritable Class reserved_processor_count
        Inherits attribute

        Public ReadOnly reserved_processor_count As Int16

        Public Sub New(ByVal reserved_processor_count As Int16)
            assert(reserved_processor_count >= 0 OrElse reserved_processor_count = -1)
            Me.reserved_processor_count = reserved_processor_count
        End Sub
    End Class

    ' Define a test case to be executed only when command line specified.
    <AttributeUsage(AttributeTargets.Class Or AttributeTargets.Method, AllowMultiple:=False, Inherited:=True)> _
    Public NotInheritable Class command_line_specified
        Inherits attribute
    End Class

    Private Sub New()
    End Sub
End Class
