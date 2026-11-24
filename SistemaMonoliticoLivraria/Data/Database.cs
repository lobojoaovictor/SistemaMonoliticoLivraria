using MySqlConnector;
using SistemaMonoliticoLivraria.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaMonoliticoLivraria.Data
{
    internal class Database
    {
        private readonly string _connectionStringServidor;
        private readonly string _connectionStringLocadora;

        public Database(string connectionStringServidor, string connectionStringLocadora)
        {
            _connectionStringServidor = connectionStringServidor;
            _connectionStringLocadora = connectionStringLocadora;
        }

        public MySqlConnection ConexaoServidor()
        {
            MySqlConnection conexao = new MySqlConnection(_connectionStringServidor);
            conexao.Open();
            return conexao;
        }

        public MySqlConnection ConexaoLocadora()
        {
            MySqlConnection conexao = new MySqlConnection(_connectionStringLocadora);
            conexao.Open();
            return conexao;
        }

        public void Initialize()
        {
            InitializeDatabase();
            InitializeTables();
            InicializeDados();
        }

        private void InitializeDatabase()
        {
            using (MySqlConnection conexao = ConexaoServidor())
            {
                MySqlCommand comando = new MySqlCommand("CREATE DATABASE IF NOT EXISTS locadora;", conexao);
                comando.ExecuteNonQuery();
            }
        }

        private void InitializeTables()
        {
            string sqlTabelas = @"
                CREATE TABLE IF NOT EXISTS filmes (
                    Id INT AUTO_INCREMENT PRIMARY KEY,
                    Titulo VARCHAR(150) NOT NULL,
                    Genero VARCHAR(100) NOT NULL,
                    Diretor VARCHAR(150) NOT NULL,
                    Preco DECIMAL(10,2) NOT NULL,
                    QuantidadeEstoque INT NOT NULL
                    );

                CREATE TABLE IF NOT EXISTS livros (
                    Id INT AUTO_INCREMENT PRIMARY KEY,
                    Titulo VARCHAR(150) NOT NULL,
                    Genero VARCHAR(100) NOT NULL,
                    Autor VARCHAR(150) NOT NULL,
                    Preco DECIMAL(10,2) NOT NULL,
                    QuantidadeEstoque INT NOT NULL
                    );

                CREATE TABLE IF NOT EXISTS vendas (
                    Id INT AUTO_INCREMENT PRIMARY KEY,
                    Produto VARCHAR(150) NOT NULL,
                    TipoProduto VARCHAR(50) NOT NULL,                    
                    Preco DECIMAL(10,2) NOT NULL,
                    QuantidadeVendida INT NOT NULL,
                    TotalVenda DECIMAL(10,2) NOT NULL,
                    DataVenda DATE NOT NULL,
                    HoraVenda TIME NOT NULL,
                    IdProduto INT NOT NULL
                    )";

                       
            using (MySqlConnection conexao = ConexaoLocadora())
            {
                MySqlCommand comando = new MySqlCommand(sqlTabelas, conexao);
                try
                {
                    comando.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine($"Erro MySQL {ex.Number}: {ex.Message}");
                    throw;
                }
            }


        }
    
        private void InicializeDados()
        {
            string dadosVendasContent =
                "O Senhor dos Aneis A Sociedade do Anel;Filme; 34.90; 2; 2026-11-03; 14:32; 1" + Environment.NewLine +
                "Interestelar; Filme; 29.90; 1; 2026-11-03; 15:10; 2" + Environment.NewLine +
                "O Poderoso Chefao; Filme; 24.90; 3; 2026-11-04; 10:45; 3" + Environment.NewLine +
                "Dom Casmurro; Livro; 39.90; 2; 2026-11-04; 11:20; 1" + Environment.NewLine +
                "O Hobbit; Livro; 59.90; 1; 2026-11-05; 16:05; 2" + Environment.NewLine +
                "1984; Livro; 44.90; 3; 2026-11-06;	13:18; 3" + Environment.NewLine +
                "Toy Story; Filme; 19.90; 2; 2026-11-07; 17:42; 4" + Environment.NewLine +
                "Parasita; Filme; 29.90; 4; 2026-10-08; 09:10; 5" + Environment.NewLine +
                "Capitães de Areia; Livro; 29.90; 3; 2026-10-08; 08:07; 4" + Environment.NewLine +
                "Uma Breve Historia do Tempo;Livro; 69.90; 1; 2026-11-08; 09:55; 5" + Environment.NewLine;
            string dadosFilmesContent =
                "O Senhor dos Aneis A Sociedade do Anel;Fantasia; Peter Jackson; 34.90; 8" + Environment.NewLine +
                "Interestelar; Ficcao Cientifica; Christopher Nolan; 29.90; 6" + Environment.NewLine +
                "O Poderoso Chefao; Drama; Francis Ford Coppola; 24.90; 5" + Environment.NewLine +
                "Toy Story; Animacao; John Lasseter; 19.90; 7" + Environment.NewLine +
                "Parasita; Suspense; Bong Joon-ho; 29.90; 4" + Environment.NewLine;
            string dadosLivrosContent =
                "Dom Casmurro; Romance; Machado de Assis; 39.90; 10" + Environment.NewLine +
                "O Hobbit; Fantasia; J.R.R. Tolkien; 59.90; 5" + Environment.NewLine +
                "1984; Distopia; George Orwell; 44.90; 8" + Environment.NewLine +
                "Capitães da Areia; Aventura; Jorge Amado; 29.90; 12" + Environment.NewLine +
                "Uma Breve Historia do Tempo; Ciencia; Stephen Hawking; 69.90; 6" + Environment.NewLine;
                


            using (MySqlConnection conexao = ConexaoLocadora())
            {
                string sql = @"
                        SELECT
                            (SELECT COUNT(*) FROM filmes) +
                            (SELECT COUNT(*) FROM livros) +
                            (SELECT COUNT(*) FROM vendas) AS Total;
                    ";

                MySqlCommand comando = new MySqlCommand(sql, conexao);

                int total = Convert.ToInt32(comando.ExecuteScalar());

                if (total == 0)
                {
                    FilmeRepository filmeRepository = new FilmeRepository(this);
                    LivroRepository livroRepository = new LivroRepository(this);
                    VendaRepository vendaRepository = new VendaRepository(this);


                    foreach (var linha in dadosFilmesContent.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        filmeRepository.InsertFromString(linha);
                    }

                    foreach (var linha in dadosLivrosContent.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        livroRepository.InsertFromString(linha);
                    }

                    foreach (var linha in dadosVendasContent.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        vendaRepository.Insert(vendaRepository.CreateFromString(linha));
                    }


                }
            }
        }
    }
}
