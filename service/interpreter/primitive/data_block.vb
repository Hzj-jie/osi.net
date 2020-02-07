
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

Namespace primitive
    Public Class data_block
        Implements exportable, IComparable(Of data_block), IComparable, ICloneable(Of data_block), ICloneable

        Public Class prefix
            Public Const [int] As Char = "i"c
            Public Const [long] As Char = "l"c
            Public Const [double] As Char = "d"c
            Public Const [boolean] As Char = "b"c
            Public Const [array] As Char = "a"c
            Public Const [c_escaped_string] As Char = "E"c
            Public Const [string] As Char = "s"c
        End Class

        Private buff() As Byte

        Public Shared Function random(Optional ByRef type As Char = Nothing,
                                      Optional ByVal safe_str_double As Boolean = True) As data_block
            Dim i As UInt32 = 0
            i = rnd_uint() Mod CUInt(6)
            Select Case i
                Case 0
                    type = prefix.int
                    Return New data_block(rnd_int())
                Case 1
                    type = prefix.long
                    Return New data_block(rnd_int64())
                Case 2
                    type = prefix.boolean
                    Return New data_block(rnd_bool())
                Case 3
                    type = prefix.array
                    Return New data_block(rnd_bytes())
                Case 4
                    type = prefix.double
                    If safe_str_double Then
                        ' Avoid precision issue
                        Return New data_block(CDbl(CInt(rnd_double(min_int16, max_int16) * 10000)) / 10000)
                    Else
                        Return New data_block(rnd_double())
                    End If
                Case 5
                    type = prefix.string
                    Return New data_block(rnd_chars(rnd_int(100, 200)))
                Case Else
                    assert(False)
                    Return Nothing
            End Select
        End Function

        Public Sub New()
            buff = Nothing
        End Sub

        Public Sub New(ByVal i As Int32)
            buff = int32_bytes(i)
        End Sub

        Public Sub New(ByVal i As UInt32)
            buff = uint32_bytes(i)
        End Sub

        Public Sub New(ByVal l As Int64)
            buff = int64_bytes(l)
        End Sub

        Public Sub New(ByVal l As UInt64)
            buff = uint64_bytes(l)
        End Sub

        Public Sub New(ByVal d As Double)
            buff = double_bytes(d)
        End Sub

        Public Sub New(ByVal b As Boolean)
            buff = bool_bytes(b)
        End Sub

        Public Sub New(ByVal a() As Byte)
            assert(Not a Is Nothing)
            buff = a
        End Sub

        ' unescaped string
        Public Sub New(ByVal s As String)
            buff = str_bytes(s)
        End Sub

        Public Function str(ByVal type As Char, ByRef o As String) As Boolean
            assert(Not buff Is Nothing)
            Select Case type
                Case prefix.boolean
                    Dim b As Boolean = False
                    If as_bool(b) Then
                        o = strcat(type, b)
                        Return True
                    End If
                Case prefix.c_escaped_string
                    Dim s As String = Nothing
                    If as_c_escaped_string(s) Then
                        o = strcat(type, s)
                        Return True
                    End If
                Case prefix.double
                    Dim d As Double = 0
                    If as_double(d) Then
                        o = strcat(type, d)
                        Return True
                    End If
                Case prefix.int
                    Dim i As Int32 = 0
                    If as_int(i) Then
                        o = strcat(type, i)
                        Return True
                    End If
                Case prefix.long
                    Dim l As Int64 = 0
                    If as_long(l) Then
                        o = strcat(type, l)
                        Return True
                    End If
                Case prefix.string
                    Dim s As String = Nothing
                    If as_string(s) Then
                        o = strcat(type, s)
                        Return True
                    End If
                Case prefix.array
                    o = strcat(type, buff.hex_str())
                    Return True
                Case Else
                    assert(False)
            End Select
            Return False
        End Function

        Public Function as_bool(ByRef o As Boolean) As Boolean
            assert(Not buff Is Nothing)
            Return entire_bytes_bool(buff, o)
        End Function

        Public Function as_bool() As Boolean
            Dim o As Boolean = False
            assert(as_bool(o))
            Return o
        End Function

        Public Function as_c_escaped_string(ByRef o As String) As Boolean
            assert(Not buff Is Nothing)
            Dim s As String = Nothing
            Return as_string(s) AndAlso
                   c_unescape(s, o)
        End Function

        Public Function as_c_escaped_string() As String
            Dim o As String = Nothing
            assert(as_c_escaped_string(o))
            Return o
        End Function

        Public Function as_double(ByRef o As Double) As Boolean
            assert(Not buff Is Nothing)
            Return entire_bytes_double(buff, o)
        End Function

        Public Function as_double() As Double
            Dim o As Double = 0
            assert(as_double(o))
            Return o
        End Function

        Public Function as_int(ByRef o As Int32) As Boolean
            assert(Not buff Is Nothing)
            Return entire_bytes_int32(buff, o)
        End Function

        Public Function as_int() As Int32
            Dim o As Int32 = 0
            assert(as_int(o))
            Return o
        End Function

        Public Function as_long(ByRef o As Int64) As Boolean
            assert(Not buff Is Nothing)
            Return entire_bytes_int64(buff, o)
        End Function

        Public Function as_long() As Int64
            Dim o As Int64 = 0
            assert(as_long(o))
            Return o
        End Function

        Public Function as_string(ByRef o As String) As Boolean
            assert(Not buff Is Nothing)
            o = bytes_str(buff)
            Return True
        End Function

        Public Function as_string() As String
            Dim o As String = Nothing
            assert(as_string(o))
            Return o
        End Function

        Public Function as_bytes() As Byte()
            Return buff
        End Function

        Public Function value_bytes_size() As UInt32
            assert(Not buff Is Nothing)
            Return array_size(buff)
        End Function

        Public Function bytes_size() As UInt32 Implements exportable.bytes_size
            Return value_bytes_size() + sizeof_uint32
        End Function

        Public Function export(ByRef b() As Byte) As Boolean Implements exportable.export
            assert(Not buff Is Nothing)
            ReDim b(CInt(bytes_size() - uint32_1))
            Dim p As UInt32 = 0
            assert(uint32_bytes(array_size(buff), b, p))
            assert(p = sizeof_uint32)
            memcpy(b, p, buff)
            Return True
        End Function

        Public Function export(ByRef s As String) As Boolean Implements exportable.export
            Return str(prefix.array, s)
        End Function

        Public Function import(ByVal i() As Byte, ByRef p As UInt32) As Boolean Implements exportable.import
            Dim l As UInt32 = 0
            If bytes_uint32(i, l, p) AndAlso array_size(i) >= p + l Then
                ReDim buff(CInt(l - uint32_1))
                memcpy(buff, uint32_0, i, p, l)
                p += l
                Return True
            End If
            Return False
        End Function

        Public Function import(ByVal s As vector(Of String), ByRef p As UInt32) As Boolean Implements exportable.import
            If s.null_or_empty() OrElse s.size() <= p Then
                Return False
            End If
            Dim raw As String = Nothing
            raw = strmid(s(p), uint32_1)
            Select Case s(p)(0)
                Case prefix.array
                    buff = raw.hex_bytes_buff()
                    If Not raw.hex_bytes(buff) Then
                        Return False
                    End If
                Case prefix.boolean
                    buff = bool_bytes(raw.true())
                Case prefix.double
                    Dim d As Double = 0
                    If Not Double.TryParse(raw, d) Then
                        Return False
                    End If
                    buff = double_bytes(d)
                Case prefix.int
                    Dim i As Int32 = 0
                    If Not Int32.TryParse(raw, i) Then
                        Return False
                    End If
                    buff = int32_bytes(i)
                Case prefix.long
                    Dim l As Int64 = 0
                    If Not Int64.TryParse(raw, l) Then
                        Return False
                    End If
                    buff = int64_bytes(l)
                Case prefix.c_escaped_string
                    Dim ss As String = Nothing
                    If Not raw.c_unescape(ss) Then
                        Return False
                    End If
                    buff = str_bytes(ss)
                Case prefix.string
                    buff = str_bytes(raw)
                Case Else
                    Return False
            End Select
            assert(Not buff Is Nothing)
            p += uint32_1
            Return True
        End Function

        Public Function CompareTo(ByVal obj As Object) As Int32 Implements IComparable.CompareTo
            Return CompareTo(cast(Of data_block)(obj, False))
        End Function

        Public Function CompareTo(ByVal other As data_block) As Int32 Implements IComparable(Of data_block).CompareTo
            Dim c As Int32 = 0
            c = object_compare(Me, other)
            If c = object_compare_undetermined Then
                assert(Not other Is Nothing)
                Return memcmp(buff, other.buff)
            End If
            Return c
        End Function

        Public NotOverridable Overrides Function ToString() As String
            Dim o As String = Nothing
            assert(str(prefix.array, o))
            Return o
        End Function

        Public Function CloneT() As data_block Implements ICloneable(Of data_block).Clone
            Return New data_block(copy(buff))
        End Function

        Public Function Clone() As Object Implements ICloneable.Clone
            Return CloneT()
        End Function
    End Class
End Namespace
