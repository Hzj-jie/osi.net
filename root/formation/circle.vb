
Imports osi.root.connector

Public Class circle(Of T)
#If Not (PocketPC OrElse Smartphone) Then
    Public Const defaultSize As Int64 = 1024 * 1024
#Else
    Public Const defaultSize As Int64 = 128 * 1024
#End If

    Public waittime As Int64 = 100

    Protected Friend _last As Int32
    Protected Friend _first As Int32
    Protected Friend _buff() As T

    Public ReadOnly Property buff() As T()
        Get
            Return _buff
        End Get
    End Property

    Public ReadOnly Property last() As T
        Get
            Return pick(size() - 1)
        End Get
    End Property

    Public ReadOnly Property first() As T
        Get
            Return pick(0)
        End Get
    End Property

    Public Function full() As Boolean
        Return _first = _last
    End Function

    Public Function remindSize(ByVal size As Int64) As Boolean
        Return ((_first - _last + buffsize()) Mod _buff.Length()) >= size
    End Function

    Public Function size() As Int64
        Return ((_last - _first + _buff.Length()) Mod _buff.Length())
    End Function

    Public Function haveSize(ByVal requestsize As Int64) As Boolean
        Return size() >= requestsize
    End Function

    Public Function buffsize() As Int64
        Return _buff.Length() - 1
    End Function

    Public Function empty() As Boolean
        Return remindSize(buffsize())
    End Function

    Protected Friend Sub initial(ByVal size As Int64)
        ReDim _buff(size)
        clear()
    End Sub

    Public Sub clear()
        _first = 0
        _last = 0
    End Sub

    Public Sub New()
        initial(defaultSize)
    End Sub

    Public Sub New(ByVal size As Int64)
        initial(size)
    End Sub

    Public Function push_back(ByVal data As T, Optional ByVal waitIfFull As Boolean = False) As Boolean
retry:
        If Not remindSize(1) Then
            If waitIfFull Then
                sleep(waittime)
                GoTo retry
            Else
                Return False
            End If
        End If

        copy(_buff(_last), data)
        _last += 1
        If _last > buffsize() Then
            _last = 0
        End If

        Return True
    End Function

    Public Function push_back(ByVal data() As T, Optional ByVal start As Int64 = 0, _
                              Optional ByVal len As Int64 = -1, _
                              Optional ByVal waitIfFull As Boolean = False) As Boolean
        If data Is Nothing OrElse data.Length = 0 Then
            Return True
        End If
        If len = -1 Then
            len = data.Length() - start
        End If

retry:
        If Not remindSize(len) Then
            If waitIfFull AndAlso buffsize() >= len Then
                sleep(waittime)
                GoTo retry
            Else
                Return False
            End If
        End If

        Dim i As Int32
        For i = 0 To len - 1
            copy(_buff(_last), data(i + start))
            _last += 1
            If _last > buffsize() Then
                _last = 0
            End If
        Next

        Return True
    End Function

    Public Function push_back(ByVal data As vector(Of T),
                              Optional ByVal start As Int64 = 0,
                              Optional ByVal len As Int64 = -1,
                              Optional ByVal waitIfFull As Boolean = False) As Boolean
        Return push_back(+data, start, len, waitIfFull)
    End Function

    Public Function pop_front(ByRef data() As T,
                              Optional ByVal start As Int64 = 0,
                              Optional ByVal len As Int64 = -1,
                              Optional ByVal waitIfEmpty As Boolean = False) As Boolean
        If len = -1 Then
            len = size()
        End If
        If data Is Nothing OrElse data.Length() <> len + start Then
            ReDim data(len + start - 1)
        End If

retry:
        If Not haveSize(len) Then
            If waitIfEmpty Then
                sleep(waittime)
                GoTo retry
            Else
                Return False
            End If
        End If

        Dim i As Int32
        For i = 0 To len - 1
            copy(data(i + start), _buff(_first))
            _first += 1
            If _first > buffsize() Then
                _first = 0
            End If
        Next

        Return True
    End Function

    Default Public Property pick(ByVal index As Int64) As T
        Get
            If index >= size() Then
                Return Nothing
            Else
                Return _buff(_first + index)
            End If
        End Get
        Set(ByVal value As T)
            If index < size() Then
                _buff(_first + index) = value
            End If
        End Set
    End Property

End Class
