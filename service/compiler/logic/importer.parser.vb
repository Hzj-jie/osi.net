
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation

Partial Public NotInheritable Class logic
    Partial Public NotInheritable Class importer
        Public Structure place_holder
        End Structure

        Private Shared Function parse_typed_parameters(ByVal o As vector(Of pair(Of String, String)),
                                                       ByVal v As vector(Of String),
                                                       ByRef p As UInt32) As Boolean
            assert(Not o Is Nothing)
            assert(Not v Is Nothing)
            assert(v.size() > p)
            If Not strsame(v(p), "(") Then
                Return False
            End If
            p += uint32_1
            While p < v.size()
                If strsame(v(p), ")") Then
                    p += uint32_1
                    Return True
                End If

                If p = v.size() - uint32_1 Then
                    Return False
                End If

                o.emplace_back(pair.emplace_of(v(p), v(p + uint32_1)))
                p += uint32_2
            End While
            errors.typed_parameters_is_not_closed()
            Return False
        End Function

        Private Shared Function parse_parameters(ByVal o As vector(Of String),
                                                 ByVal v As vector(Of String),
                                                 ByRef p As UInt32) As Boolean
            assert(Not o Is Nothing)
            assert(Not v Is Nothing)
            assert(v.size() > p)
            If Not strsame(v(p), "(") Then
                Return False
            End If
            p += uint32_1
            While p < v.size()
                If strsame(v(p), ")") Then
                    p += uint32_1
                    Return True
                End If

                o.emplace_back(v(p))
                p += uint32_1
            End While
            errors.parameters_is_not_closed()
            Return False
        End Function

        Private Function parse_paragraph(ByRef o As paragraph,
                                         ByVal v As vector(Of String),
                                         ByRef p As UInt32) As Boolean
            assert(Not v Is Nothing)
            assert(v.size() > p)
            If Not strsame(v(p), "{") Then
                Return False
            End If
            o = New paragraph()
            p += uint32_1
            While p < v.size()
                If strsame(v(p), "}") Then
                    p += uint32_1
                    Return True
                End If

                Dim e As instruction_gen = Nothing
                If Not parse(v, p, e) Then
                    Return False
                End If

                o.push(e)
            End While
            errors.paragraph_is_not_closed()
            Return False
        End Function
    End Class
End Class
