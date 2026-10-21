using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

// empresa de locação de livros e filmes

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
            Filme filme = new Filme();
            Livro livro = new Livro();

            CarregarDados(listaDeVendas, listaDeFilmes, listaDeLivros);

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //                                        CAMADA DE INTERFACE DO USUÁRIO                                          ///
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            int opcao;

            do{
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
    
                opcao = int.Parse(Console.ReadLine());
    
                switch (opcao)
                {
                    case 1:
                        CadastrarProduto(filme, livro, listaDeFilmes, listaDeLivros);
                        break;
                    case 2:
                        RealizarVenda(venda, filme, livro, listaDeVendas, listaDeFilmes, listaDeLivros);
                        break;
                    case 3:
                        ConsultarExcluirProduto(filme, livro, listaDeFilmes, listaDeLivros);
                        break;
                    case 4:
                        ImprimirEstoque(filme, livro, listaDeFilmes, listaDeLivros);
                        break;
                    case 5:
                        ImprimirRelatorioVendas(listaDeVendas);
                        break;
                    case 0:
                        SalvarDados(listaDeVendas, listaDeFilmes, listaDeLivros);
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
            }while (opcao != 0);
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///                                             CAMADA DE DADOS                                                   ///
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        struct Venda
        {
            public string Produto;
            public string TipoProduto;
            public double Preco;
            public int QuantidadeVendida;
            public double TotalVenda;

            public Venda(string produto, string tipoProduto, double preco, int quantidadeVendida)
            {
                Produto = produto;
                TipoProduto = tipoProduto;
                Preco = preco;
                QuantidadeVendida = quantidadeVendida;
                TotalVenda = preco * quantidadeVendida;
            }

            public string LinhaDeSalvamento()
            {
                return $"{Produto};{TipoProduto};{Preco};{QuantidadeVendida};{TotalVenda}";

            }

            public static Venda CarregarLinha(string linha)
            {
                string[] dados = linha.Split(';');

                Venda venda = new Venda();

                venda.Produto = dados[0];
                venda.TipoProduto = dados[1];
                venda.Preco = double.Parse(dados[2]);
                venda.QuantidadeVendida = int.Parse(dados[3]);
                venda.TotalVenda = double.Parse(dados[4]);

                return venda;
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

        struct Livro
        {
            public string Titulo;
            public string Genero;
            public string Autor;
            public double Preco;
            public int QuantidadeEstoque;

            public void AlterarEstoque(int valor)
            {
                QuantidadeEstoque -= valor;
            }

            public string LinhaDeSalvamento()
            {
                return $"{Titulo};{Genero};{Autor};{Preco};{QuantidadeEstoque}";

            }

            public static Livro CarregarLinha(string linha)
            {
                string[] dados = linha.Split(';');

                Livro livro = new Livro();

                livro.Titulo = dados[0];
                livro.Genero = dados[1];
                livro.Autor = dados[2];
                livro.Preco = double.Parse(dados[3]);
                livro.QuantidadeEstoque = int.Parse(dados[4]);

                return livro;
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

        struct Filme
        {
            public string Titulo;
            public string Genero;
            public string Diretor;
            public double Preco;
            public int QuantidadeEstoque;

            public void AlterarEstoque(int valor)
            {
                QuantidadeEstoque -= valor;
            }
            public String LinhaDeSalvamento()
            {
                string linhaDeSalvamento = $"{Titulo};{Genero};{Diretor};{Preco};{QuantidadeEstoque}";
                return linhaDeSalvamento;

            }

            public static Filme CarregarLinha(string linha)
            {
                string[] dados = linha.Split(';');

                Filme filme = new Filme();

                filme.Titulo = dados[0];
                filme.Genero = dados[1];
                filme.Diretor = dados[2];
                filme.Preco = double.Parse(dados[3]);
                filme.QuantidadeEstoque = int.Parse(dados[4]);

                return filme;
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

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///                                         CAMADA DE LÓGICA DE NEGÓCIO                                           ///
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        static void CadastrarProduto(Filme filme, Livro livro, List<Filme> listaDeFilmes, List<Livro> listaDeLivros)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("===============================================================");
            Console.WriteLine("=                     Cadastrar Produto                       =");
            Console.WriteLine("===============================================================");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Digite 1 para cadastrar um livro ou 2 para cadastrar um filme: ");
            Console.WriteLine("===============================================================");

            if (Console.ReadLine() == "1")
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
                livro.Preco = double.Parse(Console.ReadLine());
                Console.Write("Digite a quantidade em estoque do livro: ");
                livro.QuantidadeEstoque = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("===============================================================");

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

                listaDeFilmes.Add(filme);
            }
        }



        static void RealizarVenda(Venda venda, Filme Produtofilme, Livro Produtolivro, List<Venda> listaDeVendas, List<Filme> listaDeFilmes, List<Livro> listaDeLivros)
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
                string tituloVenda = Console.ReadLine();

                Console.WriteLine("===============================================================");

                for (int i = 0; i < listaDeLivros.Count; i++)
                {
                    if (listaDeLivros[i].Titulo == tituloVenda)
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
                string tituloVenda = Console.ReadLine();

                Console.WriteLine("===============================================================");

                for (int i = 0; i < listaDeFilmes.Count; i++)
                {
                    if (listaDeFilmes[i].Titulo == tituloVenda)
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


        static void ConsultarExcluirProduto(Filme filme, Livro livro, List<Filme> listaDeFilmes, List<Livro> listaDeLivros)
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
                string tituloConsulta = Console.ReadLine();
                Console.WriteLine("===============================================================");

                for (int i = 0; i < listaDeLivros.Count; i++)
                {
                    if (listaDeLivros[i].Titulo == tituloConsulta)
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
                    if (listaDeFilmes[i].Titulo == tituloConsulta)
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

        static void ImprimirEstoque(Filme filme, Livro livro, List<Filme> listaDeFilmes, List<Livro> listaDeLivros)
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

       static void CarregarDados(List<Venda> listaDeVendas, List<Filme> listaDeFilmes, List<Livro> listaDeLivros)
        {
            string caminhoArquivoVendas = "vendas.txt";
            string caminhoArquivoFilmes = "filmes.txt";
            string caminhoArquivoLivros = "livros.txt";
        
            if (!File.Exists(caminhoArquivoVendas))
            {
                File.WriteAllText(caminhoArquivoVendas, "");
            }
            else
            {
                foreach (string linha in File.ReadAllLines(caminhoArquivoVendas))
                {
                    listaDeVendas.Add(Venda.CarregarLinha(linha));
                }
            }
        
            if (!File.Exists(caminhoArquivoFilmes))
            {
                File.WriteAllText(caminhoArquivoFilmes, "");
            }
            else
            {
                foreach (string linha in File.ReadAllLines(caminhoArquivoFilmes))
                {
                    listaDeFilmes.Add(Filme.CarregarLinha(linha));
                }
            }
        
            if (!File.Exists(caminhoArquivoLivros))
            {
                File.WriteAllText(caminhoArquivoLivros, "");
            }
            else
            {
                foreach (string linha in File.ReadAllLines(caminhoArquivoLivros))
                {
                    listaDeLivros.Add(Livro.CarregarLinha(linha));
                }
            }
        }

        static void SalvarDados(List<Venda> listaDeVendas, List<Filme> listaDeFilmes, List<Livro> listaDeLivros)
            {
            string caminhoArquivoVendas = "vendas.txt";
            string caminhoArquivoFilmes = "filmes.txt";
            string caminhoArquivoLivros = "livros.txt";

            List<string> linhasVendas = new List<string>();
        
            foreach (Venda venda in listaDeVendas)
            {
                linhasVendas.Add(venda.LinhaDeSalvamento());
            }

            File.WriteAllLines(caminhoArquivoVendas, linhasVendas);
        
        
            List<string> linhasFilmes = new List<string>();
        
            foreach (Filme filme in listaDeFilmes)
            {
                linhasFilmes.Add(filme.LinhaDeSalvamento());
            }
        
            File.WriteAllLines(caminhoArquivoFilmes, linhasFilmes);
        
        
            List<string> linhasLivros = new List<string>();
        
            foreach (Livro livro in listaDeLivros)
            {
                linhasLivros.Add(livro.LinhaDeSalvamento());
            }
        
            File.WriteAllLines(caminhoArquivoLivros, linhasLivros);
        }
            }
}
