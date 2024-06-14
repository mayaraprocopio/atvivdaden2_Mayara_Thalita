
Imports System.Data.SqlClient
Module conexao
    Public conectar As SqlConnection
    Public comando As SqlCommand
    Public local As DataSet
    Public adaptar As SqlDataAdapter
    Public sql As String
    Public resultado As Integer
    Public registro As DataRow
    Public dt As DataSet
    Public cnn As String = Application.StartupPath & "Data Source=LAPTOP-UUBF1MS0; Initial Catalog=admin; Integrated Security=SSPI;"
    Public vnivel As String
    Public contador As Integer


    Public usuarioNome As String

    Public con As New SqlConnection("Server=LAPTOP-UUBF1MS0; DataBase=admin; Integrated Security=SSPI")

    Sub abrir()
        If con.State = 0 Then
            con.Open()
        End If
    End Sub

    Sub fechar()
        If con.State = 1 Then
            con.Close()
        End If
    End Sub
End Module


