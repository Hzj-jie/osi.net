﻿
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.utils

Partial Public Class rlexer
    Public Class macros
        Public Shared ReadOnly [default] As macros
        Private ReadOnly vs As vector(Of pair(Of String, String))

        Shared Sub New()
            [default] = New macros()
            [default].define({({"lw", "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z"}),
                              ({"uw", "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z"}),
                              ({"w", "\lw,\uw"}),
                              ({"W", "[\w]!"}),
                              ({"d", "0,1,2,3,4,5,6,7,8,9"}),
                              ({"b", " ,\t,\f,\v,\r,\n,\uFEFF"}),
                              ({"D", "[\d]!"}),
                              ({",", "\x2C"}),
                              ({"[", "\x5B"}),
                              ({"]", "\x5D"}),
                              ({"*", "\x2A"}),
                              ({"?", "\x3F"}),
                              ({"!", "\x21"}),
                              ({"-", "\x2A"}),
                              ({"\", "\x5C"})})
        End Sub

        Public Sub New(Optional ByVal c As characters = Nothing)
            Me.vs = New vector(Of pair(Of String, String))()
        End Sub

        Public Sub define(ByVal macro As String, ByVal expanded As String)
            assert(Not String.IsNullOrEmpty(macro))
            assert(Not String.IsNullOrEmpty(expanded))
            vs.emplace_back(pair.emplace_of(characters.macro_escape + macro, expanded))
        End Sub

        Public Sub define(ByVal pairs()() As String)
            assert(Not isemptyarray(pairs))
            For i As UInt32 = 0 To array_size(pairs) - uint32_1
                assert(array_size(pairs(i)) = CUInt(2))
                define(pairs(i)(0), pairs(i)(1))
            Next
        End Sub

        Public Sub define(ByVal p As pair(Of String, String))
            assert(Not p Is Nothing)
            define(p.first, p.second)
        End Sub

        Public Sub define(ByVal pairs() As pair(Of String, String))
            assert(Not isemptyarray(pairs))
            For i As UInt32 = 0 To array_size(pairs) - uint32_1
                define(pairs(i))
            Next
        End Sub

        Public Sub define(ByVal v As vector(Of pair(Of String, String)))
            define(+v)
        End Sub

        Public Function expand(ByVal s As String) As String
            If Not String.IsNullOrEmpty(s) AndAlso Not vs.empty() Then
                Dim matched As Int32 = 0
                While True
                    matched = npos
                    Dim id As UInt32 = 0
                    For i As UInt32 = 0 To vs.size() - uint32_1
                        Dim x As Int32 = 0
                        x = strindexof(s, vs(i).first)
                        If x <> npos AndAlso (matched = npos OrElse x < matched) Then
                            matched = x
                            id = i
                        End If
                    Next
                    If matched = npos Then
                        Exit While
                    Else
                        strrplc(s, CUInt(matched), strlen(vs(id).first), vs(id).second)
                    End If
                End While
            End If
            Return s
        End Function

        Public Function export() As vector(Of pair(Of String, String))
            Dim r As vector(Of pair(Of String, String)) = Nothing
            r = New vector(Of pair(Of String, String))()
            If Not vs.empty() Then
                For i As UInt32 = 0 To vs.size() - uint32_1
                    assert(vs(i).first(0) = characters.macro_escape)
                    r.emplace_back(pair.emplace_of(strmid(vs(i).first, uint32_1), vs(i).second))
                Next
            End If
            Return r
        End Function

        Public Function defined(ByVal macro As String) As Boolean
            If String.IsNullOrEmpty(macro) Then
                Return False
            Else
                If strlen(macro) = uint32_1 OrElse Not macro.StartsWith(characters.macro_escape) Then
                    macro = strcat(characters.macro_escape, macro)
                End If
                Dim i As UInt32 = 0
                While i < vs.size()
                    If strsame(vs(i).first, macro) Then
                        Return True
                    End If
                    i += 1
                End While
                Return False
            End If
        End Function
    End Class
End Class
