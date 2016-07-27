
Imports osi.root.formation
Imports osi.root.connector

Namespace fullstack.parser
    Public Class functions
        Private ReadOnly m As map(Of String, vector(Of executor.[function]))

        Public Sub New()
            m = New map(Of String, vector(Of executor.[function]))()
        End Sub

        Public Function define(ByVal name As String,
                               ByVal f As executor.[function]) As Boolean
            assert(Not String.IsNullOrEmpty(name))
            assert(Not f Is Nothing)
            m(name).push_back(f)
            Return True
        End Function

        Public Function resolve(ByVal name As String,
                                ByVal inputs() As type,
                                ByRef f As executor.[function]) As Boolean
            assert(Not String.IsNullOrEmpty(name))
            Dim it As map(Of String, vector(Of executor.[function])).iterator = Nothing
            it = m.find(name)
            If it = m.end() Then
                Return False
            Else
                Dim vf As vector(Of executor.[function]) = Nothing
                vf = (+it).second
                assert(Not vf Is Nothing)
                For i As Int32 = 0 To vf.size() - 1
                    If vf(i).acceptable_inputs(inputs) Then
                        f = vf(i)
                        Return True
                    End If
                Next
                Return False
            End If
        End Function
    End Class
End Namespace
