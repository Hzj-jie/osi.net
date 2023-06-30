
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.utt
Imports osi.root.utt.attributes
Imports osi.service.constructor

<test>
Public NotInheritable Class slim_constructor_test
    <test>
    Private Shared Sub select_first_succeeded_constructor()
        Dim c As slim_constructor(Of Int32, Int32, UInt32) = Nothing
        c = New slim_constructor(Of Int32, Int32, UInt32)()
        c.register(1,
                   Function(ByVal i As Int32, ByRef o As UInt32) As Boolean
                       If (i And 1) = 1 Then
                           o = int32_uint32(unchecked_inc(i))
                           Return True
                       End If
                       Return False
                   End Function)
        c.register(1,
                   Function(ByVal i As Int32) As UInt32
                       Return int32_uint32(i)
                   End Function)
        c.register(2,
                   Function(ByVal i As Int32) As UInt32
                       Return int32_uint32(unchecked_dec(i))
                   End Function)

        For x As Int32 = 0 To 100
            Dim key As Int32 = 0
            key = If(rnd_bool(), 1, 2)
            Dim i As Int32 = 0
            i = rnd_int()
            Dim o As UInt32 = 0
            assertion.is_true(c.[New](key, i, o))
            If key = 1 Then
                If (i And 1) = 1 Then
                    assertion.equal(o, int32_uint32(unchecked_inc(i)))
                Else
                    assertion.equal(o, int32_uint32(i))
                End If
            Else
                assertion.equal(o, int32_uint32(unchecked_dec(i)))
            End If
        Next
    End Sub

    <test>
    Private Shared Sub not_key_registered()
        Dim c As slim_constructor(Of Int32, Int32, UInt32) = Nothing
        c = New slim_constructor(Of Int32, Int32, UInt32)()
        c.register(1, Function(ByVal i As Int32) As UInt32
                          Return int32_uint32(i)
                      End Function)
        c.register(2, Function(ByVal i As Int32) As UInt32
                          Return int32_uint32(i)
                      End Function)
        Dim o As UInt32 = 0
        assertion.is_true(c.[New](1, 1, o))
        assertion.is_true(c.[New](2, 1, o))
        assertion.is_false(c.[New](3, 1, o))
    End Sub

    <test>
    Private Shared Sub no_constructors_succeeded()
        Dim c As slim_constructor(Of Int32, Int32, UInt32) = Nothing
        c = New slim_constructor(Of Int32, Int32, UInt32)()
        c.register(1,
                   Function(ByVal i As Int32, ByRef r As UInt32) As Boolean
                       Return False
                   End Function)
        Dim o As UInt32 = 0
        assertion.is_false(c.[New](1, 1, o))
    End Sub

    <test>
    Private Shared Sub keyless_first_succeeded()
        Dim c As keyless_slim_constructor(Of Int32, UInt32) = Nothing
        c = New keyless_slim_constructor(Of Int32, UInt32)()
        c.register(Function(ByVal i As Int32, ByRef o As UInt32) As Boolean
                       If (i And 1) = 1 Then
                           o = int32_uint32(i)
                           Return True
                       End If
                       Return False
                   End Function)
        c.register(Function(ByVal i As Int32) As UInt32
                       Return int32_uint32(unchecked_dec(i))
                   End Function)
        For x As Int32 = 0 To 100
            Dim i As Int32 = 0
            i = rnd_int()
            Dim o As UInt32 = 0
            assertion.is_true(c.[New](i, o))
            If (i And 1) = 1 Then
                assertion.equal(o, int32_uint32(i))
            Else
                assertion.equal(o, int32_uint32(unchecked_dec(i)))
            End If
        Next
    End Sub

    <test>
    Private Shared Sub keyless_no_constructors_succeeded()
        Dim c As keyless_slim_constructor(Of Int32, UInt32) = Nothing
        c = New keyless_slim_constructor(Of Int32, UInt32)()
        c.register(Function(ByVal i As Int32, ByRef x As UInt32) As Boolean
                       Return False
                   End Function)
        c.register(Function(ByVal i As Int32, ByRef x As UInt32) As Boolean
                       Return False
                   End Function)
        Dim o As UInt32 = 0
        assertion.is_false(c.[New](1, o))
        assertion.is_false(c.[New](2, o))
    End Sub

    Private Sub New()
    End Sub
End Class
