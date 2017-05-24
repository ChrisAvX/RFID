Imports System.Data.SQLite

Public Class sql_info
    Dim dbconnection As String

    Public Sub set_db(src As String)
        dbconnection = "Data Source=" + src
    End Sub

    Public Function send_query(query As String) As DataTable
        Dim dt As New DataTable()
        Dim cnn As New SQLiteConnection(dbconnection)
        cnn.Open()
        Try
            Dim mycommand As New SQLiteCommand(cnn)
            mycommand.CommandText = query
            Dim reader As SQLiteDataReader = mycommand.ExecuteReader()
            dt.Load(reader)
            reader.Close()
            cnn.Close()
        Catch e As SQLiteException
            MsgBox(e.Message)
        End Try

        Return dt

    End Function
End Class
