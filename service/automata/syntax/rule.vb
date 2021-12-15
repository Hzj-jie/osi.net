
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.utils

Partial Public NotInheritable Class syntaxer
    Partial Public NotInheritable Class rule
        Inherits configuration.rule

        Public Const command_ignore_types As String = "IGNORE_TYPES"
        Public Const command_root_types As String = "ROOT_TYPES"
        Public Const ignore_types_separator As Char = character.comma
        Public Shared ReadOnly ignore_types_separators() As String = {Convert.ToString(character.comma)}
        Private Shared ReadOnly empty_surround_strs() As String = Nothing
        Private ReadOnly m As map(Of String, Func(Of String, Boolean))
        Private ReadOnly ignores As [set](Of UInt32)
        Private ReadOnly roots As vector(Of UInt32)
        Private ReadOnly collection As syntax_collection

        Public Sub New(ByVal collection As syntax_collection)
            assert(Not collection Is Nothing)
            m = New map(Of String, Func(Of String, Boolean))()
            m.emplace(command_ignore_types, AddressOf ignore_types)
            m.emplace(command_root_types, AddressOf root_types)
            ignores = New [set](Of UInt32)()
            roots = New vector(Of UInt32)()
            Me.collection = collection
        End Sub

        Public Sub New(ByVal token_str_type As map(Of String, UInt32))
            Me.New(New syntax_collection(token_str_type))
        End Sub

        Public Sub New()
            Me.New(New syntax_collection())
        End Sub

        Protected Overrides Function command_mapping() As map(Of String, Func(Of String, Boolean))
            Return m
        End Function

        Protected Overrides Function [default](ByVal s As String, ByVal f As String) As Boolean
            Return syntax.create(f, ignores, s, collection)
        End Function

        Protected Overrides Function finish() As Boolean
            Return collection.complete()
        End Function

        Private Shared Function split_types(ByVal s As String) As vector(Of String)
            Dim v As vector(Of String) = Nothing
            If strsplit(s, ignore_types_separators, empty_surround_strs, v, True, False) AndAlso
               Not v.null_or_empty() Then
                For i As UInt32 = 0 To v.size() - uint32_1
                    v(i) = v(i).Trim()
                Next
                Return v
            End If
            Return Nothing
        End Function

        Private Function ignore_types(ByVal s As String) As Boolean
            ignores.clear()
            Dim v As vector(Of String) = Nothing
            v = split_types(s)
            If v Is Nothing Then
                Return True
            End If
            For i As UInt32 = 0 To v.size() - uint32_1
                Dim j As UInt32 = 0
                If collection.token_type(v(i), j) Then
                    Dim p As tuple(Of [set](Of UInt32).iterator, Boolean) = Nothing
                    p = ignores.emplace(j)
                    If Not p.second Then
                        raise_error(error_type.user, "failed to add type ", j)
                        Return False
                    End If
                Else
                    raise_error(error_type.user, "cannot find token type ", v(i), " for ignore types.")
                    Return False
                End If
            Next
            Return True
        End Function

        Private Function root_types(ByVal s As String) As Boolean
            roots.clear()
            Dim v As vector(Of String) = Nothing
            v = split_types(s)
            If v Is Nothing Then
                Return True
            End If
            For i As UInt32 = 0 To v.size() - uint32_1
                Dim j As UInt32 = 0
                If collection.define(v(i), j) Then
                    roots.emplace_back(j)
                Else
                    raise_error(error_type.user, "cannot define syntax type ", v(i), " for root types.")
                    Return False
                End If
            Next
            Return True
        End Function
    End Class
End Class
