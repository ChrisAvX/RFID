Imports System.IO
Imports System.IO.Ports
Imports System.Threading

Public Class options

    Dim lock As New locked
    Dim serialPort1 As SerialPort

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim rf As New Form1
        rf.set_serial(serialPort1)
        rf.Show()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim sql_form As New sql_test
        sql_form.Show()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        lock.set_serial(serialPort1)
        lock.Opacity = 1
        lock.is_locked = True
        lock.Show()
        lock.start_serial()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Dim manage As New manage_users
        manage.set_serial(serialPort1)
        manage.Show()
    End Sub

    Private Sub options_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    End Sub

    Public Sub set_serial(ByVal ser As SerialPort)
        serialPort1 = ser
    End Sub
End Class