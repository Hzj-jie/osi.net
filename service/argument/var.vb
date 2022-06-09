
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

Partial Public NotInheritable Class var
    Public Shared ReadOnly [default] As var = New var(streams.of(Environment.GetCommandLineArgs()).skip(1).to_array())

    Private ReadOnly raw As map(Of String, vector(Of String))
    Private ReadOnly binded As map(Of String, vector(Of String))
    Private ReadOnly others As vector(Of String)
    Private ReadOnly c As config

    <copy_constructor()>
    Protected Sub New(ByVal c As config,
                      ByVal raw As map(Of String, vector(Of String)),
                      ByVal binded As map(Of String, vector(Of String)),
                      ByVal others As vector(Of String))
        Me.c = c
        Me.raw = raw
        Me.binded = binded
        Me.others = others
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
        Me.New(If(c Is Nothing, config.default, c),
               New map(Of String, vector(Of String))(),
               New map(Of String, vector(Of String))(),
               New vector(Of String)())
    End Sub

    Private Function find(ByVal m As map(Of String, vector(Of String)),
                          ByVal n As String,
                          ByRef o As vector(Of String)) As Boolean
        assert(Not m Is Nothing)
        assert(Not n.null_or_empty())
        Dim it As map(Of String, vector(Of String)).iterator = m.find(n)
        If it = m.end() Then
            Return False
        End If
        o = (+it).second
        Return True
    End Function

    Public Function value(ByVal n As String, ByRef o As vector(Of String)) As Boolean
        If n.null_or_empty() Then
            Return False
        End If
        If Not c.case_sensitive Then
            strtolower(n)
        End If
        Return find(raw, n, o) OrElse
               find(binded, n, o)
    End Function

    Public Function defined(ByVal n As String) As Boolean
        Return value(n, direct_cast(Of vector(Of String))(Nothing))
    End Function

    Public Function value(ByVal i As String,
                          ByRef o As String,
                          Optional ByVal select_first As Boolean = False,
                          Optional ByVal separator As String = character.blank) As Boolean
        Dim v As vector(Of String) = Nothing
        If Not value(i, v) Then
            Return False
        End If
        If v Is Nothing Then
            Return True
        End If
        If select_first Then
            If v.empty() Then
                o = Nothing
            Else
                o = v(0)
            End If
        Else
            o = v.str(separator)
        End If
        Return True
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
        If Not value(i, s, True) Then
            Return False
        End If
        If s Is Nothing Then
            o = True
        Else
            o = s.to(Of Boolean)()
        End If
        Return True
    End Function

    Public Function switch(ByVal i As String) As Boolean
        Dim o As Boolean = False
        If switch(i, o) Then
            Return o
        End If
        Return False
    End Function

    Public Function reverse_switch(ByVal i As String) As Boolean
        Dim o As Boolean = False
        If switch(i, o) Then
            Return Not o
        End If
        Return False
    End Function

    Public Function other_values() As vector(Of String)
        Return others
    End Function
End Class
