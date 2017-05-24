Imports System.IO
Imports System.IO.Ports

Public Class edit_user
    Dim usr_ID As String
    Dim sql As New sql_info
    Dim serialPort1 As SerialPort

    Private Sub edit_user_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Public Sub set_user(ByVal usr As String)
        usr_ID = usr
    End Sub

    Public Sub set_up_connection()
        sql.set_db("RFID.s3db")
        Dim tabledata As DataTable = sql.send_query("SELECT * FROM users WHERE ID='" + usr_ID + "'")
        Label2.Text = tabledata.Rows(0)("username")
        TextBox1.Text = tabledata.Rows(0)("username")
        TextBox2.Text = tabledata.Rows(0)("RFID_TAG")
        TextBox3.Text = tabledata.Rows(0)("device_name")

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        sql.send_query("UPDATE users SET username='" + TextBox1.Text + "', RFID_TAG='" + TextBox2.Text + "', device_name='" + TextBox3.Text + "' WHERE ID=" + usr_ID)
        MsgBox("Updated")
        Me.Close()
    End Sub

    Public Sub set_serial(ByVal ser As SerialPort)
        serialPort1 = ser
    End Sub

    Public Sub set_rfid(rfid As String)
        TextBox2.Text = rfid
        RemoveHandler serialPort1.DataReceived, AddressOf DataReceivedHandler
    End Sub

    Public Sub DataReceivedHandler(
                    sender As Object,
                    e As SerialDataReceivedEventArgs)
        Dim sp As SerialPort = CType(sender, SerialPort)
        Dim indata As String = sp.ReadTo("#")
        Me.Invoke(Sub()
                      set_rfid(indata)
                  End Sub)
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If Not serialPort1.IsOpen Then
            serialPort1.Open()
        Else
            serialPort1.DiscardInBuffer()
        End If
        TextBox2.Text = "waiting for input....."
        AddHandler serialPort1.DataReceived, AddressOf DataReceivedHandler
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        sql.send_query("DELETE FROM users WHERE ID='" + usr_ID + "'")
        MsgBox("User deleted")
        Me.Close()
    End Sub
End Class