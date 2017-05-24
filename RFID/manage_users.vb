Imports System.IO
Imports System.IO.Ports

Public Class manage_users

    Public usr_sql As New sql_info
    Dim all_users As DataTable
    Dim user_count As Integer = 0
    Dim serialPort1 As serialPort
    Private Sub manage_users_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        usr_sql.set_db("RFID.s3db")
        update_table()
    End Sub



    Private Sub DataGridView1_CellMouseDoubleClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles DataGridView1.CellMouseDoubleClick
        If e.RowIndex > user_count Then
            Dim new_usr As New add_user
            new_usr.set_up_connection()
            new_usr.set_serial(serialPort1)
            new_usr.Show()
            AddHandler new_usr.FormClosed, AddressOf update_table
        Else
            Dim edit_usr As New edit_user
            edit_usr.set_user(DataGridView1.Rows(e.RowIndex).Cells(0).Value)
            edit_usr.set_up_connection()
            edit_usr.set_serial(serialPort1)
            edit_usr.Show()
            AddHandler edit_usr.FormClosed, AddressOf update_table

            ' MsgBox(DataGridView1.Rows(e.RowIndex).Cells(0).Value)
        End If
    End Sub

    Public Sub update_table()
        all_users = usr_sql.send_query("SELECT * FROM users")

        DataGridView1.DataSource = all_users
        user_count = all_users.Rows.Count - 1
    End Sub

    Public Sub set_serial(ByVal ser As SerialPort)
        serialPort1 = ser
    End Sub
End Class