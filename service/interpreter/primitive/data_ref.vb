
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

Namespace primitive
    Public NotInheritable Class data_ref
        Implements exportable, IComparable(Of data_ref), IComparable

        Public Const max_value As Int64 = (max_int64 >> 2)
        Public Const rel_min_value As Int64 = (min_int64 >> 2)
        Public Const abs_min_value As Int64 = 0
        Public Const heap_min_value As Int64 = 0
        Private Const rel_str As String = "rel"
        Private Const abs_str As String = "abs"
        Private Const heap_str As String = "heap"
        Private Shared ReadOnly ref_types() As String = {rel_str, abs_str, heap_str}

        Private Enum location As Long
            absolute = 0 'in stack
            relative = 1 'in stack
            heap = 2     'in heap
        End Enum

        Private l As location
        Private o As Int64

        Public Shared Function random(Optional ByRef r As Int64 = 0) As data_ref
            Dim o As data_ref = Nothing
            Do
                r = rnd_int64()
            Loop Until [New](r, o)
            assert(Not o Is Nothing)
            Return o
        End Function

        Public Sub New()
            Me.New(0)
        End Sub

        Public Sub New(ByVal i As Int64)
            assert([set](i))
        End Sub

        Public Shared Function [New](ByVal i As Int64, ByRef o As data_ref) As Boolean
            Dim x As New data_ref()
            If x.[set](i) Then
                o = x
                Return True
            End If
            Return False
        End Function

        Public Shared Function rel(ByVal i As Int64, ByRef o As data_ref) As Boolean
            If i <= max_value AndAlso i >= rel_min_value Then
                o = New data_ref() With {.l = location.relative, .o = i}
                Return True
            End If
            Return False
        End Function

        Public Shared Function rel(ByVal i As Int64) As data_ref
            Dim o As data_ref = Nothing
            assert(rel(i, o))
            Return o
        End Function

        Public Shared Function abs(ByVal i As Int64, ByRef o As data_ref) As Boolean
            If i <= max_value AndAlso i >= abs_min_value Then
                o = New data_ref() With {.l = location.absolute, .o = i}
                Return True
            End If
            Return False
        End Function

        Public Shared Function abs(ByVal i As Int64) As data_ref
            Dim o As data_ref = Nothing
            assert(abs(i, o))
            Return o
        End Function

        Public Shared Function heap(ByVal i As Int64, ByRef o As data_ref) As Boolean
            If i <= max_value AndAlso i >= heap_min_value Then
                o = New data_ref() With {.l = location.heap, .o = i}
                Return True
            End If
            Return False
        End Function

        Public Shared Function heap(ByVal i As Int64) As data_ref
            Dim o As data_ref = Nothing
            assert(heap(i, o))
            Return o
        End Function

        Public Function to_rel(ByVal size As UInt64, ByRef o As data_ref) As Boolean
            If relative() Then
                o = Me
                Return True
            End If
            Return rel(CLng(size) - offset() - 1, o)
        End Function

        Public Function to_rel(ByVal size As UInt64) As data_ref
            Dim o As data_ref = Nothing
            assert(to_rel(size, o))
            Return o
        End Function

        Public Function to_abs(ByVal size As UInt64, ByRef o As data_ref) As Boolean
            If absolute() Then
                o = Me
                Return True
            End If
            Return abs(CLng(size) - offset() - 1, o)
        End Function

        Public Function to_abs(ByVal size As UInt64) As data_ref
            Dim o As data_ref = Nothing
            assert(to_abs(size, o))
            Return o
        End Function

        Public Function [set](ByVal i As Int64) As Boolean
            Dim v As Int64 = (i And 3)
            If Not [Enum].IsDefined(GetType(location), v) Then
                Return False
            End If
            Dim l As location = CType(v, location)
            Dim o As Int64 = (i >> 2)
            Select Case l
                Case location.relative
                    If o < rel_min_value OrElse o > max_value Then
                        Return False
                    End If
                Case location.absolute
                    If o < abs_min_value OrElse o > max_value Then
                        Return False
                    End If
                Case location.heap
                    If o < heap_min_value OrElse o > max_value Then
                        Return False
                    End If
                Case Else
                    Return assert(False)
            End Select
            Me.l = l
            Me.o = o
            Return True
        End Function

        Public Function relative() As Boolean
            Return l = location.relative
        End Function

        Public Function absolute() As Boolean
            Return l = location.absolute
        End Function

        Public Function heap() As Boolean
            Return l = location.heap
        End Function

        Public Function offset() As Int64
            Return o
        End Function

        Public Function export() As Int64
            Return (o << 2) + l
        End Function

        Public Function bytes_size() As UInt32 Implements exportable.bytes_size
            Return sizeof_int64
        End Function

        Public Function export(ByRef b() As Byte) As Boolean Implements exportable.export
            b = int64_bytes(export())
            Return True
        End Function

        Public Function export(ByRef s As String) As Boolean Implements exportable.export
            s = strcat(If(heap(), heap_str, If(relative(), rel_str, abs_str)), o)
            Return True
        End Function

        Public Function import(ByVal i() As Byte, ByRef p As UInt32) As Boolean Implements exportable.import
            Dim o As Int64 = 0
            Return bytes_int64(i, o, p) AndAlso
                   [set](o)
        End Function

        Private Shared Function separate(ByVal s As String, ByRef t As String, ByRef o As Int64) As Boolean
            If s.null_or_whitespace() Then
                Return False
            End If
            t = Nothing
            For i As Int32 = 0 To array_size_i(ref_types) - 1
                If strstartwith(s, ref_types(i)) Then
                    t = ref_types(i)
                    assert(Not t Is Nothing)
                    Exit For
                End If
            Next
            If t Is Nothing Then
                Return False
            End If
            Return Int64.TryParse(strmid(s, strlen(t)), o)
        End Function

        Public Function import(ByVal s As vector(Of String), ByRef p As UInt32) As Boolean Implements exportable.import
            If s Is Nothing OrElse s.size() <= p Then
                Return False
            End If
            Dim ref_type As String = Nothing
            Dim offset As Int64 = 0
            If Not separate(s(p), ref_type, offset) Then
                Return False
            End If

            If ref_type.Equals(heap_str) Then
                l = location.heap
            ElseIf ref_type.Equals(rel_str) Then
                l = location.relative
            ElseIf ref_type.Equals(abs_sTR) Then
                l = location.absolute
            Else
                Return False
            End If
            o = offset
            p += uint32_1
            Return True
        End Function

        Public Function CompareTo(ByVal obj As Object) As Int32 Implements IComparable.CompareTo
            Return CompareTo(cast(Of data_ref)(obj, False))
        End Function

        Public Function CompareTo(ByVal other As data_ref) As Int32 Implements IComparable(Of data_ref).CompareTo
            Dim c As Int32 = object_compare(Me, other)
            If c <> object_compare_undetermined Then
                Return c
            End If
            assert(Not other Is Nothing)
            Return export().CompareTo(other.export())
        End Function

        Public Overrides Function ToString() As String
            Dim s As String = Nothing
            assert(export(s))
            Return s
        End Function
    End Class
End Namespace
