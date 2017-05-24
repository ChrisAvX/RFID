Imports System.IO.Ports

Public Class waiter
    Inherits System.Windows.Forms.Form
    Public refForm As Form
    Public is_locked As Boolean = False

    Private Enum HotKeyModifiers
        None = &H0
        Alt = &H1
        Control = &H2
        Shift = &H4
        Windows = &H8
    End Enum

    Dim lock As New locked
    Dim serialPort1 As SerialPort

    Private Declare Function RegisterHotKey Lib "user32" (ByVal hWnd As IntPtr, ByVal id As Integer, ByVal fsModifiers As Integer, ByVal vk As Integer) As Boolean
    Private Declare Function UnregisterHotKey Lib "user32" (ByVal hWnd As IntPtr, ByVal id As Integer) As Boolean

    Private ctrl_shift_a As Boolean = False
    Private ctrl_shift_x As Boolean = False

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim mods As HotKeyModifiers
        mods = HotKeyModifiers.Control Or HotKeyModifiers.Shift

        ctrl_shift_a = RegisterHotKey(Me.Handle, 0, mods, Keys.L)
        ctrl_shift_x = RegisterHotKey(Me.Handle, 1, mods, Keys.X)
    End Sub

    Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)
        Const WM_HOTKEY As Integer = &H312

        Select Case m.Msg
            Case WM_HOTKEY
                hotKeyPressed(m.WParam.ToInt32)

        End Select
        MyBase.WndProc(m)
    End Sub

    Private Sub hotKeyPressed(ByVal id As Int32)
        Select Case id
            Case 0 ' 
                'Debug.WriteLine("Ctrl_Shift_LLLLLLLLLL pressed")
                lock.set_serial(serialPort1)
                lock.Opacity = 1
                lock.is_locked = True
                is_locked = True
                lock.Show()
                lock.start_serial()
            Case 1
                If Not lock.is_locked Then
                    refForm.Invoke(Sub()
                                       refForm.Show()
                                       refForm.ShowInTaskbar = True
                                   End Sub)
                    Me.Close()
                End If
        End Select
    End Sub

    Private Sub Form1_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        If ctrl_shift_a Then
            UnregisterHotKey(Me.Handle, 0)
        End If
        If ctrl_shift_x Then
            UnregisterHotKey(Me.Handle, 1)
        End If
    End Sub


    Public Sub set_serial(ByVal ser As SerialPort)
        serialPort1 = ser
    End Sub

    Public Sub setForm(ByVal f As Form)
        refForm = f
    End Sub
End Class