
#Const recursive_compare = False
Imports osi.root.constants
Imports osi.root.connector

Public NotInheritable Class comparable_type
    Implements IComparable(Of comparable_type), IComparable(Of Type), IComparable

    Private Shared ReadOnly cmp As Func(Of Type, Type, Int32)
    Private ReadOnly t As Type

    Shared Sub New()
        If use_recursive_compare() Then
            cmp = Function(x As Type, y As Type) As Int32
                      Return x.GUID().CompareTo(y.GUID())
                  End Function
        Else
            cmp = Function(x As Type, y As Type) As Int32
                      Return x.AssemblyQualifiedName().CompareTo(y.AssemblyQualifiedName())
                  End Function
        End If
    End Sub

    Private Shared Function use_recursive_compare() As Boolean
#If recursive_compare Then
        Return Not envs.mono
#Else
        Return False
#End If
    End Function

    Public Shared Function [New](Of T)() As comparable_type
        Return New comparable_type(GetType(T))
    End Function

    Public Sub New(ByVal i As Type)
        Me.t = i
    End Sub

    Public Sub New(ByVal i As comparable_type)
        Me.New(+i)
    End Sub

    Public Shared Function compare_type(ByVal i As Type, ByVal j As Type) As Int32
        Dim r As Int32 = 0
        r = object_compare(i, j)
        If r = object_compare_undetermined Then
            If use_recursive_compare() Then
                r = cmp(i, j)
                If r = 0 Then
                    Dim gl() As Type = Nothing
                    Dim gr() As Type = Nothing
                    gl = i.GetGenericArguments()
                    gr = j.GetGenericArguments()
                    assert(array_size(gl) = array_size(gr))
                    Dim k As UInt32 = uint32_0
                    While k < array_size(gl)
                        r = compare_type(gl(k), gr(k))
                        If r <> 0 Then
                            Return r
                        End If
                        k += uint32_1
                    End While
                    Return 0
                Else
                    Return r
                End If
            Else
                Return cmp(i, j)
            End If
        Else
            Return r
        End If
    End Function

    Public Shared Operator +(ByVal this As comparable_type) As Type
        If this Is Nothing Then
            Return Nothing
        Else
            Return this.t
        End If
    End Operator

    Public Shared Widening Operator CType(ByVal this As comparable_type) As Type
        Return +this
    End Operator

    Public Shared Widening Operator CType(ByVal this As Type) As comparable_type
        Return New comparable_type(this)
    End Operator

    Public Function CompareTo(ByVal other As comparable_type) As Int32 _
                             Implements IComparable(Of comparable_type).CompareTo
        Return compare_type(+Me, +other)
    End Function

    Public Function CompareTo(ByVal obj As Object) As Int32 Implements IComparable.CompareTo
        Dim a As comparable_type = Nothing
        If cast(Of comparable_type)(obj, a) Then
            Return CompareTo(a)
        Else
            Return CompareTo(cast(Of Type)(obj, False))
        End If
    End Function

    Public Function CompareTo(ByVal other As Type) As Int32 Implements IComparable(Of Type).CompareTo
        Return compare_type(+Me, other)
    End Function

    Public Overrides Function GetHashCode() As Int32
        Return If(t Is Nothing, 0, t.GetHashCode())
    End Function

    Public Overrides Function ToString() As String
        Return If(t Is Nothing, Nothing, t.ToString())
    End Function
End Class
