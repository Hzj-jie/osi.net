
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

Partial Public NotInheritable Class tar
    Public NotInheritable Class writer
        Private ReadOnly fs As fs
        Private ReadOnly max_size As UInt32
        Private readonly output_base As string
        Private ReadOnly v As vector(Of String)

        Public Sub New(ByVal max_size As UInt32, ByVal output_base As String, ByVal files As vector(Of String))
            Me.New(default_fs.instance, max_size, output_base, files)
        End Sub

        Private Sub New(ByVal fs As fs,
                        ByVal max_size As UInt32,
                        ByVal output_base As String,
                        ByVal files As vector(Of String))
            assert(Not fs Is Nothing)
            assert(max_size > 0)
            assert(Not String.IsNullOrWhiteSpace(output_base))
            assert(Not files Is Nothing)
            Me.fs = fs
            Me.max_size = max_size
            Me.output_base = output_base
            Me.v = files
        End Sub

        Public Shared Function of_testing(ByVal fs As testing_fs, ByVal max_size As UInt32) As writer
            Return New writer(fs, max_size, "testing_output_", fs.list_files())
        End Function

        Public Shared Function zip(ByVal max_size As UInt32,
                                   ByVal output_base As String,
                                   ByVal files As vector(Of String)) As writer
            Return New writer(zip_fs.instance, max_size, output_base, files)
        End Function

        Private Function output_file(ByVal write_index As UInt32) As String
            Return strcat(output_base, write_index)
        End Function

        Private Function dump(ByVal write_index As UInt32) As Boolean
            Dim m As MemoryStream = Nothing
            m = New MemoryStream()
            Dim it As vector(Of String).iterator = Nothing
            it = v.begin()
            While it <> v.end()
                Dim c As MemoryStream = Nothing
                c = New MemoryStream()
                If Not fs.read(+it, c) Then
                    Return False
                End If
                c.Position() = 0
                If Not bytes_serializer.append_to(+it, m) OrElse
                   Not bytes_serializer.append_to(c, m) Then
                    Return False
                End If
                If m.Length() >= max_size Then
                    If Not fs.write(output_file(write_index), m) Then
                        Return False
                    End If
                    write_index += uint32_1
                    m.clear()
                End If
                it += 1
            End While
            If Not m.empty() AndAlso Not fs.write(output_file(write_index), m) Then
                Return False
            End If
            Return True
        End Function

        Public Function dump() As Boolean
            Return dump(0)
        End Function
    End Class
End Class
