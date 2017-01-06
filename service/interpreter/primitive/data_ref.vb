
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation

Namespace primitive
    Public Class data_ref
        Implements exportable, IComparable(Of data_ref), IComparable

        Public Const max_value As Int64 = (max_int64 >> 1)
        Public Const min_value As Int64 = (min_int64 >> 1)
        Private Const rel_str As String = "rel"
        Private Const abs_str As String = "abs"
        Private Shared ReadOnly ref_types() As String = {rel_str, abs_str}
        Private r As Boolean
        Private o As Int64

        Public Shared Function random(Optional ByRef r As Int64 = 0) As data_ref
            r = rnd_int64()
            Return New data_ref(r)
        End Function

        Public Sub New()
            Me.New(0)
        End Sub

        Public Sub New(ByVal i As Int64)
            [set](i)
        End Sub

        Public Shared Function valid_offset(ByVal i As Int64) As Boolean
            Return i <= max_value AndAlso i >= min_value
        End Function

        Public Shared Function rel(ByVal i As Int64) As data_ref
            assert(valid_offset(i))
            Return New data_ref() With {.r = True, .o = i}
        End Function

        Public Shared Function abs(ByVal i As Int64) As data_ref
            assert(valid_offset(i))
            Return New data_ref() With {.r = False, .o = i}
        End Function

        Public Sub [set](ByVal i As Int64)
            r = ((i And int64_1) <> 0)
            o = (i >> 1)
        End Sub

        Public Function relative() As Boolean
            Return r
        End Function

        Public Function absolute() As Boolean
            Return Not r
        End Function

        Public Function offset() As Int64
            Return o
        End Function

        Public Function export() As Int64
            If r Then
                Return (o << 1) + int64_1
            Else
                Return (o << 1)
            End If
        End Function

        Public Function bytes_size() As UInt32 Implements exportable.bytes_size
            Return sizeof_int64
        End Function

        Public Function export(ByRef b() As Byte) As Boolean Implements exportable.export
            b = int64_bytes(export())
            Return True
        End Function

        Public Function export(ByRef s As String) As Boolean Implements exportable.export
            s = strcat(If(r, rel_str, abs_str), o)
            Return True
        End Function

        Public Function import(ByVal i() As Byte, ByRef p As UInt32) As Boolean Implements exportable.import
            Dim o As Int64 = 0
            If bytes_int64(i, o, p) Then
                [set](o)
                Return True
            Else
                Return False
            End If
        End Function

        Private Shared Function separate(ByVal s As String, ByRef t As String, ByRef o As Int64) As Boolean
            If s.null_or_whitespace() Then
                Return False
            Else
                t = Nothing
                For i As UInt32 = uint32_0 To array_size(ref_types) - uint32_1
                    If strstartwith(s, ref_types(i)) Then
                        t = ref_types(i)
                        assert(Not t Is Nothing)
                        Exit For
                    End If
                Next
                If t Is Nothing Then
                    Return False
                Else
                    Return Int64.TryParse(strmid(s, strlen(t)), o)
                End If
            End If
        End Function

        Public Function import(ByVal s As vector(Of String), ByRef p As UInt32) As Boolean Implements exportable.import
            If s Is Nothing OrElse s.size() <= p Then
                Return False
            Else
                Dim ref_type As String = Nothing
                Dim offset As Int64 = 0
                If separate(s(p), ref_type, offset) Then
                    If strsame(ref_type, rel_str) Then
                        r = True
                    ElseIf strsame(ref_type, abs_str) Then
                        r = False
                    Else
                        Return False
                    End If
                    o = offset
                    p += 1
                    Return True
                Else
                    Return False
                End If
            End If
        End Function

        Public Function CompareTo(ByVal obj As Object) As Int32 Implements IComparable.CompareTo
            Return CompareTo(cast(Of data_ref)(obj, False))
        End Function

        Public Function CompareTo(ByVal other As data_ref) As Int32 Implements IComparable(Of data_ref).CompareTo
            Dim c As Int32 = 0
            c = object_compare(Me, other)
            If c = object_compare_undetermined Then
                assert(Not other Is Nothing)
                Dim x As Int64 = 0
                Dim y As Int64 = 0
                x = export()
                y = other.export()
                If x > y Then
                    Return 1
                ElseIf x < y Then
                    Return -1
                Else
                    Return 0
                End If
            Else
                Return c
            End If
        End Function

        Public NotOverridable Overrides Function ToString() As String
            Dim s As String = Nothing
            assert(export(s))
            Return s
        End Function
    End Class
End Namespace
