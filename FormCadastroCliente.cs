using System;
using System.Windows.Forms;
using System.Drawing;

namespace ControleContas
{
    public class FormCadastroCliente : Form
    {
        private GerenciadorContas gerenciador;
        private RadioButton rbPessoaFisica;
        private RadioButton rbPessoaJuridica;
        private TextBox txtIdentificador;
        private TextBox txtNome;
        private Button btnSalvar;

        public FormCadastroCliente(GerenciadorContas gerenciador)
        {
            this.gerenciador = gerenciador;
            ConfigurarInterface();
        }

        private void ConfigurarInterface()
        {
            this.Text = "Cadastro de Cliente";
            this.Size = new Size(400, 250);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            int y = 20;

            // Tipo de pessoa
            Label lblTipo = new Label { Text = "Tipo:", Location = new Point(20, y), AutoSize = true };
            this.Controls.Add(lblTipo);

            rbPessoaFisica = new RadioButton { Text = "Pessoa Física (CPF)", Location = new Point(120, y), AutoSize = true, Checked = true };
            rbPessoaJuridica = new RadioButton { Text = "Pessoa Jurídica (CNPJ)", Location = new Point(120, y + 25), AutoSize = true };
            this.Controls.Add(rbPessoaFisica);
            this.Controls.Add(rbPessoaJuridica);

            y += 70;

            // Identificador
            Label lblIdentificador = new Label { Text = "CPF/CNPJ:", Location = new Point(20, y), AutoSize = true };
            txtIdentificador = new TextBox { Location = new Point(120, y), Width = 200 };
            this.Controls.Add(lblIdentificador);
            this.Controls.Add(txtIdentificador);

            y += 30;

            // Nome
            Label lblNome = new Label { Text = "Nome:", Location = new Point(20, y), AutoSize = true };
            txtNome = new TextBox { Location = new Point(120, y), Width = 200 };
            this.Controls.Add(lblNome);
            this.Controls.Add(txtNome);

            y += 40;

            // Botão Salvar
            btnSalvar = new Button { Text = "Salvar", Location = new Point(120, y), Width = 100 };
            btnSalvar.Click += BtnSalvar_Click;
            this.Controls.Add(btnSalvar);
        }

        private void BtnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtIdentificador.Text) || string.IsNullOrWhiteSpace(txtNome.Text))
                {
                    MessageBox.Show("Preencha todos os campos!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Pessoa pessoa;
                if (rbPessoaFisica.Checked)
                {
                    pessoa = new PessoaFisica
                    {
                        CPF = txtIdentificador.Text,
                        Nome = txtNome.Text
                    };
                }
                else
                {
                    pessoa = new PessoaJuridica
                    {
                        CNPJ = txtIdentificador.Text,
                        Nome = txtNome.Text
                    };
                }

                gerenciador.AdicionarPessoa(pessoa);
                MessageBox.Show("Cliente cadastrado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}