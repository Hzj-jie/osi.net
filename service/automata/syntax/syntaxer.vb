
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Text
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.envs
Imports osi.root.formation

Partial Public NotInheritable Class syntaxer
    Public Shared ReadOnly detailed_debug_log As Boolean =
                               env_bool(env_keys("syntaxer", "detailed", "debug")) OrElse
                               env_bool(env_keys("syntaxer", "detailed", "debugging")) OrElse
                               env_bool(env_keys("syntaxer", "detailed", "debug", "log"))
    Public Shared ReadOnly debug_log As Boolean =
                               env_bool(env_keys("syntaxer", "debug")) OrElse
                               env_bool(env_keys("syntaxer", "debugging")) OrElse
                               env_bool(env_keys("syntaxer", "debug", "log")) OrElse
                               detailed_debug_log
    Public Shared ReadOnly dump_rules As Boolean =
                               env_bool(env_keys("syntaxer", "rules", "debug")) OrElse
                               env_bool(env_keys("syntaxer", "debug", "rules")) OrElse
                               env_bool(env_keys("syntaxer", "dump", "rules")) OrElse
                               detailed_debug_log

    Private ReadOnly collection As syntax_collection
    Private ReadOnly root_types As vector(Of UInt32)
    Private ReadOnly mg As matching_group

    Public Sub New(ByVal collection As syntax_collection, ByVal root_types As vector(Of UInt32))
        assert(Not collection Is Nothing)
        assert(collection.complete())
        assert(Not root_types.null_or_empty())
        Me.collection = collection
        Me.root_types = root_types
        Me.mg = New matching_group(collection, arrays.type_erasure(Of matching, syntax)(+collection.get(+root_types)))
    End Sub

    Public Shared Function debug_str(ByVal input As String, ByVal type_name As String) As String
        Return strcat(input, "(", strlen(input), ")", ":[", type_name, "]")
    End Function

    Private Function debug_str(ByVal v As vector(Of typed_word), ByVal p As UInt32) As String
        Dim start As UInt32 = CUInt(max(0, p - 6))
        Dim [end] As UInt32 = CUInt(min(start + 13, v.size()))
        Dim r As New StringBuilder()
        Dim i As UInt32 = start
        While i < [end]
            If i = p Then
                r.Append(">>> ")
            End If
            r.Append(debug_str(v(i).str(), collection.type_name(v(i).type))).
              Append(character.blank)
            If i = p Then
                r.Append(" <<<")
            End If
            i += uint32_1
        End While
        Return Convert.ToString(r)
    End Function

    Public Function match(ByVal v As vector(Of typed_word)) As [optional](Of typed_node)
        assert(Not root_types.null_or_empty())
        If v.null_or_empty() Then
            Return [optional].empty(Of typed_node)()
        End If
        Dim root As typed_node = typed_node.of_root(v)
        Dim p As UInt32 = 0
        While p < v.size()
            Dim m As one_of(Of matching_group.best_match_result, matching.failure) = mg.best_match(v, p)
            If m.is_second() Then
                Dim l As Func(Of UInt32, String()) = Function(ByVal pos As UInt32) As String()
                                                         Return {
                                                             If(pos < v.size(), v(pos).str(), "{END-OF-INPUT}"),
                                                             " @ ",
                                                             Convert.ToString(pos),
                                                             " - ",
                                                             debug_str(v, pos)
                                                         }
                                                     End Function
                raise_error(error_type.user,
                            "[syntaxer] Cannot match token ",
                            l(p),
                            ". Longest match ",
                            l(m.second().pos))
                Return [optional].empty(Of typed_node)()
            End If
            assert(Not m.first() Is Nothing)
            Dim r As matching.result = m.first().result
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
        Dim r As [optional](Of typed_node) = match(v)
        If r Then
            root = (+r)
            Return True
        End If
        Return False
    End Function
End Class
