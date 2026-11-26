using SistemaMonoliticoLivraria.Data;
using SistemaMonoliticoLivraria.Models;
using SistemaMonoliticoLivraria.Services;
using SistemaMonoliticoLivraria.UI;
using System;
using System.Collections.Generic;
using System.Configuration;


namespace SistemaMonoliticoLivraria
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string connectionStringServidor = ConfigurationManager.ConnectionStrings["Servidor"].ConnectionString;
            string connectionStringLocadora = ConfigurationManager.ConnectionStrings["Locadora"].ConnectionString;

            Database database = new Database(connectionStringServidor, connectionStringLocadora);
            database.Initialize();

            ProdutoService produtoService = new ProdutoService(database);
            VendaService vendaService = new VendaService(database);

            Menu menu = new Menu();

            int opcao;
            do
            {
                opcao = produtoService.ConvertStringForInt(menu.MenuPrincipal());
                switch (opcao)
                {
                    case 1:
                        CadastrarProduto(produtoService, menu);
                        break;
                    case 2:
                        RealizarVenda(produtoService, vendaService, menu);
                        break;
                    case 3:
                        ConsultarProduto(produtoService, menu);
                        break;
                    case 4:
                        ExcluirProduto(produtoService, menu);
                        break;
                    case 5:
                        GerenciarEstoque(produtoService, menu);
                        break;
                    case 6:
                        ImprimirRelatorioDeVendas(vendaService, produtoService, menu);
                        break;
                    case 0:
                        Environment.Exit(0);
                        break;
                    default:
                        menu.MensagemOpcaoInvalida();
                        break;
                }
            } while (true);




        }

        private static void CadastrarProduto(ProdutoService produtoService, Menu menu)
        {
            int opcaoCadastro;
            do
            {
                opcaoCadastro = produtoService.ConvertStringForInt(menu.MenuCadastrarProduto());

                if (opcaoCadastro == 1)
                    produtoService.CadastrarFilmePorMenu(menu.MenuCadastrarProdutoFilme());
                else if (opcaoCadastro == 2)
                    produtoService.CadastrarLivroPorMenu(menu.MenuCadastrarProdutoLivro());
                else if (opcaoCadastro != 0)
                    menu.MensagemOpcaoInvalida();
            } while (opcaoCadastro != 0);
        }

        private static void RealizarVenda(ProdutoService produtoService, VendaService vendaService, Menu menu)
        {
            int opcaoVenda;
            do
            {
                opcaoVenda = produtoService.ConvertStringForInt(menu.MenuRealizarVenda());

                if (opcaoVenda == 1)
                {
                    List<string> entradas = menu.MenuRealizarVendaFilme();
                    if (entradas.Count > 0)
                        vendaService.VenderPorMenu(entradas, "Filme");
                }
                else if (opcaoVenda == 2)
                {
                    List<string> entradas = menu.MenuRealizarVendaLivro();
                    if (entradas.Count > 0)
                        vendaService.VenderPorMenu(entradas, "Livro");                    
                }
                else if (opcaoVenda != 0)
                    menu.MensagemOpcaoInvalida();
            } while (opcaoVenda != 0);
        }

        private static void ConsultarProduto(ProdutoService produtoService, Menu menu)
        {
            int opcaoConsulta;
            do
            {
                opcaoConsulta = produtoService.ConvertStringForInt(menu.MenuConsultarProduto());

                if (opcaoConsulta == 1)
                {
                    int opcaoFilme;
                    do
                    {
                        opcaoFilme = produtoService.ConvertStringForInt(menu.MenuConsultarProdutoFilme());
                        if (opcaoFilme == 1)
                        {
                            string texto = menu.MenuConsultarProdutoFilmePorId();
                            if (produtoService.VerifyStringNotEmpity(texto))
                            {
                                menu.ExibirFilme();
                                ExibirFilmeOuMensagem(produtoService.BuscarFilmePorId(produtoService.ConvertStringForInt(texto)), menu);
                                menu.MensagemSairMenu();
                            }
                        }
                        else if (opcaoFilme == 2)
                        {
                            string texto = menu.MenuConsultarProdutoFilmePorTitulo();
                            if (produtoService.VerifyStringNotEmpity(texto))
                            {
                                menu.ExibirFilme();
                                ExibirFilmeOuMensagem(produtoService.BuscarFilmePorTitulo(texto), menu);
                                menu.MensagemSairMenu();
                            }
                        }
                        else if (opcaoFilme != 0)
                            menu.MensagemOpcaoInvalida();
                    } while (opcaoFilme != 0);
                }
                else if (opcaoConsulta == 2)
                {
                    int opcaoLivro;
                    do
                    {
                        opcaoLivro = produtoService.ConvertStringForInt(menu.MenuConsultarProdutoLivro());
                        if (opcaoLivro == 1)
                        {
                            string texto = menu.MenuConsultarProdutoLivroPorId();
                            if (produtoService.VerifyStringNotEmpity(texto))
                            {
                                menu.ExibirLivro();
                                ExibirLivroOuMensagem(produtoService.BuscarLivroPorId(produtoService.ConvertStringForInt(texto)), menu);
                                menu.MensagemSairMenu();
                            }
                        }
                        else if (opcaoLivro == 2)
                        {
                            string texto = menu.MenuConsultarProdutoLivroPorTitulo();
                            if (produtoService.VerifyStringNotEmpity(texto))
                            {
                                menu.ExibirLivro();
                                ExibirLivroOuMensagem(produtoService.BuscarLivroPorTitulo(texto), menu);
                                menu.MensagemSairMenu();
                            }
                        }
                        else if (opcaoLivro != 0)
                            menu.MensagemOpcaoInvalida();
                    } while (opcaoLivro != 0);
                }
                else if (opcaoConsulta != 0)
                    menu.MensagemOpcaoInvalida();
            } while (opcaoConsulta != 0);

        }

        private static void ExcluirProduto(ProdutoService produtoService, Menu menu)
        {
            int opcaoExclusao;
            do
            {
                opcaoExclusao = produtoService.ConvertStringForInt(menu.MenuExcluirProduto());

                if (opcaoExclusao == 1)
                {
                    int idProduto = produtoService.ConvertStringForInt(menu.MenuExcluirFilme());
                    produtoService.VerifyIdFilmeExist(idProduto);
                    produtoService.DeletarFilme(idProduto);
                }
                else if (opcaoExclusao == 2)
                {
                    int idProduto = produtoService.ConvertStringForInt(menu.MenuExcluirLivro());
                    produtoService.VerifyIdLivroExist(idProduto);
                    produtoService.DeletarLivro(idProduto);
                }
                else if (opcaoExclusao != 0)
                    menu.MensagemOpcaoInvalida();
            } while (opcaoExclusao != 0);
        }

        private static void GerenciarEstoque(ProdutoService produtoService, Menu menu)
        {
            int opcaoEstoque;
            do
            {
                opcaoEstoque = produtoService.ConvertStringForInt(menu.MenuGerenciarEstoque());

                if (opcaoEstoque == 1)
                {
                    int opcaoAdicionar;
                    do
                    {
                        opcaoAdicionar = produtoService.ConvertStringForInt(menu.MenuAdicionarEstoque());
                        if (opcaoAdicionar == 1)
                        {
                            List<string> entradas = menu.MenuAdicionarEstoqueFilme();
                            if (entradas.Count > 0 && produtoService.SetStockFilme(
                                produtoService.ConvertStringForInt(entradas[0]),
                                produtoService.ConvertStringForInt(entradas[1])))
                                menu.ConfirmacaoAtualizacaoEstoque();
                        }
                        else if (opcaoAdicionar == 2)
                        {
                            List<string> entradas = menu.MenuAdicionarEstoqueLivro();
                            if (entradas.Count > 0 && produtoService.SetStockLivro(
                                produtoService.ConvertStringForInt(entradas[0]),
                                produtoService.ConvertStringForInt(entradas[1])))
                                menu.ConfirmacaoAtualizacaoEstoque();
                        }
                        else if (opcaoAdicionar != 0)
                            menu.MensagemOpcaoInvalida();
                    } while (opcaoAdicionar != 0);
                }
                else if (opcaoEstoque == 2)
                {
                    int opcaoExibir;
                    do
                    {
                        opcaoExibir = produtoService.ConvertStringForInt(menu.MenuExibirEstoque());
                        if (opcaoExibir == 1)
                            produtoService.ExibirTodosFilmes();
                        else if (opcaoExibir == 2)
                            produtoService.ExibirTodosLivros();
                        else if (opcaoExibir != 0)
                            menu.MensagemOpcaoInvalida();
                    } while (opcaoExibir != 0);
                }
                else if (opcaoEstoque == 3)
                {
                    produtoService.ExibirTodosFilmes();
                    produtoService.ExibirTodosLivros();
                }
                else if (opcaoEstoque != 0)
                    menu.MensagemOpcaoInvalida();
            } while (opcaoEstoque != 0);
        }

        private static void ImprimirRelatorioDeVendas(VendaService vendaService, ProdutoService produtoService, Menu menu)
        {
            int opcaoRelatorio;
            do
            {
                opcaoRelatorio = produtoService.ConvertStringForInt(menu.MenuRelatorioVendas());

                if (opcaoRelatorio == 1)
                    vendaService.ExibirVendasFilmes();
                else if (opcaoRelatorio == 2)
                    vendaService.ExibirVendasLivros();
                else if (opcaoRelatorio == 3)
                    vendaService.ExibirRelatorioCompleto();
                else if (opcaoRelatorio != 0)
                    menu.MensagemOpcaoInvalida();
            } while (opcaoRelatorio != 0);

        }

        private static void ExibirFilmeOuMensagem(Filme filme, Menu menu)
        {
            if (filme == null)
            {
                menu.MensagemProdutoNaoEncontrado();
                return;
            }
            menu.ExibirInformacoes(filme);
        }

        private static void ExibirLivroOuMensagem(Livro livro, Menu menu)
        {
            if (livro == null)
            {
                menu.MensagemProdutoNaoEncontrado();
                return;
            }
            menu.ExibirInformacoes(livro);
        }

    }
}

