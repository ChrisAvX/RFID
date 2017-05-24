Imports System.IO
Imports System.IO.Ports

Public Class add_user
    Dim sql As New sql_info
    Dim serialPort1 As SerialPort

    Private Sub edit_user_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Public Sub set_up_connection()
        sql.set_db("RFID.s3db")
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        sql.send_query("INSERT INTO users (`username`,`RFID_TAG`,`device_name`) VALUES ('" + TextBox1.Text + "','" + TextBox2.Text + "','" + TextBox3.Text + "')")
        MsgBox("User added")
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
        Me.Close()
    End Sub
End Class