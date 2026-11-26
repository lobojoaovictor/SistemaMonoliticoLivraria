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
            int opcaoCadastro = produtoService.ConvertStringForInt(menu.MenuCadastrarProduto());

            if (opcaoCadastro == 0)
            {
                menu.MensagemSairMenu();
            }
            else if (opcaoCadastro == 1)
            {
                produtoService.CadastrarFilmePorMenu(menu.MenuCadastrarProdutoFilme());
            }
            else if (opcaoCadastro == 2)
            {
                produtoService.CadastrarLivroPorMenu(menu.MenuCadastrarProdutoLivro());
            }
            else
            {
                menu.MensagemOpcaoInvalida();
            }
        }

        private static void RealizarVenda(ProdutoService produtoService, VendaService vendaService, Menu menu)
        {
            int opcaoVenda = produtoService.ConvertStringForInt(menu.MenuRealizarVenda());

            if (opcaoVenda == 0)
            {
                menu.MensagemSairMenu();
            }
            else if (opcaoVenda == 1)
            {
                List<string> entradas = menu.MenuRealizarVendaLivro();
                if (entradas.Count > 0)
                {
                    vendaService.VenderPorMenu(entradas, "Livro");
                }
            }
            else if (opcaoVenda == 2)
            {
                List<string> entradas = menu.MenuRealizarVendaFilme();
                if (entradas.Count > 0)
                {
                    vendaService.VenderPorMenu(entradas, "Filme");
                }
            }
            else
            {
                menu.MensagemOpcaoInvalida();
            }
        }

        private static void ConsultarProduto(ProdutoService produtoService, Menu menu)
        {
            int opcaoConsulta = produtoService.ConvertStringForInt(menu.MenuConsultarProduto());

            if (opcaoConsulta == 0)
            {
                menu.MensagemSairMenu();
            }
            else if (opcaoConsulta == 1)
            {
                int opcaoConsultaFilme = produtoService.ConvertStringForInt(menu.MenuConsultarProdutoFilme());

                if (opcaoConsultaFilme == 0)
                {
                    menu.MensagemSairMenu();
                }
                else if (opcaoConsultaFilme == 1)
                {
                    string texto = menu.MenuConsultarProdutoFilmePorId();
                    if (!produtoService.VerifyStringNotEmpity(texto))
                    {
                        menu.MensagemSairMenu();
                    }
                    else
                    {
                        Filme filme = produtoService.BuscarFilmePorId(produtoService.ConvertStringForInt(texto));
                        ExibirFilmeOuMensagem(filme, menu);
                    }
                }
                else if (opcaoConsultaFilme == 2)
                {
                    string texto = menu.MenuConsultarProdutoFilmePorTitulo();
                    if (!produtoService.VerifyStringNotEmpity(texto))
                    {
                        menu.MensagemSairMenu();
                    }
                    else
                    {
                        Filme filme = produtoService.BuscarFilmePorTitulo(texto);
                        ExibirFilmeOuMensagem(filme, menu);
                    }
                }
                else
                {
                    menu.MensagemOpcaoInvalida();
                }
            }
            else if (opcaoConsulta == 2)
            {
                int opcaoConsultaLivro = produtoService.ConvertStringForInt(menu.MenuConsultarProdutoLivro());

                if (opcaoConsultaLivro == 0)
                {
                    menu.MensagemSairMenu();
                }
                else if (opcaoConsultaLivro == 1)
                {
                    string texto = menu.MenuConsultarProdutoLivroPorId();
                    if (!produtoService.VerifyStringNotEmpity(texto))
                    {
                        menu.MensagemSairMenu();
                    }
                    else
                    {
                        Livro livro = produtoService.BuscarLivroPorId(produtoService.ConvertStringForInt(texto));
                        ExibirLivroOuMensagem(livro, menu);
                    }

                }
                else if (opcaoConsultaLivro == 2)
                {
                    string texto = menu.MenuConsultarProdutoLivroPorTitulo();
                    if (!produtoService.VerifyStringNotEmpity(texto))
                    {
                        menu.MensagemSairMenu();
                    }
                    else
                    {
                        Livro livro = produtoService.BuscarLivroPorTitulo(texto);
                        ExibirLivroOuMensagem(livro, menu);
                    }
                }
                else
                {
                    menu.MensagemOpcaoInvalida();
                }
            }
            else
            {
                menu.MensagemOpcaoInvalida();
            }

        }

        private static void ExcluirProduto(ProdutoService produtoService, Menu menu)
        {
            int opcaoExclusao = produtoService.ConvertStringForInt(menu.MenuExcluirProduto());

            if (opcaoExclusao == 0)
            {
                menu.MensagemSairMenu();
            }
            else if (opcaoExclusao == 1)
            {
                int idProduto = produtoService.ConvertStringForInt(menu.MenuExcluirFilme());
                produtoService.VerifyIdFilmeExist(idProduto);
                produtoService.DeletarFilme(idProduto);
                menu.ConfirmacaoExcluirFilme();                
            }
            else if (opcaoExclusao == 2)
            {
                int idProduto = produtoService.ConvertStringForInt(menu.MenuExcluirLivro());
                produtoService.VerifyIdLivroExist(idProduto);
                produtoService.DeletarLivro(idProduto);
                menu.ConfirmacaoExcluirLivro();
            }
            else
            {
                menu.MensagemOpcaoInvalida();
            }
        }

        private static void GerenciarEstoque(ProdutoService produtoService, Menu menu)
        {
            int opcaoEstoque = produtoService.ConvertStringForInt(menu.MenuGerenciarEstoque());

            if (opcaoEstoque == 1)
            {
                produtoService.ExibirTodosFilmes();
                produtoService.ExibirTodosLivros();
            }
            else if (opcaoEstoque == 2)
            {
                int opcaoAdicionarEstoque = produtoService.ConvertStringForInt(menu.MenuAdicionarEstoque());

                if (opcaoAdicionarEstoque == 0)
                {
                    menu.MensagemSairMenu();
                }
                else if (opcaoAdicionarEstoque == 1)
                {
                    List<string> entradas = menu.MenuAdicionarEstoqueFilme();
                    if (entradas.Count == 0)
                    {
                        return;
                    }
                    int idFilme = produtoService.ConvertStringForInt(entradas[0]);
                    int estoqueAtual = produtoService.ConvertStringForInt(entradas[1]);
                    if (produtoService.SetStockFilme(idFilme, estoqueAtual))
                    {
                        menu.ConfirmacaoAtualizacaoEstoque();
                    }

                }
                else if (opcaoAdicionarEstoque == 2)
                {
                    List<string> entradas = menu.MenuAdicionarEstoqueLivro();
                    if (entradas.Count == 0)
                    {
                        return;
                    }
                    int idLivro = produtoService.ConvertStringForInt(entradas[0]);
                    int estoqueAtual = produtoService.ConvertStringForInt(entradas[1]);
                    if (produtoService.SetStockLivro(idLivro, estoqueAtual))
                    {
                        menu.ConfirmacaoAtualizacaoEstoque();
                    }

                }
                else
                {
                    menu.MensagemOpcaoInvalida();
                }

            }
            else if (opcaoEstoque == 3)
            {
                int escolhaMenuExibirEstoque = produtoService.ConvertStringForInt(menu.MenuExibirEstoque());

                if (escolhaMenuExibirEstoque == 0)
                {
                    menu.MensagemSairMenu();
                }

                else if (escolhaMenuExibirEstoque == 1)
                {
                    produtoService.ExibirTodosFilmes();
                }
                else if (escolhaMenuExibirEstoque == 2)
                {
                    produtoService.ExibirTodosLivros();
                }
                else
                {
                    menu.MensagemOpcaoInvalida();
                }

            }
            else
            {
                menu.MensagemOpcaoInvalida();
            }
        }

        private static void ImprimirRelatorioDeVendas(VendaService vendaService, ProdutoService produtoService, Menu menu)
        {
            int opcaoRelatorio = produtoService.ConvertStringForInt(menu.MenuRelatorioVendas());

            if (opcaoRelatorio == 0)
            {
                menu.MensagemSairMenu();
            }
            else if (opcaoRelatorio == 1)
            {
                vendaService.ExibirVendasFilmes();
            }
            else if (opcaoRelatorio == 2)
            {
                vendaService.ExibirVendasLivros();
            }
            else if (opcaoRelatorio == 3)
            {
                vendaService.ExibirRelatorioCompleto();
            }
            else
            {
                menu.MensagemOpcaoInvalida();
            }

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

