
Option Explicit On
Option Infer Off
Option Strict On

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with string_serializer_registry.vbp ----------
'so change string_serializer_registry.vbp instead of this file


Imports System.IO
Imports osi.root.constants

<global_init(global_init_level.functor)>
Friend NotInheritable Class string_serializer_registry
    Private Shared ReadOnly run_shared_sub_new As cctor_delegator = New cctor_delegator(
        Sub()

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with all_number_types.vbp ----------
'so change all_number_types.vbp instead of this file




'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with string_serializer_registry_impl.vbp ----------
'so change string_serializer_registry_impl.vbp instead of this file


            string_serializer.register(Sub(ByVal i As SByte, ByVal o As StringWriter)
                                           assert(Not o Is Nothing)
                                           o.Write(i)
                                       End Sub,
                                       Function(ByVal i As StringReader, ByRef o As SByte) As Boolean
                                           assert(Not i Is Nothing)
                                           Return SByte.TryParse(i.ReadToEnd(), o)
                                       End Function)
#If "SByte" <> "Char" Then
            json_serializer.register(Sub(ByVal i As SByte, ByVal o As StringWriter)
                                         assert(Not o Is Nothing)
                                         o.Write(i)
                                     End Sub)
#End If
'finish string_serializer_registry_impl.vbp --------

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with string_serializer_registry_impl.vbp ----------
'so change string_serializer_registry_impl.vbp instead of this file


            string_serializer.register(Sub(ByVal i As Byte, ByVal o As StringWriter)
                                           assert(Not o Is Nothing)
                                           o.Write(i)
                                       End Sub,
                                       Function(ByVal i As StringReader, ByRef o As Byte) As Boolean
                                           assert(Not i Is Nothing)
                                           Return Byte.TryParse(i.ReadToEnd(), o)
                                       End Function)
#If "Byte" <> "Char" Then
            json_serializer.register(Sub(ByVal i As Byte, ByVal o As StringWriter)
                                         assert(Not o Is Nothing)
                                         o.Write(i)
                                     End Sub)
#End If
'finish string_serializer_registry_impl.vbp --------

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with string_serializer_registry_impl.vbp ----------
'so change string_serializer_registry_impl.vbp instead of this file


            string_serializer.register(Sub(ByVal i As Int16, ByVal o As StringWriter)
                                           assert(Not o Is Nothing)
                                           o.Write(i)
                                       End Sub,
                                       Function(ByVal i As StringReader, ByRef o As Int16) As Boolean
                                           assert(Not i Is Nothing)
                                           Return Int16.TryParse(i.ReadToEnd(), o)
                                       End Function)
#If "Int16" <> "Char" Then
            json_serializer.register(Sub(ByVal i As Int16, ByVal o As StringWriter)
                                         assert(Not o Is Nothing)
                                         o.Write(i)
                                     End Sub)
#End If
'finish string_serializer_registry_impl.vbp --------

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with string_serializer_registry_impl.vbp ----------
'so change string_serializer_registry_impl.vbp instead of this file


            string_serializer.register(Sub(ByVal i As UInt16, ByVal o As StringWriter)
                                           assert(Not o Is Nothing)
                                           o.Write(i)
                                       End Sub,
                                       Function(ByVal i As StringReader, ByRef o As UInt16) As Boolean
                                           assert(Not i Is Nothing)
                                           Return UInt16.TryParse(i.ReadToEnd(), o)
                                       End Function)
#If "UInt16" <> "Char" Then
            json_serializer.register(Sub(ByVal i As UInt16, ByVal o As StringWriter)
                                         assert(Not o Is Nothing)
                                         o.Write(i)
                                     End Sub)
#End If
'finish string_serializer_registry_impl.vbp --------

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with string_serializer_registry_impl.vbp ----------
'so change string_serializer_registry_impl.vbp instead of this file


            string_serializer.register(Sub(ByVal i As Int32, ByVal o As StringWriter)
                                           assert(Not o Is Nothing)
                                           o.Write(i)
                                       End Sub,
                                       Function(ByVal i As StringReader, ByRef o As Int32) As Boolean
                                           assert(Not i Is Nothing)
                                           Return Int32.TryParse(i.ReadToEnd(), o)
                                       End Function)
#If "Int32" <> "Char" Then
            json_serializer.register(Sub(ByVal i As Int32, ByVal o As StringWriter)
                                         assert(Not o Is Nothing)
                                         o.Write(i)
                                     End Sub)
#End If
'finish string_serializer_registry_impl.vbp --------

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with string_serializer_registry_impl.vbp ----------
'so change string_serializer_registry_impl.vbp instead of this file


            string_serializer.register(Sub(ByVal i As UInt32, ByVal o As StringWriter)
                                           assert(Not o Is Nothing)
                                           o.Write(i)
                                       End Sub,
                                       Function(ByVal i As StringReader, ByRef o As UInt32) As Boolean
                                           assert(Not i Is Nothing)
                                           Return UInt32.TryParse(i.ReadToEnd(), o)
                                       End Function)
#If "UInt32" <> "Char" Then
            json_serializer.register(Sub(ByVal i As UInt32, ByVal o As StringWriter)
                                         assert(Not o Is Nothing)
                                         o.Write(i)
                                     End Sub)
#End If
'finish string_serializer_registry_impl.vbp --------

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with string_serializer_registry_impl.vbp ----------
'so change string_serializer_registry_impl.vbp instead of this file


            string_serializer.register(Sub(ByVal i As Int64, ByVal o As StringWriter)
                                           assert(Not o Is Nothing)
                                           o.Write(i)
                                       End Sub,
                                       Function(ByVal i As StringReader, ByRef o As Int64) As Boolean
                                           assert(Not i Is Nothing)
                                           Return Int64.TryParse(i.ReadToEnd(), o)
                                       End Function)
#If "Int64" <> "Char" Then
            json_serializer.register(Sub(ByVal i As Int64, ByVal o As StringWriter)
                                         assert(Not o Is Nothing)
                                         o.Write(i)
                                     End Sub)
#End If
'finish string_serializer_registry_impl.vbp --------

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with string_serializer_registry_impl.vbp ----------
'so change string_serializer_registry_impl.vbp instead of this file


            string_serializer.register(Sub(ByVal i As UInt64, ByVal o As StringWriter)
                                           assert(Not o Is Nothing)
                                           o.Write(i)
                                       End Sub,
                                       Function(ByVal i As StringReader, ByRef o As UInt64) As Boolean
                                           assert(Not i Is Nothing)
                                           Return UInt64.TryParse(i.ReadToEnd(), o)
                                       End Function)
#If "UInt64" <> "Char" Then
            json_serializer.register(Sub(ByVal i As UInt64, ByVal o As StringWriter)
                                         assert(Not o Is Nothing)
                                         o.Write(i)
                                     End Sub)
#End If
'finish string_serializer_registry_impl.vbp --------

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with string_serializer_registry_impl.vbp ----------
'so change string_serializer_registry_impl.vbp instead of this file


            string_serializer.register(Sub(ByVal i As Double, ByVal o As StringWriter)
                                           assert(Not o Is Nothing)
                                           o.Write(i)
                                       End Sub,
                                       Function(ByVal i As StringReader, ByRef o As Double) As Boolean
                                           assert(Not i Is Nothing)
                                           Return Double.TryParse(i.ReadToEnd(), o)
                                       End Function)
#If "Double" <> "Char" Then
            json_serializer.register(Sub(ByVal i As Double, ByVal o As StringWriter)
                                         assert(Not o Is Nothing)
                                         o.Write(i)
                                     End Sub)
#End If
'finish string_serializer_registry_impl.vbp --------

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with string_serializer_registry_impl.vbp ----------
'so change string_serializer_registry_impl.vbp instead of this file


            string_serializer.register(Sub(ByVal i As Single, ByVal o As StringWriter)
                                           assert(Not o Is Nothing)
                                           o.Write(i)
                                       End Sub,
                                       Function(ByVal i As StringReader, ByRef o As Single) As Boolean
                                           assert(Not i Is Nothing)
                                           Return Single.TryParse(i.ReadToEnd(), o)
                                       End Function)
#If "Single" <> "Char" Then
            json_serializer.register(Sub(ByVal i As Single, ByVal o As StringWriter)
                                         assert(Not o Is Nothing)
                                         o.Write(i)
                                     End Sub)
#End If
'finish string_serializer_registry_impl.vbp --------

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with string_serializer_registry_impl.vbp ----------
'so change string_serializer_registry_impl.vbp instead of this file


            string_serializer.register(Sub(ByVal i As Char, ByVal o As StringWriter)
                                           assert(Not o Is Nothing)
                                           o.Write(i)
                                       End Sub,
                                       Function(ByVal i As StringReader, ByRef o As Char) As Boolean
                                           assert(Not i Is Nothing)
                                           Return Char.TryParse(i.ReadToEnd(), o)
                                       End Function)
#If "Char" <> "Char" Then
            json_serializer.register(Sub(ByVal i As Char, ByVal o As StringWriter)
                                         assert(Not o Is Nothing)
                                         o.Write(i)
                                     End Sub)
#End If
'finish string_serializer_registry_impl.vbp --------

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with string_serializer_registry_impl.vbp ----------
'so change string_serializer_registry_impl.vbp instead of this file


            string_serializer.register(Sub(ByVal i As Boolean, ByVal o As StringWriter)
                                           assert(Not o Is Nothing)
                                           o.Write(i)
                                       End Sub,
                                       Function(ByVal i As StringReader, ByRef o As Boolean) As Boolean
                                           assert(Not i Is Nothing)
                                           Return Boolean.TryParse(i.ReadToEnd(), o)
                                       End Function)
#If "Boolean" <> "Char" Then
            json_serializer.register(Sub(ByVal i As Boolean, ByVal o As StringWriter)
                                         assert(Not o Is Nothing)
                                         o.Write(i)
                                     End Sub)
#End If
'finish string_serializer_registry_impl.vbp --------
'finish all_number_types.vbp --------
        End Sub)

    Private Shared Sub init()
    End Sub

    Private Sub New()
    End Sub
End Class

'finish string_serializer_registry.vbp --------
