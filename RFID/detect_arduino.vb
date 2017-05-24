Imports System.IO.Ports
Imports System.Threading

Public Class detect_arduino

    Dim ac As Boolean
    Public SerialPort1 As SerialPort
    Public foundPort As String = ""
    Public response As starting_form

    Dim res As IAsyncResult
    Dim rc As New Thread(AddressOf Looper)

    Dim file_string() As String = {"x64\SQLite.Interop.dll",
                             "x86\SQLite.Interop.dll",
                             "EntityFramework.dll",
                             "EntityFramework.SqlServer.dll",
                             "RFID.s3db",
                             "System.Data.SQLite.dll",
                             "System.Data.SQLite.EF6.dll",
                             "System.Data.SQLite.Linq.dll",
                             "missingfile.dll"}
    Dim files As New Dictionary(Of String, Byte())

    Dim missing() As String

    Dim total_number = (My.Computer.Ports.SerialPortNames.Count + file_string.Count())
    Dim total_files As Integer
    Dim total_add = 100 / total_number

    Private Delegate Sub RunLoop() ' THIS IS NEEDED, ITS CALLED FROM BUTTON CLICK
    Private Sub detect_arduino_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Focus()
        files.Add("x64\SQLite.Interop.dll", My.Resources.Resource1.SQLite_Interop_64)
        files.Add("x86\SQLite.Interop.dll", My.Resources.Resource1.SQLite_Interop_86)
        files.Add("EntityFramework.dll", My.Resources.Resource1.EntityFramework)
        files.Add("EntityFramework.SqlServer.dll", My.Resources.Resource1.EntityFramework_SqlServer)
        files.Add("RFID.s3db", My.Resources.Resource1.RFID)
        files.Add("System.Data.SQLite.dll", My.Resources.Resource1.System_Data_SQLite)
        files.Add("System.Data.SQLite.EF6.dll", My.Resources.Resource1.System_Data_SQLite_EF6)
        files.Add("System.Data.SQLite.Linq.dll", My.Resources.Resource1.System_Data_SQLite_Linq)
        files.Add("missingfile.dll", My.Resources.Resource1.missingfile)

    End Sub
    Public Sub start_detect()
        rc.Start()
        AddHandler SerialPort2.DataReceived, AddressOf DataReceivedHandler
    End Sub
    Public Sub setForm(ByVal ref As Form)
        response = ref
    End Sub

    Public Sub Createfile(ByVal sender() As Byte, ByVal FileName As String)
        Dim FileStream As New System.IO.FileStream(FileName, System.IO.FileMode.OpenOrCreate)
        Dim BinaryWriter As New System.IO.BinaryWriter(FileStream)

        BinaryWriter.Write(sender)
        BinaryWriter.Close()
        FileStream.Close()
    End Sub
    Public Sub Looper()
        ac = False
        For Each file As String In file_string

            Me.Invoke(Sub()
                          Label3.Text = file
                      End Sub)
            If Not My.Computer.FileSystem.FileExists(file) Then
                MsgBox(file + " does not exist!")
                Createfile(files(file), file)
            Else
                'MsgBox(file + " has been found!")
            End If

            Me.Invoke(Sub()
                          ProgressBar1.Value += total_add
                      End Sub)
            Thread.Sleep(200)
        Next
        Me.Invoke(Sub()
                      Label1.Text = "Detecting arduino"
                  End Sub)
        For Each sp As String In My.Computer.Ports.SerialPortNames
            Try
                Me.Invoke(Sub()
                              Label3.Text = sp
                          End Sub)
                SerialPort2.PortName = sp
                SerialPort2.BaudRate = 9600
                SerialPort2.DataBits = 8
                SerialPort2.Parity = Parity.None
                SerialPort2.StopBits = StopBits.One
                SerialPort2.Handshake = Handshake.None
                SerialPort2.Encoding = System.Text.Encoding.Default
                SerialPort2.Open()
                SerialPort2.Write("3")
                Timer2.Interval = 500
                Timer2.Enabled = True
                While Timer2.Enabled And ac = False
                    Application.DoEvents()
                End While
                If ac Then
                    SerialPort2.Close()
                    Me.Invoke(Sub()
                                  carryOn()
                              End Sub)
                    Exit For
                End If
                SerialPort2.Close()
            Catch ex As Exception
                ' MsgBox(ex.Message) <-- usually thread ended message
            End Try
            Me.Invoke(Sub()
                          ProgressBar1.Value += total_add
                      End Sub)
        Next
        If Not ac Then
            MsgBox("no results, please manually add the COM port")
            abort_check()
        End If
    End Sub
    Public Sub abort_check()
        rc.Abort()
        Me.Close()
    End Sub

    Public Sub carryOn()
        response.Invoke(Sub()
                            response.TextBox1.Text = foundPort
                            response.TextBox1.ReadOnly = True
                            response.Button1.Hide()
                            response.start_com()
                        End Sub)
        rc.Abort()
        Me.Close()
    End Sub
    Public Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick
        Timer2.Stop()
    End Sub

    Public Sub DataReceivedHandler(
                    sender As Object,
                    e As SerialDataReceivedEventArgs)
        Dim sp As SerialPort = CType(sender, SerialPort)
        Dim indata As String = sp.ReadTo("#")
        'MsgBox("receive: " + indata)
        If indata = "Arduino" Then
            Me.Invoke(Sub()
                          foundPort = sp.PortName
                          ac = True
                      End Sub)
        End If
    End Sub
End Class