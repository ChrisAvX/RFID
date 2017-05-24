Imports System.IO
Imports System.IO.Ports
Imports System.Threading

Public Class locked
    Public usr_sql As New sql_info
    Public allowed As DataTable
    Public is_locked As Boolean = True
    Dim serialPort1 As SerialPort
    Private Delegate Sub RunLoop()
    ' THIS IS NEEDED, ITS CALLED FROM BUTTON CLICK

    <System.Runtime.InteropServices.DllImportAttribute("user32.dll", EntryPoint:="BlockInput")> _
    Public Shared Function BlockInput(<System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)> fBlockIt As Boolean) As <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)> Boolean
    End Function

    Private Sub locked_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        Me.Focus()
    End Sub

    Private Sub locked_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        usr_sql.set_db("RFID.s3db")
        allowed = usr_sql.send_query("SELECT * FROM users")
        AddHandler serialPort1.DataReceived, AddressOf DataReceivedHandler
        Me.TopMost = True
        Timer1.Start()
    End Sub



    Public Sub set_serial(ByVal serial As SerialPort)
        serialPort1 = serial
    End Sub

    Public Sub start_serial()
        If Not serialPort1.IsOpen Then
            serialPort1.Open()
        End If
    End Sub

    Public Sub stop_serial()
        serialPort1.Close()
    End Sub

    Public Sub anim_exit()
        Dim opac = 10
        Do Until opac = 0
            'MsgBox("opac: " + opac.ToString + " -- " + (opac / 10).ToString)
            Me.Opacity = (opac / 10)
            opac = opac - 1
            Thread.Sleep(30)
        Loop
        'MsgBox("finished")
        Me.Opacity = 0
        is_locked = False
        Me.Hide()
    End Sub
    Public Sub reset_allowed()
        allowed = usr_sql.send_query("SELECT * FROM users")
    End Sub
    Public Sub check_rfid(rfid As String)
        Dim cor As Integer = 0
        For Each user As DataRow In allowed.Rows
            If user("RFID_TAG") = rfid Then
                serialPort1.Write("1")
                BlockInput(False)
                'MsgBox("unlocked By: " + user("username").ToString + vbNewLine + "Using: " + user("device_name"))
                cor = 1
                Exit For
            End If
        Next
        If cor = 1 Then
            anim_exit()
        Else
            'MsgBox(rfid + " is not allowed")
            serialPort1.Write("2")
        End If
    End Sub

    Public Sub DataReceivedHandler(
                    sender As Object,
                    e As SerialDataReceivedEventArgs)
        Dim sp As SerialPort = CType(sender, SerialPort)
        Dim indata As String = sp.ReadTo("#")
        'MsgBox(indata(0))
        If is_locked = True Then
            If indata.Count > 1 Then
                Me.Invoke(Sub()
                              check_rfid(indata)
                          End Sub)
            End If
        End If
    End Sub


    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Me.Focus()
    End Sub
End Class