using System;
using System.Windows.Forms;
using System.Drawing;
using System.Linq;

namespace ControleContas
{
    public partial class FormCadastroConta : Form
    {
        private GerenciadorContas gerenciador;
        private ComboBox cboCliente;
        private RadioButton rbResidencial;
        private RadioButton rbComercial;
        private TextBox txtNumeroInstalacao;
        private TextBox txtLeituraAnterior;
        private TextBox txtLeituraAtual;
        private Button btnSalvar;

        public FormCadastroConta(GerenciadorContas gerenciador)
        {
            this.gerenciador = gerenciador;

            ConfigurarInterface();
            CarregarClientes();
        }

        private void ConfigurarInterface()
        {
            this.Text = "Cadastro de Conta";
            this.Size = new Size(450, 350);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            int y = 20;

            // Cliente
            Label lblCliente = new Label { Text = "Cliente:", Location = new Point(20, y), AutoSize = true };
            cboCliente = new ComboBox { Location = new Point(180, y), Width = 220, DropDownStyle = ComboBoxStyle.DropDownList };
            this.Controls.Add(lblCliente);
            this.Controls.Add(cboCliente);

            y += 40;

            // Tipo de conta
            Label lblTipo = new Label { Text = "Tipo de Conta:", Location = new Point(20, y), AutoSize = true };
            this.Controls.Add(lblTipo);

            rbResidencial = new RadioButton { Text = "Residencial", Location = new Point(180, y), AutoSize = true, Checked = true };
            rbComercial = new RadioButton { Text = "Comercial", Location = new Point(180, y + 25), AutoSize = true };
            this.Controls.Add(rbResidencial);
            this.Controls.Add(rbComercial);

            y += 70;

            // Número da instalação
            Label lblInstalacao = new Label { Text = "Nº Instalação:", Location = new Point(20, y), AutoSize = true };
            txtNumeroInstalacao = new TextBox { Location = new Point(180, y), Width = 220 };
            this.Controls.Add(lblInstalacao);
            this.Controls.Add(txtNumeroInstalacao);

            y += 30;

            // Leitura anterior
            Label lblLeituraAnt = new Label { Text = "Leitura Anterior (kWh):", Location = new Point(20, y), Width = 150, AutoSize = false };
            txtLeituraAnterior = new TextBox { Location = new Point(180, y), Width = 220 };
            this.Controls.Add(lblLeituraAnt);
            this.Controls.Add(txtLeituraAnterior);

            y += 30;

            // Leitura atual
            Label lblLeituraAtual = new Label { Text = "Leitura Atual (kWh):", Location = new Point(20, y), Width = 150, AutoSize = false };
            txtLeituraAtual = new TextBox { Location = new Point(180, y), Width = 220 };
            this.Controls.Add(lblLeituraAtual);
            this.Controls.Add(txtLeituraAtual);

            y += 40;

            // Botão Salvar
            btnSalvar = new Button { Text = "Salvar Conta", Location = new Point(180, y), Width = 120 };
            btnSalvar.Click += BtnSalvar_Click;
            this.Controls.Add(btnSalvar);
        }

        private void CarregarClientes()
        {
            cboCliente.Items.Clear();
            foreach (var pessoa in gerenciador.ListarPessoas())
            {
                cboCliente.Items.Add(new ClienteItem { Identificador = pessoa.Identificador, Nome = pessoa.Nome });
            }
        }

        private void BtnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboCliente.SelectedItem == null)
                {
                    MessageBox.Show("Selecione um cliente!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtNumeroInstalacao.Text) ||
                    string.IsNullOrWhiteSpace(txtLeituraAnterior.Text) ||
                    string.IsNullOrWhiteSpace(txtLeituraAtual.Text))
                {
                    MessageBox.Show("Preencha todos os campos!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var cliente = (ClienteItem)cboCliente.SelectedItem;

                Conta conta;
                if (rbResidencial.Checked)
                {
                    conta = new ContaResidencial();
                }
                else
                {
                    conta = new ContaComercial();
                }

                conta.NumeroInstalacao = txtNumeroInstalacao.Text;
                conta.LeituraMesAnterior = double.Parse(txtLeituraAnterior.Text);
                conta.LeituraMesAtual = double.Parse(txtLeituraAtual.Text);

                if (conta.LeituraMesAtual < conta.LeituraMesAnterior)
                {
                    MessageBox.Show("A leitura atual não pode ser menor que a anterior!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                gerenciador.AdicionarConta(cliente.Identificador, conta);
                MessageBox.Show("Conta cadastrada com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private class ClienteItem
        {
            public string Identificador { get; set; }
            public string Nome { get; set; }

            public override string ToString()
            {
                return $"{Nome} ({Identificador})";
            }
        }
    }
}