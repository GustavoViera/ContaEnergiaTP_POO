// Pessoa.cs
using System;
using System.Collections.Generic;

namespace ControleContas
{
    public abstract class Pessoa
    {
        public string Identificador { get; set; } // CPF ou CNPJ
        public string Nome { get; set; }
        public List<Conta> Contas { get; set; }

        public Pessoa()
        {
            Contas = new List<Conta>();
        }

        public void AdicionarConta(Conta conta)
        {
            Contas.Add(conta);
        }

        public abstract string TipoDocumento();
    }

    public class PessoaFisica : Pessoa
    {
        public string CPF
        {
            get => Identificador;
            set => Identificador = value;
        }

        public override string TipoDocumento()
        {
            return "CPF";
        }
    }

    public class PessoaJuridica : Pessoa
    {
        public string CNPJ
        {
            get => Identificador;
            set => Identificador = value;
        }
        public string RazaoSocial { get; set; }

        public override string TipoDocumento()
        {
            return "CNPJ";
        }
    }
}