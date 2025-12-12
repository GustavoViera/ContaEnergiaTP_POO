using System;
using System.Windows.Forms;
using System.Drawing;
using System.Linq;

namespace ControleContas
{
    public class FormConsultaContas : Form
    {
        private GerenciadorContas gerenciador;
        private ComboBox cboCliente;
        private ListBox lstContas;
        private Label lblConsumo;
        private Label lblValorTotal;
        private Label lblValorSemImpostos;
        private Button btnConsultar;

        public FormConsultaContas(GerenciadorContas gerenciador)
        {
            this.gerenciador = gerenciador;
            ConfigurarInterface();
            CarregarClientes();
        }

        private void ConfigurarInterface()
        {
            this.Text = "Consulta de Contas";
            this.Size = new Size(600, 500);
            this.StartPosition = FormStartPosition.CenterScreen;

            int y = 20;

            // Cliente
            Label lblCliente = new Label { Text = "Selecione o Cliente:", Location = new Point(20, y), AutoSize = true };
            this.Controls.Add(lblCliente);

            y += 25;

            cboCliente = new ComboBox { Location = new Point(20, y), Width = 400, DropDownStyle = ComboBoxStyle.DropDownList };
            this.Controls.Add(cboCliente);

            btnConsultar = new Button { Text = "Consultar", Location = new Point(430, y), Width = 120 };
            btnConsultar.Click += BtnConsultar_Click;
            this.Controls.Add(btnConsultar);

            y += 40;

            // Lista de contas
            Label lblContas = new Label { Text = "Contas do Cliente:", Location = new Point(20, y), AutoSize = true };
            this.Controls.Add(lblContas);

            y += 25;

            lstContas = new ListBox { Location = new Point(20, y), Size = new Size(530, 120) };
            lstContas.SelectedIndexChanged += LstContas_SelectedIndexChanged;
            this.Controls.Add(lstContas);

            y += 140;

            // Informações da conta selecionada
            Label lblInfo = new Label { Text = "Informações da Conta Selecionada:", Location = new Point(20, y), AutoSize = true, Font = new Font("Arial", 10, FontStyle.Bold) };
            this.Controls.Add(lblInfo);

            y += 30;

            lblConsumo = new Label { Text = "Consumo do mês: -", Location = new Point(40, y), AutoSize = true };
            this.Controls.Add(lblConsumo);

            y += 30;

            lblValorSemImpostos = new Label { Text = "Valor sem impostos: -", Location = new Point(40, y), AutoSize = true };
            this.Controls.Add(lblValorSemImpostos);

            y += 30;

            lblValorTotal = new Label { Text = "Valor total da conta: -", Location = new Point(40, y), AutoSize = true };
            this.Controls.Add(lblValorTotal);
        }

        private void CarregarClientes()
        {
            cboCliente.Items.Clear();
            foreach (var pessoa in gerenciador.ListarPessoas())
            {
                cboCliente.Items.Add(new ClienteItem { Identificador = pessoa.Identificador, Nome = pessoa.Nome });
            }
        }

        private void BtnConsultar_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboCliente.SelectedItem == null)
                {
                    MessageBox.Show("Selecione um cliente!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var cliente = (ClienteItem)cboCliente.SelectedItem;
                var pessoa = gerenciador.BuscarPessoa(cliente.Identificador);

                lstContas.Items.Clear();
                LimparInformacoes();

                if (pessoa.Contas.Count == 0)
                {
                    MessageBox.Show("Este cliente não possui contas cadastradas.", "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                foreach (var conta in pessoa.Contas)
                {
                    lstContas.Items.Add(conta);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LstContas_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstContas.SelectedItem != null)
            {
                var conta = (Conta)lstContas.SelectedItem;
                ExibirInformacoesConta(conta);
            }
        }

        private void ExibirInformacoesConta(Conta conta)
        {
            double consumo = conta.CalcularConsumo();
            double valorSemImpostos = conta.CalcularValorSemImpostos();
            double valorTotal = conta.CalcularValorTotal();

            lblConsumo.Text = $"Consumo do mês: {consumo:F2} kWh";
            lblValorSemImpostos.Text = $"Valor sem impostos: R$ {valorSemImpostos:F2}";
            lblValorTotal.Text = $"Valor total da conta: R$ {valorTotal:F2}";
        }

        private void LimparInformacoes()
        {
            lblConsumo.Text = "Consumo do mês: -";
            lblValorSemImpostos.Text = "Valor sem impostos: -";
            lblValorTotal.Text = "Valor total da conta: -";
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