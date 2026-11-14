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
using File = System.IO.File;


namespace SistemaMonoliticoLivraria
{
    internal class Program
    {
        static void Main(string[] args)
        {

            CriarBancoETabelas();
            VerificarDadosIniciais();

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
                Console.WriteLine("= 4 - Imprimir Estoque / Adicionar Estoque                    =");
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
                        ImprimirAdicionarEstoque(listaDeFilmes, listaDeLivros);
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
            public DateTime DataVenda;
            public TimeSpan HoraVenda;

            public Venda(string produto, string tipoProduto, double preco, int quantidadeVendida)
            {
                Id = 0;                
                Produto = produto;
                TipoProduto = tipoProduto;
                Preco = preco;
                QuantidadeVendida = quantidadeVendida;
                TotalVenda = preco * quantidadeVendida;
                DataVenda = DateTime.Now;
                HoraVenda = DateTime.Now.TimeOfDay;
            }
            public Venda(string produto, string tipoProduto, double preco, int quantidadeVendida, double totalVenda, DateTime dataVenda, TimeSpan horaVenda)
            {
                Id = 0;
                Produto = produto;
                TipoProduto = tipoProduto;
                Preco = preco;
                QuantidadeVendida = quantidadeVendida;
                TotalVenda = totalVenda;
                DataVenda = dataVenda;
                HoraVenda = horaVenda;
            }

            public string InsertVenda()
            {
                string sql =
                    "INSERT INTO vendas " +
                    "(Produto, TipoProduto, Preco, QuantidadeVendida, TotalVenda, DataVenda, HoraVenda)" +
                    "VALUES (@Produto, @TipoProduto, @Preco, @QuantidadeVendida, @TotalVenda, @DataVenda, @HoraVenda);";

                return sql;
            }

            public void ExibirInformacoesVenda()
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("===============================================================");                             
                Console.WriteLine($"Produto: {Produto}");
                Console.WriteLine($"Tipo de Produto: {TipoProduto}");
                Console.WriteLine($"Preço: {Preco.ToString("C", CultureInfo.GetCultureInfo("pt-BR"))}");
                Console.WriteLine($"Quantidade Vendida: {QuantidadeVendida}");
                Console.WriteLine($"Total da Venda: {TotalVenda.ToString("C", CultureInfo.GetCultureInfo("pt-BR"))}");
                Console.WriteLine($"Data da Venda: {DataVenda:dd/MM/yyyy}");
                Console.WriteLine($"Hora da Venda: {HoraVenda:hh\\:mm\\:ss}");
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

            public void AdicionarEstoque(int valor)
            {
                QuantidadeEstoque += valor;
            }

            public string InsertFilme()
            {
                string sql =
                    "INSERT INTO filmes" +
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
                    "WHERE Id = @Id " +
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

            public Livro(string titulo, string genero, string autor, double preco, int quantidadeEstoque)
            {
                Id = 0;
                Titulo = titulo;
                Genero = genero;
                Autor = autor;
                Preco = preco;
                QuantidadeEstoque = quantidadeEstoque;

            }

            public void AlterarEstoque(int valor)
            {
                QuantidadeEstoque -= valor;
            }
            public void AdicionarEstoque(int valor)
            {
                QuantidadeEstoque += valor;
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
                    "WHERE Id = @Id " +
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
                Console.WriteLine("===============================================================");
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
                Console.Write("Digite o preço do livro: R$");
                try
                {
                    livro.Preco = double.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);
                    if (livro.Preco < 0)
                    {
                        throw new FormatException();
                    }
                }
                catch (FormatException)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("===============================================================");
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
                }
                catch (FormatException)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("===============================================================");
                    Console.WriteLine("Não é possível inserir estoque negativo. Tente novamente.");
                    Console.WriteLine("===============================================================");
                    Console.ResetColor();
                    return;
                }
                Console.WriteLine("===============================================================");

                InserirProduto(livro);
                listaDeLivros.Add(livro);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("===============================================================");
                Console.WriteLine("LIVRO CADASTRADO COM SUCESSO.");
                Console.WriteLine("===============================================================");
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
                Console.Write("Digite o preço do filme: R$");
                try
                {
                    filme.Preco = double.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);
                    if (filme.Preco < 0)
                    {
                        throw new FormatException();
                    }
                }
                catch (FormatException)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("===============================================================");
                    Console.WriteLine("Não é possível cadastrar preço negativo. Tente novamente.");
                    Console.WriteLine("===============================================================");
                    Console.ResetColor();
                    return;
                }
                Console.Write("Digite a quantidade em estoque do filme: ");
                try
                {
                    filme.QuantidadeEstoque = Convert.ToInt32(Console.ReadLine());
                    if (filme.QuantidadeEstoque < 0)
                    {
                        throw new FormatException();
                    }
                }
                catch (FormatException)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("===============================================================");
                    Console.WriteLine("Não é possível inserir estoque negativo. Tente novamente.");
                    Console.WriteLine("===============================================================");
                    Console.ResetColor();
                    return;
                }
                InserirProduto(filme);
                listaDeFilmes.Add(filme);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("===============================================================");
                Console.WriteLine("FILME CADASTRADO COM SUCESSO.");
                Console.WriteLine("===============================================================");
            }
        }



        static void RealizarVenda(Venda venda, List<Venda> listaDeVendas, List<Filme> listaDeFilmes, List<Livro> listaDeLivros)
        {
            int opcaoMenuPrincipalVenda;
            int opcaoMenuConfirmarVendaLivro;
            int opcaoMenuConfirmarVendaFilme;

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("===============================================================");
            Console.WriteLine("=                     Realizar Venda                          =");
            Console.WriteLine("===============================================================");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Digite 1 para vender um livro ou 2 para vender um filme: ");
            Console.WriteLine("===============================================================");

            try
            {
                opcaoMenuPrincipalVenda = int.Parse(Console.ReadLine());
                if (opcaoMenuPrincipalVenda != 1 && opcaoMenuPrincipalVenda != 2)
                {
                    throw new FormatException();
                }
            }
            catch (FormatException)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("===============================================================");
                Console.WriteLine("Opção inválida. Tente novamente.");
                Console.WriteLine("===============================================================");
                Console.ResetColor();
                return; // Retorna para o menu principal
            }

            if (opcaoMenuPrincipalVenda == 1)
            {
                Console.WriteLine("===============================================================");
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
                        try
                        {
                            opcaoMenuConfirmarVendaLivro = int.Parse(Console.ReadLine());
                            if (opcaoMenuConfirmarVendaLivro != 1 && opcaoMenuConfirmarVendaLivro != 2)
                            {
                                throw new FormatException();
                            }
                        }
                        catch (FormatException)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("===============================================================");
                            Console.WriteLine("Opção inválida. Tente novamente.");
                            Console.WriteLine("===============================================================");
                            Console.ResetColor();
                            return;
                        }

                        if (opcaoMenuConfirmarVendaLivro == 1)
                        {
                            Console.WriteLine("===============================================================");
                            Console.WriteLine("Digite a quantidade de unidades a serem vendidas: ");
                            Console.Write("Quantidade: ");
                            int quantidadeVendas = int.Parse(Console.ReadLine());

                            if (quantidadeVendas <= listaDeLivros[i].QuantidadeEstoque)
                            {
                                listaDeLivros[i].AlterarEstoque(quantidadeVendas);
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("===============================================================");
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
                            listaDeLivros[i].AlterarEstoque(quantidadeVendas);

                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("===============================================================");
                            Console.WriteLine("VENDA REALIZADA COM SUCESSO.");
                            Console.WriteLine("===============================================================");

                            CarregarDados(listaDeFilmes, listaDeLivros, listaDeVendas);
                            break;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("===============================================================");
                            Console.WriteLine("VENDA CANCELADA.");
                            Console.WriteLine("===============================================================");
                        }
                    }
                }
       
            }
            else
            {
                Console.WriteLine("===============================================================");
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

                        try
                        {
                            opcaoMenuConfirmarVendaFilme = int.Parse(Console.ReadLine());
                            if (opcaoMenuConfirmarVendaFilme != 1 && opcaoMenuConfirmarVendaFilme != 2)
                            {
                                throw new FormatException();
                            }

                        }
                        catch (FormatException)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("===============================================================");
                            Console.WriteLine("Opção inválida. Tente novamente.");
                            Console.WriteLine("===============================================================");
                            Console.ResetColor();
                            return;
                        }
                        if (opcaoMenuConfirmarVendaFilme == 1)
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
                                Console.WriteLine("===============================================================");
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
                            listaDeFilmes[i].AlterarEstoque(quantidadeVendas);
                            
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("===============================================================");
                            Console.WriteLine("VENDA REALIZADA COM SUCESSO.");
                            Console.WriteLine("===============================================================");

                            CarregarDados(listaDeFilmes, listaDeLivros, listaDeVendas);
                            break;

                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("===============================================================");
                            Console.WriteLine("VENDA CANCELADA.");
                            Console.WriteLine("===============================================================");
                        }
                        
                    }
                }
                
            }
        }

        static void ConsultarExcluirProduto(List<Filme> listaDeFilmes, List<Livro> listaDeLivros)
        {
            int opcaoMenuPrincipalConsulta;
            int opcaoMenuConfirmarExclusaoLivro;
            int opcaoMenuConfirmarExclusaoFilme;

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("===============================================================");
            Console.WriteLine("=                     CONSULTAR/EXCLUIR PRODUTO               =");
            Console.WriteLine("===============================================================");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Digite 1 para consultar/excluir um livro" +
                            "\nDigite 2 para consultar/excluir um filme: ");
            Console.WriteLine("===============================================================");

            try
            {
                opcaoMenuPrincipalConsulta = int.Parse(Console.ReadLine());
                if (opcaoMenuPrincipalConsulta != 1 && opcaoMenuPrincipalConsulta != 2)
                {
                    throw new FormatException();
                }

            }
            catch (FormatException)
            {
                Console.WriteLine("===============================================================");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Opção inválida. Tente novamente.");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("===============================================================");
                Console.ResetColor();
                return; 
            }

            if (opcaoMenuPrincipalConsulta == 1)
            {
                bool livroEncontrado = false;

                Console.Clear();
                Console.WriteLine("===============================================================");
                Console.WriteLine("Digite o título do livro a ser consultado/excluído: ");
                Console.Write("Título: ");
                string tituloConsulta = Console.ReadLine().ToLower();
                Console.WriteLine("===============================================================");

                for (int i = 0; i < listaDeLivros.Count; i++)
                {
                    if (listaDeLivros[i].Titulo.ToLower() == tituloConsulta.ToLower())
                    {
                        livroEncontrado = true;
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("INFORMAÇÕES DO LIVRO:");
                        Console.ForegroundColor = ConsoleColor.White;
                        listaDeLivros[i].ExibirTodasInformacoes();
                        Console.WriteLine("===============================================================");
                        Console.WriteLine("Deseja excluir este livro? (1 - SIM, 2 - NÃO)");
                        Console.WriteLine("===============================================================");

                        try
                        {
                            opcaoMenuConfirmarExclusaoLivro = int.Parse(Console.ReadLine());
                            if (opcaoMenuConfirmarExclusaoLivro != 1 && opcaoMenuConfirmarExclusaoLivro != 2)
                            {
                                throw new FormatException();
                            }
                        }
                        catch (FormatException)
                        {
                            Console.WriteLine("===============================================================");
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Opção inválida. Tente novamente.");
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.WriteLine("===============================================================");
                            Console.ResetColor();
                            return;
                        }

                        if (opcaoMenuConfirmarExclusaoLivro == 1)
                        {
                            ExcluirProduto("livros", listaDeLivros[i].Id);
                            listaDeLivros.RemoveAt(i);
                            Console.WriteLine("===============================================================");
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("LIVRO EXCLUÍDO COM SUCESSO.");
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.WriteLine("===============================================================");
                        }
                        else
                        {
                            Console.WriteLine("===============================================================");
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("EXCLUSÃO CANCELADA.");
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.WriteLine("===============================================================");
                        }

                        break;
                    }                  

                }

                if (!livroEncontrado)
                {
                    Console.WriteLine("===============================================================");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("LIVRO NÃO ENCONTRADO.");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("===============================================================");
                }

            }
            else
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("===============================================================");
                Console.WriteLine("Digite o título do filme a ser consultado/excluído: ");
                Console.Write("Título: ");
                string tituloConsulta = Console.ReadLine();
                Console.WriteLine("===============================================================");

                bool filmeEncontrado = false;

                for (int i = 0; i < listaDeFilmes.Count; i++)
                {
                    if (listaDeFilmes[i].Titulo.ToLower() == tituloConsulta.ToLower())
                    {
                        filmeEncontrado = true;
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("INFORMAÇÕES DO FILME:");
                        Console.ForegroundColor = ConsoleColor.White;
                        listaDeFilmes[i].ExibirTodasInformacoes();
                        Console.WriteLine("===============================================================");
                        Console.WriteLine("Deseja excluir este filme? (1 - SIM, 2 - NÃO)");
                        Console.WriteLine("===============================================================");

                        try
                        {
                            opcaoMenuConfirmarExclusaoFilme = int.Parse(Console.ReadLine());
                            if (opcaoMenuConfirmarExclusaoFilme != 1 && opcaoMenuConfirmarExclusaoFilme != 2)
                            {
                                throw new FormatException();
                            }
                        }
                        catch (FormatException)
                        {
                            Console.WriteLine("===============================================================");
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Opção inválida. Tente novamente.");
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.WriteLine("===============================================================");
                            Console.ResetColor();
                            return;
                        }

                        if (opcaoMenuConfirmarExclusaoFilme == 1)
                        {
                            ExcluirProduto("filmes", listaDeFilmes[i].Id);
                            listaDeFilmes.RemoveAt(i);
                            Console.WriteLine("===============================================================");
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("FILME EXCLUÍDO COM SUCESSO.");
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.WriteLine("===============================================================");
                        }
                        else
                        {
                            Console.WriteLine("===============================================================");
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("EXCLUSÃO CANCELADA.");
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.WriteLine("===============================================================");
                        }                                             

                        break;
                    }                    
                }

                if (!filmeEncontrado)
                {
                    Console.WriteLine("===============================================================");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("FILME NÃO ENCONTRADO.");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("===============================================================");
                }
            }
        }

            static void ImprimirAdicionarEstoque(List<Filme> listaDeFilmes, List<Livro> listaDeLivros)
            {
            int opcaoMenuPrincipal;
            int opcaoMenuEstoque;
            

                Console.Clear();
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("================================================================");
                Console.WriteLine("=                 Imprimir / Adicionar Estoque                 =");
                Console.WriteLine("================================================================");
                Console.WriteLine("Digite 1 para imprimir o estoque ou 2 para adicionar ao estoque:");
                try
                {
                    opcaoMenuPrincipal = int.Parse(Console.ReadLine());
                    if (opcaoMenuPrincipal != 1 && opcaoMenuPrincipal != 2)
                    {
                        throw new FormatException();
                    }

                }
                catch (FormatException)
                {
                    Console.WriteLine("===============================================================");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Opção inválida. Tente novamente.");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("===============================================================");
                    Console.ResetColor();
                    return;
                }

            if (opcaoMenuPrincipal == 1)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Estoque de filmes: ");
                Console.ForegroundColor = ConsoleColor.Cyan;
                foreach (var filmes in listaDeFilmes)
                {
                    filmes.ExibirEstoque();
                }

                Console.WriteLine("===============================================================");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Estoque de livros: ");
                Console.ForegroundColor = ConsoleColor.White;
                foreach (var livros in listaDeLivros)
                {
                    livros.ExibirEstoque();
                }

                Console.WriteLine("===============================================================");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("===============================================================");
                Console.WriteLine("Digite 1 para alterar estoque de um livro" +
                                "\nDigite 2 para alterar estoque de um filme: ");
                Console.WriteLine("===============================================================");

                try
                {
                    opcaoMenuEstoque = int.Parse(Console.ReadLine());
                    if (opcaoMenuEstoque != 1 && opcaoMenuEstoque != 2)
                    {
                        throw new FormatException();
                    }

                }
                catch (FormatException)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("===============================================================");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Opção inválida. Tente novamente.");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("===============================================================");
                    Console.ResetColor();
                    return;
                }


                if (opcaoMenuEstoque == 1)
                {
                    string tituloConsulta;
                    bool livroEncontrado = false;
                    int valorAdicional;

                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("===============================================================");
                    Console.Write("Digite o nome do livro: ");
                    tituloConsulta = Console.ReadLine();

                    for (int i = 0; i < listaDeLivros.Count; i++)
                    {
                        if (listaDeLivros[i].Titulo.ToLower() == tituloConsulta.ToLower())
                        {
                            livroEncontrado = true;
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.WriteLine("INFORMAÇÕES DO LIVRO:");
                            Console.ForegroundColor = ConsoleColor.White;
                            listaDeLivros[i].ExibirTodasInformacoes();
                            
                            Console.WriteLine("===============================================================");
                            Console.Write("Digite o valor a adicionar no estoque: ");
                            
                            try
                            {
                                valorAdicional = int.Parse(Console.ReadLine());
                                if (valorAdicional < 0)
                                {
                                    new FormatException();
                                }
                            }
                            catch (FormatException)
                            {
                                Console.ForegroundColor = ConsoleColor.Cyan;
                                Console.WriteLine("===============================================================");
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Não é possível adicionar valor menor que zero ao estoque!");
                                Console.ForegroundColor = ConsoleColor.Cyan;
                                Console.WriteLine("===============================================================");
                                Console.ResetColor();
                                return;
                            }

                            listaDeLivros[i].AdicionarEstoque(valorAdicional);
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("===============================================================");
                            Console.WriteLine("Estoque alterado com sucesso!");
                            Console.WriteLine("===============================================================");

                            break;
                        }

                    }
                    if (!livroEncontrado)
                    {
                        Console.WriteLine("===============================================================");
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("FILME NÃO ENCONTRADO.");
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("===============================================================");
                    }
                }
                else
                {
                    string tituloConsulta;
                    bool filmeEncontrado = false;
                    int valorAdicional;

                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("===============================================================");
                    Console.Write("Digite o nome do Filme: ");
                    tituloConsulta = Console.ReadLine();

                    for (int i = 0; i < listaDeFilmes.Count; i++)
                    {
                        if (listaDeFilmes[i].Titulo.ToLower() == tituloConsulta.ToLower())
                        {
                            filmeEncontrado = true;
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.WriteLine("INFORMAÇÕES DO FILME:");
                            Console.ForegroundColor = ConsoleColor.White;
                            listaDeFilmes[i].ExibirTodasInformacoes();

                            Console.WriteLine("===============================================================");
                            Console.Write("Digite o valor a adicionar no estoque: ");

                            try
                            {
                                valorAdicional = int.Parse(Console.ReadLine());
                                if (valorAdicional < 0)
                                {
                                    new FormatException();
                                }
                            }
                            catch (FormatException)
                            {
                                Console.ForegroundColor = ConsoleColor.Cyan;
                                Console.WriteLine("===============================================================");
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Não é possível adicionar valor menor que zero ao estoque!");
                                Console.ForegroundColor = ConsoleColor.Cyan;
                                Console.WriteLine("===============================================================");
                                Console.ResetColor();
                                return;
                            }

                            listaDeLivros[i].AdicionarEstoque(valorAdicional);
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("===============================================================");
                            Console.WriteLine("Estoque alterado com sucesso!");
                            Console.WriteLine("===============================================================");

                            break;
                        }

                    }
                    if (!filmeEncontrado)
                    {
                        Console.WriteLine("===============================================================");
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("FILME NÃO ENCONTRADO.");
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("===============================================================");
                    }
                }                          


            }
            }

            static void ImprimirRelatorioVendas(List<Venda> listaDeVendas)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("===============================================================");
                Console.WriteLine("=                     Relatório de Vendas                    =");
                Console.WriteLine("===============================================================");
                Console.ForegroundColor = ConsoleColor.DarkYellow;

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

            static void CriarBancoETabelas()
            {
                using (MySqlConnection conexao = new MySqlConnection(
                "Server=localhost;Port=3306;User ID=root;Password=;"))
                {
                    conexao.Open();

                    string sqlBanco = "CREATE DATABASE IF NOT EXISTS locadora";

                    MySqlCommand comandoBanco = new MySqlCommand(sqlBanco, conexao);
                    comandoBanco.ExecuteNonQuery();
                }

                using (MySqlConnection conexao = Conexao())
                {
                    string sqlTabelas = @"
                CREATE TABLE IF NOT EXISTS filmes (
                    Id INT AUTO_INCREMENT PRIMARY KEY,
                    Titulo VARCHAR(150) NOT NULL,
                    Genero VARCHAR(100) NOT NULL,
                    Diretor VARCHAR(150) NOT NULL,
                    Preco DOUBLE NOT NULL,
                    QuantidadeEstoque INT NOT NULL
                    );

                CREATE TABLE IF NOT EXISTS livros (
                    Id INT AUTO_INCREMENT PRIMARY KEY,
                    Titulo VARCHAR(150) NOT NULL,
                    Genero VARCHAR(100) NOT NULL,
                    Autor VARCHAR(150) NOT NULL,
                    Preco DOUBLE NOT NULL,
                    QuantidadeEstoque INT NOT NULL
                    );

                CREATE TABLE IF NOT EXISTS vendas (
                    Id INT AUTO_INCREMENT PRIMARY KEY,                    
                    Produto VARCHAR(150) NOT NULL,
                    TipoProduto VARCHAR(50) NOT NULL,
                    Preco DOUBLE NOT NULL,
                    QuantidadeVendida INT NOT NULL,
                    TotalVenda DOUBLE NOT NULL,
                    DataVenda DATE NOT NULL,
                    HoraVenda TIME NOT NULL
                    );
                    ";

                    MySqlCommand comando = new MySqlCommand(sqlTabelas, conexao);
                    comando.ExecuteNonQuery();
                }
            }

        static void VerificarDadosIniciais()
            {
                using (MySqlConnection conexao = Conexao())
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
                        InsertDadosTxt();
                    }
                }
            }

            static void InsertDadosTxt()
            {                           
                string caminhoArquivoFilmes = Path.Combine(AppContext.BaseDirectory, "dadosFilmes.txt");
                string caminhoArquivoLivros = Path.Combine(AppContext.BaseDirectory, "dadosLivros.txt");
                string caminhoArquivoVendas = Path.Combine(AppContext.BaseDirectory, "dadosVendas.txt");

                if (File.Exists(caminhoArquivoFilmes))
                {
                    string[] linhasFilmes = File.ReadAllLines(caminhoArquivoFilmes);
                    foreach (string linha in linhasFilmes)
                    {
                        string[] dados = linha.Split(';');
                        Filme filme = new Filme(dados[0], dados[1], dados[2], double.Parse(dados[3], CultureInfo.InvariantCulture), int.Parse(dados[4]));

                        InserirProduto(filme);

                    }
                }

                if (File.Exists(caminhoArquivoLivros))
                {
                    string[] linhasLivros = File.ReadAllLines(caminhoArquivoLivros);
                    foreach (string linha in linhasLivros)
                    {
                        string[] dados = linha.Split(';');
                        Livro livro = new Livro(dados[0], dados[1], dados[2], double.Parse(dados[3], CultureInfo.InvariantCulture), int.Parse(dados[4]));
                        InserirProduto(livro);
                    }
                }

                if (File.Exists(caminhoArquivoVendas))
                {
                    string[] linhasVendas = File.ReadAllLines(caminhoArquivoVendas);
                    foreach (string linha in linhasVendas)
                    {
                        string[] dados = linha.Split(';');
                        Venda venda = new Venda(dados[0], dados[1], double.Parse(dados[2], CultureInfo.InvariantCulture), int.Parse(dados[3]), double.Parse(dados[4], CultureInfo.InvariantCulture), DateTime.Parse(dados[5]), TimeSpan.Parse(dados[6]));
                        InserirVenda(venda);

                    }
                }

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
                    vendas.TotalVenda = leitor.GetDouble("TotalVenda");
                    vendas.DataVenda = leitor.GetDateTime("DataVenda");
                    vendas.HoraVenda = leitor.GetTimeSpan("HoraVenda");

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
                comando.Parameters.AddWithValue("@DataVenda", venda.DataVenda);
                comando.Parameters.AddWithValue("@HoraVenda", venda.HoraVenda);


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

