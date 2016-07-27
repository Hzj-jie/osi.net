
Namespace fullstack.instruction
    Public Enum [operator] As Int32
        variable    'not an instruction, but shows this instruction is a variable
        [set]       'set A to B
        [goto]      'set ip to A
        goto_if     'if A is true, goto B
        push        'push A to the stack
        pop         'pop one from stack
        syscall     'call interpreter supported functions A is the function id
    End Enum
End Namespace
