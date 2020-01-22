
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Text
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.envs
Imports osi.root.formation

Partial Public NotInheritable Class syntaxer
    Public Shared ReadOnly detailed_debug_log As Boolean
    Public Shared ReadOnly debug_log As Boolean
    Public Shared ReadOnly dump_rules As Boolean

    Private ReadOnly collection As syntax_collection
    Private ReadOnly root_types As vector(Of UInt32)
    Private ReadOnly mg As matching_group

    Shared Sub New()
        detailed_debug_log = env_bool(env_keys("syntaxer", "detailed", "debug")) OrElse
                             env_bool(env_keys("syntaxer", "detailed", "debugging")) OrElse
                             env_bool(env_keys("syntaxer", "detailed", "debug", "log"))
        debug_log = env_bool(env_keys("syntaxer", "debug")) OrElse
                    env_bool(env_keys("syntaxer", "debugging")) OrElse
                    env_bool(env_keys("syntaxer", "debug", "log")) OrElse
                    detailed_debug_log
        dump_rules = env_bool(env_keys("syntaxer", "rules", "debug")) OrElse
                     env_bool(env_keys("syntaxer", "debug", "rules")) OrElse
                     env_bool(env_keys("syntaxer", "dump", "rules")) OrElse
                     detailed_debug_log
    End Sub

    Public Sub New(ByVal collection As syntax_collection, ByVal root_types As vector(Of UInt32))
        assert(Not collection Is Nothing)
        assert(collection.complete())
        assert(Not root_types.null_or_empty())
        Me.collection = collection
        Me.root_types = root_types
        Me.mg = New matching_group(collection, arrays.type_erasure(Of matching, syntax)(+collection.get(+root_types)))
    End Sub

    Private Function debug_str(ByVal v As vector(Of typed_word), ByVal p As UInt32) As String
        Dim start As UInt32 = 0
        start = CUInt(max(0, p - 3))
        Dim [end] As UInt32 = 0
        [end] = CUInt(min(start + 6, v.size()))
        Dim r As StringBuilder = Nothing
        r = New StringBuilder()
        Dim i As UInt32 = 0
        i = start
        While i < [end]
            r.Append(v(i).str()).
              Append(":[").
              Append(collection.type_name(v(i).type)).
              Append("]").
              Append(v(i).len).
              Append(character.blank)
            i += uint32_1
        End While
        Return Convert.ToString(r)
    End Function

    Public Function match(ByVal v As vector(Of typed_word)) As [optional](Of typed_node)
        assert(Not root_types.null_or_empty())
        If v.null_or_empty() Then
            Return [optional].empty(Of typed_node)()
        End If
        Dim root As typed_node = Nothing
        root = typed_node.of_root(v)
        Dim p As UInt32 = 0
        While p < v.size()
            Dim m As [optional](Of matching_group.best_match_result) = Nothing
            m = mg.best_match(v, p)
            If Not m Then
                raise_error(error_type.user, "Cannot match token ", v(p).str(), " @ ", p, " - ", debug_str(v, p))
                Return [optional].empty(Of typed_node)()
            End If
            assert(Not (+m) Is Nothing)
            Dim r As matching.result = Nothing
            r = (+m).result
            assert(Not r Is Nothing)
            If p = r.pos Then
                Return [optional].empty(Of typed_node)()
            End If
            assert(r.pos <= v.size())
            root.attach(r.nodes)
            p = r.pos
        End While
        assert(p = v.size())
        assert(Not root.leaf())
        Return [optional].of(root)
    End Function

    Public Function match(ByVal v As vector(Of typed_word), ByRef root As typed_node) As Boolean
        Dim r As [optional](Of typed_node) = Nothing
        r = match(v)
        If r Then
            root = (+r)
            Return True
        End If
        Return False
    End Function
End Class
