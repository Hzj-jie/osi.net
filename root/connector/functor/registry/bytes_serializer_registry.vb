
Option Explicit On
Option Infer Off
Option Strict On

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with bytes_serializer_registry.vbp ----------
'so change bytes_serializer_registry.vbp instead of this file


Imports System.IO
Imports osi.root.constants

<global_init(default_global_init_level.functor)>
Friend NotInheritable Class bytes_serializer_registry
    Shared Sub New()

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with all_number_types.vbp ----------
'so change all_number_types.vbp instead of this file




'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with bytes_serializer_registry_impl.vbp ----------
'so change bytes_serializer_registry_impl.vbp instead of this file


        bytes_serializer.fixed.register(Function(ByVal i As SByte, ByVal o As MemoryStream) As Boolean
                                            assert(Not o Is Nothing)
                                            Return o.write(BitConverter.GetBytes(endian.to_little_endian(i)))
                                        End Function,
                                        Function(ByVal i As MemoryStream, ByRef o As SByte) As Boolean
                                            assert(Not i Is Nothing)
                                            If i.Length() - i.Position() < type_info(Of SByte).size_uint32() Then
                                                Return False
                                            End If
                                            #If "SByte" = "Byte" Then
                                                o = CByte(i.ReadByte())
                                            #ElseIf "SByte" = "SByte" Then
                                                o = byte_sbyte(CByte(i.ReadByte()))
                                            #Else
                                                Dim b() As Byte = Nothing
                                                Dim offset As UInt32 = 0
                                                i.get_buffer(b, offset)
                                                o = endian.to_little_endian(BitConverter.ToSByte(b, CInt(offset)))
                                                i.Position() += type_info(Of SByte).size()
                                            #End If
                                            Return True
                                        End Function)
'finish bytes_serializer_registry_impl.vbp --------

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with bytes_serializer_registry_impl.vbp ----------
'so change bytes_serializer_registry_impl.vbp instead of this file


        bytes_serializer.fixed.register(Function(ByVal i As Byte, ByVal o As MemoryStream) As Boolean
                                            assert(Not o Is Nothing)
                                            Return o.write(BitConverter.GetBytes(endian.to_little_endian(i)))
                                        End Function,
                                        Function(ByVal i As MemoryStream, ByRef o As Byte) As Boolean
                                            assert(Not i Is Nothing)
                                            If i.Length() - i.Position() < type_info(Of Byte).size_uint32() Then
                                                Return False
                                            End If
                                            #If "Byte" = "Byte" Then
                                                o = CByte(i.ReadByte())
                                            #ElseIf "Byte" = "SByte" Then
                                                o = byte_sbyte(CByte(i.ReadByte()))
                                            #Else
                                                Dim b() As Byte = Nothing
                                                Dim offset As UInt32 = 0
                                                i.get_buffer(b, offset)
                                                o = endian.to_little_endian(BitConverter.ToByte(b, CInt(offset)))
                                                i.Position() += type_info(Of Byte).size()
                                            #End If
                                            Return True
                                        End Function)
'finish bytes_serializer_registry_impl.vbp --------

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with bytes_serializer_registry_impl.vbp ----------
'so change bytes_serializer_registry_impl.vbp instead of this file


        bytes_serializer.fixed.register(Function(ByVal i As Int16, ByVal o As MemoryStream) As Boolean
                                            assert(Not o Is Nothing)
                                            Return o.write(BitConverter.GetBytes(endian.to_little_endian(i)))
                                        End Function,
                                        Function(ByVal i As MemoryStream, ByRef o As Int16) As Boolean
                                            assert(Not i Is Nothing)
                                            If i.Length() - i.Position() < type_info(Of Int16).size_uint32() Then
                                                Return False
                                            End If
                                            #If "Int16" = "Byte" Then
                                                o = CByte(i.ReadByte())
                                            #ElseIf "Int16" = "SByte" Then
                                                o = byte_sbyte(CByte(i.ReadByte()))
                                            #Else
                                                Dim b() As Byte = Nothing
                                                Dim offset As UInt32 = 0
                                                i.get_buffer(b, offset)
                                                o = endian.to_little_endian(BitConverter.ToInt16(b, CInt(offset)))
                                                i.Position() += type_info(Of Int16).size()
                                            #End If
                                            Return True
                                        End Function)
'finish bytes_serializer_registry_impl.vbp --------

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with bytes_serializer_registry_impl.vbp ----------
'so change bytes_serializer_registry_impl.vbp instead of this file


        bytes_serializer.fixed.register(Function(ByVal i As UInt16, ByVal o As MemoryStream) As Boolean
                                            assert(Not o Is Nothing)
                                            Return o.write(BitConverter.GetBytes(endian.to_little_endian(i)))
                                        End Function,
                                        Function(ByVal i As MemoryStream, ByRef o As UInt16) As Boolean
                                            assert(Not i Is Nothing)
                                            If i.Length() - i.Position() < type_info(Of UInt16).size_uint32() Then
                                                Return False
                                            End If
                                            #If "UInt16" = "Byte" Then
                                                o = CByte(i.ReadByte())
                                            #ElseIf "UInt16" = "SByte" Then
                                                o = byte_sbyte(CByte(i.ReadByte()))
                                            #Else
                                                Dim b() As Byte = Nothing
                                                Dim offset As UInt32 = 0
                                                i.get_buffer(b, offset)
                                                o = endian.to_little_endian(BitConverter.ToUInt16(b, CInt(offset)))
                                                i.Position() += type_info(Of UInt16).size()
                                            #End If
                                            Return True
                                        End Function)
'finish bytes_serializer_registry_impl.vbp --------

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with bytes_serializer_registry_impl.vbp ----------
'so change bytes_serializer_registry_impl.vbp instead of this file


        bytes_serializer.fixed.register(Function(ByVal i As Int32, ByVal o As MemoryStream) As Boolean
                                            assert(Not o Is Nothing)
                                            Return o.write(BitConverter.GetBytes(endian.to_little_endian(i)))
                                        End Function,
                                        Function(ByVal i As MemoryStream, ByRef o As Int32) As Boolean
                                            assert(Not i Is Nothing)
                                            If i.Length() - i.Position() < type_info(Of Int32).size_uint32() Then
                                                Return False
                                            End If
                                            #If "Int32" = "Byte" Then
                                                o = CByte(i.ReadByte())
                                            #ElseIf "Int32" = "SByte" Then
                                                o = byte_sbyte(CByte(i.ReadByte()))
                                            #Else
                                                Dim b() As Byte = Nothing
                                                Dim offset As UInt32 = 0
                                                i.get_buffer(b, offset)
                                                o = endian.to_little_endian(BitConverter.ToInt32(b, CInt(offset)))
                                                i.Position() += type_info(Of Int32).size()
                                            #End If
                                            Return True
                                        End Function)
'finish bytes_serializer_registry_impl.vbp --------

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with bytes_serializer_registry_impl.vbp ----------
'so change bytes_serializer_registry_impl.vbp instead of this file


        bytes_serializer.fixed.register(Function(ByVal i As UInt32, ByVal o As MemoryStream) As Boolean
                                            assert(Not o Is Nothing)
                                            Return o.write(BitConverter.GetBytes(endian.to_little_endian(i)))
                                        End Function,
                                        Function(ByVal i As MemoryStream, ByRef o As UInt32) As Boolean
                                            assert(Not i Is Nothing)
                                            If i.Length() - i.Position() < type_info(Of UInt32).size_uint32() Then
                                                Return False
                                            End If
                                            #If "UInt32" = "Byte" Then
                                                o = CByte(i.ReadByte())
                                            #ElseIf "UInt32" = "SByte" Then
                                                o = byte_sbyte(CByte(i.ReadByte()))
                                            #Else
                                                Dim b() As Byte = Nothing
                                                Dim offset As UInt32 = 0
                                                i.get_buffer(b, offset)
                                                o = endian.to_little_endian(BitConverter.ToUInt32(b, CInt(offset)))
                                                i.Position() += type_info(Of UInt32).size()
                                            #End If
                                            Return True
                                        End Function)
'finish bytes_serializer_registry_impl.vbp --------

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with bytes_serializer_registry_impl.vbp ----------
'so change bytes_serializer_registry_impl.vbp instead of this file


        bytes_serializer.fixed.register(Function(ByVal i As Int64, ByVal o As MemoryStream) As Boolean
                                            assert(Not o Is Nothing)
                                            Return o.write(BitConverter.GetBytes(endian.to_little_endian(i)))
                                        End Function,
                                        Function(ByVal i As MemoryStream, ByRef o As Int64) As Boolean
                                            assert(Not i Is Nothing)
                                            If i.Length() - i.Position() < type_info(Of Int64).size_uint32() Then
                                                Return False
                                            End If
                                            #If "Int64" = "Byte" Then
                                                o = CByte(i.ReadByte())
                                            #ElseIf "Int64" = "SByte" Then
                                                o = byte_sbyte(CByte(i.ReadByte()))
                                            #Else
                                                Dim b() As Byte = Nothing
                                                Dim offset As UInt32 = 0
                                                i.get_buffer(b, offset)
                                                o = endian.to_little_endian(BitConverter.ToInt64(b, CInt(offset)))
                                                i.Position() += type_info(Of Int64).size()
                                            #End If
                                            Return True
                                        End Function)
'finish bytes_serializer_registry_impl.vbp --------

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with bytes_serializer_registry_impl.vbp ----------
'so change bytes_serializer_registry_impl.vbp instead of this file


        bytes_serializer.fixed.register(Function(ByVal i As UInt64, ByVal o As MemoryStream) As Boolean
                                            assert(Not o Is Nothing)
                                            Return o.write(BitConverter.GetBytes(endian.to_little_endian(i)))
                                        End Function,
                                        Function(ByVal i As MemoryStream, ByRef o As UInt64) As Boolean
                                            assert(Not i Is Nothing)
                                            If i.Length() - i.Position() < type_info(Of UInt64).size_uint32() Then
                                                Return False
                                            End If
                                            #If "UInt64" = "Byte" Then
                                                o = CByte(i.ReadByte())
                                            #ElseIf "UInt64" = "SByte" Then
                                                o = byte_sbyte(CByte(i.ReadByte()))
                                            #Else
                                                Dim b() As Byte = Nothing
                                                Dim offset As UInt32 = 0
                                                i.get_buffer(b, offset)
                                                o = endian.to_little_endian(BitConverter.ToUInt64(b, CInt(offset)))
                                                i.Position() += type_info(Of UInt64).size()
                                            #End If
                                            Return True
                                        End Function)
'finish bytes_serializer_registry_impl.vbp --------

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with bytes_serializer_registry_impl.vbp ----------
'so change bytes_serializer_registry_impl.vbp instead of this file


        bytes_serializer.fixed.register(Function(ByVal i As Double, ByVal o As MemoryStream) As Boolean
                                            assert(Not o Is Nothing)
                                            Return o.write(BitConverter.GetBytes(endian.to_little_endian(i)))
                                        End Function,
                                        Function(ByVal i As MemoryStream, ByRef o As Double) As Boolean
                                            assert(Not i Is Nothing)
                                            If i.Length() - i.Position() < type_info(Of Double).size_uint32() Then
                                                Return False
                                            End If
                                            #If "Double" = "Byte" Then
                                                o = CByte(i.ReadByte())
                                            #ElseIf "Double" = "SByte" Then
                                                o = byte_sbyte(CByte(i.ReadByte()))
                                            #Else
                                                Dim b() As Byte = Nothing
                                                Dim offset As UInt32 = 0
                                                i.get_buffer(b, offset)
                                                o = endian.to_little_endian(BitConverter.ToDouble(b, CInt(offset)))
                                                i.Position() += type_info(Of Double).size()
                                            #End If
                                            Return True
                                        End Function)
'finish bytes_serializer_registry_impl.vbp --------

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with bytes_serializer_registry_impl.vbp ----------
'so change bytes_serializer_registry_impl.vbp instead of this file


        bytes_serializer.fixed.register(Function(ByVal i As Single, ByVal o As MemoryStream) As Boolean
                                            assert(Not o Is Nothing)
                                            Return o.write(BitConverter.GetBytes(endian.to_little_endian(i)))
                                        End Function,
                                        Function(ByVal i As MemoryStream, ByRef o As Single) As Boolean
                                            assert(Not i Is Nothing)
                                            If i.Length() - i.Position() < type_info(Of Single).size_uint32() Then
                                                Return False
                                            End If
                                            #If "Single" = "Byte" Then
                                                o = CByte(i.ReadByte())
                                            #ElseIf "Single" = "SByte" Then
                                                o = byte_sbyte(CByte(i.ReadByte()))
                                            #Else
                                                Dim b() As Byte = Nothing
                                                Dim offset As UInt32 = 0
                                                i.get_buffer(b, offset)
                                                o = endian.to_little_endian(BitConverter.ToSingle(b, CInt(offset)))
                                                i.Position() += type_info(Of Single).size()
                                            #End If
                                            Return True
                                        End Function)
'finish bytes_serializer_registry_impl.vbp --------
'finish all_number_types.vbp --------
    End Sub

    Private Shared Sub init()
    End Sub

    Private Sub New()
    End Sub
End Class

'finish bytes_serializer_registry.vbp --------