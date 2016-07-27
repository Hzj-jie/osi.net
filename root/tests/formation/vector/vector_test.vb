
Imports osi.root.utt
Imports osi.root.connector
Imports osi.root.formation

Partial Public Class vector_test
    Inherits repeat_case_wrapper
    Private ReadOnly size_base As Int32 = 0

    Friend Sub New(ByVal validation As Boolean, ByVal size_base As Int32)
        MyBase.New(New vector_case(validation))
        Me.size_base = size_base
    End Sub

    Public Sub New()
        Me.New(True, 1024)
    End Sub

    Protected Overrides Function test_size() As Int64
        Return If(isreleasebuild(), 4, 1) * size_base
    End Function

    Partial Private Class vector_case
        Inherits random_run_case

        Private ReadOnly v As vector(Of String) = Nothing
        Private ReadOnly validation As Boolean = True

        Public Sub New(ByVal validation As Boolean)
            MyBase.New()
            Me.v = New vector(Of String)()
            Me.validation = validation

            insert_call(0.12, AddressOf size)
            insert_call(0.04, AddressOf clear)
            insert_call(0.1, AddressOf data)
            insert_call(0.1, AddressOf push_back)
            insert_call(0.05, AddressOf insert)
            insert_call(0.05, AddressOf pop_back)
            insert_call(0.02, AddressOf find)
            insert_call(0.07, AddressOf fill)
            insert_call(0.07, AddressOf empty)
            insert_call(0.06, AddressOf front)
            insert_call(0.1, AddressOf back)
            insert_call(0.1, AddressOf [erase])
            insert_call(0.1, AddressOf shrink_to_fit)
            insert_call(0.02, AddressOf clone)
        End Sub

        Public Overrides Function finish() As Boolean
            v.clear()
            Return MyBase.finish()
        End Function
    End Class
End Class
