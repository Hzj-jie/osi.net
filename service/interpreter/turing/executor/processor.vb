
Imports System.Runtime.CompilerServices
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.connector

Namespace turing.executor
    Public Class processor
        Private Const end_ip As UInt32 = max_uint32
        Public ReadOnly space As vector(Of variable)
        Private ReadOnly interrupters As vector(Of interrupter)
        Private ReadOnly instructions As vector(Of instruction)
        Private ip As UInt32

        Public Sub New(ByVal instructions As vector(Of instruction))
            Me.space = New vector(Of variable)()
            Me.interrupters = New vector(Of interrupter)()
            assert(Not instructions Is Nothing)
            Me.instructions = instructions
            Me.ip = 0
        End Sub

        Public Function abolute_location(ByVal loc As location,
                                         ByRef dest_ip As UInt32) As Boolean
            assert(Not loc Is Nothing)
            If loc.type = location.def.relative Then
                If ip >= loc.offset Then
                    dest_ip = ip + loc.offset
                    Return True
                Else
                    Return False
                End If
            ElseIf loc.type = location.def.absolute Then
                If loc.offset >= 0 Then
                    dest_ip = loc.offset
                    Return True
                Else
                    Return False
                End If
            Else
                Return assert(False)
            End If
        End Function

        Public Function variable(ByVal i As UInt32, ByRef var As variable) As Boolean
            If i < instructions.size() AndAlso
               TypeOf instructions(i) Is variable Then
                var = DirectCast(instructions(i), variable)
                Return True
            Else
                Return False
            End If
        End Function

        Public Function variable(ByVal loc As location, ByRef var As variable) As Boolean
            Dim ip As UInt32 = 0
            Return abolute_location(loc, ip) AndAlso
                   variable(ip, var)
        End Function

        Public Function interrupt(ByVal id As UInt32) As Boolean
            If id < interrupters.size() Then
                Return interrupters(id).execute(space)
            Else
                Return False
            End If
        End Function

        Public Function define(ByVal f As interrupter) As UInt32
            assert(Not f Is Nothing)
            Dim r As UInt32 = 0
            r = interrupters.size()
            interrupters.emplace_back(f)
            Return r
        End Function

        Public Function define(ByVal f As Func(Of variable(), variable)) As UInt32
            Return define(New interrupter(f))
        End Function

        Public Function move(ByVal target_ip As UInt32) As Boolean
            If target_ip >= instructions.size() AndAlso target_ip <> end_ip Then
                Return False
            Else
                ip = target_ip
                Return True
            End If
        End Function

        Public Function move(ByVal loc As location) As Boolean
            Dim ip As UInt32 = 0
            Return abolute_location(loc, ip) AndAlso
                   move(ip)
        End Function

        Public Function move_next() As Boolean
            Return move(ip + 1)
        End Function

        Public Sub [end]()
            assert(move(end_ip))
        End Sub

        Public Function execute() As Boolean
            While ip <> end_ip
                assert(ip < instructions.size())
                If Not instructions(ip).execute(Me) Then
                    Return False
                End If
            End While
            Return True
        End Function
    End Class
End Namespace
