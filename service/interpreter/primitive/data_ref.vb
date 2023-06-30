
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

        Private Enum ref_types
            abs = 0
            rel = 1
            habs = 2
            hrel = 3
        End Enum

        Private r As ref_types
        Private o As Int64

        Public Shared Function random(Optional ByRef r As Int64 = 0) As data_ref
            Dim o As data_ref = Nothing
            Do
                ' rnd_int64 is not very well distributed because of the precision.
                r = (rnd_int64() Or rnd_int(ref_types.abs, ref_types.hrel + 1))
            Loop Until [New](r, o)
            assert(Not o Is Nothing)
            Return o
        End Function

        Public Shared Function [New](ByVal i As Int64, ByRef o As data_ref) As Boolean
            Dim x As New data_ref()
            If x.set(i) Then
                o = x
                Return True
            End If
            Return False
        End Function

        Public Shared Function rel(ByVal i As Int64, ByRef o As data_ref) As Boolean
            If i > max_value OrElse i < rel_min_value Then
                Return False
            End If
            o = New data_ref() With {.r = ref_types.rel, .o = i}
            Return True
        End Function

        Public Shared Function rel(ByVal i As Int64) As data_ref
            Dim o As data_ref = Nothing
            assert(rel(i, o))
            Return o
        End Function

        Public Shared Function hrel(ByVal i As Int64, ByRef o As data_ref) As Boolean
            If i > max_value OrElse i < rel_min_value Then
                Return False
            End If
            o = New data_ref() With {.r = ref_types.hrel, .o = i}
            Return True
        End Function

        Public Shared Function hrel(ByVal i As Int64) As data_ref
            Dim o As data_ref = Nothing
            assert(hrel(i, o))
            Return o
        End Function

        Public Shared Function abs(ByVal i As Int64, ByRef o As data_ref) As Boolean
            If i > max_value OrElse i < abs_min_value Then
                Return False
            End If
            o = New data_ref() With {.r = ref_types.abs, .o = i}
            Return True
        End Function

        Public Shared Function abs(ByVal i As Int64) As data_ref
            Dim o As data_ref = Nothing
            assert(abs(i, o))
            Return o
        End Function

        Public Shared Function habs(ByVal i As Int64, ByRef o As data_ref) As Boolean
            If i > max_value OrElse i < abs_min_value Then
                Return False
            End If
            o = New data_ref() With {.r = ref_types.habs, .o = i}
            Return True
        End Function

        Public Shared Function habs(ByVal i As Int64) As data_ref
            Dim o As data_ref = Nothing
            assert(habs(i, o))
            Return o
        End Function

        Private Function [set](ByVal i As Int64) As Boolean
            Dim r As ref_types = enum_def(Of ref_types)().from(i And 3)
            Dim o As Int64 = (i >> 2)
            If r = ref_types.rel OrElse r = ref_types.hrel Then
                If o < rel_min_value OrElse o > max_value Then
                    Return False
                End If
            Else
                If o < abs_min_value OrElse o > max_value Then
                    Return False
                End If
            End If
            Me.r = r
            Me.o = o
            Return True
        End Function

        'VisibleForTesting
        Public Function to_rel(ByVal size As UInt64, ByRef o As data_ref) As Boolean
            If relative() OrElse heap_relative() Then
                o = Me
                Return True
            End If
            If on_heap() Then
                Return hrel(CLng(size) - offset() - 1, o)
            End If
            Return rel(CLng(size) - offset() - 1, o)
        End Function

        Public Function to_heap() As data_ref
            assert(on_stack())
            Dim r As New data_ref()
            r.r = If(Me.r = ref_types.abs, ref_types.habs, ref_types.hrel)
            r.o = o
            Return r
        End Function

        Public Function on_stack() As Boolean
            Return relative() OrElse absolute()
        End Function

        Public Function on_heap() As Boolean
            Return heap_relative() OrElse heap_absolute()
        End Function

        Public Function relative() As Boolean
            Return r = ref_types.rel
        End Function

        Public Function absolute() As Boolean
            Return r = ref_types.abs
        End Function

        Public Function heap_relative() As Boolean
            Return r = ref_types.hrel
        End Function

        Public Function heap_absolute() As Boolean
            Return r = ref_types.habs
        End Function

        Public Function offset() As Int64
            Return o
        End Function

        Public Function export() As Int64
            Return (o << 2) Or enum_def(Of ref_types)().to(Of Int32)(r)
        End Function

        Public Function bytes_size() As UInt32 Implements exportable.bytes_size
            Return sizeof_int64
        End Function

        Public Function export(ByRef b() As Byte) As Boolean Implements exportable.export
            b = int64_bytes(export())
            Return True
        End Function

        Public Function export(ByRef s As String) As Boolean Implements exportable.export
            s = strcat(r, o)
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
            For Each i As ref_types In {ref_types.abs, ref_types.rel, ref_types.habs, ref_types.hrel}
                If strstartwith(s, i.ToString()) Then
                    t = i.ToString()
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

            If Not enum_def(Of ref_types)().from(ref_type, r) Then
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
