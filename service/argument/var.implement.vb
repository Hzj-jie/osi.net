
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Text
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports cc = osi.root.connector

Partial Public NotInheritable Class var
    Implements ICloneable, ICloneable(Of var), IComparable, IComparable(Of var)

    Public Function Clone() As Object Implements ICloneable.Clone
        Return CloneT()
    End Function

    Public Function CloneT() As var Implements ICloneable(Of var).Clone
        Return copy_constructor(Of var).copy_from(c, raw, binded, others)
    End Function

    Public Shared Function compare(ByVal this As var, ByVal that As var) As Int32
        Dim c As Int32 = object_compare(this, that)
        If c <> object_compare_undetermined Then
            Return c
        End If
        assert(Not this Is Nothing)
        assert(Not that Is Nothing)
        c = cc.compare(this.raw, that.raw)
        If c <> 0 Then
            Return c
        End If
        c = cc.compare(this.binded, that.binded)
        If c <> 0 Then
            Return c
        End If
        Return cc.compare(this.others, that.others)
    End Function

    Public Function CompareTo(ByVal obj As Object) As Int32 Implements IComparable.CompareTo
        Return CompareTo(cast(Of var)(obj, False))
    End Function

    Public Function CompareTo(ByVal other As var) As Int32 Implements IComparable(Of var).CompareTo
        Return compare(Me, other)
    End Function

    Public Overrides Function ToString() As String
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
