using System;
using System.Windows.Forms;
using System.Drawing;

namespace ControleContas // Notei no seu print que o projeto chama ControleContas
{
    public partial class FormPrincipal : Form
    {
        private GerenciadorContas gerenciador;

        public FormPrincipal()
        {
            InitializeComponent(); // O VS precisa disso!
            gerenciador = new GerenciadorContas();
            ConfigurarInterface(); // Nossa configuração manual
        }

        private void ConfigurarInterface()
        {
            this.Text = "Sistema de Controle de Contas de Energia";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Menu
            MenuStrip menu = new MenuStrip();

            ToolStripMenuItem menuCadastro = new ToolStripMenuItem("Cadastro");
            // Aqui chamamos os métodos que estavam faltando antes
            menuCadastro.DropDownItems.Add("Cadastrar Cliente", null, (s, e) => AbrirCadastroCliente());
            menuCadastro.DropDownItems.Add("Cadastrar Conta", null, (s, e) => AbrirCadastroConta());

            ToolStripMenuItem menuConsulta = new ToolStripMenuItem("Consulta");
            menuConsulta.DropDownItems.Add("Consultar Contas", null, (s, e) => AbrirConsultaContas());

            menu.Items.Add(menuCadastro);
            menu.Items.Add(menuConsulta);

            this.MainMenuStrip = menu;
            this.Controls.Add(menu);

            // Label de boas-vindas
            Label lblBemVindo = new Label();
            lblBemVindo.Text = "Bem-vindo ao Sistema de Controle de Contas de Energia\n\n" +
                              "Use o menu acima para cadastrar clientes e contas\n" +
                              "ou consultar informações sobre consumo.";
            lblBemVindo.Font = new Font("Arial", 12, FontStyle.Regular);
            lblBemVindo.AutoSize = false;
            // AQUI ESTAVA O ERRO DE DIGITAÇÃO (MiddleCente -> MiddleCenter)
            lblBemVindo.TextAlign = ContentAlignment.MiddleCenter;
            lblBemVindo.Dock = DockStyle.Fill;
            this.Controls.Add(lblBemVindo);
        }

        // Estes métodos precisavam existir para o menu funcionar
        private void AbrirCadastroCliente()
        {
            FormCadastroCliente form = new FormCadastroCliente(gerenciador);
            form.ShowDialog();
        }

        private void AbrirCadastroConta()
        {
            FormCadastroConta form = new FormCadastroConta(gerenciador);
            form.ShowDialog();
        }

        private void AbrirConsultaContas()
        {
            FormConsultaContas form = new FormConsultaContas(gerenciador);
            form.ShowDialog();
        }
    }
}