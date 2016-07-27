
Namespace turing
    Public Enum [operator]
        'A B C ... are locations if there is no specific definition
        variable    'not an instruction, but shows this instruction is a variable
        [set]       'set A to B
        [goto]      'set ip to A
        goto_if     'if A is true, goto B, otherwise C
        push        'push a variable A to the space
        pop         'pop a variable from the space and store in A
        clear       'clear space
        [call]      'call function A as function id, A is an int
        [end]       'finish the block by setting ip to the end_ip
    End Enum
End Namespace

