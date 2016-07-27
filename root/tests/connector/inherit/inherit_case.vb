
Imports osi.root.utt
Imports osi.root.connector

Friend Class inherit_case
    Inherits [case]

    Public Const size As Int32 = 200000000

    Private Class static_class
        <Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Public Shared Sub work()
        End Sub
    End Class

    Private Interface inter_interface
        Sub work()
    End Interface

    Private Class impl_class
        Implements inter_interface

        <Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Public Sub work() Implements inter_interface.work
        End Sub
    End Class

    Private Class base_class
        <Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Public Sub work()
        End Sub
    End Class

    Private Class inherit_class
        Inherits base_class

        <Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Public Sub work2()
        End Sub
    End Class

    Private Class base_class2
        <Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Public Overridable Sub work()
        End Sub
    End Class

    Private Class override_class
        Inherits base_class2

        <Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
        Public Overrides Sub work()
        End Sub
    End Class

    Private ReadOnly base As Boolean = False
    Private ReadOnly base2 As Boolean = False
    Private ReadOnly inherit As Boolean = False
    Private ReadOnly override As Boolean = False
    Private ReadOnly inter As Boolean = False
    Private ReadOnly impl As Boolean = False
    Private ReadOnly [static] As Boolean = False

    Public Sub New(Optional ByVal base As Boolean = False,
                   Optional ByVal base2 As Boolean = False,
                   Optional ByVal inherit As Boolean = False,
                   Optional ByVal override As Boolean = False,
                   Optional ByVal inter As Boolean = False,
                   Optional ByVal impl As Boolean = False,
                   Optional ByVal [static] As Boolean = False)
        Me.base = base
        Me.base2 = base2
        Me.inherit = inherit
        Me.override = override
        Me.inter = inter
        Me.impl = impl
        Me.static = [static]
    End Sub

    Public Overrides Function run() As Boolean
        If base Then
            Dim c As base_class = Nothing
            c = New base_class()
            For i As Int32 = 0 To size - 1
                c.work()
            Next
        ElseIf base2 Then
            Dim c As base_class2 = Nothing
            c = New base_class2()
            For i As Int32 = 0 To size - 1
                c.work()
            Next
        ElseIf inherit Then
            Dim c As inherit_class = Nothing
            c = New inherit_class()
            For i As Int32 = 0 To size - 1
                c.work2()
            Next
        ElseIf override Then
            Dim c As base_class2 = Nothing
            c = New override_class()
            For i As Int32 = 0 To size - 1
                c.work()
            Next
        ElseIf inter Then
            Dim c As inter_interface = Nothing
            c = New impl_class()
            For i As Int32 = 0 To size - 1
                c.work()
            Next
        ElseIf impl Then
            Dim c As impl_class = Nothing
            c = New impl_class()
            For i As Int32 = 0 To size - 1
                c.work()
            Next
        ElseIf [static] Then
            For i As Int32 = 0 To size - 1
                static_class.work()
            Next
        Else
            Return assert(False)
        End If

        Return True
    End Function
End Class
