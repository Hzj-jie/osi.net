
Imports System.IO
Imports osi.root.connector

'should i move it to utils?
Public Module localfile
    Public Function assert_fullpath(ByRef f As String) As String
        Try
            f = Path.GetFullPath(f)
        Catch ex As Exception
            assert(False, ex.Message())
        End Try
        Return f
    End Function
End Module