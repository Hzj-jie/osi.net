
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.delegates
Imports osi.root.constants
Imports osi.root.utils
Imports osi.service.configuration.constants.filter_selector

Public NotInheritable Class filter_selector
    Implements ICloneable

    Private Shared ReadOnly types As map(Of String, Func(Of String, ifilter))
    Private Shared ReadOnly [default] As Func(Of String, ifilter)

    Shared Sub New()
        [default] = Function(s) New string_filter(s)
        types = New map(Of String, Func(Of String, ifilter))()

        assert_register_filter([string], [default])
        assert_register_filter(string_pattern, Function(s) New string_pattern_filter(s))
        assert_register_filter(string_serial, Function(s) New string_serial_filter(s))
        assert_register_filter(string_patterns, Function(s) New string_patterns_filter(s))
        assert_register_filter(string_compare, Function(s) New string_compare_filter(s))
        assert_register_filter(string_interval, Function(s) New string_interval_filter(s))
        assert_register_filter(string_pattern_cache, Function(s) New cached_filter(New string_pattern_filter(s)))
        assert_register_filter(string_patterns_cache, Function(s) New cached_filter(New string_patterns_filter(s)))
        assert_register_filter(string_prefix, Function(s) New string_prefix_filter(s))
        assert_register_filter(string_prefixes, Function(s) New string_prefixes_filter(s))

        assert_register_filter(string_case_sensitive, Function(s) New string_case_sensitive_filter(s))
        assert_register_filter(string_pattern_case_sensitive, Function(s) New string_case_sensitive_pattern_filter(s))
        assert_register_filter(string_serial_case_sensitive, Function(s) New string_case_sensitive_serial_filter(s))
        assert_register_filter(string_patterns_case_sensitive, Function(s) New string_case_sensitive_patterns_filter(s))
        assert_register_filter(string_compare_case_sensitive, Function(s) New string_case_sensitive_compare_filter(s))
        assert_register_filter(string_interval_case_sensitive, Function(s) New string_case_sensitive_interval_filter(s))
        assert_register_filter(string_pattern_case_sensitive_cache,
            Function(s) New cached_filter(New string_case_sensitive_pattern_filter(s)))
        assert_register_filter(string_patterns_case_sensitive_cache,
            Function(s) New cached_filter(New string_case_sensitive_patterns_filter(s)))
        assert_register_filter(string_prefix_case_sensitive, Function(s) New string_case_sensitive_prefix_filter(s))
        assert_register_filter(string_prefixes_case_sensitive, Function(s) New string_case_sensitive_prefixes_filter(s))

        assert_register_filter(int, Function(s) New int_filter(s))
        assert_register_filter(int_interval, Function(s) New int_interval_filter(s))
        assert_register_filter(int_serial, Function(s) New int_serial_filter(s))
        assert_register_filter(int_compare, Function(s) New int_compare_filter(s))

        assert_register_filter([double], Function(s) New double_filter(s))
        assert_register_filter(double_interval, Function(s) New double_interval_filter(s))
        assert_register_filter(double_serial, Function(s) New double_serial_filter(s))
        assert_register_filter(double_compare, Function(s) New double_compare_filter(s))
    End Sub

    Public Shared Function register_filter(ByVal name As String, ByVal ctor As Func(Of String, ifilter)) As Boolean
        If String.IsNullOrEmpty(name) OrElse ctor Is Nothing Then
            Return False
        Else
            types(name) = ctor
            Return True
        End If
    End Function

    Public Shared Sub assert_register_filter(ByVal name As String, ByVal ctor As Func(Of String, ifilter))
        assert(register_filter(name, ctor))
    End Sub

    Private ReadOnly m As map(Of String, Func(Of String, ifilter))

    Public Sub New()
        m = New map(Of String, Func(Of String, ifilter))()
    End Sub

    Public Sub New(ByVal m As map(Of String, Func(Of String, ifilter)))
        assert(Not m Is Nothing)
        Me.m = m
    End Sub

    Public Sub [set](ByVal name As String, ByVal [type] As String)
        Dim c As Func(Of String, ifilter) = Nothing
        Dim i As map(Of String, Func(Of String, ifilter)).iterator = Nothing
        i = types.find([type])
        If i = types.end() Then
            raise_error(error_type.warning,
                        "filter type ",
                        [type],
                        " is not supported, fall back to default as ",
                        [string],
                        " filter")
        Else
            m(name) = (+i).second
        End If
    End Sub

    Friend Function create(ByVal name As String, ByVal value As String) As ifilter
        Dim c As Func(Of String, ifilter) = Nothing
        Dim i As map(Of String, Func(Of String, ifilter)).iterator = Nothing
        i = m.find(name)
        If i = m.end() Then
            Return [default](value)
        Else
            Return (+i).second(value)
        End If
    End Function

    Public Function Clone() As Object Implements ICloneable.Clone
        Return New filter_selector(m)
    End Function
End Class
