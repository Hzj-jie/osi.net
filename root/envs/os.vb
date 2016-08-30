
Public NotInheritable Class os
    Public Enum family_t
        windows
        macosx
        unix
        xbox
        unknown
    End Enum

    Public Enum windows_major_t
        _9x     ' 95 - Me
        _4     ' NT 4
        _5      ' 2000 - 2003
        _6      ' vista - 8.1
        _10     ' 10
        _CE     ' All Windows CE
        _32_subset

        _unknown
    End Enum

    Public Enum windows_ver_t
        _9x_me
        _2000
        _xp
        _2003_or_xp_64
        _2008_or_vista
        _7_or_2008_r2
        _8_or_2012
        _8_1_or_2012_r2
        _10_or_2016
        _CE
        _32_subset  ' WIN32S, a subset of 32 bit Windows API

        _unknown
    End Enum

    Public Shared ReadOnly full_name As String
    Public Shared ReadOnly platform As String
    Public Shared ReadOnly version As String
    Public Shared ReadOnly family As family_t
    Public Shared ReadOnly windows_major As windows_major_t
    Public Shared ReadOnly windows_ver As windows_ver_t

    Private Shared Function detect_family() As family_t
        Dim p As PlatformID = Nothing
        p = Environment.OSVersion().Platform()
        Select Case p
            Case PlatformID.MacOSX
                Return family_t.macosx
            Case PlatformID.Unix
                Return family_t.unix
            Case PlatformID.Win32NT,
                 PlatformID.Win32S,
                 PlatformID.Win32Windows,
                 PlatformID.WinCE
                Return family_t.windows
            Case PlatformID.Xbox
                Return family_t.xbox
        End Select
        Return family_t.unknown
    End Function

    Private Shared Function detect_windows_major() As windows_major_t
        Dim p As PlatformID = Nothing
        p = Environment.OSVersion().Platform()
        Select Case p
            Case PlatformID.WinCE
                Return windows_major_t._CE
            Case PlatformID.Win32S
                Return windows_major_t._32_subset
            Case PlatformID.Win32Windows
                Return windows_major_t._9x
        End Select

        Dim v As Version = Nothing
        v = Environment.OSVersion().Version()
        Select Case v.Major()
            Case 4
                Return windows_major_t._4
            Case 5
                Return windows_major_t._5
            Case 6
                Return windows_major_t._6
            Case 10
                Return windows_major_t._10
        End Select

        Return windows_major_t._unknown
    End Function

    Private Shared Function detect_windows_ver() As windows_ver_t
        If family = family_t.windows Then
            Dim p As PlatformID = Nothing
            p = Environment.OSVersion().Platform()
            Select Case p
                Case PlatformID.WinCE
                    Return windows_ver_t._CE
                Case PlatformID.Win32S
                    Return windows_ver_t._32_subset
                Case PlatformID.Win32Windows
                    Return windows_ver_t._9x_me
            End Select
            Dim v As Version = Nothing
            v = Environment.OSVersion().Version()
            Select Case v.Major()
                Case 5
                    Select Case v.Minor()
                        Case 0
                            Return windows_ver_t._2000
                        Case 1
                            Return windows_ver_t._xp
                        Case 2
                            Return windows_ver_t._2003_or_xp_64
                    End Select
                Case 6
                    Select Case v.Minor()
                        Case 0
                            Return windows_ver_t._2008_or_vista
                        Case 1
                            Return windows_ver_t._7_or_2008_r2
                        Case 2
                            Return windows_ver_t._8_or_2012
                        Case 3
                            Return windows_ver_t._8_1_or_2012_r2
                    End Select
                Case 10
                    Select Case v.Minor()
                        Case 0
                            Return windows_ver_t._10_or_2016
                    End Select
            End Select
        End If
        Return windows_ver_t._unknown
    End Function

    Shared Sub New()
        full_name = computer.Info().OSFullName()
        platform = computer.Info().OSPlatform()
        version = computer.Info().OSVersion()

        family = detect_family()
        windows_major = detect_windows_major()
        windows_ver = detect_windows_ver()
    End Sub

    Private Sub New()
    End Sub
End Class
