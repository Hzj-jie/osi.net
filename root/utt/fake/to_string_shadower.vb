
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Diagnostics.CodeAnalysis
Imports System.Runtime.CompilerServices
Imports osi.root.connector

Public Module _to_string_shadower
    <Extension()> Public Function to_string_shadower(Of T)(ByVal this As T) As to_string_shadower(Of T)
        Return New to_string_shadower(Of T)(this)
    End Function
End Module

<SuppressMessage("Microsoft.Design", "BC42333")>
Public Class to_string_shadower(Of T)
#Disable Warning BC42333
    Implements IComparable(Of to_string_shadower(Of T)), IComparable, IComparable(Of T)
#Enable Warning BC42333

    Private Shared ReadOnly type_name As String = strcat("to_string_shadower(Of ", type_info(Of T).name, ")")
    Private ReadOnly x As T

    Public Sub New(ByVal x As T)
        Me.x = x
    End Sub

    Public Function CompareTo(ByVal obj As Object) As Int32 Implements IComparable.CompareTo
        Return CompareTo(cast(Of to_string_shadower(Of T))(obj, False))
    End Function

    Public Function CompareTo(ByVal other As to_string_shadower(Of T)) As Int32 _
                             Implements IComparable(Of to_string_shadower(Of T)).CompareTo
        Dim cmp As Int32 = 0
        cmp = object_compare(Me, other)
        If cmp <> object_compare_undetermined Then
            Return cmp
        End If
        assert(Not other Is Nothing)
        Return compare(x, other.x)
    End Function

    Public Function CompareTo(ByVal other As T) As Int32 Implements IComparable(Of T).CompareTo
        Return compare(x, other)
    End Function

    Public NotOverridable Overrides Function ToString() As String
        Return type_name
    End Function
End Class
