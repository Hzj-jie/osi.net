
Public Module _vm
    Public ReadOnly clr_version As Version
    Public ReadOnly clr_1 As Boolean
    Public ReadOnly clr_2 As Boolean
    Public ReadOnly clr_4 As Boolean

    Public ReadOnly framework_2 As Boolean
    Public ReadOnly framework_3 As Boolean
    Public ReadOnly framework_3_5 As Boolean
    Public ReadOnly framework_4 As Boolean
    Public ReadOnly framework_4_5 As Boolean

    Public ReadOnly mono As Boolean

    Sub New()
        clr_version = Environment.Version()
        clr_1 = (clr_version.Major() = 1)
        clr_2 = (clr_version.Major() = 2)
        clr_4 = (clr_version.Major() = 4)

        framework_2 = (Not Type.GetType("System.Net.FtpWebRequest", False) Is Nothing)
        'TODO: detect framework 3
        framework_3 = framework_2
        framework_3_5 = (Not Type.GetType("System.Threading.ReaderWriterLockSlim", False) Is Nothing)
        framework_4 = (Not Type.GetType("System.Threading.Tasks.Parallel", False) Is Nothing)
        framework_4_5 = (Not Type.GetType("System.Reflection.ReflectionContext", False) Is Nothing)

        mono = connector.on_mono()
    End Sub
End Module
