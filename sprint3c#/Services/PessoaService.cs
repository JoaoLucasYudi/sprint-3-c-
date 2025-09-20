// Arquivo: Services/PessoaService.cs
using System;
using System.Collections.Generic;
using System.Linq;

public class PessoaService
{
    // CREATE
    public void AdicionarPessoa(Pessoa pessoa)
    {
        using (var context = new AppDbContext())
        {
            context.Pessoas.Add(pessoa);
            context.SaveChanges();
        }
    }

    // READ (Todos)
    public List<Pessoa> ObterTodasPessoas()
    {
        using (var context = new AppDbContext())
        {
            return context.Pessoas.ToList();
        }
    }

    // READ (Por Id) - O NOVO MÉTODO
    public Pessoa ObterPessoaPorId(int id)
    {
        using (var context = new AppDbContext())
        {
            return context.Pessoas.Find(id);
        }
    }

    // UPDATE
    public void AtualizarPessoa(Pessoa pessoa)
    {
        using (var context = new AppDbContext())
        {
            context.Pessoas.Update(pessoa);
            context.SaveChanges();
        }
    }

    // DELETE
    public void RemoverPessoa(int id)
    {
        using (var context = new AppDbContext())
        {
            var pessoa = context.Pessoas.Find(id);
            if (pessoa != null)
            {
                context.Pessoas.Remove(pessoa);
                context.SaveChanges();
            }
        }
    }
}