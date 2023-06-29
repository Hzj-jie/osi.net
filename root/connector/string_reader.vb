
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports osi.root.constants

Public Module _string_reader
    Private ReadOnly _pos As FieldInfo =
        Function() As FieldInfo
            Dim _pos As FieldInfo = Nothing
            Try
                _pos = GetType(StringReader).GetField("_pos", binding_flags.instance_private)
            Catch ex As Exception
                assert(False, ex.details())
            End Try
            assert(Not _pos Is Nothing)
            assert(Not _pos.IsStatic())
            Return _pos
        End Function()

    Private ReadOnly _length As FieldInfo =
        Function() As FieldInfo
            Dim _length As FieldInfo = Nothing
            Try
                _length = GetType(StringReader).GetField("_length", binding_flags.instance_private)
            Catch ex As Exception
                assert(False, ex.details())
            End Try
            assert(Not _length Is Nothing)
            assert(Not _length.IsStatic())
            Return _length
        End Function()

    <Extension()> Public Function length(ByVal this As StringReader) As UInt32
        assert(Not this Is Nothing)
        Dim r As Int32 = 0
        r = direct_cast(Of Int32)(_length.GetValue(this))
        assert(r >= 0)
        Return CUInt(r)
    End Function

    <Extension()> Public Function position(ByVal this As StringReader) As UInt32
        assert(Not this Is Nothing)
        Dim r As Int32 = 0
        r = direct_cast(Of Int32)(_pos.GetValue(this))
        assert(r >= 0)
        Return CUInt(r)
    End Function

    <Extension()> Public Function position(ByVal this As StringReader, ByVal p As UInt32) As Boolean
        assert(Not this Is Nothing)
        If p > this.length() Then
            Return False
        Else
            assert(p <= max_int32)
            _pos.SetValue(this, CInt(p))
            Return True
        End If
    End Function
End Module