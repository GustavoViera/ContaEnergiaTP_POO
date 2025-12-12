// Conta.cs
using System;

namespace ControleContas
{
    public abstract class Conta
    {
        public string NumeroInstalacao { get; set; }
        public double LeituraMesAtual { get; set; }
        public double LeituraMesAnterior { get; set; }
        public string IdentificadorCliente { get; set; }

        protected const double CONTRIBUICAO_ILUMINACAO = 9.25;

        public double CalcularConsumo()
        {
            return LeituraMesAtual - LeituraMesAnterior;
        }

        public abstract double ObterTarifa();
        public abstract double ObterImposto();
        public abstract string TipoConta();

        public double CalcularValorSemImpostos()
        {
            double consumo = CalcularConsumo();
            double valorConsumo = consumo * ObterTarifa();
            return valorConsumo + CONTRIBUICAO_ILUMINACAO;
        }

        public double CalcularValorTotal()
        {
            double valorSemImpostos = CalcularValorSemImpostos();
            double imposto = valorSemImpostos * ObterImposto();
            return valorSemImpostos + imposto;
        }

        public override string ToString()
        {
            return $"Instalação: {NumeroInstalacao} | Tipo: {TipoConta()} | Consumo: {CalcularConsumo():F2} kWh";
        }
    }

    public class ContaResidencial : Conta
    {
        private const double TARIFA = 0.40;
        private const double IMPOSTO = 0.30;

        public override double ObterTarifa()
        {
            return TARIFA;
        }

        public override double ObterImposto()
        {
            return IMPOSTO;
        }

        public override string TipoConta()
        {
            return "Residencial";
        }
    }

    public class ContaComercial : Conta
    {
        private const double TARIFA = 0.35;
        private const double IMPOSTO = 0.18;

        public override double ObterTarifa()
        {
            return TARIFA;
        }

        public override double ObterImposto()
        {
            return IMPOSTO;
        }

        public override string TipoConta()
        {
            return "Comercial";
        }
    }
}