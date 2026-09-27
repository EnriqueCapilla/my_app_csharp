using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace MyFirstConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=========================================");
            Console.WriteLine(" Criando o banco de dados SQLite... ");
            Console.WriteLine("=========================================");

            using (var db = new MeuBancoContext())
            {
                // Este comando cria o arquivo .db e as tabelas na hora!
                db.Database.EnsureCreated();

                // Insere um dado de teste se o banco estiver vazio
                if (!db.Usuarios.Any())
                {
                    db.Usuarios.Add(new Usuario { Nome = "Enrique", Email = "enrique@teste.com" });
                    db.Usuarios.Add(new Usuario { Nome = "Amigo do Deploy", Email = "amigo@teste.com" });
                    db.SaveChanges();
                    Console.WriteLine("👉 Dados de teste salvos com sucesso!");
                }
            }

            Console.WriteLine("\n✅ Banco pronto! Procure o arquivo 'meubanco.db' na barra lateral.");
        }
    }

    // Definição da tabela
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
    }

    // Configuração do SQLite
    public class MeuBancoContext : DbContext
    {
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=meubanco.db");
    }
}

