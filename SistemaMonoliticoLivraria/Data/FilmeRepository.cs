using MySqlConnector;
using SistemaMonoliticoLivraria.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace SistemaMonoliticoLivraria.Data
{
    internal class FilmeRepository
    {
        private readonly Database _database;

        public FilmeRepository(Database database)
        {
            _database = database;
        }

        public Filme CreateFilme(List<string> entradas)
        {
            Filme filme = new Filme(
                entradas[0],
                entradas[1],
                entradas[2],
                decimal.Parse(entradas[3], CultureInfo.InvariantCulture),
                int.Parse(entradas[4]));
            return filme;
        }
        public bool Insert(Filme filme)
        {
            const string sql =
                "INSERT INTO filmes" +
                "(Titulo, Genero, Diretor, Preco, QuantidadeEstoque)" +
                "VALUES (@Titulo, @Genero, @Diretor, @Preco, @QuantidadeEstoque);";
            using (MySqlConnection conexao = _database.ConexaoLocadora())
            {
                MySqlCommand comando = new MySqlCommand(sql, conexao);
                comando.Parameters.AddWithValue("@Titulo", filme.Titulo);
                comando.Parameters.AddWithValue("@Genero", filme.Genero);
                comando.Parameters.AddWithValue("@Diretor", filme.Diretor);
                comando.Parameters.AddWithValue("@Preco", filme.Preco);
                comando.Parameters.AddWithValue("@QuantidadeEstoque", filme.QuantidadeEstoque);
                int linhasAfetadas = comando.ExecuteNonQuery();
                return linhasAfetadas > 0;
            }
        }
        
        public bool InsertFromString(string filmeString)
        {
            string[] dados = filmeString.Split(';');
            if (dados.Length != 5)
            {
                return false;
            }
            Filme filme = new Filme(
                dados[0].Trim(),
                dados[1].Trim(),
                dados[2].Trim(),
                decimal.Parse(dados[3].Trim(), CultureInfo.InvariantCulture),
                int.Parse(dados[4].Trim()));
            
            return Insert(filme);
        }       
        public bool GeneralUpdate(Filme filme)
        {
            const string sql = "UPDATE filmes SET Titulo = @Titulo, Genero = @Genero, Diretor = @Diretor, Preco = @Preco, QuantidadeEstoque = @QuantidadeEstoque WHERE Id = @Id";
            using (MySqlConnection conexao = _database.ConexaoLocadora())
            {
                MySqlCommand comando = new MySqlCommand(sql, conexao);
                comando.Parameters.AddWithValue("@Titulo", filme.Titulo);
                comando.Parameters.AddWithValue("@Genero", filme.Genero);
                comando.Parameters.AddWithValue("@Diretor", filme.Diretor);
                comando.Parameters.AddWithValue("@Preco", filme.Preco);
                comando.Parameters.AddWithValue("@QuantidadeEstoque", filme.QuantidadeEstoque);
                comando.Parameters.AddWithValue("@Id", filme.Id);
                int linhasAfetadas = comando.ExecuteNonQuery();
                return linhasAfetadas > 0;
            }
        }

        public bool SetStockFilme(int id, int estoqueAtual)
        {
            const string sql =
                "UPDATE filmes " +
                "SET QuantidadeEstoque = @Quantidade " +
                "WHERE Id = @Id ";
            using (MySqlConnection conexao = _database.ConexaoLocadora())
            {
                MySqlCommand comando = new MySqlCommand(sql, conexao);
                comando.Parameters.AddWithValue("@Quantidade", estoqueAtual);
                comando.Parameters.AddWithValue("@Id", id);
                int linhasAfetadas = comando.ExecuteNonQuery();
                return linhasAfetadas > 0;
            }
        }
        
        public bool DecStock(Filme filme, int quantidade)
        {
            const string sql =
                "UPDATE filmes " +
                "SET QuantidadeEstoque = QuantidadeEstoque - @Quantidade " +
                "WHERE Id = @Id " +
                "AND QuantidadeEstoque >= @Quantidade";
            using (MySqlConnection conexao = _database.ConexaoLocadora())
            {
                MySqlCommand comando = new MySqlCommand(sql, conexao);
                comando.Parameters.AddWithValue("@Quantidade", quantidade);
                comando.Parameters.AddWithValue("@Id", filme.Id);
                int linhasAfetadas = comando.ExecuteNonQuery();
                return linhasAfetadas > 0;
            }
        }

        public bool AddStock(Filme filme, int quantidade)
        {
            const string sql =
                "UPDATE filmes " +
                "SET QuantidadeEstoque = QuantidadeEstoque + @Quantidade " +
                "WHERE Id = @Id ";
            using (MySqlConnection conexao = _database.ConexaoLocadora())
            {
                MySqlCommand comando = new MySqlCommand(sql, conexao);
                comando.Parameters.AddWithValue("@Quantidade", quantidade);
                comando.Parameters.AddWithValue("@Id", filme.Id);
                int linhasAfetadas = comando.ExecuteNonQuery();
                return linhasAfetadas > 0;
            }
        }

        public bool Delete(int id)
        {
            const string sql = "DELETE FROM filmes WHERE Id = @Id";
            using (MySqlConnection conexao = _database.ConexaoLocadora())
            {
                MySqlCommand comando = new MySqlCommand(sql, conexao);
                comando.Parameters.AddWithValue("@Id", id);
                int linhasAfetadas = comando.ExecuteNonQuery();
                return linhasAfetadas > 0;
            }
        }

        public Filme GetById(int id)
        {
            const string sql = "SELECT * FROM filmes WHERE Id = @Id";
            using (MySqlConnection conexao = _database.ConexaoLocadora())
            {
                MySqlCommand comando = new MySqlCommand(sql, conexao);
                comando.Parameters.AddWithValue("@Id", id);
                using (MySqlDataReader reader = comando.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Filme(reader.GetInt32("Id"), reader.GetString("Titulo"), reader.GetString("Genero"), reader.GetString("Diretor"), reader.GetDecimal("Preco"), reader.GetInt32("QuantidadeEstoque"));
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        public Filme GetByTitle(string title)
        {
            const string sql = "SELECT * FROM filmes WHERE Titulo = @Titulo";
            using (MySqlConnection conexao = _database.ConexaoLocadora())
            {
                MySqlCommand comando = new MySqlCommand(sql, conexao);
                comando.Parameters.AddWithValue("@Titulo", title);
                using (MySqlDataReader reader = comando.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Filme(reader.GetInt32("Id"), reader.GetString("Titulo"), reader.GetString("Genero"), reader.GetString("Diretor"), reader.GetDecimal("Preco"), reader.GetInt32("QuantidadeEstoque"));
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        public List<Filme> GetAll()
        {
            const string sql = "SELECT * FROM filmes";
            using (MySqlConnection conexao = _database.ConexaoLocadora())
            {
                MySqlCommand comando = new MySqlCommand(sql, conexao);
                using (MySqlDataReader reader = comando.ExecuteReader())
                {
                    List<Filme> filmes = new List<Filme>();
                    while (reader.Read())
                    {
                        Filme filme = new Filme (
                            reader.GetInt32("Id"), 
                            reader.GetString("Titulo"), 
                            reader.GetString("Genero"), 
                            reader.GetString("Diretor"), 
                            reader.GetDecimal("Preco"), 
                            reader.GetInt32("QuantidadeEstoque"));
                        filmes.Add(filme);
                    }
                    return filmes;
                }
            }
        }
    }
}
