
Option Explicit On
Option Infer Off
Option Strict On

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with binary_operator_registry.vbp ----------
'so change binary_operator_registry.vbp instead of this file


Imports osi.root.constants

<global_init(global_init_level.functor)>
Friend NotInheritable Class binary_operator_registry
    Shared Sub New()

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with all_number_types.vbp ----------
'so change all_number_types.vbp instead of this file




'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with binary_operator_registry_impl.vbp ----------
'so change binary_operator_registry_impl.vbp instead of this file


        binary_operator.register_add(Function(ByVal x As SByte, ByVal y As SByte) As SByte
                                         Return x + y
                                     End Function)

        binary_operator.register_minus(Function(ByVal x As SByte, ByVal y As SByte) As SByte
                                           Return x - y
                                       End Function)
'finish binary_operator_registry_impl.vbp --------

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with binary_operator_registry_impl.vbp ----------
'so change binary_operator_registry_impl.vbp instead of this file


        binary_operator.register_add(Function(ByVal x As Byte, ByVal y As Byte) As Byte
                                         Return x + y
                                     End Function)

        binary_operator.register_minus(Function(ByVal x As Byte, ByVal y As Byte) As Byte
                                           Return x - y
                                       End Function)
'finish binary_operator_registry_impl.vbp --------

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with binary_operator_registry_impl.vbp ----------
'so change binary_operator_registry_impl.vbp instead of this file


        binary_operator.register_add(Function(ByVal x As Int16, ByVal y As Int16) As Int16
                                         Return x + y
                                     End Function)

        binary_operator.register_minus(Function(ByVal x As Int16, ByVal y As Int16) As Int16
                                           Return x - y
                                       End Function)
'finish binary_operator_registry_impl.vbp --------

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with binary_operator_registry_impl.vbp ----------
'so change binary_operator_registry_impl.vbp instead of this file


        binary_operator.register_add(Function(ByVal x As UInt16, ByVal y As UInt16) As UInt16
                                         Return x + y
                                     End Function)

        binary_operator.register_minus(Function(ByVal x As UInt16, ByVal y As UInt16) As UInt16
                                           Return x - y
                                       End Function)
'finish binary_operator_registry_impl.vbp --------

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with binary_operator_registry_impl.vbp ----------
'so change binary_operator_registry_impl.vbp instead of this file


        binary_operator.register_add(Function(ByVal x As Int32, ByVal y As Int32) As Int32
                                         Return x + y
                                     End Function)

        binary_operator.register_minus(Function(ByVal x As Int32, ByVal y As Int32) As Int32
                                           Return x - y
                                       End Function)
'finish binary_operator_registry_impl.vbp --------

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with binary_operator_registry_impl.vbp ----------
'so change binary_operator_registry_impl.vbp instead of this file


        binary_operator.register_add(Function(ByVal x As UInt32, ByVal y As UInt32) As UInt32
                                         Return x + y
                                     End Function)

        binary_operator.register_minus(Function(ByVal x As UInt32, ByVal y As UInt32) As UInt32
                                           Return x - y
                                       End Function)
'finish binary_operator_registry_impl.vbp --------

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with binary_operator_registry_impl.vbp ----------
'so change binary_operator_registry_impl.vbp instead of this file


        binary_operator.register_add(Function(ByVal x As Int64, ByVal y As Int64) As Int64
                                         Return x + y
                                     End Function)

        binary_operator.register_minus(Function(ByVal x As Int64, ByVal y As Int64) As Int64
                                           Return x - y
                                       End Function)
'finish binary_operator_registry_impl.vbp --------

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with binary_operator_registry_impl.vbp ----------
'so change binary_operator_registry_impl.vbp instead of this file


        binary_operator.register_add(Function(ByVal x As UInt64, ByVal y As UInt64) As UInt64
                                         Return x + y
                                     End Function)

        binary_operator.register_minus(Function(ByVal x As UInt64, ByVal y As UInt64) As UInt64
                                           Return x - y
                                       End Function)
'finish binary_operator_registry_impl.vbp --------

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with binary_operator_registry_impl.vbp ----------
'so change binary_operator_registry_impl.vbp instead of this file


        binary_operator.register_add(Function(ByVal x As Double, ByVal y As Double) As Double
                                         Return x + y
                                     End Function)

        binary_operator.register_minus(Function(ByVal x As Double, ByVal y As Double) As Double
                                           Return x - y
                                       End Function)
'finish binary_operator_registry_impl.vbp --------

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with binary_operator_registry_impl.vbp ----------
'so change binary_operator_registry_impl.vbp instead of this file


        binary_operator.register_add(Function(ByVal x As Single, ByVal y As Single) As Single
                                         Return x + y
                                     End Function)

        binary_operator.register_minus(Function(ByVal x As Single, ByVal y As Single) As Single
                                           Return x - y
                                       End Function)
'finish binary_operator_registry_impl.vbp --------
'finish all_number_types.vbp --------
    End Sub

    Private Shared Sub init()
    End Sub

    Private Sub New()
    End Sub
End Class

'finish binary_operator_registry.vbp --------
