Imports System.Data.SqlClient
Imports System.Security


Public Class Form1
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim usuario As String = txtUsuario.Text
        Dim senha As String = txtSenha.Text

        If usuario = "" Or senha = "" Then
            'MsgBox("Preencha os Campos!!")
        Else

            Dim cmd As New SqlCommand("login", con)

            Try
                abrir()
                cmd.CommandType = 4
                With cmd.Parameters
                    .AddWithValue("@nome", usuario)
                    .AddWithValue("@cpf", senha)
                    .Add("@msg", SqlDbType.VarChar, 100).Direction = 2
                    cmd.ExecuteNonQuery()
                End With

                usuarioNome = txtUsuario.Text

                Dim msg As String = cmd.Parameters("@msg").Value
                MsgBox(msg, vbInformation)

                If (msg = "Dados Incorretos") Then
                    txtSenha.Clear()
                    txtUsuario.Clear()
                    txtUsuario.Focus()
                Else
                    Dim form = New F_principal
                    Me.Hide()
                    form.ShowDialog()
                End If


            Catch ex As Exception
                MessageBox.Show("Erro ao Listar" + ex.Message)
                fechar()
            End Try

        End If


    End Sub

    Private Sub txtCPF_MaskInputRejected(sender As Object, e As MaskInputRejectedEventArgs)

    End Sub

    Private Sub Txt_usuario_TextChanged(sender As Object, e As EventArgs)

    End Sub

    Private Sub BTN_login_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub txtUsuario_TextChanged(sender As Object, e As EventArgs) Handles txtUsuario.TextChanged

    End Sub

    Private Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        Dim usuario As String = txtUsuario.Text
        Dim senha As String = txtSenha.Text

        If usuario = "" Or senha = "" Then
            MsgBox("Preencha os Campos!!")
        Else

            Dim cmd As New SqlCommand("login", con)

            Try
                abrir()
                cmd.CommandType = 4
                With cmd.Parameters
                    .AddWithValue("@nome", usuario)
                    .AddWithValue("@cpf", senha)
                    .Add("@msg", SqlDbType.VarChar, 100).Direction = 2
                    cmd.ExecuteNonQuery()
                End With

                usuarioNome = txtUsuario.Text

                Dim msg As String = cmd.Parameters("@msg").Value
                MsgBox(msg, vbInformation)

                If (msg = "Dados Incorretos") Then
                    txtSenha.Clear()
                    txtUsuario.Clear()
                    txtUsuario.Focus()
                Else
                    Dim form = New F_principal
                    Me.Hide()
                    form.ShowDialog()
                End If


            Catch ex As Exception
                MessageBox.Show("Erro ao Listar" + ex.Message)
                fechar()
            End Try

        End If
    End Sub
End Class
