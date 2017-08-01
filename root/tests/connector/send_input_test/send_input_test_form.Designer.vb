<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class send_input_test_form
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.textbox = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'textbox
        '
        Me.textbox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.textbox.Location = New System.Drawing.Point(0, 0)
        Me.textbox.Multiline = True
        Me.textbox.Name = "textbox"
        Me.textbox.ReadOnly = True
        Me.textbox.Size = New System.Drawing.Size(282, 253)
        Me.textbox.TabIndex = 0
        '
        'send_input_test_form
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(282, 253)
        Me.Controls.Add(Me.textbox)
        Me.Name = "send_input_test_form"
        Me.Text = "send_input_test_form"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Private WithEvents textbox As Windows.Forms.TextBox
End Class
