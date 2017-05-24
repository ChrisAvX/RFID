Imports System.IO.Ports

Public Class starting_form
    Dim checker As New detect_arduino
    Dim serialPort1 As New SerialPort
    Dim serial As String


    Private Sub starting_form_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        check_arduino()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs)
        check_arduino()
    End Sub

    Public Sub check_arduino()
        Try
            checker.setForm(Me)
            checker.TopMost = True
            checker.Show()
            checker.Focus()
            checker.start_detect()
        Catch e As Exception
            'MsgBox("exeption")
            checker = New detect_arduino
            checker.TopLevel = True
            checker.setForm(Me)
            checker.Show()

            checker.start_detect()
        End Try

    End Sub

    Public Sub start_com()
        serialPort1.PortName = TextBox1.Text 'change com port to match your Arduino port
        serialPort1.BaudRate = 9600 ' speed
        serialPort1.DataBits = 8
        serialPort1.Parity = Parity.None
        serialPort1.StopBits = StopBits.Two

        serialPort1.Handshake = Handshake.None
        serialPort1.Encoding = System.Text.Encoding.Default 'very important!
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim rc As New options
        rc.set_serial(serialPort1)
        rc.Show()
    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click

        checker = New detect_arduino
        checker.TopLevel = True
        checker.setForm(Me)
        checker.Show()

        checker.start_detect()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim rf As New waiter
        rf.setForm(Me)
        rf.set_serial(serialPort1)
        rf.Show()

        Me.ShowInTaskbar = False
        Me.Hide()
    End Sub
End Class