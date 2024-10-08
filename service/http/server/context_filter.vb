
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Reflection
Imports osi.root.connector

<AttributeUsage(AttributeTargets.All, AllowMultiple:=True, Inherited:=True)>
Public MustInherit Class context_filter
    Inherits Attribute

    Protected Sub New()
    End Sub

    Public MustOverride Function [select](ByVal context As server.context) As Boolean

    Public Shared Function [New](ByVal mi As MemberInfo) As context_filter
        If mi Is Nothing Then
            Return Nothing
        End If

        Dim attrs() As context_filter = Nothing
        custom_attributes(mi, attrs, True)
        Return New filter_set(attrs)
    End Function

    Private Class filter_set
        Inherits context_filter

        Private ReadOnly v() As context_filter

        Public Sub New(ByVal v() As context_filter)
            MyBase.New()
            Me.v = v
        End Sub

        Public Overrides Function [select](ByVal context As server.context) As Boolean
            If context Is Nothing Then
                Return False
            End If
            If isemptyarray(v) Then
                Return True
            End If
            For i As Int32 = 0 To array_size_i(v) - 1
                assert(Not v(i) Is Nothing)
                If Not v(i).select(context) Then
                    Return False
                End If
            Next
            Return True
        End Function
    End Class
End Class
