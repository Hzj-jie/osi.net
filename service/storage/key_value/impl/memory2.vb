
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.service.argument
Imports osi.service.device
Imports store_t = osi.root.formation.unordered_map(Of osi.root.connector.array_ref(Of Byte), Byte())

<global_init(global_init_level.server_services)>
Public NotInheritable Class memory2
    Implements isynckeyvalue2(Of store_t.iterator)

    Private ReadOnly max_value_size As Int64
    Private ReadOnly m As store_t
    Private vs As Int64

    Public Sub New(Optional ByVal max_value_size As Int64 = npos)
        Me.max_value_size = If(max_value_size <= 0, max_int64, max_value_size)
        m = New store_t()
        vs = 0
    End Sub

    Public Shared Function ctor(Optional ByVal max_value_size As Int64 = npos) As istrkeyvt
        Return adapt(New memory2(max_value_size))
    End Function

    Private Function enough_storage(ByVal inc As Int64) As Boolean
        If inc < 0 Then
            Return True
        End If
        Dim c As Int64 = 0
        assert(capacity(c))
        Return inc + vs <= c
    End Function

    Public Function append_existing(ByVal it As store_t.iterator,
                                    ByVal value() As Byte,
                                    ByRef result As Boolean) As Boolean _
                                   Implements isynckeyvalue2(Of store_t.iterator).append_existing
        assert(it <> m.end())
        If Not enough_storage(array_size(value)) Then
            Return False
        End If
        Dim original_size As UInt32 = 0
        original_size = array_size((+it).second)
        ReDim Preserve (+it).second(CInt(original_size) + array_size_i(value) - 1)
        arrays.copy((+it).second, original_size, value)
        result = True
        Return True
    End Function

    Public Function capacity(ByRef result As Int64) As Boolean Implements _
                            isynckeyvalue2(Of store_t.iterator).capacity
        result = max_value_size
        Return True
    End Function

    Public Function delete_existing(ByVal it As store_t.iterator,
                                    ByRef result As Boolean) As Boolean _
                                   Implements isynckeyvalue2(Of store_t.iterator).delete_existing
        result = assert(m.erase(it))
        Return True
    End Function

    Public Function empty(ByRef result As Boolean) As Boolean Implements isynckeyvalue2(Of store_t.iterator).empty
        result = m.empty()
        Return True
    End Function

    Public Function full(ByRef result As Boolean) As Boolean Implements isynckeyvalue2(Of store_t.iterator).full
        result = Not enough_storage(0)
        Return True
    End Function

    Public Function heartbeat() As Boolean Implements isynckeyvalue2(Of store_t.iterator).heartbeat
        Return True
    End Function

    Public Function keycount(ByRef result As Int64) As Boolean Implements isynckeyvalue2(Of store_t.iterator).keycount
        result = m.size()
        Return True
    End Function

    Public Function list(ByRef result As vector(Of Byte())) As Boolean _
                        Implements isynckeyvalue2(Of store_t.iterator).list
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

    Public Function read_existing(ByVal it As store_t.iterator,
                                  ByRef value() As Byte) As Boolean _
                                 Implements isynckeyvalue2(Of store_t.iterator).read_existing
        assert(it <> m.end())
        value = copy((+it).second)
        Return True
    End Function

    Public Function retire() As Boolean Implements isynckeyvalue2(Of store_t.iterator).retire
        m.clear()
        vs = 0
        Return True
    End Function

    Public Function seek(ByVal key() As Byte,
                         ByRef it As store_t.iterator,
                         ByRef result As Boolean) As Boolean Implements isynckeyvalue2(Of store_t.iterator).seek
        it = m.find(array_ref.of(key))
        result = (it <> m.end())
        Return True
    End Function

    Public Function sizeof_existing(ByVal it As store_t.iterator,
                                    ByRef result As Int64) As Boolean _
                                   Implements isynckeyvalue2(Of store_t.iterator).sizeof_existing
        assert(it <> m.end())
        result = array_size((+it).second)
        Return True
    End Function

    Public Function [stop]() As Boolean Implements isynckeyvalue2(Of store_t.iterator).stop
        Return True
    End Function

    Public Function valuesize(ByRef result As Int64) As Boolean _
                             Implements isynckeyvalue2(Of store_t.iterator).valuesize
        result = vs
        Return True
    End Function

    Public Function write_new(ByVal key() As Byte,
                              ByVal value() As Byte,
                              ByRef result As Boolean) As Boolean _
                             Implements isynckeyvalue2(Of store_t.iterator).write_new
        If enough_storage(array_size(value)) Then
            m(array_ref.of(key)) = value
            result = True
        Else
            result = False
        End If
        Return True
    End Function

    Public Shared Function create(ByVal p As var, ByRef o As memory2) As Boolean
        If p Is Nothing Then
            Return False
        End If
        Dim v As Int64 = 0
        If p.other_values().empty() OrElse
           Not Int64.TryParse(p.other_values()(0), v) Then
            o = New memory2()
        Else
            o = New memory2(v)
        End If
        Return True
    End Function

    Private Shared Sub init()
        assert(constructor.register(Of memory2)(AddressOf create))
        assert(constructor.register(Of istrkeyvt)(
                   "memory2",
                   Function(v As var, ByRef o As istrkeyvt) As Boolean
                       Dim m As memory2 = Nothing
                       If memory2.create(v, m) Then
                           o = adapt(m)
                           Return True
                       Else
                           Return False
                       End If
                   End Function))
    End Sub
End Class
