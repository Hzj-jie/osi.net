
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utt

Public Class map_compare_test
    Inherits repeat_case_wrapper

    Public Sub New()
        MyBase.New(New map_compare_case(), 100 * If(isdebugbuild(), 1, 20))
    End Sub

    Private Class map_compare_case
        Inherits [case]

        Private Shared Function create_map(ByVal v As vector(Of pair(Of String, String))) As map(Of String, String)
            assert(Not v Is Nothing)
            assert(Not v.empty())
            Dim r As map(Of String, String) = Nothing
            r = New map(Of String, String)()
            Dim b() As Boolean = Nothing
            ReDim b(CInt(v.size()) - 1)
            For i As Int32 = 0 To CInt(v.size()) - 1
                Dim j As Int32 = 0
                j = rnd_int(0, CInt(v.size()))
                While b(j)
                    j = rnd_int(0, CInt(v.size()))
                End While
                b(j) = True
                r(v(CUInt(j)).first) = v(CUInt(j)).second
            Next
            Return r
        End Function

        Private Shared Function create_pair() As pair(Of String, String)
            Return emplace_make_pair(guid_str(), guid_str())
        End Function

        Private Shared Function create_vector() As vector(Of pair(Of String, String))
            Dim r As vector(Of pair(Of String, String)) = Nothing
            r = New vector(Of pair(Of String, String))()
            Dim c As Int32 = 0
            c = rnd_int(If(isdebugbuild(), 10, 100), If(isdebugbuild(), 100, 1000))
            While c > 0
                c -= 1
                r.emplace_back(create_pair())
            End While
            Return r
        End Function

        Private Shared Function run_case(ByVal l As vector(Of pair(Of String, String)),
                                         ByVal r As vector(Of pair(Of String, String)),
                                         ByVal exp As Int32) As Boolean
            assert_compare(create_map(l), create_map(r), exp)
            assert_compare(create_map(l).direct_cast_to(Of bt(Of first_const_pair(Of String, String)))(),
                           create_map(r).direct_cast_to(Of bt(Of first_const_pair(Of String, String)))())
            Return True
        End Function

        Private Shared Function equal_case() As Boolean
            Dim v As vector(Of pair(Of String, String)) = Nothing
            v = create_vector()
            Return run_case(v, v, 0)
        End Function

        Private Shared Function unequal_case() As Boolean
            Dim v As vector(Of pair(Of String, String)) = Nothing
            v = create_vector()
            Dim v2 As vector(Of pair(Of String, String)) = Nothing
            copy(v2, v)
            v2.emplace_back(create_pair())
            Return run_case(v, v2, -1)
        End Function

        Public Overrides Function run() As Boolean
            Return equal_case() AndAlso
                   unequal_case()
        End Function
    End Class
End Class
