
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.service.argument
Imports osi.service.device
Imports store_t = osi.root.formation.unordered_map(Of osi.root.formation.array_pointer(Of Byte), Byte())

<global_init(global_init_level.server_services)>
Public NotInheritable Class memory
    Implements isynckeyvalue
    Private ReadOnly max_value_size As Int64
    Private ReadOnly m As store_t
    Private vs As Int64

    Public Sub New(Optional ByVal max_value_size As Int64 = npos)
        Me.max_value_size = If(max_value_size <= 0, max_int64, max_value_size)
        m = New store_t()
        vs = 0
    End Sub

    Public Shared Function ctor(Optional ByVal max_value_size As Int64 = npos) As istrkeyvt
        Return adapt(New memory(max_value_size))
    End Function

    Private Function enough_storage(ByVal inc As Int64) As Boolean
        If inc < 0 Then
            Return True
        Else
            Dim c As Int64 = 0
            assert(capacity(c))
            Return inc + vs <= c
        End If
    End Function

    Public Function append(ByVal key() As Byte,
                           ByVal value() As Byte,
                           ByRef result As Boolean) As Boolean Implements isynckeyvalue.append
        If enough_storage(array_size(value)) Then
            Dim k As array_pointer(Of Byte) = Nothing
            k = New array_pointer(Of Byte)(key)
            Dim it As store_t.iterator = Nothing
            it = m.find(k)
            If it = m.end() Then
                m(k) = value
            Else
                Dim original_size As UInt32 = 0
                original_size = array_size((+it).second)
                ReDim Preserve (+it).second(CInt(original_size) + array_size_i(value) - 1)
                arrays.copy((+it).second, original_size, value)
            End If
            vs += array_size(value)
            result = True
        Else
            result = False
        End If
        Return True
    End Function

    Public Function capacity(ByRef result As Int64) As Boolean Implements isynckeyvalue.capacity
        result = max_value_size
        Return True
    End Function

    Public Function delete(ByVal key() As Byte,
                           ByRef result As Boolean) As Boolean Implements isynckeyvalue.delete
        Dim it As store_t.iterator = Nothing
        it = m.find(array_pointer.of(key))
        If it = m.end() Then
            result = False
        Else
            vs -= array_size((+it).second)
            assert(m.erase(it))
            result = True
        End If
        Return True
    End Function

    Public Function empty(ByRef result As Boolean) As Boolean Implements isynckeyvalue.empty
        result = m.empty()
        Return True
    End Function

    Public Function full(ByRef result As Boolean) As Boolean Implements isynckeyvalue.full
        result = Not enough_storage(0)
        Return True
    End Function

    Public Function heartbeat() As Boolean Implements isynckeyvalue.heartbeat
        Return True
    End Function

    Public Function keycount(ByRef result As Int64) As Boolean Implements isynckeyvalue.keycount
        result = m.size()
        Return True
    End Function

    Public Function list(ByRef result As vector(Of Byte())) As Boolean Implements isynckeyvalue.list
        If result Is Nothing Then
            result = New vector(Of Byte())()
        Else
            result.clear()
        End If
        Dim it As store_t.iterator = Nothing
        it = m.begin()
        While it <> m.end()
            result.push_back(+((+it).first))
            it += 1
        End While
        Return True
    End Function

    Public Function modify(ByVal key() As Byte,
                           ByVal value() As Byte,
                           ByRef result As Boolean) As Boolean Implements isynckeyvalue.modify
        Dim sz As Int64 = 0
        Dim inc As Int64 = 0
        assert(sizeof(key, sz))
        If sz >= 0 Then
            inc = array_size(value) - sz
        Else
            inc = array_size(value)
        End If
        If enough_storage(inc) Then
            m(array_pointer.of(key)) = value
            vs += inc
            result = True
        Else
            result = False
        End If
        Return True
    End Function

    Public Function read(ByVal key() As Byte,
                         ByRef value() As Byte) As Boolean Implements isynckeyvalue.read
        Dim it As store_t.iterator = Nothing
        it = m.find(New array_pointer(Of Byte)(key))
        If it = m.end() Then
            value = Nothing
        Else
            value = copy((+it).second)
        End If
        Return True
    End Function

    Public Function retire() As Boolean Implements isynckeyvalue.retire
        m.clear()
        vs = 0
        Return True
    End Function

    Public Function seek(ByVal key() As Byte,
                         ByRef result As Boolean) As Boolean Implements isynckeyvalue.seek
        result = (m.find(array_pointer.of(Of Byte)(key)) <> m.end())
        Return True
    End Function

    Public Function sizeof(ByVal key() As Byte,
                           ByRef result As Int64) As Boolean Implements isynckeyvalue.sizeof
        Dim it As store_t.iterator = Nothing
        it = m.find(New array_pointer(Of Byte)(key))
        If it = m.end() Then
            result = npos
        Else
            result = array_size((+it).second)
        End If
        Return True
    End Function

    Public Function [stop]() As Boolean Implements isynckeyvalue.stop
        Return True
    End Function

    Public Function valuesize(ByRef result As Int64) As Boolean Implements isynckeyvalue.valuesize
        result = vs
        Return True
    End Function

    Public Shared Function create(ByVal p As var, ByRef o As memory) As Boolean
        If p Is Nothing Then
            Return False
        End If
        Dim v As Int64 = 0
        If p.other_values().empty() OrElse
               Not Int64.TryParse(p.other_values()(0), v) Then
            o = New memory()
        Else
            o = New memory(v)
        End If
        Return True
    End Function

    Private Shared Sub init()
        assert(constructor.register(Of memory)(AddressOf create))
        assert(constructor.register(Of istrkeyvt)(
                   "memory",
                   Function(v As var, ByRef o As istrkeyvt) As Boolean
                       Dim m As memory = Nothing
                       If memory.create(v, m) Then
                           o = adapt(m)
                           Return True
                       Else
                           Return False
                       End If
                   End Function))
        assert(constructor.register(Of istrkeyvt)(
                   "cached-memory",
                   Function(v As var, ByRef o As istrkeyvt) As Boolean
                       Dim m As memory = Nothing
                       If memory.create(v, m) Then
                           o = adapt(adapter.cached(m))
                           Return True
                       Else
                           Return False
                       End If
                   End Function))
    End Sub
End Class
