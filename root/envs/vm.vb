
Option Explicit On
Option Infer Off
Option Strict On

Public Module _vm
    Public ReadOnly clr_version As Version = Environment.Version()
    Public ReadOnly clr_1 As Boolean = (clr_version.Major() = 1)
    Public ReadOnly clr_2 As Boolean = (clr_version.Major() = 2)
    Public ReadOnly clr_4 As Boolean = (clr_version.Major() = 4)

    Public ReadOnly framework_2 As Boolean = (Type.GetType("System.Net.FtpWebRequest", False) IsNot Nothing)
    'TODO: detect framework 3
    Public ReadOnly framework_3 As Boolean = framework_2
    Public ReadOnly framework_3_5 As Boolean =
        (Type.GetType("System.Threading.ReaderWriterLockSlim", False) IsNot Nothing)
    Public ReadOnly framework_4 As Boolean = (Type.GetType("System.Threading.Tasks.Parallel", False) IsNot Nothing)
    Public ReadOnly framework_4_5 As Boolean =
        (Type.GetType("System.Reflection.ReflectionContext", False) IsNot Nothing)

    Public ReadOnly mono As Boolean = connector.on_mono()
End Module
