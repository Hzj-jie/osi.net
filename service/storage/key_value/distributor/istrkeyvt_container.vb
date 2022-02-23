
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utils
Imports osi.service.argument
Imports osi.service.device

Public Class istrkeyvt_container
    Private Const unknown_istrkeyvt_name As String = "__unknown_istrkeyvt_instance_name__"
    Private ReadOnly targets As vector(Of pair(Of istrkeyvt, String))

    Public Sub New(ByVal targets As vector(Of pair(Of istrkeyvt, String)))
        assert(targets IsNot Nothing AndAlso Not targets.empty())
        Me.targets = targets
    End Sub

    Public Shared Function ctor(ByVal names() As String, ByRef r As istrkeyvt_container) As Boolean
        If isemptyarray(names) Then
            Return False
        Else
            Dim ts As vector(Of pair(Of istrkeyvt, String)) = Nothing
            ts = New vector(Of pair(Of istrkeyvt, String))()
            For i As UInt32 = 0 To array_size(names) - uint32_1
                Dim c As container(Of istrkeyvt) = Nothing
                If container(Of istrkeyvt).create(names(i), c) Then
                    assert(c IsNot Nothing)
                    assert(c.device_pool() Is Nothing AndAlso c.device() Is Nothing AndAlso c.instance() IsNot Nothing)
                    ts.emplace_back(pair.emplace_of(+c, names(i)))
                    c.release()
                Else
                    Return False
                End If
            Next

            r = New istrkeyvt_container(ts)
            Return True
        End If
    End Function

    Public Shared Function ctor(ByVal ParamArray names() As String) As istrkeyvt_container
        Dim r As istrkeyvt_container = Nothing
        assert(ctor(names, r))
        Return r
    End Function

    Public Shared Function create(ByVal v As var, ByRef o As istrkeyvt_container) As Boolean
        Return v IsNot Nothing AndAlso
               ctor(+(v.other_values()), o)
    End Function

    Public Function size() As UInt32
        Return targets.size()
    End Function

    Default Public ReadOnly Property target(ByVal i As Int32) As istrkeyvt
        Get
            assert(i >= 0 AndAlso i < targets.size())
            assert(targets(i).first IsNot Nothing)
            Return targets(i).first
        End Get
    End Property

    Public Function random_select() As istrkeyvt
        Return target(rnd_int(0, size()))
    End Function

    Public Function name(ByVal i As UInt32) As String
        assert(i < targets.size())
        If String.IsNullOrEmpty(targets(i).second) Then
            Return unknown_istrkeyvt_name
        End If
        Return targets(i).second
    End Function
End Class
