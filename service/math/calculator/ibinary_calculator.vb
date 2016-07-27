
Public Interface ibinary_calculator(Of T)
    'this = this + that
    Function increment(ByRef this As T, ByVal that As T) As calculator_error
    'this = this - that
    Function decrement(ByRef this As T, ByVal that As T) As calculator_error
    'this = this * that
    Function multiply(ByRef this As T, ByVal that As T) As calculator_error
    'this = this \ that, remainder = this mod that
    Function divide(ByRef this As T, ByVal that As T, ByRef remainder As T) As calculator_error
    'this = this mod that
    Function [mod](ByRef this As T, ByVal that As T) As calculator_error
    'this = (this < that) AS T, the defination of bool to T depends on implementation
    Function less(ByRef this As T, ByVal that As T) As calculator_error
    'this = (this == that) AS T
    Function equal(ByRef this As T, ByVal that As T) As calculator_error
    'this = (this <= that) AS T
    Function less_or_equal(ByRef this As T, ByVal that As T) As calculator_error
    'this = (this != that) AS T
    Function not_equal(ByRef this As T, ByVal that As T) As calculator_error
    'this = (this > that) AS T
    Function more(ByRef this As T, ByVal that As T) As calculator_error
    'this = (this >= that) AS T
    Function more_or_equal(ByRef this As T, ByVal that As T) As calculator_error
    'this = (this << that)
    Function left_shift(ByRef this As T, ByVal that As T) As calculator_error
    'this = (this >> that)
    Function right_shift(ByRef this As T, ByVal that As T) As calculator_error
    'this = pow(this, that)
    Function power(ByRef this As T, ByVal that As T) As calculator_error
    'this = yroot(this, that), remainder = this - (that ^ (yroot(this, that)))
    Function extract(ByRef this As T,
                     ByVal that As T,
                     ByRef remainder As T) As calculator_error
End Interface
