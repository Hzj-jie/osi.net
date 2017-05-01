
Imports Microsoft.VisualBasic
Imports System.Text
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.convertor

Partial Public Class var
    Public Shared ReadOnly [default] As var

    Shared Sub New()
        [default] = New var()
        [default].parse(Command())
    End Sub

    Private ReadOnly raw As map(Of String, vector(Of String))
    Private ReadOnly binded As map(Of String, vector(Of String))
    Private ReadOnly others As vector(Of String)
    Private ReadOnly c As config

    Private Sub New(ByVal c As config,
                    ByVal raw As map(Of String, vector(Of String)),
                    ByVal binded As map(Of String, vector(Of String)),
                    ByVal others As vector(Of String))
        If c Is Nothing Then
            Me.c = config.default
        Else
            Me.c = c
        End If
        If raw Is Nothing Then
            Me.raw = New map(Of String, vector(Of String))()
        Else
            Me.raw = raw
        End If
        If binded Is Nothing Then
            Me.binded = New map(Of String, vector(Of String))()
        Else
            Me.binded = binded
        End If
        If others Is Nothing Then
            Me.others = New vector(Of String)()
        Else
            Me.others = others
        End If
    End Sub

    Public Sub New(ByVal parameters() As String, Optional ByVal c As config = Nothing)
        Me.New(c)
        Me.parse(parameters)
    End Sub

    Public Sub New(ByVal parameters As vector(Of pair(Of String, String)), Optional ByVal c As config = Nothing)
        Me.New(c)
        Me.parse(parameters)
    End Sub

    Public Sub New(ByVal parameter As String, Optional ByVal c As config = Nothing)
        Me.New(c)
        Me.parse(parameter)
    End Sub

    Public Sub New(Optional ByVal c As config = Nothing)
        Me.New(c, Nothing, Nothing, Nothing)
    End Sub

    Private Function find(ByVal m As map(Of String, vector(Of String)),
                          ByVal n As String,
                          ByRef o As vector(Of String)) As Boolean
        assert(Not m Is Nothing)
        assert(Not String.IsNullOrEmpty(n))
        Dim it As map(Of String, vector(Of String)).iterator = Nothing
        it = m.find(n)
        If it = m.end() Then
            Return False
        Else
            o = (+it).second
            Return True
        End If
    End Function

    Public Function value(ByVal n As String, ByRef o As vector(Of String)) As Boolean
        If String.IsNullOrEmpty(n) Then
            Return False
        Else
            If Not c.case_sensitive Then
                strtolower(n)
            End If
            Return find(raw, n, o) OrElse
                   find(binded, n, o)
        End If
    End Function

    Public Function value(ByVal i As String,
                          ByRef o As String,
                          Optional ByVal select_first As Boolean = False,
                          Optional ByVal separator As String = character.blank) As Boolean
        Dim v As vector(Of String) = Nothing
        If value(i, v) Then
            If Not v Is Nothing Then
                If select_first Then
                    If v.empty() Then
                        o = Nothing
                    Else
                        o = v(0)
                    End If
                Else
                    o = v.ToString(separator)
                End If
            End If
            Return True
        Else
            Return False
        End If
    End Function

    Public Function value(ByVal i As String, ByRef o As SByte) As Boolean
        Dim s As String = Nothing
        Return value(i, s) AndAlso SByte.TryParse(s, o)
    End Function

    Public Function value(ByVal i As String, ByRef o As Byte) As Boolean
        Dim s As String = Nothing
        Return value(i, s) AndAlso Byte.TryParse(s, o)
    End Function

    Public Function value(ByVal i As String, ByRef o As Int16) As Boolean
        Dim s As String = Nothing
        Return value(i, s) AndAlso Int16.TryParse(s, o)
    End Function

    Public Function value(ByVal i As String, ByRef o As UInt16) As Boolean
        Dim s As String = Nothing
        Return value(i, s) AndAlso UInt16.TryParse(s, o)
    End Function

    Public Function value(ByVal i As String, ByRef o As Int32) As Boolean
        Dim s As String = Nothing
        Return value(i, s) AndAlso Int32.TryParse(s, o)
    End Function

    Public Function value(ByVal i As String, ByRef o As UInt32) As Boolean
        Dim s As String = Nothing
        Return value(i, s) AndAlso UInt32.TryParse(s, o)
    End Function

    Public Function value(ByVal i As String, ByRef o As Int64) As Boolean
        Dim s As String = Nothing
        Return value(i, s) AndAlso Int64.TryParse(s, o)
    End Function

    Public Function value(ByVal i As String, ByRef o As UInt64) As Boolean
        Dim s As String = Nothing
        Return value(i, s) AndAlso UInt64.TryParse(s, o)
    End Function

    Public Function value(ByVal i As String, ByRef o As Single) As Boolean
        Dim s As String = Nothing
        Return value(i, s) AndAlso Single.TryParse(s, o)
    End Function

    Public Function value(ByVal i As String, ByRef o As Double) As Boolean
        Dim s As String = Nothing
        Return value(i, s) AndAlso Double.TryParse(s, o)
    End Function

    Public Function value(ByVal i As String, ByRef o As Decimal) As Boolean
        Dim s As String = Nothing
        Return value(i, s) AndAlso Decimal.TryParse(s, o)
    End Function

    Public Function value(ByVal i As String) As String
        Dim o As String = Nothing
        If value(i, o) Then
            Return If(o Is Nothing, Nothing, o)
        Else
            Return Nothing
        End If
    End Function

    Default Public ReadOnly Property v(ByVal i As String) As String
        Get
            Return value(i)
        End Get
    End Property

    Public Function switch(ByVal i As String, ByRef o As Boolean) As Boolean
        Dim s As String = Nothing
        If value(i, s, True) Then
            If s Is Nothing Then
                o = True
            Else
                o = s.to_bool()
            End If
            Return True
        Else
            Return False
        End If
    End Function

    Public Function switch(ByVal i As String) As Boolean
        Dim o As Boolean = False
        If switch(i, o) Then
            Return o
        Else
            Return False
        End If
    End Function

    Public Function reverse_switch(ByVal i As String) As Boolean
        Dim o As Boolean = False
        If switch(i, o) Then
            Return Not o
        Else
            Return False
        End If
    End Function

    Public Function other_values() As vector(Of String)
        Return others
    End Function

    Public NotOverridable Overrides Function ToString() As String
        Dim s As StringBuilder = Nothing
        s = New StringBuilder()
        Dim it As map(Of String, vector(Of String)).iterator = Nothing
        it = raw.begin()
        While it <> raw.end()
            If (+it).second Is Nothing Then
                s.Append(c.create_full_switcher((+it).first))
                s.Append(c.argument_separator)
            Else
                For i As Int32 = 0 To (+it).second.size() - 1
                    s.Append(c.create_arg((+it).first, (+it).second(i)))
                    s.Append(c.argument_separator)
                Next
            End If
            it += 1
        End While

        For i As Int32 = 0 To other_values().size() - 1
            s.Append(other_values()(i))
            s.Append(c.argument_separator)
        Next
        s.Remove(strlen(s) - 1, 1)
        Return Convert.ToString(s)
    End Function
End Class
