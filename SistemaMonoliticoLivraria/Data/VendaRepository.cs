using MySqlConnector;
using SistemaMonoliticoLivraria.Models;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace SistemaMonoliticoLivraria.Data
{
    internal class VendaRepository
    {
        private readonly Database _database;

        public VendaRepository(Database database)
        {
            _database = database;
        }

        public bool Insert(Venda venda)
        {
            const string sql =
                 "INSERT INTO vendas" +
                 "(Produto, TipoProduto, Preco, QuantidadeVendida, TotalVenda, DataVenda, HoraVenda, IdProduto)" +
                 "VALUES (@Produto, @TipoProduto, @Preco, @QuantidadeVendida, @TotalVenda, @DataVenda, @HoraVenda, @IdProduto);";
            using (MySqlConnection conexao = _database.ConexaoLocadora())
            {
                MySqlCommand comando = new MySqlCommand(sql, conexao);
                comando.Parameters.AddWithValue("@Produto", venda.Produto);
                comando.Parameters.AddWithValue("@TipoProduto", venda.TipoProduto);
                comando.Parameters.AddWithValue("@Preco", venda.Preco);
                comando.Parameters.AddWithValue("@QuantidadeVendida", venda.QuantidadeVendida);
                comando.Parameters.AddWithValue("@TotalVenda", venda.TotalVenda);
                comando.Parameters.AddWithValue("@DataVenda", venda.DataVenda);
                comando.Parameters.AddWithValue("@HoraVenda", venda.HoraVenda);
                comando.Parameters.AddWithValue("@IdProduto", venda.IdProduto);

                int linhasAfetadas = comando.ExecuteNonQuery();
                return linhasAfetadas > 0;
            }

        }

        public Venda CreateFromString(string vendaString)
        {
            string[] dados = vendaString.Split(';');
            if (dados.Length != 7)
            {
                throw new ArgumentException("Número de dados incorreto", nameof(dados.Length));
            }
            Venda venda = new Venda(
                dados[0].Trim(),
                dados[1].Trim(),
                decimal.Parse(dados[2].Trim(), CultureInfo.InvariantCulture),
                int.Parse(dados[3].Trim()),
                DateTime.Parse(dados[4].Trim()),
                TimeSpan.Parse(dados[5].Trim()),
                int.Parse(dados[6].Trim()));
            return venda;
        }

        public bool Delete(Venda venda)
        {
            const string sql = "DELETE FROM vendas WHERE Id = @Id";
            using (MySqlConnection conexao = _database.ConexaoLocadora())
            {
                MySqlCommand comando = new MySqlCommand(sql, conexao);
                comando.Parameters.AddWithValue("@Id", venda.Id);
                int linhasAfetadas = comando.ExecuteNonQuery();
                return linhasAfetadas > 0;
            }
        }

        public List<Venda> GetAll()
        {
            const string sql = "SELECT * FROM vendas";
            using (MySqlConnection conexao = _database.ConexaoLocadora())
            {
                MySqlCommand comando = new MySqlCommand(sql, conexao);
                MySqlDataReader reader = comando.ExecuteReader();
                List<Venda> vendas = new List<Venda>();
                while (reader.Read())
                {
                    int id = reader.GetInt32("Id");
                    string produto = reader.GetString("Produto");
                    string tipoProduto = reader.GetString("TipoProduto");
                    decimal preco = reader.GetDecimal("Preco");
                    int quantidadeVendida = reader.GetInt32("QuantidadeVendida");
                    decimal totalVenda = reader.GetDecimal("TotalVenda");
                    DateTime dataVenda = reader.GetDateTime("DataVenda");
                    TimeSpan horaVenda = reader.GetTimeSpan("HoraVenda");
                    int idProduto = reader.IsDBNull(reader.GetOrdinal("IdProduto")) ? 0 : reader.GetInt32("IdProduto");
                    Venda venda = new Venda(id, produto, tipoProduto, preco, quantidadeVendida, totalVenda, dataVenda, horaVenda, idProduto);
                    vendas.Add(venda);
                }
                return vendas;
            }
        }

        public List<Venda> GetAllFilmes()
        {
            const string sql = "SELECT * FROM vendas WHERE TipoProduto = 'Filme'";
            using (MySqlConnection conexao = _database.ConexaoLocadora())
            {
                MySqlCommand comando = new MySqlCommand(sql, conexao);
                MySqlDataReader reader = comando.ExecuteReader();
                List<Venda> vendas = new List<Venda>();
                while (reader.Read())
                {
                    int id = reader.GetInt32("Id");
                    string produto = reader.GetString("Produto");
                    string tipoProduto = reader.GetString("TipoProduto");
                    decimal preco = reader.GetDecimal("Preco");
                    int quantidadeVendida = reader.GetInt32("QuantidadeVendida");
                    decimal totalVenda = reader.GetDecimal("TotalVenda");
                    DateTime dataVenda = reader.GetDateTime("DataVenda");
                    TimeSpan horaVenda = reader.GetTimeSpan("HoraVenda");
                    int idProduto = reader.IsDBNull(reader.GetOrdinal("IdProduto")) ? 0 : reader.GetInt32("IdProduto");
                    Venda venda = new Venda(id, produto, tipoProduto, preco, quantidadeVendida, totalVenda, dataVenda, horaVenda, idProduto);
                    vendas.Add(venda);
                }
                return vendas;
            }
        }

        public List<Venda> GetAllLivros()
        {
            const string sql = "SELECT * FROM vendas WHERE TipoProduto = 'Livro'";
            using (MySqlConnection conexao = _database.ConexaoLocadora())
            {
                MySqlCommand comando = new MySqlCommand(sql, conexao);
                MySqlDataReader reader = comando.ExecuteReader();
                List<Venda> vendas = new List<Venda>();
                while (reader.Read())
                {
                    int id = reader.GetInt32("Id");
                    string produto = reader.GetString("Produto");
                    string tipoProduto = reader.GetString("TipoProduto");
                    decimal preco = reader.GetDecimal("Preco");
                    int quantidadeVendida = reader.GetInt32("QuantidadeVendida");
                    decimal totalVenda = reader.GetDecimal("TotalVenda");
                    DateTime dataVenda = reader.GetDateTime("DataVenda");
                    TimeSpan horaVenda = reader.GetTimeSpan("HoraVenda");
                    int idProduto = reader.IsDBNull(reader.GetOrdinal("IdProduto")) ? 0 : reader.GetInt32("IdProduto");
                    Venda venda = new Venda(id, produto, tipoProduto, preco, quantidadeVendida, totalVenda, dataVenda, horaVenda, idProduto);
                    vendas.Add(venda);
                }
                return vendas;
            }
        }

        public Venda GetById(Venda venda)
        {
            const string sql = "SELECT * FROM vendas WHERE Id = @Id";
            using (MySqlConnection conexao = _database.ConexaoLocadora())
            {
                MySqlCommand comando = new MySqlCommand(sql, conexao);
                comando.Parameters.AddWithValue("@Id", venda.Id);
                using (MySqlDataReader reader = comando.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int id = reader.GetInt32("Id");
                        string produto = reader.GetString("Produto");
                        string tipoProduto = reader.GetString("TipoProduto");
                        decimal preco = reader.GetDecimal("Preco");
                        int quantidadeVendida = reader.GetInt32("QuantidadeVendida");
                        decimal totalVenda = reader.GetDecimal("TotalVenda");
                        DateTime dataVenda = reader.GetDateTime("DataVenda");
                        TimeSpan horaVenda = reader.GetTimeSpan("HoraVenda");
                        int idProduto = reader.IsDBNull(reader.GetOrdinal("IdProduto")) ? 0 : reader.GetInt32("IdProduto");
                        Venda vendaReturn = new Venda(id, produto, tipoProduto, preco, quantidadeVendida, totalVenda, dataVenda, horaVenda, idProduto);
                        return vendaReturn;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        public bool VerifyStockFilmes(Venda venda)
        {
            const string sql = "SELECT QuantidadeEstoque FROM filmes WHERE Id = @Id";
            using (MySqlConnection conexao = _database.ConexaoLocadora())
            {
                MySqlCommand comando = new MySqlCommand(sql, conexao);
                comando.Parameters.AddWithValue("@Id", venda.IdProduto);
                object result = comando.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    int quantidadeEstoque = Convert.ToInt32(result);
                    return quantidadeEstoque >= venda.QuantidadeVendida;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool VerifyStockLivros(Venda venda)
        {
            const string sql = "SELECT QuantidadeEstoque FROM livros WHERE Id = @Id";
            using (MySqlConnection conexao = _database.ConexaoLocadora())
            {
                MySqlCommand comando = new MySqlCommand(sql, conexao);
                comando.Parameters.AddWithValue("@Id", venda.IdProduto);
                object result = comando.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    int quantidadeEstoque = Convert.ToInt32(result);
                    return quantidadeEstoque >= venda.QuantidadeVendida;
                }
                else
                {
                    return false;
                }
            }
        }

        public List<Venda> GetAllComJoin()
{
    const string sql = @"
        SELECT
            v.Id,
            COALESCE(l.Titulo, f.Titulo, v.Produto) AS Produto,
            v.TipoProduto,
            v.Preco,
            v.QuantidadeVendida,
            v.TotalVenda,
            v.DataVenda,
            v.HoraVenda,
            v.IdProduto
        FROM vendas v
        LEFT JOIN livros l
            ON v.IdProduto = l.Id
            AND v.TipoProduto = 'Livro'
        LEFT JOIN filmes f
            ON v.IdProduto = f.Id
            AND v.TipoProduto = 'Filme'
        ORDER BY v.DataVenda DESC, v.HoraVenda DESC;";

    using (MySqlConnection conexao = _database.ConexaoLocadora())
    {
        MySqlCommand comando = new MySqlCommand(sql, conexao);

        using (MySqlDataReader reader = comando.ExecuteReader())
        {
            List<Venda> vendas = new List<Venda>();

            while (reader.Read())
            {
                int id = reader.GetInt32("Id");
                string produto = reader.GetString("Produto");
                string tipoProduto = reader.GetString("TipoProduto");
                decimal preco = reader.GetDecimal("Preco");
                int quantidadeVendida = reader.GetInt32("QuantidadeVendida");
                decimal totalVenda = reader.GetDecimal("TotalVenda");
                DateTime dataVenda = reader.GetDateTime("DataVenda");
                TimeSpan horaVenda = reader.GetTimeSpan("HoraVenda");
                int idProduto = reader.GetInt32("IdProduto");

                Venda venda = new Venda(
                    id,
                    produto,
                    tipoProduto,
                    preco,
                    quantidadeVendida,
                    totalVenda,
                    dataVenda,
                    horaVenda,
                    idProduto);

                vendas.Add(venda);
            }

            return vendas;
        }
    }
}


    }
}
