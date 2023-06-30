
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.utils
Imports osi.root.utt

' The attribute class with "Attribute" in the name is preferred.
Public Class custom_attributes_behavior_test4
    Inherits [case]

    Private Class fake
        Inherits Attribute

        Public Sub New()
            instance_count(Of fake).alloc()
        End Sub

        Public Shared Function count() As Int64
            Return instance_count(Of fake).count()
        End Function

        Protected Overrides Sub Finalize()
            instance_count(Of fake).dealloc()
            MyBase.Finalize()
        End Sub
    End Class

    Private Class fake2
        Inherits Attribute

        Public Sub New()
            instance_count(Of fake2).alloc()
        End Sub

        Public Shared Function count() As Int64
            Return instance_count(Of fake2).count()
        End Function

        Protected Overrides Sub Finalize()
            instance_count(Of fake2).dealloc()
            MyBase.Finalize()
        End Sub
    End Class

    Private Class fakeAttribute
        Inherits Attribute

        Public Sub New()
            instance_count(Of fakeAttribute).alloc()
        End Sub

        Public Shared Function count() As Int64
            Return instance_count(Of fakeAttribute).count()
        End Function

        Protected Overrides Sub Finalize()
            instance_count(Of fakeAttribute).dealloc()
            MyBase.Finalize()
        End Sub
    End Class

    <fake()>
    Private Class C1
    End Class

    <fake2()>
    Private Class C2
    End Class

    Public Overrides Function run() As Boolean
        Dim fake As fake = Nothing
        Dim fake_attribute As fakeAttribute = Nothing
        Dim fake2 As fake2 = Nothing
        assertion.is_false(GetType(C1).custom_attribute(Of fake)(fake))
        assertion.is_true(GetType(C1).custom_attribute(Of fakeAttribute)(fake_attribute))
        assertion.is_true(GetType(C2).custom_attribute(Of fake2)(fake2))

        assertion.equal(fake.count(), 0)
        assertion.equal(fakeAttribute.count(), 1)
        assertion.equal(fake2.count(), 1)
        Return True
    End Function
End Class
