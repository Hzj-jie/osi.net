
' This file is generated by commands-parser, with commands.txt file.
' So change commands-parser or commands.txt instead of this file.

Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation

Namespace primitive
    Partial Public NotInheritable Class instruction_wrapper
        Public Function import(ByVal s As vector(Of String), ByRef p As UInt32) As Boolean _
                              Implements exportable.import
            Me.i = Nothing
            If s.null_or_empty() OrElse s.size() <= p Then
                Return False
            End If
            Select Case s(p)
                Case command_str(command.push)
                    Me.i = New instructions.push()
                Case command_str(command.pop)
                    Me.i = New instructions.pop()
                Case command_str(command.jump)
                    Me.i = New instructions.jump()
                Case command_str(command.cpc)
                    Me.i = New instructions.cpc()
                Case command_str(command.mov)
                    Me.i = New instructions.mov()
                Case command_str(command.cp)
                    Me.i = New instructions.cp()
                Case command_str(command.add)
                    Me.i = New instructions.add()
                Case command_str(command.sub)
                    Me.i = New instructions.sub()
                Case command_str(command.mul)
                    Me.i = New instructions.mul()
                Case command_str(command.div)
                    Me.i = New instructions.div()
                Case command_str(command.ext)
                    Me.i = New instructions.ext()
                Case command_str(command.pow)
                    Me.i = New instructions.pow()
                Case command_str(command.jumpif)
                    Me.i = New instructions.jumpif()
                Case command_str(command.cpco)
                    Me.i = New instructions.cpco()
                Case command_str(command.cpdbz)
                    Me.i = New instructions.cpdbz()
                Case command_str(command.cpin)
                    Me.i = New instructions.cpin()
                Case command_str(command.stop)
                    Me.i = New instructions.stop()
                Case command_str(command.equal)
                    Me.i = New instructions.equal()
                Case command_str(command.less)
                    Me.i = New instructions.less()
                Case command_str(command.app)
                    Me.i = New instructions.app()
                Case command_str(command.sapp)
                    Me.i = New instructions.sapp()
                Case command_str(command.cut)
                    Me.i = New instructions.cut()
                Case command_str(command.cutl)
                    Me.i = New instructions.cutl()
                Case command_str(command.int)
                    Me.i = New instructions.int()
                Case command_str(command.clr)
                    Me.i = New instructions.clr()
                Case command_str(command.scut)
                    Me.i = New instructions.scut()
                Case command_str(command.sizeof)
                    Me.i = New instructions.sizeof()
                Case command_str(command.empty)
                    Me.i = New instructions.empty()
                Case command_str(command.and)
                    Me.i = New instructions.and()
                Case command_str(command.or)
                    Me.i = New instructions.or()
                Case command_str(command.not)
                    Me.i = New instructions.not()
                Case command_str(command.stst)
                    Me.i = New instructions.stst()
                Case command_str(command.rest)
                    Me.i = New instructions.rest()
                Case command_str(command.fadd)
                    Me.i = New instructions.fadd()
                Case command_str(command.fsub)
                    Me.i = New instructions.fsub()
                Case command_str(command.fmul)
                    Me.i = New instructions.fmul()
                Case command_str(command.fdiv)
                    Me.i = New instructions.fdiv()
                Case command_str(command.fext)
                    Me.i = New instructions.fext()
                Case command_str(command.fpow)
                    Me.i = New instructions.fpow()
                Case command_str(command.fequal)
                    Me.i = New instructions.fequal()
                Case command_str(command.fless)
                    Me.i = New instructions.fless()
                Case command_str(command.lfs)
                    Me.i = New instructions.lfs()
                Case command_str(command.rfs)
                    Me.i = New instructions.rfs()
                Case command_str(command.alloc)
                    Me.i = New instructions.alloc()
                Case command_str(command.dealloc)
                    Me.i = New instructions.dealloc()
                Case command_str(command.jmpr)
                    Me.i = New instructions.jmpr()
                Case Else
                    Return False
            End Select
            assert(Not Me.i Is Nothing)
            Return Me.i.import(s, p)
        End Function
    End Class
End Namespace
