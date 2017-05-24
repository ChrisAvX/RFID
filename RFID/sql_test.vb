Imports System.Data.SQLite

Public Class sql_test
    Dim dbconnection As String

    Private Sub sql_test_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        dbconnection = "Data Source=RFID.s3db"
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim dt As New DataTable()
        Try
            Dim cnn As New SQLiteConnection(dbconnection)
            cnn.Open()
            Dim mycommand As New SQLiteCommand(cnn)
            mycommand.CommandText = TextBox1.Text
            Dim reader As SQLiteDataReader = mycommand.ExecuteReader()
            dt.Load(reader)
            reader.Close()
            cnn.Close()
        Catch
            MsgBox("faled")
        End Try

        DataGridView1.DataSource = dt

    End Sub

End Class