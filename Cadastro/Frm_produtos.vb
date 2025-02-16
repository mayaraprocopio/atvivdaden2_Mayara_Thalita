﻿Imports System.Data.SqlClient
Imports System.IO

Public Class Frm_produtos
    Private imagemCarregada As Image
    Private Sub Frm_produtos_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'CarregarImagem()
        DesabilitarCampos()

        btnSalvar.Enabled = False



        Listar()

        btnEditar.Enabled = False
        btnExcluir.Enabled = False

    End Sub


    Private Sub Listar()
        Dim dt As New DataTable
        Dim da As SqlDataAdapter

        Try
            abrir()
            da = New SqlDataAdapter("SELECT * FROM produtos", con)
            da.Fill(dt)
            dg.DataSource = dt

            ContarLinhas()
            FormatarDG()

        Catch ex As Exception
            MessageBox.Show("Erro ao Listar" + ex.Message)
            fechar()
        End Try

    End Sub

    Private Sub FormatarDG()
        dg.Columns(0).Visible = False
        dg.Columns(6).Visible = False

        dg.Columns(1).HeaderText = "Nome"
        dg.Columns(2).HeaderText = "Descriçao"
        dg.Columns(3).HeaderText = "Quantidade"
        dg.Columns(4).HeaderText = "Valor"
        dg.Columns(5).HeaderText = "Data de Cadastro"

        dg.Columns(2).Width = 130


    End Sub

    Private Sub DesabilitarCampos()
        txtNome.Enabled = False
        txtDescricao.Enabled = False
        txtQuantidade.Enabled = False
        txtValor.Enabled = False


    End Sub

    Private Sub HabilitarCampos()
        txtNome.Enabled = True
        txtDescricao.Enabled = True
        txtQuantidade.Enabled = True
        txtValor.Enabled = True


    End Sub

    Private Sub Limpar()
        txtNome.Focus()
        txtNome.Text = ""
        txtDescricao.Text = ""
        txtQuantidade.Text = ""
        txtValor.Text = ""

        txtBuscar.Text = ""
        'CarregarImagem()

    End Sub

    Private Sub ContarLinhas()
        Dim total As Integer = dg.Rows.Count
        lblTotal.Text = CInt(total)

    End Sub

    Private Sub btnNovo_Click(sender As Object, e As EventArgs) Handles btnNovo.Click
        HabilitarCampos()
        Limpar()
        btnSalvar.Enabled = True

    End Sub

    Private Sub btnSalvar_Click(sender As Object, e As EventArgs) Handles btnSalvar.Click
        Dim cmd As SqlCommand

        If txtNome.Text <> "" Then

            Try


                'CARREGANDO IMAGEM NO BANCO
                Dim ms As New IO.MemoryStream
                imagemCarregada.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg)
                Dim byteArray = ms.ToArray


                abrir()
                cmd = New SqlCommand("sp_salvarpro", con)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@nome", txtNome.Text)
                cmd.Parameters.AddWithValue("@descricao", txtDescricao.Text)
                cmd.Parameters.AddWithValue("@quantidade", txtQuantidade.Text)
                cmd.Parameters.AddWithValue("@valor", txtValor.Text)
                cmd.Parameters.AddWithValue("@data_cadastro", Now.Date())
                cmd.Parameters.AddWithValue("@imagem", byteArray)

                cmd.Parameters.Add("@mensagem", SqlDbType.VarChar, 100).Direction = 2
                cmd.ExecuteNonQuery()

                Dim msg As String = cmd.Parameters("@mensagem").Value.ToString
                MessageBox.Show(msg, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)


                Listar()
                Limpar()

                btnSalvar.Enabled = False

            Catch ex As Exception
                MessageBox.Show("Erro ao salvar os dados" + ex.Message)
                fechar()
            End Try
        End If
    End Sub

    Private Sub dg_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dg.CellClick
        btnEditar.Enabled = True
        btnExcluir.Enabled = True
        HabilitarCampos()

        txtCodigo.Text = dg.CurrentRow.Cells(0).Value
        txtNome.Text = dg.CurrentRow.Cells(1).Value
        txtDescricao.Text = dg.CurrentRow.Cells(2).Value
        txtQuantidade.Text = dg.CurrentRow.Cells(3).Value
        txtValor.Text = CInt(dg.CurrentRow.Cells(4).Value)


        Dim tempImagem As Byte() = DirectCast(dg.CurrentRow.Cells(6).Value, Byte())
        If tempImagem Is Nothing Then
            MessageBox.Show("Imagem não localizada", "Erro")
            Exit Sub
        End If
        Dim strArquivo As String = Convert.ToString(DateTime.Now.ToFileTime())
        Dim fs As New FileStream(strArquivo, FileMode.CreateNew, FileAccess.Write)
        fs.Write(tempImagem, 0, tempImagem.Length)
        fs.Flush()
        fs.Close()
        imagemCarregada = Image.FromFile(strArquivo)
        imagem.Image = imagemCarregada

    End Sub

    Private Sub btnEditar_Click(sender As Object, e As EventArgs) Handles btnEditar.Click
        Dim cmd As SqlCommand

        If txtNome.Text <> "" Then

            Try

                'CARREGANDO IMAGEM NO BANCO
                Dim ms As New IO.MemoryStream
                imagemCarregada.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg)
                Dim byteArray = ms.ToArray


                abrir()
                cmd = New SqlCommand("sp_editarpro", con)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@nome", txtNome.Text)
                cmd.Parameters.AddWithValue("@descricao", txtDescricao.Text)
                cmd.Parameters.AddWithValue("@quantidade", txtQuantidade.Text)
                cmd.Parameters.AddWithValue("@valor", txtValor.Text)
                cmd.Parameters.AddWithValue("@id_produto", txtCodigo.Text)
                cmd.Parameters.AddWithValue("@imagem", byteArray)

                cmd.Parameters.Add("@mensagem", SqlDbType.VarChar, 100).Direction = 2
                cmd.ExecuteNonQuery()

                Dim msg As String = cmd.Parameters("@mensagem").Value.ToString
                MessageBox.Show(msg, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)


                Listar()
                Limpar()

            Catch ex As Exception
                MessageBox.Show("Erro ao editar os dados" + ex.Message)
                fechar()
            End Try
        End If
    End Sub

    Private Sub btnExcluir_Click(sender As Object, e As EventArgs) Handles btnExcluir.Click
        Dim cmd As SqlCommand

        If txtCodigo.Text <> "" Then

            Try
                abrir()
                cmd = New SqlCommand("sp_excluirPro", con)
                cmd.CommandType = CommandType.StoredProcedure

                cmd.Parameters.AddWithValue("@id_produto", txtCodigo.Text)

                cmd.Parameters.Add("@mensagem", SqlDbType.VarChar, 100).Direction = 2
                cmd.ExecuteNonQuery()

                Dim msg As String = cmd.Parameters("@mensagem").Value.ToString
                MessageBox.Show(msg, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)


                Listar()
                Limpar()

                btnExcluir.Enabled = False
                btnEditar.Enabled = False

            Catch ex As Exception
                MessageBox.Show("Erro ao excluir os dados" + ex.Message)
                fechar()
            End Try
        End If
    End Sub

    Private Sub txtBuscar_TextChanged(sender As Object, e As EventArgs) Handles txtBuscar.TextChanged
        If txtBuscar.Text = "" And dg.Rows.Count > 0 Then
            Listar()

        Else
            Dim dt As New DataTable
            Dim da As SqlDataAdapter

            Try
                abrir()
                da = New SqlDataAdapter("sp_buscarProNome", con)
                da.SelectCommand.CommandType = CommandType.StoredProcedure
                da.SelectCommand.Parameters.AddWithValue("@nome", txtBuscar.Text)

                da.Fill(dt)
                dg.DataSource = dt

                ContarLinhas()


            Catch ex As Exception
                MessageBox.Show("Erro ao Listar" + ex.Message)
                fechar()
            End Try
        End If
    End Sub

    Private Sub btnImagem_Click(sender As Object, e As EventArgs) Handles btnImagem.Click

        Using OFD As New OpenFileDialog With {.Filter = "Image File(*.jpg;*.bmp;*.png)|*.jpg;*.bmp;*.png"}
            If OFD.ShowDialog = DialogResult.OK Then
                imagemCarregada = Image.FromFile(OFD.FileName)
                imagem.Image = imagemCarregada
            End If
        End Using

    End Sub


    'Sub CarregarImagem()
    'Dim img As String = "http://www.buritama.sp.leg.br/imagens/parlamentares-2013-2016/sem-foto.jpg/image"
    'Dim req As System.Net.HttpWebRequest
    'D'im resp As System.Net.HttpWebResponse
    '  req = req.Create(img)
    '  resp = req.GetResponse
    '   imagemCarregada = New Bitmap(resp.GetResponseStream)
    '   imagem.Image = imagemCarregada
    '  req.Abort()
    'End Sub

    Private Sub imagem_Click(sender As Object, e As EventArgs) Handles imagem.Click

    End Sub
End Class
