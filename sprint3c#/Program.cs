using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

class Program
{
    // Instância do serviço para interagir com o banco de dados
    private static PessoaService _pessoaService = new PessoaService();

    static void Main(string[] args)
    {
        // Garante que o banco de dados e as tabelas estejam criados antes de iniciar
        using (var context = new AppDbContext())
        {
            context.Database.Migrate();
        }

        bool sair = false;
        while (!sair)
        {
            ExibirMenu();
            string opcao = Console.ReadLine();

            switch (opcao)
            {
                case "1":
                    CadastrarPessoa();
                    break;
                case "2":
                    ListarPessoas();
                    break;
                case "3":
                    AtualizarPessoa();
                    break;
                case "4":
                    RemoverPessoa();
                    break;
                case "5":
                    ExportarDados();
                    break;
                case "6":
                    ImportarDados();
                    break;
                case "7":
                    sair = true;
                    break;
                default:
                    Console.WriteLine("Opção inválida. Tente novamente.");
                    break;
            }

            if (!sair)
            {
                Console.WriteLine("\nPressione qualquer tecla para continuar...");
                Console.ReadKey();
            }
        }
    }

    private static void ExibirMenu()
    {
        Console.Clear();
        Console.WriteLine("=================================================");
        Console.WriteLine("=== Sistema de Mitigação de Vício em Apostas ===");
        Console.WriteLine("=================================================");
        Console.WriteLine("1. Cadastrar nova pessoa");
        Console.WriteLine("2. Listar todas as pessoas");
        Console.WriteLine("3. Atualizar dados de uma pessoa");
        Console.WriteLine("4. Remover uma pessoa");
        Console.WriteLine("5. Exportar dados para JSON");
        Console.WriteLine("6. Importar dados de JSON");
        Console.WriteLine("7. Sair");
        Console.WriteLine("-------------------------------------------------");
        Console.Write("Escolha uma opção: ");
    }

    private static void CadastrarPessoa()
    {
        Console.WriteLine("\n--- Cadastro de Pessoa ---");
        Console.Write("Nome: ");
        string nome = Console.ReadLine();
        Console.Write("CPF: ");
        string cpf = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(nome) || string.IsNullOrWhiteSpace(cpf))
        {
            Console.WriteLine("Nome e CPF não podem ser vazios.");
            return;
        }

        var novaPessoa = new Pessoa { Nome = nome, CPF = cpf };
        _pessoaService.AdicionarPessoa(novaPessoa);
        Console.WriteLine("\nPessoa cadastrada com sucesso!");
    }

    private static void ListarPessoas()
    {
        Console.WriteLine("\n--- Pessoas Cadastradas ---");
        var pessoas = _pessoaService.ObterTodasPessoas();

        if (pessoas.Count == 0)
        {
            Console.WriteLine("Nenhuma pessoa cadastrada.");
            return;
        }

        foreach (var p in pessoas)
        {
            Console.WriteLine($"ID: {p.Id} | Nome: {p.Nome} | CPF: {p.CPF} | Cadastro: {p.DataCadastro.ToShortDateString()}");
        }
    }

    private static void AtualizarPessoa()
    {
        Console.WriteLine("\n--- Atualizar Pessoa ---");
        Console.Write("Digite o ID da pessoa que deseja atualizar: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("ID inválido.");
            return;
        }

        var pessoa = _pessoaService.ObterPessoaPorId(id);
        if (pessoa == null)
        {
            Console.WriteLine($"Pessoa com ID {id} não encontrada.");
            return;
        }

        Console.WriteLine($"Pessoa encontrada: {pessoa.Nome} | CPF: {pessoa.CPF}");
        Console.Write("Novo nome (deixe em branco para não alterar): ");
        string novoNome = Console.ReadLine();
        Console.Write("Novo CPF (deixe em branco para não alterar): ");
        string novoCpf = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(novoNome))
        {
            pessoa.Nome = novoNome;
        }

        if (!string.IsNullOrWhiteSpace(novoCpf))
        {
            pessoa.CPF = novoCpf;
        }

        _pessoaService.AtualizarPessoa(pessoa);
        Console.WriteLine("Pessoa atualizada com sucesso!");
    }

    private static void RemoverPessoa()
    {
        Console.WriteLine("\n--- Remover Pessoa ---");
        Console.Write("Digite o ID da pessoa que deseja remover: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("ID inválido.");
            return;
        }

        var pessoa = _pessoaService.ObterPessoaPorId(id);
        if (pessoa == null)
        {
            Console.WriteLine($"Pessoa com ID {id} não encontrada.");
            return;
        }

        _pessoaService.RemoverPessoa(id);
        Console.WriteLine($"Pessoa com ID {id} removida com sucesso!");
    }

    private static void ExportarDados()
    {
        Console.WriteLine("\n--- Exportar Dados para JSON ---");
        var pessoas = _pessoaService.ObterTodasPessoas();
        if (pessoas.Count == 0)
        {
            Console.WriteLine("Nenhum dado para exportar.");
            return;
        }
        FileService.ExportarParaJson(pessoas);
    }

    private static void ImportarDados()
    {
        Console.WriteLine("\n--- Importar Dados de JSON ---");
        var pessoasImportadas = FileService.ImportarDeJson();
        if (pessoasImportadas.Count == 0)
        {
            return;
        }

        Console.WriteLine($"Importando {pessoasImportadas.Count} pessoas...");
        foreach (var p in pessoasImportadas)
        {
            _pessoaService.AdicionarPessoa(p);
        }
        Console.WriteLine("Importação concluída com sucesso!");
    }
}