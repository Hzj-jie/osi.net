
' This file is generated by commands-parser, with commands.txt file.
' So change commands-parser or commands.txt instead of this file.

Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Namespace primitive
    Partial Public NotInheritable Class instruction_wrapper
        Public Function import(ByVal i() As Byte, ByRef p As UInt32) As Boolean _
                              Implements exportable.import
            Me.i = Nothing
            Dim x As UInt32 = 0
            If Not bytes_uint32(i, x, unref(p)) Then
                Return False
            End If
            Select Case x
                Case command.push
                    Me.i = New instructions.push()
                Case command.pop
                    Me.i = New instructions.pop()
                Case command.jump
                    Me.i = New instructions.jump()
                Case command.cpc
                    Me.i = New instructions.cpc()
                Case command.mov
                    Me.i = New instructions.mov()
                Case command.cp
                    Me.i = New instructions.cp()
                Case command.add
                    Me.i = New instructions.add()
                Case command.sub
                    Me.i = New instructions.sub()
                Case command.mul
                    Me.i = New instructions.mul()
                Case command.div
                    Me.i = New instructions.div()
                Case command.ext
                    Me.i = New instructions.ext()
                Case command.pow
                    Me.i = New instructions.pow()
                Case command.jumpif
                    Me.i = New instructions.jumpif()
                Case command.cpco
                    Me.i = New instructions.cpco()
                Case command.cpdbz
                    Me.i = New instructions.cpdbz()
                Case command.cpin
                    Me.i = New instructions.cpin()
                Case command.stop
                    Me.i = New instructions.stop()
                Case command.equal
                    Me.i = New instructions.equal()
                Case command.less
                    Me.i = New instructions.less()
                Case command.app
                    Me.i = New instructions.app()
                Case command.sapp
                    Me.i = New instructions.sapp()
                Case command.cut
                    Me.i = New instructions.cut()
                Case command.cutl
                    Me.i = New instructions.cutl()
                Case command.int
                    Me.i = New instructions.int()
                Case command.clr
                    Me.i = New instructions.clr()
                Case command.scut
                    Me.i = New instructions.scut()
                Case command.sizeof
                    Me.i = New instructions.sizeof()
                Case command.empty
                    Me.i = New instructions.empty()
                Case command.and
                    Me.i = New instructions.and()
                Case command.or
                    Me.i = New instructions.or()
                Case command.not
                    Me.i = New instructions.not()
                Case command.stst
                    Me.i = New instructions.stst()
                Case command.rest
                    Me.i = New instructions.rest()
                Case command.fadd
                    Me.i = New instructions.fadd()
                Case command.fsub
                    Me.i = New instructions.fsub()
                Case command.fmul
                    Me.i = New instructions.fmul()
                Case command.fdiv
                    Me.i = New instructions.fdiv()
                Case command.fext
                    Me.i = New instructions.fext()
                Case command.fpow
                    Me.i = New instructions.fpow()
                Case command.fequal
                    Me.i = New instructions.fequal()
                Case command.fless
                    Me.i = New instructions.fless()
                Case command.lfs
                    Me.i = New instructions.lfs()
                Case command.rfs
                    Me.i = New instructions.rfs()
                Case command.alloc
                    Me.i = New instructions.alloc()
                Case command.dealloc
                    Me.i = New instructions.dealloc()
                Case command.hcpin
                    Me.i = New instructions.hcpin()
                Case command.hcpout
                    Me.i = New instructions.hcpout()
                Case command.hmovin
                    Me.i = New instructions.hmovin()
                Case command.hmovout
                    Me.i = New instructions.hmovout()
                Case Else
                    Return False
            End Select
            assert(Not Me.i Is Nothing)
            Return Me.i.import(i, p)
        End Function
    End Class
End Namespace
