
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.automata
Imports osi.service.automata.syntaxer

Namespace syntaxer
    Public Module _matching_delegate
        Private ReadOnly get_type As Func(Of matching_delegate, UInt32)

        Sub New()
            Dim f As FieldInfo = Nothing
            f = GetType(matching_delegate).GetField("type", BindingFlags.NonPublic Or BindingFlags.Instance)
            assert(Not f Is Nothing)
            get_type = Function(this As matching_delegate) As UInt32
                           assert(Not this Is Nothing)
                           Return direct_cast(Of UInt32)(f.GetValue(this))
                       End Function
        End Sub

        <Extension()> Public Function [type](ByVal this As matching_delegate) As UInt32
            Return get_type(this)
        End Function
    End Module

    Public NotInheritable Class fake_matching_delegate
        Inherits matching
        Implements IComparable(Of matching_delegate)

        Private ReadOnly type As UInt32

        Public Sub New(ByVal c As syntax_collection, ByVal type As UInt32)
            MyBase.New(c)
            Me.type = type
        End Sub

        Public Overrides Function match(ByVal v As vector(Of typed_word),
                                        ByRef p As UInt32,
                                        ByVal parent As typed_node) As Boolean
            Return assert(False)
        End Function

        Public Overrides Function CompareTo(ByVal other As matching) As Int32
            Return CompareTo(cast(Of matching_delegate).from(other, False))
        End Function

        Public Overloads Function CompareTo(ByVal other As matching_delegate) As Int32 _
                                           Implements IComparable(Of matching_delegate).CompareTo
            Dim c As Int32 = 0
            c = object_compare(Me, other)
            If c <> object_compare_undetermined Then
                Return c
            End If
            Return compare(Me.type, other.type())
        End Function
    End Class
End Namespace
