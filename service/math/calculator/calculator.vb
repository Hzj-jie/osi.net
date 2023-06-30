
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.connector

Public Class calculator(Of T)
    Public Enum [operator]
        increment
        decrement
        multiply
        divide
        [mod]
        less
        equal
        less_or_equal
        not_equal
        more
        more_or_equal
        left_shift
        right_shift
        power
        extract
    End Enum

    Public Enum bracket
        left
        right
    End Enum

    Private Class priority_operator
        Public ReadOnly [operator] As [operator]
        Public ReadOnly priority As UInt32

        Public Sub New(ByVal [operator] As [operator], ByVal priority As UInt32)
            Me.[operator] = [operator]
            Me.priority = priority
        End Sub
    End Class

    Private Const priority_range As UInt32 = 5
    Private ReadOnly os As stack(Of priority_operator)
    Private ReadOnly ts As stack(Of T)
    Private ReadOnly bc As ibinary_calculator(Of T)
    Private priority_level As UInt32

    Public Sub New(ByVal b As ibinary_calculator(Of T))
        assert(Not b Is Nothing)
        os = New stack(Of priority_operator)()
        ts = New stack(Of T)()
        bc = b
    End Sub

    Private Shared Function base_priority(ByVal o As [operator]) As UInt32
        Select Case o
            Case calculator(Of T).operator.increment,
                 calculator(Of T).operator.decrement
                Return 2
            Case calculator(Of T).operator.multiply,
                 calculator(Of T).operator.divide,
                 calculator(Of T).operator.mod
                Return 3
            Case calculator(Of T).operator.less,
                 calculator(Of T).operator.equal,
                 calculator(Of T).operator.less_or_equal,
                 calculator(Of T).operator.not_equal,
                 calculator(Of T).operator.more,
                 calculator(Of T).operator.more_or_equal
                Return 0
            Case calculator(Of T).operator.left_shift,
                 calculator(Of T).operator.right_shift
                Return 1
            Case calculator(Of T).operator.power,
                 calculator(Of T).operator.extract
                Return 4
            Case Else
                assert(False)
                Return priority_range
        End Select
    End Function

    Private Function priority(ByVal o As [operator]) As UInt32
        Return priority_level * priority_range + base_priority(o)
    End Function

    Private Sub calculate(ByRef e As calculator_error)
        assert(Not os.empty() AndAlso ts.size() >= 2)
        Dim this As T = Nothing
        Dim that As T = Nothing
        Dim pop As priority_operator = Nothing
        that = ts.back()
        ts.pop()
        this = ts.back()
        ts.pop()
        pop = os.back()
        os.pop()
        Select Case pop.[operator]
            Case calculator(Of T).operator.decrement
                e = bc.decrement(this, that)
            Case calculator(Of T).operator.divide
                e = bc.divide(this, that, Nothing)
            Case calculator(Of T).operator.equal
                e = bc.equal(this, that)
            Case calculator(Of T).operator.extract
                e = bc.extract(this, that, Nothing)
            Case calculator(Of T).operator.increment
                e = bc.increment(this, that)
            Case calculator(Of T).operator.left_shift
                e = bc.left_shift(this, that)
            Case calculator(Of T).operator.less
                e = bc.less(this, that)
            Case calculator(Of T).operator.less_or_equal
                e = bc.less_or_equal(this, that)
            Case calculator(Of T).operator.mod
                e = bc.mod(this, that)
            Case calculator(Of T).operator.more
                e = bc.more(this, that)
            Case calculator(Of T).operator.more_or_equal
                e = bc.more_or_equal(this, that)
            Case calculator(Of T).operator.multiply
                e = bc.multiply(this, that)
            Case calculator(Of T).operator.not_equal
                e = bc.not_equal(this, that)
            Case calculator(Of T).operator.power
                e = bc.power(this, that)
            Case calculator(Of T).operator.right_shift
                e = bc.right_shift(this, that)
            Case Else
                assert(False)
        End Select
        ts.emplace(this)
    End Sub

    Public Sub emplace(ByVal op As [operator], ByRef e As calculator_error)
        Dim p As UInt32 = 0
        p = priority(op)
        'though the expression error can be checked here
        While Not os.empty() AndAlso os.back().priority >= p AndAlso ts.size() >= 2
            calculate(e)
            If e.has_error() Then
                Return
            End If
        End While
        os.emplace(New priority_operator(op, priority(op)))
    End Sub

    Private Function execute() As calculator_error
        While Not os.empty() AndAlso ts.size() >= 2
            Dim e As calculator_error = Nothing
            calculate(e)
            If e.has_error() Then
                Return e
            End If
        End While
        assert(Not ts.empty())
        If Not os.empty OrElse
           ts.size() > 1 Then
            Return calculator_error.operand_mismatch_error
        ElseIf priority_level > 0 Then
            Return calculator_error.bracket_mismatch_error
        Else
            Return Nothing
        End If
    End Function

    Public Function execute(ByRef e As calculator_error) As T
        e = execute()
        Dim r As T = Nothing
        r = ts.back()
        ts.pop()
        reset()
        Return r
    End Function

    Public Sub emplace(ByVal br As bracket, ByRef e As calculator_error)
        Select Case br
            Case calculator(Of T).bracket.left
                priority_level += uint32_1
            Case calculator(Of T).bracket.right
                If priority_level = 0 Then
                    e = calculator_error.bracket_mismatch_error
                Else
                    priority_level -= uint32_1
                End If
            Case Else
                assert(False)
        End Select
    End Sub

    Public Sub emplace(ByVal v As T, ByRef e As calculator_error)
        If ts.size() = os.size() Then
            ts.push(v)
        Else
            e = calculator_error.operand_mismatch_error
        End If
    End Sub

    Public Sub reset()
        ts.clear()
        os.clear()
        priority_level = 0
    End Sub
End Class
