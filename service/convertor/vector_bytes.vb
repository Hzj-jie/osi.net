
#If RETIRED
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.delegates
Imports osi.root.formation

Public Module _vector_bytes
    <Extension()> Public Function from_vector_bytes(Of T)(ByVal b As vector(Of Byte()),
                                                          ByRef o As vector(Of T),
                                                          Optional ByVal bytes_T As bytes_serializer(Of T) = Nothing) _
                                                         As Boolean
        If b Is Nothing Then
            Return False
        Else
            o.renew()
            If Not b.empty() Then
                For i As UInt32 = 0 To b.size() - uint32_1
                    Dim v As T = Nothing
                    If (+bytes_T).from_bytes(b(i), v) Then
                        o.emplace_back(v)
                    Else
                        Return False
                    End If
                Next
            End If
            Return True
        End If
    End Function

    <Extension()> Public Function to_vector_bytes(Of T)(ByVal b As vector(Of T),
                                                        ByRef o As vector(Of Byte()),
                                                        Optional ByVal T_bytes As bytes_serializer(Of T) = Nothing) _
                                                       As Boolean
        If b Is Nothing Then
            Return False
        Else
            o.renew()
            If Not b.empty() Then
                For i As UInt32 = 0 To b.size() - uint32_1
                    Dim v() As Byte = Nothing
                    If (+T_bytes).to_bytes(b(i), v) Then
                        o.emplace_back(v)
                    Else
                        Return False
                    End If
                Next
            End If
            Return True
        End If
    End Function
End Module
#End If
