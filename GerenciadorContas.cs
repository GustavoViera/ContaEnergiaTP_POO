// GerenciadorContas.cs
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ControleContas
{
    public class GerenciadorContas
    {
        private List<Pessoa> pessoas;
        private const string ARQUIVO_DADOS = "dados_contas.json";

        public GerenciadorContas()
        {
            pessoas = new List<Pessoa>();
            CarregarDados();
        }

        public void AdicionarPessoa(Pessoa pessoa)
        {
            if (pessoas.Any(p => p.Identificador == pessoa.Identificador))
            {
                throw new Exception("Já existe uma pessoa cadastrada com este identificador!");
            }
            pessoas.Add(pessoa);
            SalvarDados();
        }

        public Pessoa BuscarPessoa(string identificador)
        {
            return pessoas.FirstOrDefault(p => p.Identificador == identificador);
        }

        public List<Pessoa> ListarPessoas()
        {
            return pessoas;
        }

        public void AdicionarConta(string identificador, Conta conta)
        {
            var pessoa = BuscarPessoa(identificador);
            if (pessoa == null)
            {
                throw new Exception("Pessoa não encontrada!");
            }

            if (pessoa.Contas.Any(c => c.NumeroInstalacao == conta.NumeroInstalacao))
            {
                throw new Exception("Já existe uma conta com este número de instalação!");
            }

            conta.IdentificadorCliente = identificador;
            pessoa.AdicionarConta(conta);
            SalvarDados();
        }

        public void SalvarDados()
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Converters = { new PessoaConverter(), new ContaConverter() }
                };

                string json = JsonSerializer.Serialize(pessoas, options);
                File.WriteAllText(ARQUIVO_DADOS, json);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao salvar dados: {ex.Message}");
            }
        }

        public void CarregarDados()
        {
            try
            {
                if (File.Exists(ARQUIVO_DADOS))
                {
                    string json = File.ReadAllText(ARQUIVO_DADOS);
                    var options = new JsonSerializerOptions
                    {
                        Converters = { new PessoaConverter(), new ContaConverter() }
                    };
                    pessoas = JsonSerializer.Deserialize<List<Pessoa>>(json, options) ?? new List<Pessoa>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao carregar dados: {ex.Message}");
            }
        }
    }

    // Converters para JSON
    public class PessoaConverter : JsonConverter<Pessoa>
    {
        public override Pessoa Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
            {
                var root = doc.RootElement;
                string tipo = root.GetProperty("Tipo").GetString();

                Pessoa pessoa = tipo == "PessoaFisica" ? new PessoaFisica() : new PessoaJuridica();
                pessoa.Identificador = root.GetProperty("Identificador").GetString();
                pessoa.Nome = root.GetProperty("Nome").GetString();

                if (root.TryGetProperty("Contas", out JsonElement contasElement))
                {
                    var contaConverter = new ContaConverter();
                    foreach (var contaJson in contasElement.EnumerateArray())
                    {
                        var contaReader = new Utf8JsonReader(JsonSerializer.SerializeToUtf8Bytes(contaJson));
                        var conta = contaConverter.Read(ref contaReader, typeof(Conta), options);
                        pessoa.Contas.Add(conta);
                    }
                }

                return pessoa;
            }
        }

        public override void Write(Utf8JsonWriter writer, Pessoa value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString("Tipo", value.GetType().Name);
            writer.WriteString("Identificador", value.Identificador);
            writer.WriteString("Nome", value.Nome);

            writer.WriteStartArray("Contas");
            var contaConverter = new ContaConverter();
            foreach (var conta in value.Contas)
            {
                contaConverter.Write(writer, conta, options);
            }
            writer.WriteEndArray();

            writer.WriteEndObject();
        }
    }

    public class ContaConverter : JsonConverter<Conta>
    {
        public override Conta Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
            {
                var root = doc.RootElement;
                string tipo = root.GetProperty("Tipo").GetString();

                Conta conta = tipo == "ContaResidencial" ? new ContaResidencial() : new ContaComercial();
                conta.NumeroInstalacao = root.GetProperty("NumeroInstalacao").GetString();
                conta.LeituraMesAtual = root.GetProperty("LeituraMesAtual").GetDouble();
                conta.LeituraMesAnterior = root.GetProperty("LeituraMesAnterior").GetDouble();
                conta.IdentificadorCliente = root.GetProperty("IdentificadorCliente").GetString();

                return conta;
            }
        }

        public override void Write(Utf8JsonWriter writer, Conta value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString("Tipo", value.GetType().Name);
            writer.WriteString("NumeroInstalacao", value.NumeroInstalacao);
            writer.WriteNumber("LeituraMesAtual", value.LeituraMesAtual);
            writer.WriteNumber("LeituraMesAnterior", value.LeituraMesAnterior);
            writer.WriteString("IdentificadorCliente", value.IdentificadorCliente);
            writer.WriteEndObject();
        }
    }
}