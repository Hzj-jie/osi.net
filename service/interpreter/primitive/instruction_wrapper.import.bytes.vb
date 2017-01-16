
' This file is generated by commands-parser, with commands.txt file.
' So change commands-parser or commands.txt instead of this file.

Imports osi.root.connector

Namespace primitive
    Partial Public NotInheritable Class instruction_wrapper
        Public Function import(ByVal i() As Byte, ByRef p As UInt32) As Boolean _
                              Implements exportable.import
            Me.i = Nothing
            Dim x As UInt32 = 0
            If bytes_uint32(i, x, unref(p)) Then
                Select Case x
                    Case command.push
                        Me.i = New instructions.push()
                    Case command.pop
                        Me.i = New instructions.pop()
                    Case command.jump
                        Me.i = New instructions.jump()
                    Case command.add
                        Me.i = New instructions.add()
                    Case command.sub
                        Me.i = New instructions.sub()
                    Case command.movc
                        Me.i = New instructions.movc()
                    Case command.mov
                        Me.i = New instructions.mov()
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
                    Case command.cpnip
                        Me.i = New instructions.cpnip()
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
                    Case command.extern
                        Me.i = New instructions.extern()
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
                    Case command.esc
                        Me.i = New instructions.esc()
                    Case Else
                        Return False
                End Select
                assert(Not Me.i Is Nothing)
                Return Me.i.import(i, p)
            Else
                Return False
            End If
        End Function
    End Class
End Namespace
