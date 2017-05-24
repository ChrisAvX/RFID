Imports System.IO
Imports System.IO.Ports
Imports System.Threading

Public Class Form1
    Dim serialPort1 As New SerialPort
    Private Delegate Sub RunLoop() ' THIS IS NEEDED, ITS CALLED FROM BUTTON CLICK

    Private Sub Form1_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed

        RemoveHandler serialPort1.DataReceived, AddressOf DataReceivedHandler
        serialPort1.Close()
    End Sub

    Private Sub Form1_Leave(sender As Object, e As EventArgs) Handles Me.Leave
        serialPort1.Close()
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Public Sub set_serial(ByVal serial As SerialPort)
        serialPort1 = serial
    End Sub
    Public Sub Setup()
        If Not serialPort1.IsOpen Then
            serialPort1.Open()
        End If
        AddHandler serialPort1.DataReceived, AddressOf DataReceivedHandler

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Setup()
        Label2.Text = "CONNECTED"
    End Sub

    Public Sub DataReceivedHandler(
                    sender As Object,
                    e As SerialDataReceivedEventArgs)
        Dim sp As SerialPort = CType(sender, SerialPort)
        Dim indata As String = sp.ReadTo("#")
        If indata.Count > 1 Then
            Me.Invoke(Sub()

                          TextBox1.Text += "Data Received:" + indata + vbTab + "(" + indata.Count.ToString() + ")" + vbNewLine
                      End Sub)
        End If

    End Sub
End Class
