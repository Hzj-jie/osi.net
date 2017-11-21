
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Text
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports cc = osi.root.connector

Partial Public Class var
    Implements ICloneable, ICloneable(Of var), IComparable, IComparable(Of var)

    Protected Function clone(Of R As var)() As R
        Return copy_constructor(Of R).invoke(copy(c), copy(raw), copy(binded), copy(others))
    End Function

    Public Function Clone() As Object Implements ICloneable.Clone
        Return CloneT()
    End Function

    Public Function CloneT() As var Implements ICloneable(Of var).Clone
        Return clone(Of var)()
    End Function

    Public Shared Function compare(ByVal this As var, ByVal that As var) As Int32
        Dim c As Int32 = 0
        c = object_compare(this, that)
        If c = object_compare_undetermined Then
            assert(Not this Is Nothing)
            assert(Not that Is Nothing)
            c = cc.compare(this.raw, that.raw)
            If c = 0 Then
                c = cc.compare(this.binded, that.binded)
                If c = 0 Then
                    c = cc.compare(this.others, that.others)
                    Return c
                Else
                    Return c
                End If
            Else
                Return c
            End If
        Else
            Return c
        End If
    End Function

    Public Function CompareTo(ByVal obj As Object) As Int32 Implements IComparable.CompareTo
        Return CompareTo(cast(Of var)(obj, False))
    End Function

    Public Function CompareTo(ByVal other As var) As Int32 Implements IComparable(Of var).CompareTo
        Return compare(Me, other)
    End Function

    Public NotOverridable Overrides Function ToString() As String
        Dim s As StringBuilder = Nothing
        s = New StringBuilder()
        Dim it As map(Of String, vector(Of String)).iterator = Nothing
        it = raw.begin()
        While it <> raw.end()
            If (+it).second Is Nothing Then
                s.Append(c.create_full_switcher((+it).first))
                s.Append(c.argument_separator)
            Else
                Dim i As UInt32 = 0
                While i < (+it).second.size()
                    s.Append(c.create_arg((+it).first, (+it).second(i)))
                    s.Append(c.argument_separator)
                    i += uint32_1
                End While
            End If
            it += 1
        End While

        If Not other_values().empty() Then
            Dim i As UInt32 = 0
            While i < other_values().size()
                s.Append(other_values()(i))
                s.Append(c.argument_separator)
                i += uint32_1
            End While
        End If
        s.Remove(CInt(strlen(s) - strlen(c.argument_separator)), CInt(strlen(c.argument_separator)))
        Return Convert.ToString(s)
    End Function
End Class
