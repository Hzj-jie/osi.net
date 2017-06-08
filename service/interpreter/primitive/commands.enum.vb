
' This file is generated by commands-parser, with commands.txt file.
' So change commands-parser or commands.txt instead of this file.

Option Explicit On
Option Infer Off
Option Strict On

Namespace primitive
    Public Enum command As UInt32
        ' push a data slot in to stack 
        [push]
        ' pop a data slot from stack 
        [pop]
        ' jump to instruction @A 
        [jump]
        ' (*A) = B 
        [cpc]
        ' (*A) = (*B), clr B 
        [mov]
        ' (*A) = (*B) 
        [cp]
        ' (*A big_uint) = (*B big_uint) + (*C big_uint) 
        [add]
        ' (*A big_uint) = (*B big_uint) - (*C big_uint) 
        [sub]
        ' (*A big_uint) = (*B big_uint) * (*C big_uint) 
        [mul]
        ' (*A big_uint) = (*C big_uint) / (*D big_uint) (*B big_uint) = (*C big_uint) % (*D big_uint) 
        [div]
        ' (*A big_uint) = extract((*C big_uint), (*D big_uint)) (*B big_uint) = remainder(extract((*C big_uint), (*D big_uint))) 
        [ext]
        ' (*A big_uint) = pow((*B big_uint), (*C big_uint)) 
        [pow]
        ' jump to instruction @A, if (*B bool) is true 
        [jumpif]
        ' (*A) = CO 
        [cpco]
        ' (*A) = DBZ 
        [cpdbz]
        ' (*A) = IN 
        [cpin]
        ' finish execution, and leave the executor in a stop but not error status. 
        [stop]
        ' (*A) = ((*B big_uint) == (*C big_uint)) 
        [equal]
        ' (*A) = ((*B big_uint) < (*C big_uint)) 
        [less]
        ' (*A) += (*B) 
        [app]
        ' (*A) += sizeof(*B) + (*B) 
        [sapp]
        ' (*A) = sub-array(src=(*B), from=(*C uint)) 
        [cut]
        ' (*A) = sub-array(src=(*B), from=(*C uint), len=(*D uint)) 
        [cutl]
        ' execute an interrupt function (*A uint), with parameter (*B), return value will be set to (*C) 
        [int]
        ' set (*A) to empty array 
        [clr]
        ' (*A) = the (*C uint) chunk of (*B) 
        [scut]
        ' (*A) = array-size(*B) 
        [sizeof]
        ' (*A) = ((*B) == null) 
        [empty]
        ' (*A big_uint) = (*B big_uint) AND (*C big_uint) 
        [and]
        ' (*A big_uint) = (*B big_uint) OR (*C big_uint) 
        [or]
        ' (*A big_uint) = NOT (*B big_uint) 
        [not]
        ' push current state into states, such as IP_AFTER_THIS_INSTRUCTION, i.e. IP + 1, Stack Size, a typical usage is [ stst, jump ? ] 
        [stst]
        ' pop extra (StackSize - states.top.StackSize) slots from stack, and jump to instruction @(* states.top.IP uint64) 
        [rest]

        COUNT
    End Enum
End Namespace
