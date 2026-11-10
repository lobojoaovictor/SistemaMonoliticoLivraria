using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;


namespace SistemaMonoliticoLivraria
{
    internal class Program
    {
        static void Main(string[] args)
        {

            List<Venda> listaDeVendas = new List<Venda>();
            List<Filme> listaDeFilmes = new List<Filme>();
            List<Livro> listaDeLivros = new List<Livro>();

            Venda venda = new Venda();
            

            CarregarDados(listaDeFilmes, listaDeLivros, listaDeVendas);

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //                                        CAMADA DE INTERFACE DO USUÁRIO                                          ///
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            int opcao;

            do
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("===============================================================");
                Console.WriteLine("=                    Bem-vindo ao PlotSys!                    =");
                Console.WriteLine("===============================================================");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("= 1 - Cadastrar Produto                                       =");
                Console.WriteLine("= 2 - Realizar Venda                                          =");
                Console.WriteLine("= 3 - Consultar / Excluir Produto                             =");
                Console.WriteLine("= 4 - Imprimir Estoque                                        =");
                Console.WriteLine("= 5 - Imprimir Relatório de Vendas                            =");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("= 0 - Sair                                                    =");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("===============================================================");
                Console.WriteLine("= Digite a opção desejada:                                    =");
                Console.WriteLine("===============================================================");

                try
                {
                    opcao = int.Parse(Console.ReadLine());
                }
                catch (FormatException)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Opção inválida. Tente novamente.");
                    Console.WriteLine("===============================================================");
                    Console.ResetColor();
                    opcao = -1; // Define uma opção inválida para continuar o loop
                }
                switch (opcao)
                {
                    case 1:
                        CadastrarProduto(listaDeFilmes, listaDeLivros);
                        break;
                    case 2:
                        RealizarVenda(venda, listaDeVendas, listaDeFilmes, listaDeLivros);
                        break;
                    case 3:
                        ConsultarExcluirProduto(listaDeFilmes, listaDeLivros);
                        break;
                    case 4:
                        ImprimirEstoque(listaDeFilmes, listaDeLivros);
                        break;
                    case 5:
                        ImprimirRelatorioVendas(listaDeVendas);
                        break;
                    case 0:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Saindo do sistema...");
                        Console.WriteLine("===============================================================");
                        Console.ResetColor();
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Opção inválida. Tente novamente.");
                        Console.WriteLine("===============================================================");
                        Console.ResetColor();
                        break;
                }
                if (opcao != 0)
                {
                    Console.WriteLine("\nPressione qualquer tecla para voltar ao menu...");
                    Console.ReadKey();
                }
            } while (opcao != 0);
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///                                             CAMADA DE DADOS                                                   ///
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        struct Venda
        {
            public int Id;
            public string Produto;
            public string TipoProduto;
            public double Preco;
            public int QuantidadeVendida;
            public double TotalVenda;

            public Venda(string produto, string tipoProduto, double preco, int quantidadeVendida)
            {
                Id = 0;
                Produto = produto;
                TipoProduto = tipoProduto;
                Preco = preco;
                QuantidadeVendida = quantidadeVendida;
                TotalVenda = preco * quantidadeVendida;
            }

            public string InsertVenda()
            {
                string sql =
                    "INSERT INTO livro " +
                    "(Produto, TipoProduto, Preco, QuantidadeVendida, TotalVenda)" +
                    "VALUES (@Produto, @TipoProduto, @Preco, @QuantidadeVendida, @TotalVenda);";

                return sql;
            }           

            public void ExibirInformacoesVenda()
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("===============================================================");
                Console.WriteLine($"Produto: {Produto}");
                Console.WriteLine($"Tipo de Produto: {TipoProduto}");
                Console.WriteLine($"Preço: {Preco:C}");
                Console.WriteLine($"Quantidade Vendida: {QuantidadeVendida}");
                Console.WriteLine($"Total da Venda: {TotalVenda:C}");
                Console.WriteLine("===============================================================");
            }
        }

        struct Filme
        {
            public int Id;
            public string Titulo;
            public string Genero;
            public string Diretor;
            public double Preco;
            public int QuantidadeEstoque;

            public Filme(string titulo, string genero, string diretor, double preco, int quantidadeEstoque)
            {
                Id = 0;
                Titulo = titulo;
                Genero = genero;
                Diretor = diretor;
                Preco = preco;
                QuantidadeEstoque = quantidadeEstoque;

            }

            public void AlterarEstoque(int valor)
            {
                QuantidadeEstoque -= valor;
            }

            public string InsertFilme()
            {
                string sql =
                    "INSERT INTO filme" +
                    "(Titulo, Genero, Diretor, Preco, QuantidadeEstoque)" +
                    "VALUES (@Titulo, @Genero, @Diretor, @Preco, @QuantidadeEstoque);";

                return sql;
            }

            public string UpdateFilme()
            {
                string sql = "UPDATE filmes SET Titulo = @Titulo, Genero = @Genero, Diretor = @Diretor, Preco = @Preco, QuantidadeEstoque = @QuantidadeEstoque WHERE Id = @Id";
                return sql;
            }

            public string UpdateStockFilme()
            {
                string sql =
                    "UPDATE filmes " +
                    "SET QuantidadeEstoque = QuantidadeEstoque - @Quantidade " +
                    "WHERE Id = @Id" +
                    "AND QuantidadeEstoque >= @Quantidade";
                return sql;
                
            }


            public void ExibirTodasInformacoes()
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"Título: {Titulo}");
                Console.WriteLine($"Gênero: {Genero}");
                Console.WriteLine($"Diretor: {Diretor}");
                Console.WriteLine($"Preço: {Preco:C}");
                Console.WriteLine($"Quantidade em Estoque: {QuantidadeEstoque}");
            }

            public void ExibirEstoque()
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("===============================================================");
                Console.WriteLine($"Título: {Titulo}");
                Console.WriteLine($"Quantidade em Estoque: {QuantidadeEstoque}");
                Console.WriteLine("===============================================================");
            }

        }

        struct Livro
        {
            public int Id;
            public string Titulo;
            public string Genero;
            public string Autor;
            public double Preco;
            public int QuantidadeEstoque;

            public void AlterarEstoque(int valor)
            {
                QuantidadeEstoque -= valor;
            }

            public string InsertLivro()
            {
                string sql =
                    "INSERT INTO Livros " +
                    "(Titulo, Genero, Autor, Preco, QuantidadeEstoque)" +
                    "VALUES (@Titulo, @Genero, @Autor, @Preco, @QuantidadeEstoque);";

                return sql;
            }

            public string UpdateLivro()
            {
                string sql = "UPDATE livros SET Titulo = @Titulo, Genero = @Genero, Autor = @Autor, Preco = @Preco, QuantidadeEstoque = @QuantidadeEstoque WHERE Id = @Id";
                return sql;
            }
            public string UpdateStockLivro()
            {
                string sql =
                    "UPDATE livros " +
                    "SET QuantidadeEstoque = QuantidadeEstoque - @Quantidade " +
                    "WHERE Id = @Id" +
                    "AND QuantidadeEstoque >= @Quantidade";
                return sql;

            }

            public void ExibirTodasInformacoes()
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"Título: {Titulo}");
                Console.WriteLine($"Gênero: {Genero}");
                Console.WriteLine($"Autor: {Autor}");
                Console.WriteLine($"Preço: {Preco:C}");
                Console.WriteLine($"Quantidade em Estoque: {QuantidadeEstoque}");
            }

            public void ExibirEstoque()
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("===============================================================");
                Console.WriteLine($"Título: {Titulo}");
                Console.WriteLine($"Quantidade em Estoque: {QuantidadeEstoque}");
                Console.WriteLine("===============================================================");
            }
        }



        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///                                         CAMADA DE LÓGICA DE NEGÓCIO                                           ///
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        static void CadastrarProduto(List<Filme> listaDeFilmes, List<Livro> listaDeLivros)
        {
            Filme filme = new Filme();
            Livro livro = new Livro();

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("===============================================================");
            Console.WriteLine("=                     Cadastrar Produto                       =");
            Console.WriteLine("===============================================================");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Digite 1 para cadastrar um livro ou 2 para cadastrar um filme: ");
            Console.WriteLine("===============================================================");

            int opcao;

            try
            {
                opcao = int.Parse(Console.ReadLine());
                if (opcao != 1 && opcao != 2)
                {
                    throw new FormatException();
                }
            }
            catch (FormatException)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Opção inválida. Tente novamente.");
                Console.WriteLine("===============================================================");
                Console.ResetColor();
                return; // Retorna para o menu principal
            }

            if (opcao == 1)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("===============================================================");
                Console.WriteLine("=                    Cadastrar Livro                          =");
                Console.WriteLine("===============================================================");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("Digite o título do livro: ");
                livro.Titulo = Console.ReadLine();
                Console.Write("Digite o gênero do livro: ");
                livro.Genero = Console.ReadLine();
                Console.Write("Digite o autor do livro: ");
                livro.Autor = Console.ReadLine();
                Console.Write("Digite o preço do livro: ");
                try
                {
                    livro.Preco = double.Parse(Console.ReadLine());
                    if (livro.Preco < 0)
                    {
                        throw new FormatException();
                    }
                } catch (FormatException)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Não é possível cadastrar preço negativo. Tente novamente.");
                    Console.WriteLine("===============================================================");
                    Console.ResetColor();
                    return;
                }
                Console.Write("Digite a quantidade em estoque do livro: ");
                try
                {
                    livro.QuantidadeEstoque = Convert.ToInt32(Console.ReadLine());
                    if (livro.QuantidadeEstoque < 0)
                    {
                        throw new FormatException();
                    }
                } catch (FormatException)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Não é possível inserir estoque negativo. Tente novamente.");
                    Console.WriteLine("===============================================================");
                    Console.ResetColor();
                    return;
                }
                Console.WriteLine("===============================================================");
                                
                InserirProduto(livro);
                listaDeLivros.Add(livro);
            }
            else
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("===============================================================");
                Console.WriteLine("=                     Cadastrar Filme                         =");
                Console.WriteLine("===============================================================");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("Digite o título do filme: ");
                filme.Titulo = Console.ReadLine();
                Console.Write("Digite o gênero do filme: ");
                filme.Genero = Console.ReadLine();
                Console.Write("Digite o diretor do filme: ");
                filme.Diretor = Console.ReadLine();
                Console.Write("Digite o preço do filme: ");
                filme.Preco = double.Parse(Console.ReadLine());
                Console.Write("Digite a quantidade em estoque do filme: ");
                filme.QuantidadeEstoque = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("===============================================================");
                                
                InserirProduto(filme);
                listaDeFilmes.Add(filme);
            }
        }



        static void RealizarVenda(Venda venda, List<Venda> listaDeVendas, List<Filme> listaDeFilmes, List<Livro> listaDeLivros)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("===============================================================");
            Console.WriteLine("=                     Realizar Venda                          =");
            Console.WriteLine("===============================================================");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Digite 1 para vender um livro ou 2 para vender um filme: ");
            Console.WriteLine("===============================================================");

            if (Console.ReadLine() == "1")
            {
                Console.WriteLine("Digite o título do livro a ser vendido. ");
                Console.Write("Título: ");
                string tituloVenda = Console.ReadLine().ToLower();

                Console.WriteLine("===============================================================");

                for (int i = 0; i < listaDeLivros.Count; i++)
                {
                    if (listaDeLivros[i].Titulo.ToLower() == tituloVenda.ToLower())
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("INFORMAÇÕES DO LIVRO:");
                        Console.ForegroundColor = ConsoleColor.White;
                        listaDeLivros[i].ExibirTodasInformacoes();

                        Console.WriteLine("===============================================================");
                        Console.WriteLine("Confirmar venda? (1 - SIM, 2 - NÃO)");
                        Console.WriteLine("===============================================================");

                        if (Console.ReadLine() == "1")
                        {
                            Console.WriteLine("Digite a quantidade de unidades a serem vendidas: ");
                            int quantidadeVendas = int.Parse(Console.ReadLine());

                            if (quantidadeVendas <= listaDeLivros[i].QuantidadeEstoque)
                            {
                                listaDeLivros[i].AlterarEstoque(quantidadeVendas);
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Quantidade insuficiente em estoque.");
                                Console.WriteLine("===============================================================");
                                return;
                            }

                            venda.Produto = listaDeLivros[i].Titulo;
                            venda.TipoProduto = "Livro";
                            venda.Preco = listaDeLivros[i].Preco;
                            venda.QuantidadeVendida = quantidadeVendas;
                            venda.TotalVenda = venda.Preco * venda.QuantidadeVendida;

                            Livro livro = listaDeLivros[i];

                            VenderProduto(livro.Id, quantidadeVendas, livro);
                            InserirVenda(venda);
                            listaDeVendas.Add(venda);
                            
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("VENDA CANCELADA.");
                            Console.WriteLine("===============================================================");
                        }
                    }
                }
            }
            else
            {

                Console.WriteLine("Digite o título do filme a ser vendido. ");
                Console.Write("Título: ");
                string tituloVenda = Console.ReadLine().ToLower();

                Console.WriteLine("===============================================================");

                for (int i = 0; i < listaDeFilmes.Count; i++)
                {
                    if (listaDeFilmes[i].Titulo.ToLower() == tituloVenda.ToLower())
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("INFORMAÇÕES DO FILME:");
                        Console.ForegroundColor = ConsoleColor.White;
                        listaDeFilmes[i].ExibirTodasInformacoes();

                        Console.WriteLine("===============================================================");
                        Console.WriteLine("Confirmar venda? (1 - SIM, 2 - NÃO)");
                        Console.WriteLine("===============================================================");

                        if (Console.ReadLine() == "1")
                        {
                            Console.WriteLine("digite a quantidade de unidades a serem vendidas: ");
                            int quantidadeVendas = int.Parse(Console.ReadLine());

                            if (quantidadeVendas <= listaDeFilmes[i].QuantidadeEstoque)
                            {
                                listaDeFilmes[i].AlterarEstoque(quantidadeVendas);
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Quantidade insuficiente em estoque.");
                                Console.WriteLine("===============================================================");
                                return;
                            }

                            venda.Produto = listaDeFilmes[i].Titulo;
                            venda.TipoProduto = "Filme";
                            venda.Preco = listaDeFilmes[i].Preco;
                            venda.QuantidadeVendida = quantidadeVendas;
                            venda.TotalVenda = venda.Preco * venda.QuantidadeVendida;


                            Filme filme = listaDeFilmes[i];

                            VenderProduto(filme.Id, quantidadeVendas, filme);
                            InserirVenda(venda);
                            listaDeVendas.Add(venda);
                            
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("VENDA CANCELADA.");
                            Console.WriteLine("===============================================================");
                        }
                    }
                }
            }
        }


        static void ConsultarExcluirProduto(List<Filme> listaDeFilmes, List<Livro> listaDeLivros)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("===============================================================");
            Console.WriteLine("=                     CONSULTAR/EXCLUIR PRODUTO               =");
            Console.WriteLine("===============================================================");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Digite 1 para consultar/excluir um livro" +
                            "\nDigite 2 para consultar/excluir um filme: ");
            Console.WriteLine("===============================================================");

            if (Console.ReadLine() == "1")
            {
                bool livroEncontrado = false;

                Console.WriteLine("Digite o título do livro a ser consultado/excluído: ");
                Console.Write("Título: ");
                string tituloConsulta = Console.ReadLine().ToLower();
                Console.WriteLine("===============================================================");

                for (int i = 0; i < listaDeLivros.Count; i++)
                {
                    if (listaDeLivros[i].Titulo.ToLower() == tituloConsulta.ToLower())
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("INFORMAÇÕES DO LIVRO:");
                        Console.ForegroundColor = ConsoleColor.White;
                        listaDeLivros[i].ExibirTodasInformacoes();
                        Console.WriteLine("===============================================================");
                        Console.WriteLine("Deseja excluir este livro? (1 - SIM, 2 - NÃO)");
                        Console.WriteLine("===============================================================");
                        if (Console.ReadLine() == "1")
                        {
                            ExcluirProduto("livros", listaDeLivros[i].Id);
                            listaDeLivros.RemoveAt(i);
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("LIVRO EXCLUÍDO COM SUCESSO.");
                            Console.WriteLine("===============================================================");
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("EXCLUSÃO CANCELADA.");
                            Console.WriteLine("===============================================================");
                        }

                        livroEncontrado = true;
                    }
                    else
                    {
                        livroEncontrado = false;
                    }

                }

                if (!livroEncontrado)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("LIVRO NÃO ENCONTRADO.");
                    Console.WriteLine("===============================================================");
                }

            }
            else
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Digite o título do filme a ser consultado/excluído: ");
                Console.Write("Título: ");
                string tituloConsulta = Console.ReadLine();
                Console.WriteLine("===============================================================");

                bool filmeEncontrado = false;

                for (int i = 0; i < listaDeFilmes.Count; i++)
                {
                    if (listaDeFilmes[i].Titulo.ToLower() == tituloConsulta.ToLower())
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("INFORMAÇÕES DO FILME:");
                        Console.ForegroundColor = ConsoleColor.White;
                        listaDeFilmes[i].ExibirTodasInformacoes();
                        Console.WriteLine("===============================================================");
                        Console.WriteLine("Deseja excluir este filme? (1 - SIM, 2 - NÃO)");
                        Console.WriteLine("===============================================================");
                        if (Console.ReadLine() == "1")
                        {
                            ExcluirProduto("filmes", listaDeFilmes[i].Id);
                            listaDeFilmes.RemoveAt(i);
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("FILME EXCLUÍDO COM SUCESSO.");
                            Console.WriteLine("===============================================================");
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("EXCLUSÃO CANCELADA.");
                            Console.WriteLine("===============================================================");
                        }

                        filmeEncontrado = true;
                    }
                    else
                    {
                        filmeEncontrado = false;
                    }
                }

                if (!filmeEncontrado)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("FILME NÃO ENCONTRADO.");
                    Console.WriteLine("===============================================================");
                }
            }
        }

        static void ImprimirEstoque(List<Filme> listaDeFilmes, List<Livro> listaDeLivros)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("===============================================================");
            Console.WriteLine("=                     Imprimir Estoque                        =");
            Console.WriteLine("===============================================================");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Estoque de filmes: ");
            foreach (var filmes in listaDeFilmes)
            {
                filmes.ExibirEstoque();
            }

            Console.WriteLine("===============================================================");
            Console.WriteLine("Estoque de livros: ");
            foreach (var livros in listaDeLivros)
            {
                livros.ExibirEstoque();
            }

            Console.WriteLine("===============================================================");

        }

        static void ImprimirRelatorioVendas(List<Venda> listaDeVendas)
        {
            foreach (var vendas in listaDeVendas)
            {
                vendas.ExibirInformacoesVenda();
            }
        }


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///                                             CAMADA DE PERSISTÊNCIA                                            ///
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        static MySqlConnection Conexao()
        {
            MySqlConnection conexao = new MySqlConnection("Server=localhost;Port=3306;Database=locadora;User ID=root;Password=;");
            conexao.Open();

            return conexao;
        }      

        static void CarregarDados(List<Filme> listaDeFilmes, List<Livro> listaDeLivros, List<Venda> listaDeVendas)
        {
            CarregarFilmes(listaDeFilmes);
            CarregarLivros(listaDeLivros);
            CarregarVendas(listaDeVendas);
        }

        static void CarregarFilmes(List<Filme> listaDeFilmes)
        {
            MySqlConnection conexao = Conexao();

            string comandoExec = "SELECT * FROM filmes";
            MySqlCommand comando = new MySqlCommand(comandoExec, conexao);

            MySqlDataReader leitor = comando.ExecuteReader();

            while (leitor.Read())
            {
                Filme filme = new Filme();

                filme.Id = leitor.GetInt32("Id");
                filme.Titulo = leitor.GetString("Titulo");
                filme.Genero = leitor.GetString("Genero");
                filme.Diretor = leitor.GetString("Diretor");
                filme.Preco = leitor.GetDouble("Preco");
                filme.QuantidadeEstoque = leitor.GetInt32("QuantidadeEstoque");

                listaDeFilmes.Add(filme);
            }

            conexao.Close();
        }

        static void CarregarLivros(List<Livro> listaDeLivros)
        {
            MySqlConnection conexao = Conexao();

            string comandoExec = "SELECT * FROM livros";
            MySqlCommand comando = new MySqlCommand(comandoExec, conexao);

            MySqlDataReader leitor = comando.ExecuteReader();
            
            while (leitor.Read())
            {
                Livro livro = new Livro();

                livro.Id = leitor.GetInt32("Id");
                livro.Titulo = leitor.GetString("Titulo");
                livro.Genero = leitor.GetString("Genero");
                livro.Autor = leitor.GetString("Autor");
                livro.Preco = leitor.GetDouble("Preco");
                livro.QuantidadeEstoque = leitor.GetInt32("QuantidadeEstoque");

                listaDeLivros.Add(livro);
            }
            conexao.Close();
        }

        static void CarregarVendas(List<Venda> listaDeVendas)
        {
            MySqlConnection conexao = Conexao();

            string comandoExec = "SELECT * FROM vendas ";
            MySqlCommand comando = new MySqlCommand(comandoExec, conexao);

            MySqlDataReader leitor = comando.ExecuteReader();

            while (leitor.Read())
            {
                Venda vendas = new Venda();

                vendas.Id = leitor.GetInt32("Id");
                vendas.Produto = leitor.GetString("Produto");
                vendas.TipoProduto = leitor.GetString("TipoProduto");
                vendas.Preco = leitor.GetDouble("Preco");
                vendas.QuantidadeVendida = leitor.GetInt32("QuantidadeVendida");
                vendas.TotalVenda = leitor.GetInt32("TotalVenda");

                listaDeVendas.Add(vendas);
            }
            conexao.Close();
        }

        static bool ExcluirProduto(string tabela, int id)
        {
            string sql = $"DELETE FROM {tabela} WHERE Id = @Id";

            MySqlConnection conexao = Conexao();
          
            MySqlCommand comando = new MySqlCommand(sql, conexao);
            comando.Parameters.AddWithValue("@Id", id);

            int linhasAfetadas = comando.ExecuteNonQuery();

            conexao.Close();
            return linhasAfetadas > 0;
        }

        static bool InserirProduto(Filme filme)
        {           
            MySqlConnection conexao = Conexao();


            MySqlCommand comando = new MySqlCommand(filme.InsertFilme(), conexao);

            comando.Parameters.AddWithValue("@Titulo", filme.Titulo);
            comando.Parameters.AddWithValue("@Genero", filme.Genero);
            comando.Parameters.AddWithValue("@Diretor", filme.Diretor);
            comando.Parameters.AddWithValue("@Preco", filme.Preco);
            comando.Parameters.AddWithValue("@QuantidadeEstoque", filme.QuantidadeEstoque);

            int linhasAfetadas = comando.ExecuteNonQuery();
            bool resultBool = linhasAfetadas > 0;

            conexao.Close();
            return resultBool;
        }

        static bool InserirProduto(Livro livro)
        {
            MySqlConnection conexao = Conexao();

            MySqlCommand comando = new MySqlCommand(livro.InsertLivro(), conexao);

            comando.Parameters.AddWithValue("@Titulo", livro.Titulo);
            comando.Parameters.AddWithValue("@Genero", livro.Genero);
            comando.Parameters.AddWithValue("@Autor", livro.Autor);
            comando.Parameters.AddWithValue("@Preco", livro.Preco);
            comando.Parameters.AddWithValue("@QuantidadeEstoque", livro.QuantidadeEstoque);

            int linhasAfetadas = comando.ExecuteNonQuery();
            bool resultBool = linhasAfetadas > 0;

            conexao.Close();
            return resultBool;
        }
        static bool InserirVenda(Venda venda) 
        {

            MySqlConnection conexao = Conexao();

            MySqlCommand comando = new MySqlCommand(venda.InsertVenda(), conexao);

            comando.Parameters.AddWithValue("@Produto", venda.Produto);
            comando.Parameters.AddWithValue("@TipoProduto", venda.TipoProduto);
            comando.Parameters.AddWithValue("@Preco", venda.Preco);
            comando.Parameters.AddWithValue("@QuantidadeVendida", venda.QuantidadeVendida);
            comando.Parameters.AddWithValue("@TotalVenda", venda.TotalVenda);

            int linhasAfetadas = comando.ExecuteNonQuery();
            bool resultBool = linhasAfetadas > 0;

            conexao.Close();
            return resultBool;
        }        
        
        static void EditarProduto(int id, Filme filme)
        {
            MySqlConnection conexao = Conexao();
            
                MySqlCommand comando = new MySqlCommand(filme.UpdateFilme(), conexao);

                comando.Parameters.AddWithValue("@Titulo", filme.Titulo);
                comando.Parameters.AddWithValue("@Genero", filme.Genero);
                comando.Parameters.AddWithValue("@Diretor", filme.Diretor);
                comando.Parameters.AddWithValue("@Preco", filme.Preco);
                comando.Parameters.AddWithValue("@QuantidadeEstoque", filme.QuantidadeEstoque);
                comando.Parameters.AddWithValue("@Id", id);
                comando.ExecuteNonQuery();

            conexao.Close();
        }

        static void EditarProduto(int id, Livro livro)
        {
            MySqlConnection conexao = Conexao();
            
                MySqlCommand comando = new MySqlCommand(livro.UpdateLivro(), conexao);

                comando.Parameters.AddWithValue("@Titulo", livro.Titulo);
                comando.Parameters.AddWithValue("@Genero", livro.Genero);
                comando.Parameters.AddWithValue("@Autor", livro.Autor);
                comando.Parameters.AddWithValue("@Preco", livro.Preco);
                comando.Parameters.AddWithValue("@QuantidadeEstoque", livro.QuantidadeEstoque);
                comando.Parameters.AddWithValue("@Id", id);
                comando.ExecuteNonQuery();

            conexao.Close();
        }

        static bool VenderProduto(int id, int quantidade, Filme filme)
        {
            MySqlConnection conexao = Conexao();
            MySqlCommand comando = new MySqlCommand(filme.UpdateStockFilme(), conexao);

            comando.Parameters.AddWithValue("@Quantidade", quantidade);
            comando.Parameters.AddWithValue("@Id", id);

            int linhasAfetadas = comando.ExecuteNonQuery();
            bool resultBool = linhasAfetadas > 0;

            conexao.Close();
            return resultBool;
        }

        static bool VenderProduto(int id, int quantidade, Livro livro)
        {
            MySqlConnection conexao = Conexao();
            MySqlCommand comando = new MySqlCommand(livro.UpdateStockLivro(), conexao);

            comando.Parameters.AddWithValue("@Quantidade", quantidade);
            comando.Parameters.AddWithValue("@Id", id);

            int linhasAfetadas = comando.ExecuteNonQuery();
            bool resultBool = linhasAfetadas > 0;

            conexao.Close();
            return resultBool;
        }                    
    }
}
