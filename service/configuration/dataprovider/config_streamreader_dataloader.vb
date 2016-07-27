
Imports System.IO
Imports osi.service.dataprovider
Imports osi.root.delegates
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.utils

Friend Class config_streamreader_dataloader
    Inherits streamreader_dataloader(Of config)

    Private ReadOnly c As characters
    Private ReadOnly base_fs As filter_selector
    Private ReadOnly static_variants As vector(Of pair(Of String, String))

    Public Sub New(ByVal c As characters,
                   ByVal fs As filter_selector,
                   ByVal static_variants As vector(Of pair(Of String, String)))
        assert(Not c Is Nothing)
        assert(Not fs Is Nothing)
        Me.c = c
        Me.base_fs = fs
        Me.static_variants = static_variants
    End Sub

    Private Class session
        Public ReadOnly fs As filter_selector
        Public ReadOnly rc As raw_config
        Public rs As raw_section

        Public Sub New(ByVal fs As filter_selector)
            assert(Not fs Is Nothing)
            Me.fs = fs
            rc = New raw_config()
        End Sub
    End Class

    Private Function split_filter_value(ByVal f As String,
                                        ByVal r As vector(Of pair(Of String, String))) As Boolean
        assert(Not String.IsNullOrEmpty(f))
        assert(Not r Is Nothing)
        Dim v As String = Nothing
        If strsep(f, f, v, c.filter_key_value_separator, False) AndAlso
           Not String.IsNullOrEmpty(f) AndAlso
           Not String.IsNullOrEmpty(v) Then
            r.push_back(make_pair(f, v))
            Return True
        Else
            Return False
        End If
    End Function

    Private Function split_filters(ByVal f As String,
                                   ByRef r As vector(Of pair(Of String, String))) As Boolean
        If Not String.IsNullOrEmpty(f) Then
            Dim ss() As String = Nothing
            ss = f.Split(c.filter_separator)
            assert(array_size(ss) > 0)
            r = New vector(Of pair(Of String, String))()
            For i As Int32 = 0 To array_size(ss) - 1
                If Not String.IsNullOrEmpty(ss(i)) AndAlso
                   Not split_filter_value(ss(i), r) Then
                    Return False
                End If
            Next
        End If
        Return True
    End Function

    Private Function split_key(ByRef k As String,
                               ByRef sf As vector(Of pair(Of String, String)),
                               ByRef df As vector(Of pair(Of String, String))) As Boolean
        Dim sfs As String = Nothing
        Dim dfs As String = Nothing
        strsep(k, sfs, k, c.static_filter_mark, False)
        strsep(k, k, dfs, c.dynamic_filter_mark, False)
        If String.IsNullOrEmpty(k) Then
            Return False
        End If

        Return split_filters(sfs, sf) AndAlso
               split_filters(dfs, df)
    End Function

    Private Sub load_line(ByVal l As String, ByVal s As session)
        If Not String.IsNullOrEmpty(l) Then
            If Not strstartwith(l, c.remark, False) Then
                Dim v As String = Nothing
                Dim is_section_line As Boolean = False
                is_section_line = strstartwith(l, c.section_left, False) AndAlso
                                   strendwith(l, c.section_right, False)
                If is_section_line Then
                    l = strmid(l,
                               strlen(c.section_left),
                               strlen(l) - strlen(c.section_left) - strlen(c.section_right))
                    If String.IsNullOrEmpty(l) Then
                        raise_error(error_type.warning,
                                    "a line with only section begin and section end, ignore")
                        Return

                        l = l.Trim()
                        If String.IsNullOrEmpty(l) Then
                            raise_error(error_type.warning,
                                        "a line with only section begin, section end and blanks, ignore")
                            Return
                        End If
                    End If
                Else
                    If strsep(l, l, v, c.key_value_separator, False) Then
                        If String.IsNullOrEmpty(l) Then
                            If String.IsNullOrEmpty(v) Then
                                raise_error(error_type.warning,
                                            "a line with only key value separator, ignore")
                            Else
                                l = v
                                v = String.Empty
                            End If
                        End If
                    Else
                        assert(Not String.IsNullOrEmpty(l))
                        v = String.Empty
                    End If
                End If

                assert(Not String.IsNullOrEmpty(l))
                Dim sf As vector(Of pair(Of String, String)) = Nothing
                Dim df As vector(Of pair(Of String, String)) = Nothing
                If Not split_key(l, sf, df) Then
                    raise_error(error_type.warning,
                                "cannot split static filters and dynamic filters from key ",
                                l,
                                ", ignore")
                    Return
                End If

                If static_filter.match(s.fs, sf, static_variants) Then
                    If is_section_line Then
                        inject_section(s)
                        s.rs = New raw_section(l, df)
                    ElseIf s.rs Is Nothing Then
                        If Not df Is Nothing Then
                            raise_error(error_type.warning,
                                        "dynamic filter does not work with filter selector configuration, ignore")
                            Return
                        End If
                        s.fs.set(l, v)
                    Else
                        s.rs.insert(l, v, df)
                    End If
                End If
            End If
        End If
    End Sub

    Private Shared Sub inject_section(ByVal s As session)
        assert(Not s Is Nothing)
        If Not s.rs Is Nothing AndAlso Not s.rs.empty() Then
            s.rc.insert(s.rs.name(), New section(s.fs, s.rs), s.rs.raw_filters())
        End If
    End Sub

    Protected Overrides Function load(ByVal s As StreamReader, ByRef result As config) As Boolean
        Dim sec As session = Nothing
        sec = New session(copy(base_fs))
        Dim l As String = Nothing
        l = s.ReadLine()
        While Not l Is Nothing
            l = l.Trim()
            load_line(l, sec)
            l = s.ReadLine()
        End While
        inject_section(sec)
        result = New config(sec.fs, sec.rc)
        Return True
    End Function
End Class
