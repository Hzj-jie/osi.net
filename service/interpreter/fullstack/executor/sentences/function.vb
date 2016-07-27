
Imports osi.root.connector
Imports osi.root.utils

Namespace fullstack.executor
    Public Class [function]
        Implements sentence

        Public ReadOnly return_type As type
        Public ReadOnly input_types() As type

        Public Sub New(ByVal return_type As type,
                       ByVal input_types() As type)
            Me.return_type = return_type
            Me.input_types = input_types
#If DEBUG Then
            For i As Int32 = 0 To input_type_count() - 1
                assert(Not input_types(i) Is Nothing)
            Next
#End If
        End Sub

        Public Function has_return() As Boolean
            ''return_type Is Nothing' means void
            Return Not return_type Is Nothing
        End Function

        Public Function has_inputs() As Boolean
            Return input_type_count() > 0
        End Function

        Public Function acceptable_inputs(ByVal ParamArray inputs() As type) As Boolean
            If array_size(inputs) = input_type_count() Then
                For i As Int32 = 0 To input_type_count() - 1
                    If Not inputs(i).is_var() AndAlso
                       Not input_types(i).is_var() AndAlso
                       Not inputs(i).is_type(input_types(i)) Then
                        Return False
                    End If
                Next
                Return True
            Else
                Return False
            End If
        End Function

        Protected Overridable Function execute(ByVal inputs() As Object) As Object
            assert(False)
            Return Nothing
        End Function

        Protected Overridable Function execute(ByVal inputs() As variable) As variable
            Dim v() As Object = Nothing
            ReDim v(input_type_count() - 1)
            For i As Int32 = 0 To input_type_count() - 1
                assert(Not inputs(i) Is Nothing)
                If Not input_types(i).is_var() AndAlso
                   Not inputs(i).is_type(input_types(i)) Then
                    Throw invalid_runtime_casting_exception.instance
                Else
                    v(i) = inputs(i).value
                End If
            Next
            Dim o As Object = Nothing
            o = execute(v)
            If has_return() Then
                Return New variable(return_type, o)
            Else
                Return Nothing
            End If
        End Function

        Public Overridable Sub execute(ByVal domain As domain) Implements sentence.execute
            Dim v() As variable = Nothing
            ReDim v(input_type_count() - 1)
            For i As Int32 = 0 To input_type_count() - 1
                v(i) = domain.variable(0, CUInt(i))
            Next
            Dim o As variable = Nothing
            o = execute(v)
            If has_return() Then
                domain.define(o)
            End If
        End Sub

        Public Function input_type_count() As Int32
            Return array_size(input_types)
        End Function
    End Class
End Namespace
