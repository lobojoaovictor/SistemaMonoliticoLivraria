using MySqlConnector;
using SistemaMonoliticoLivraria.Models;
using System.Collections.Generic;
using System.Globalization;


namespace SistemaMonoliticoLivraria.Data
{
    internal class LivroRepository
    {
        private readonly Database _database;

        public LivroRepository(Database database)
        {
            _database = database;
        }        

        public Livro CreateLivro(List<string> entradas)
        {
            Livro livro = new Livro(
                entradas[0], 
                entradas[1], 
                entradas[2], 
                decimal.Parse(entradas[3], CultureInfo.InvariantCulture), 
                int.Parse(entradas[4]));
            return livro;
        }
        public bool Insert(Livro livro)
        {
            const string sql =
                "INSERT INTO livros" +
                "(Titulo, Genero, Autor, Preco, QuantidadeEstoque)" +
                "VALUES (@Titulo, @Genero, @Autor, @Preco, @QuantidadeEstoque);";
            using (MySqlConnection conexao = _database.ConexaoLocadora())
            {
                MySqlCommand comando = new MySqlCommand(sql, conexao);
                comando.Parameters.AddWithValue("@Titulo", livro.Titulo);
                comando.Parameters.AddWithValue("@Genero", livro.Genero);
                comando.Parameters.AddWithValue("@Autor", livro.Autor);
                comando.Parameters.AddWithValue("@Preco", livro.Preco);
                comando.Parameters.AddWithValue("@QuantidadeEstoque", livro.QuantidadeEstoque);
                int linhasAfetadas = comando.ExecuteNonQuery();
                return linhasAfetadas > 0;
            }
        }

        public bool InsertFromString(string livroString)
        {
            string[] dados = livroString.Split(';');
            if (dados.Length != 5)
            {
                return false;
            }
            Livro livro = new Livro(
                dados[0].Trim(),
                dados[1].Trim(),
                dados[2].Trim(),
                decimal.Parse(dados[3].Trim(), CultureInfo.InvariantCulture),
                int.Parse(dados[4].Trim()));

            return Insert(livro);
        }
        public bool SetStockLivro(int id, int estoqueAtual)
        {
            const string sql =
                "UPDATE livros " +
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
        public bool GeneralUpdate(Livro livro)
        {
            const string sql = "UPDATE livros SET Titulo = @Titulo, Genero = @Genero, Autor = @Autor, Preco = @Preco, QuantidadeEstoque = @QuantidadeEstoque WHERE Id = @Id";
            using (MySqlConnection conexao = _database.ConexaoLocadora())
            {
                MySqlCommand comando = new MySqlCommand(sql, conexao);
                comando.Parameters.AddWithValue("@Titulo", livro.Titulo);
                comando.Parameters.AddWithValue("@Genero", livro.Genero);
                comando.Parameters.AddWithValue("@Autor", livro.Autor);
                comando.Parameters.AddWithValue("@Preco", livro.Preco);
                comando.Parameters.AddWithValue("@QuantidadeEstoque", livro.QuantidadeEstoque);
                comando.Parameters.AddWithValue("@Id", livro.Id);
                int linhasAfetadas = comando.ExecuteNonQuery();
                return linhasAfetadas > 0;
            }
        }

        public bool DecStock(Livro livro, int quantidade)
        {
            const string sql =
                "UPDATE livros " +
                "SET QuantidadeEstoque = QuantidadeEstoque - @Quantidade " +
                "WHERE Id = @Id " +
                "AND QuantidadeEstoque >= @Quantidade";
            using (MySqlConnection conexao = _database.ConexaoLocadora())
            {
                MySqlCommand comando = new MySqlCommand(sql, conexao);
                comando.Parameters.AddWithValue("@Quantidade", quantidade);
                comando.Parameters.AddWithValue("@Id", livro.Id);
                int linhasAfetadas = comando.ExecuteNonQuery();
                return linhasAfetadas > 0;
            }
        }

        public bool AddStock(Livro livro, int quantidade)
        {
            const string sql =
                "UPDATE livros " +
                "SET QuantidadeEstoque = QuantidadeEstoque + @Quantidade " +
                "WHERE Id = @Id ";
            using (MySqlConnection conexao = _database.ConexaoLocadora())
            {
                MySqlCommand comando = new MySqlCommand(sql, conexao);
                comando.Parameters.AddWithValue("@Quantidade", quantidade);
                comando.Parameters.AddWithValue("@Id", livro.Id);
                int linhasAfetadas = comando.ExecuteNonQuery();
                return linhasAfetadas > 0;
            }
        }

        public Livro GetById(int id)
        {
            using (MySqlConnection conexao = _database.ConexaoLocadora())
            {
                MySqlCommand comando = new MySqlCommand("SELECT * FROM livros WHERE Id = @Id", conexao);
                comando.Parameters.AddWithValue("@Id", id);
                using (MySqlDataReader reader = comando.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Livro(
                            reader.GetInt32("Id"),
                            reader.GetString("Titulo"),
                            reader.GetString("Genero"),
                            reader.GetString("Autor"),
                            reader.GetDecimal("Preco"),
                            reader.GetInt32("QuantidadeEstoque")
                        );
                    }
                }
            }
            return null;
        }

        public Livro GetByTitle(string titulo)
        {
            using (MySqlConnection conexao = _database.ConexaoLocadora())
            {
                MySqlCommand comando = new MySqlCommand("SELECT * FROM livros WHERE Titulo = @Titulo", conexao);
                comando.Parameters.AddWithValue("@Titulo", titulo);
                using (MySqlDataReader reader = comando.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Livro(
                            reader.GetInt32("Id"),
                            reader.GetString("Titulo"),
                            reader.GetString("Genero"),
                            reader.GetString("Autor"),
                            reader.GetDecimal("Preco"),
                            reader.GetInt32("QuantidadeEstoque")
                        );
                    }
                }
            }
            return null;
        }

        public List<Livro> GetAll()
        {
            List<Livro> livros = new List<Livro>();
            using (MySqlConnection conexao = _database.ConexaoLocadora())
            {
                MySqlCommand comando = new MySqlCommand("SELECT * FROM livros", conexao);
                using (MySqlDataReader reader = comando.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Livro livro = new Livro(
                            reader.GetInt32("Id"),
                            reader.GetString("Titulo"),
                            reader.GetString("Genero"),
                            reader.GetString("Autor"),
                            reader.GetDecimal("Preco"),
                            reader.GetInt32("QuantidadeEstoque")
                        );
                        livros.Add(livro);
                    }
                }
            }
            return livros;
        }
        public bool Delete(int id)
        {
            const string sql = "DELETE FROM livros WHERE Id = @Id";
            using (MySqlConnection conexao = _database.ConexaoLocadora())
            {
                MySqlCommand comando = new MySqlCommand(sql, conexao);
                comando.Parameters.AddWithValue("@Id", id);
                int linhasAfetadas = comando.ExecuteNonQuery();
                return linhasAfetadas > 0;
            }
        }
    }
}
