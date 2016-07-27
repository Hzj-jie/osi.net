
Imports osi.root.constants
Imports osi.root.template
Imports osi.root.connector
Imports osi.root.formation

Public Class pather
    Inherits pather(Of default_path_separators, 
                       default_this_level_paths, 
                       default_parent_level_paths)

    Public Shared Shadows ReadOnly [default] As pather

    Shared Sub New()
        [default] = New pather()
    End Sub

    Private Sub New()
    End Sub

    Public Class default_path_separators
        Inherits _strings

        Private Shared ReadOnly r() As String

        Shared Sub New()
            'do not use DirectorySeparatorChar to avoid difference between platforms
            'and windows can support c:/windows/
            r = {character.left_slash, character.right_slash}
        End Sub

        Protected Overrides Function at() As String()
            Return r
        End Function
    End Class

    Public Class default_this_level_paths
        Inherits _strings

        Private Shared ReadOnly r() As String

        Shared Sub New()
            r = {filesystem.this_level_path_mark}
        End Sub

        Protected Overrides Function at() As String()
            Return r
        End Function
    End Class

    Public Class default_parent_level_paths
        Inherits _strings

        Private Shared ReadOnly r() As String

        Shared Sub New()
            r = {filesystem.parent_level_path_mark}
        End Sub

        Protected Overrides Function at() As String()
            Return r
        End Function
    End Class
End Class

Public Class pather(Of _PATH_SEPARATORS As _strings,
                       _THIS_LEVEL_PATHS As _strings,
                       _PARENT_LEVEL_PATHS As _strings)
    Private Shared ReadOnly path_separators() As String
    Private Shared ReadOnly this_level_paths() As String
    Private Shared ReadOnly parent_level_paths() As String

    Public Shared ReadOnly [default] As pather(Of _PATH_SEPARATORS, _THIS_LEVEL_PATHS, _PARENT_LEVEL_PATHS)

    Shared Sub New()
        path_separators = +(alloc(Of _PATH_SEPARATORS)())
        assert(Not isemptyarray(path_separators))
        this_level_paths = +(alloc(Of _THIS_LEVEL_PATHS)())
        assert(Not isemptyarray(this_level_paths))
        parent_level_paths = +(alloc(Of _PARENT_LEVEL_PATHS)())
        assert(Not isemptyarray(parent_level_paths))
        [default] = New pather(Of _PATH_SEPARATORS, _THIS_LEVEL_PATHS, _PARENT_LEVEL_PATHS)()
    End Sub

    Protected Sub New()
    End Sub

    Private Shared Function is_this_level_path(ByVal s As String) As Boolean
        Return String.IsNullOrEmpty(s) OrElse
               this_level_paths.has(s)
    End Function

    Private Shared Function split_path(ByVal path As String) As String()
        assert(Not String.IsNullOrEmpty(path))
        Return path.Split(path_separators, StringSplitOptions.None)
    End Function

    Public Function combine(ByVal parts() As String, ByRef path As String) As Boolean
        If isemptyarray(parts) Then
            Return False
        Else
            If array_size(parts) = 1 AndAlso
               is_this_level_path(parts(0)) Then
                If String.IsNullOrEmpty(parts(0)) Then
                    path = path_separators(0)
                Else
                    path = parts(0)
                End If
            Else
                path = parts.strjoin(path_separators(0))
            End If
            Return True
        End If
    End Function

    Public Function combine(ByVal ParamArray parts() As String) As String
        Dim o As String = Nothing
        assert(combine(parts, o))
        Return o
    End Function

    Public Function combine(ByVal parts As vector(Of String), ByRef path As String) As Boolean
        Return combine(+parts, path)
    End Function

    Public Function combine(ByVal parts As vector(Of String)) As String
        Dim o As String = Nothing
        assert(combine(parts, o))
        Return o
    End Function

    Public Function normalize(ByVal path As String, ByRef normalized_path As String) As Boolean
        Dim v As vector(Of String) = Nothing
        Return split(path, v) AndAlso
               assert(combine(v, normalized_path))
    End Function

    Public Function normalize(ByVal path As String) As String
        Dim o As String = Nothing
        assert(normalize(path, o))
        Return o
    End Function

    Public Function parent_path(ByVal path As String, ByRef o As String) As Boolean
        If String.IsNullOrEmpty(path) Then
            Return False
        Else
            Dim ss() As String = Nothing
            ss = split_path(path)
            Dim v As vector(Of String) = Nothing
            v = New vector(Of String)()
            assert(v.emplace_back(ss))
            v.emplace_back(parent_level_paths(0))
            Return combine(normalize(v), o)
        End If
    End Function

    Public Function parent_path(ByVal path As String) As String
        Dim o As String = Nothing
        assert(parent_path(path, o))
        Return o
    End Function

    Public Function last_level_name(ByVal path As String, ByRef o As String) As Boolean
        If String.IsNullOrEmpty(path) Then
            Return False
        Else
            Dim v As vector(Of String) = Nothing
            Return normalize(split_path(path), v) AndAlso
                   assert(Not v.empty()) AndAlso
                   Not is_this_level_path(v.back()) AndAlso
                   eva(o, v.back())
        End If
    End Function

    Public Function last_level_name(ByVal path As String) As String
        Dim o As String = Nothing
        assert(last_level_name(path, o))
        Return o
    End Function

    Public Function normalize(ByVal v As vector(Of String), ByRef o As vector(Of String)) As Boolean
        Return normalize(+v, o)
    End Function

    Public Function normalize(ByVal v As vector(Of String)) As vector(Of String)
        Dim r As vector(Of String) = Nothing
        assert(normalize(v, r))
        Return r
    End Function

    Public Function normalize(ByVal v() As String, ByRef o As vector(Of String)) As Boolean
        If isemptyarray(v) Then
            Return False
        Else
            o.renew()
            o.emplace_back(v(0))
            If array_size(v) > 1 Then
                For i As UInt32 = 1 To array_size(v) - uint32_1
                    If Not is_this_level_path(v(i)) Then
                        If parent_level_paths.has(v(i)) Then
                            If Not o.empty() Then
                                o.pop_back()
                            Else
                                o.emplace_back(v(i))
                            End If
                        Else
                            o.emplace_back(v(i))
                        End If
                    End If
                Next
            End If
            Return True
        End If
    End Function

    Public Function normalize(ByVal v() As String) As vector(Of String)
        Dim r As vector(Of String) = Nothing
        assert(normalize(v, r))
        Return r
    End Function

    Public Function split(ByVal path As String, ByRef o As vector(Of String)) As Boolean
        Return Not String.IsNullOrEmpty(path) AndAlso
               assert(normalize(split_path(path), o))
    End Function

    Public Function split(ByVal path As String) As vector(Of String)
        Dim r As vector(Of String) = Nothing
        assert(split(path, r))
        Return r
    End Function

    Default Public ReadOnly Property n(ByVal path As String, ByVal r As pointer(Of String)) As Boolean
        Get
            Dim s As String = Nothing
            Return normalize(path, s) AndAlso
                   eva(r, s)
        End Get
    End Property

    Default Public ReadOnly Property n(ByVal path As String) As String
        Get
            Return normalize(path)
        End Get
    End Property

    Default Public ReadOnly Property n(ByVal path As String,
                                       ByVal r As pointer(Of vector(Of String))) As Boolean
        Get
            Dim v As vector(Of String) = Nothing
            Return split(path, v) AndAlso
                   eva(r, v)
        End Get
    End Property

    Default Public ReadOnly Property n(ByVal parts() As String,
                                       ByVal r As pointer(Of String)) As Boolean
        Get
            Dim o As String = Nothing
            Return combine(parts, o) AndAlso
                   eva(r, o)
        End Get
    End Property

    Default Public ReadOnly Property n(ByVal first As String,
                                       ByVal ParamArray parts() As String) As String
        Get
            Dim p() As String = Nothing
            ReDim p(array_size(parts))
            p(0) = first
            memcpy(p, 1, parts)
            Return combine(p)
        End Get
    End Property

    Default Public ReadOnly Property n(ByVal parts As vector(Of String),
                                       ByVal r As pointer(Of String)) As Boolean
        Get
            Dim o As String = Nothing
            Return combine(parts, o) AndAlso
                   eva(r, o)
        End Get
    End Property

    Default Public ReadOnly Property n(ByVal parts As vector(Of String)) As String
        Get
            Return combine(parts)
        End Get
    End Property
End Class
