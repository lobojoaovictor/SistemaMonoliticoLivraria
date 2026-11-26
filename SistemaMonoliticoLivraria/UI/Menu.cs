using SistemaMonoliticoLivraria.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaMonoliticoLivraria.UI
{
    internal class Menu
    {
        private string LerCampo(string texto)
        {
            string linha = "= " + texto + ": ";
            int linhaAtual = Console.CursorTop;

            Console.WriteLine(linha.PadRight(62) + "=");
            Console.SetCursorPosition(linha.Length, linhaAtual);

            return Console.ReadLine();
        }

        private List<string> PreValidationEntradas(List<string> entradas)
        {
            foreach (var entrada in entradas)
            {
                if (string.IsNullOrWhiteSpace(entrada) || entrada.Trim() == "0")
                {
                    entradas.Clear();
                    break;
                }
            }
            if (entradas.Count == 0)
            {
                MensagemOpcaoInvalida();
            }
            return entradas;
        }
        public string MenuPrincipal()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("===============================================================");
            Console.WriteLine("=                    Bem-vindo ao PlotSys!                    =");
            Console.WriteLine("===============================================================");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("= 1 - Cadastrar Produto                                       =");
            Console.WriteLine("= 2 - Realizar Venda                                          =");
            Console.WriteLine("= 3 - Consultar Produto                                       =");
            Console.WriteLine("= 4 - Excluir Produto                                         =");
            Console.WriteLine("= 5 - Gerenciar Estoque                                       =");
            Console.WriteLine("= 6 - Imprimir Relatório de Vendas                            =");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("= 0 - Sair                                                    =");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("===============================================================");
            string idTexto = LerCampo("Digite a opção desejada");
            Console.WriteLine("===============================================================");
            Console.ResetColor();

            return idTexto;
        }

        public string MenuCadastrarProduto()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("===============================================================");
            Console.WriteLine("=                   Cadastrar Produto                         =");
            Console.WriteLine("===============================================================");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("= 1 - Cadastrar Filme                                         =");
            Console.WriteLine("= 2 - Cadastrar Livro                                         =");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("= 0 - Voltar                                                  =");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("===============================================================");
            string idTexto = LerCampo("Digite a opção desejada");
            Console.WriteLine("===============================================================");
            Console.ResetColor();

            return idTexto;
        }

        public List<string> MenuCadastrarProdutoFilme()
        {
            List<string> entradas = new List<string>();
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("===============================================================");
            Console.WriteLine("=                   Cadastrar Filme                           =");
            Console.WriteLine("===============================================================");
            Console.ForegroundColor = ConsoleColor.White;
            entradas.Add(LerCampo("Digite o título do filme"));
            entradas.Add(LerCampo("Digite o gênero do filme"));
            entradas.Add(LerCampo("Digite o diretor do filme"));           
            entradas.Add(LerCampo("Digite o preço do filme"));            
            entradas.Add(LerCampo("Digite a quantidade em estoque do filme"));
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("= 0 - Voltar                                                  =");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("===============================================================");

            return PreValidationEntradas(entradas);
        }
        public List<string> MenuCadastrarProdutoLivro()
        {
            List<string> entradas = new List<string>();
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("===============================================================");
            Console.WriteLine("=                   Cadastrar Livro                           =");
            Console.WriteLine("===============================================================");
            Console.ForegroundColor = ConsoleColor.White;           
            entradas.Add(LerCampo("Digite o título do livro"));           
            entradas.Add(LerCampo("Digite o gênero do livro"));
            entradas.Add(LerCampo("Digite o autor do livro"));           
            entradas.Add(LerCampo("Digite o preço do livro"));            
            entradas.Add(LerCampo("Digite a quantidade em estoque do livro"));
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("= Digite 0 para voltar                                        =");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("===============================================================");

            return PreValidationEntradas(entradas);
        }

        public void CadastroFilmeSucesso()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("===============================================================");
            Console.WriteLine("= Livro cadastrado com sucesso!                               =");
            Console.WriteLine("===============================================================");
            MensagemSairMenu();
        }

        public void CadastroLivroSucesso()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("===============================================================");
            Console.WriteLine("= Filme cadastrado com sucesso!                               =");
            Console.WriteLine("===============================================================");
            MensagemSairMenu();
        }
        
        public string MenuRealizarVenda()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("===============================================================");
            Console.WriteLine("=                   Realizar Venda                            =");
            Console.WriteLine("===============================================================");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("= 1 - Realizar Venda de Filme                                 =");
            Console.WriteLine("= 2 - Realizar Venda de livro                                 =");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("= 0 - Voltar                                                  =");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("===============================================================");
            string idTexto = LerCampo("Digite a opção desejada");
            Console.WriteLine("===============================================================");
            Console.ResetColor();

            return idTexto;
        }

        public List<string> MenuRealizarVendaLivro()
        {
            List<string> entradas = new List<string>();
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("===============================================================");
            Console.WriteLine("=                   Realizar Venda de Livro                   =");
            Console.WriteLine("===============================================================");
            Console.ForegroundColor = ConsoleColor.White;
            entradas.Add(LerCampo("Digite o título do livro"));
            entradas.Add(LerCampo("Digite a quantidade desejada"));
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("= 0 - Voltar                                                  =");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("===============================================================");

            return PreValidationEntradas(entradas);
        }

        public List<string> MenuRealizarVendaFilme()
        {
            List<string> entradas = new List<string>();
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("===============================================================");
            Console.WriteLine("=                   Realizar Venda de Filme                   =");
            Console.WriteLine("===============================================================");
            Console.ForegroundColor = ConsoleColor.White;
            entradas.Add(LerCampo("Digite o título do filme"));
            entradas.Add(LerCampo("Digite a quantidade desejada"));
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("= 0 - Voltar                                                  =");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("===============================================================");
            
            return PreValidationEntradas(entradas);

        }
        public void VendaRealizadaSucesso()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("===============================================================");
            Console.WriteLine("= Venda realizada com sucesso!                                =");
            Console.WriteLine("===============================================================");
            Console.ResetColor();
            MensagemSairMenu();
        }
        public string MenuConsultarProduto()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("===============================================================");
            Console.WriteLine("=                   Consultar Produto                         =");
            Console.WriteLine("===============================================================");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("= 1 - Consultar Filme                                         =");
            Console.WriteLine("= 2 - Consultar Livro                                         =");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("= 0 - Voltar                                                  =");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("===============================================================");
            string idTexto = LerCampo("Digite a opção desejada");
            Console.WriteLine("===============================================================");
            Console.ResetColor();

            return idTexto;
        }

        private void TituloConsultarFilme()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("===============================================================");
            Console.WriteLine("=                    Consultar Filme                          =");
            Console.WriteLine("===============================================================");
        }

        public void ExibirFilme()
        {
            Console.Clear();
            TituloConsultarFilme();
        }
        private void TituloConsultarLivro()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("===============================================================");
            Console.WriteLine("=                    Consultar Livro                          =");
            Console.WriteLine("===============================================================");
        }

        public void ExibirLivro()
        {
            Console.Clear();
            TituloConsultarLivro();
        }

        public string MenuConsultarProdutoFilme()
        {
            Console.Clear();
            TituloConsultarFilme();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("= 1 - Consultar por Id                                        =");
            Console.WriteLine("= 2 - Consultar por título                                    =");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("= 0 - Voltar                                                  =");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("===============================================================");
            string idTexto = LerCampo("Digite a opção desejada");
            Console.WriteLine("===============================================================");
            Console.ResetColor();

            return idTexto;
        }

        public string MenuConsultarProdutoFilmePorId()
        {
            Console.Clear();
            TituloConsultarFilme();
            Console.ForegroundColor = ConsoleColor.White;
            string idTexto = LerCampo("Digite o Id do filme");            
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("= Digite enter para voltar.                                   =");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("===============================================================");
            Console.ResetColor();

            return idTexto;
        }

        public string MenuConsultarProdutoFilmePorTitulo()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            TituloConsultarFilme();
            Console.ForegroundColor = ConsoleColor.White;
            string idTexto = LerCampo("Digite o titulo do filme");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("= Digite enter para voltar.                                   =");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("===============================================================");
            Console.ResetColor();

            return idTexto;
        }

        public string MenuConsultarProdutoLivro()
        {
            Console.Clear();
            TituloConsultarLivro();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("= 1 - Consultar por Id                                         =");
            Console.WriteLine("= 2 - Consultar por título                                     =");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("= 0 - Voltar                                                   =");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("===============================================================");
            string idTexto = LerCampo("Digite a opção desejada");
            Console.WriteLine("===============================================================");
            Console.ResetColor();

            return idTexto;
        }

        public string MenuConsultarProdutoLivroPorId()
        {
            Console.Clear();
            TituloConsultarLivro();
            Console.ForegroundColor = ConsoleColor.White;
            string idTexto = LerCampo("Digite o Id do livro");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("= Digite enter para voltar.                                   =");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("===============================================================");
            Console.ResetColor();

            return idTexto;
        }

        public string MenuConsultarProdutoLivroPorTitulo()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            TituloConsultarLivro();
            Console.ForegroundColor = ConsoleColor.White;
            string idTexto = LerCampo("Digite o titulo do livro");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("= Digite enter para voltar.                                   =");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("===============================================================");
            Console.ResetColor();

            return idTexto;
        }
        public string MenuExcluirProduto()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("===============================================================");
            Console.WriteLine("=                   Excluir Produto                           =");
            Console.WriteLine("===============================================================");
            Console.ForegroundColor = ConsoleColor.White; 
            Console.WriteLine("= 1 - Excluir Filme                                           =");
            Console.WriteLine("= 2 - Excluir Livro                                           =");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("= 0 - Voltar                                                  =");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("===============================================================");
            string idTexto = LerCampo("Digite a opção desejada: ");
            Console.WriteLine("===============================================================");
            Console.ResetColor();

            return idTexto;
        }

        public string MenuExcluirFilme()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("===============================================================");
            Console.WriteLine("=                   Excluir Filme                             =");
            Console.WriteLine("===============================================================");
            Console.ForegroundColor = ConsoleColor.White;
            string idTexto = LerCampo("Digite o ID do filme a ser excluído");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("= 0 - Voltar                                                  =");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("===============================================================");

            return idTexto;
        }

        public string MenuExcluirLivro()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("===============================================================");
            Console.WriteLine("=                   Excluir Livro                             =");
            Console.WriteLine("===============================================================");
            Console.ForegroundColor = ConsoleColor.White;
            string idTexto = LerCampo("Digite o ID do livro");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("= 0 - Voltar                                                  =");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("===============================================================");

            return idTexto;
        }

        public void ConfirmacaoExcluirLivro()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("===============================================================");
            Console.WriteLine("Livro excluido com sucesso!");
            Console.WriteLine("===============================================================");
            Console.ResetColor();
            MensagemSairMenu();
        }

        public void ConfirmacaoExcluirFilme()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("===============================================================");
            Console.WriteLine("Filme excluido com sucesso!");
            Console.WriteLine("===============================================================");
            Console.ResetColor();
            MensagemSairMenu();
        }

        public string MenuGerenciarEstoque()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("===============================================================");
            Console.WriteLine("=                   Gerenciar Estoque                         =");
            Console.WriteLine("===============================================================");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("= 1 - Atualizar Estoque                                       =");
            Console.WriteLine("= 2 - Exibir Estoque (Filme ou Livro)                         =");
            Console.WriteLine("= 3 - Exibir Estoque Completo                                 =");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("= 0 - Voltar                                                  =");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("===============================================================");
            string idTexto = LerCampo("Digite a opção desejada");
            Console.WriteLine("===============================================================");
            Console.ResetColor();

            return idTexto;
        }

        public string MenuAdicionarEstoque()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("===============================================================");
            Console.WriteLine("=                   Adicionar Estoque                         =");
            Console.WriteLine("===============================================================");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("= 1 - Atualizar Estoque de Filme                              =");
            Console.WriteLine("= 2 - Atualizar Estoque de Livro                              =");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("= 0 - Voltar                                                  =");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("===============================================================");
            string idTexto = LerCampo("Digite a opção desejada");
            Console.WriteLine("===============================================================");
            Console.ResetColor();

            return idTexto;
        }
                

        public List<string> MenuAdicionarEstoqueFilme()
        {
            List<string> entradas = new List<string>();
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("===============================================================");
            Console.WriteLine("=                   Atualizar Estoque de Filme                =");
            Console.WriteLine("===============================================================");
            Console.ForegroundColor = ConsoleColor.White;
            
            entradas.Add(LerCampo("Digite o ID do filme"));
            entradas.Add(LerCampo("Digite o estoque atual"));

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("= 0 - Voltar                                                  =");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("===============================================================");
            
            return PreValidationEntradas(entradas);

        }
        public List<string> MenuAdicionarEstoqueLivro()
        {
            List<string> entradas = new List<string>();
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("===============================================================");
            Console.WriteLine("=                   Atualizar Estoque de Livro                =");
            Console.WriteLine("===============================================================");
            Console.ForegroundColor = ConsoleColor.White;

            entradas.Add(LerCampo("Digite o ID do livro"));
            entradas.Add(LerCampo("Digite o estoque atual"));

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("= 0 - Voltar                                                  =");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("===============================================================");

            return PreValidationEntradas(entradas);
        }

        public void ConfirmacaoAtualizacaoEstoque()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("===============================================================");
            Console.WriteLine("Estoque atualizado com sucesso!");
            Console.WriteLine("===============================================================");
            Console.ResetColor();
            MensagemSairMenu();
        }

        public string MenuExibirEstoque()
        {            
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("===============================================================");
            Console.WriteLine("=                       Exibir Estoque                        =");
            Console.WriteLine("===============================================================");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("= 1 - Exibir estoque de filmes                                =");
            Console.WriteLine("= 2 - Exibir estoque de livros                                =");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("= 0 - Voltar                                                  =");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("===============================================================");
            string entrada = LerCampo("Entrada:");
            Console.WriteLine("===============================================================");

            return entrada;
        }

        public void TituloInformacoesFilmes()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("===============================================================");
            Console.WriteLine("=                    Estoque de Filmes                        =");
            Console.WriteLine("===============================================================");
            Console.ForegroundColor = ConsoleColor.White;
        }

        public void TituloInformacoesLivros()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("===============================================================");
            Console.WriteLine("=                     Estoque de Livros                       =");
            Console.WriteLine("===============================================================");
            Console.ForegroundColor = ConsoleColor.White;
        }
        public void ExibirInformacoes(Filme filme)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("===============================================================");
            Console.WriteLine($"Título: {filme.Titulo}");
            Console.WriteLine($"Gênero: {filme.Genero}");
            Console.WriteLine($"Diretor: {filme.Diretor}");
            Console.WriteLine($"Preço: {filme.Preco:C}");
            Console.WriteLine($"Quantidade em Estoque: {filme.QuantidadeEstoque}");
            Console.WriteLine("===============================================================");
        }

        public void ExibirInformacoes(Livro livro)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("===============================================================");
            Console.WriteLine($"Título: {livro.Titulo}");
            Console.WriteLine($"Gênero: {livro.Genero}");
            Console.WriteLine($"Autor: {livro.Autor}");
            Console.WriteLine($"Preço: {livro.Preco:C}");
            Console.WriteLine($"Quantidade em Estoque: {livro.QuantidadeEstoque}");
            Console.WriteLine("===============================================================");
        }
        public string MenuRelatorioVendas()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("===============================================================");
            Console.WriteLine("=                   Relatório de Vendas                       =");
            Console.WriteLine("===============================================================");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("= 1 - Relatório de Vendas de Filmes                           =");
            Console.WriteLine("= 2 - Relatório de Vendas de Livros                           =");
            Console.WriteLine("= 3 - Relatório Completo de Vendas                            =");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("= 0 - Voltar                                                  =");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("===============================================================");
            string idTexto = LerCampo("Digite a opção desejada");
            Console.WriteLine("===============================================================");
            Console.ResetColor();

            return idTexto;
        }            

        public void MenuRelatorioVendasFilme()
        {
            Console.Clear();
            Console.WriteLine("===============================================================");
            Console.WriteLine("=               Relatório de Vendas (Filmes)                  =");
            Console.WriteLine("===============================================================");
        }

        public void MenuRelatorioVendasLivro()
        {
            Console.Clear();
            Console.WriteLine("===============================================================");
            Console.WriteLine("=               Relatório de Vendas (Livros)                  =");
            Console.WriteLine("===============================================================");
        }

        public void MenuRelatorioVendasCompleto()
        {
            Console.Clear();
            Console.WriteLine("===============================================================");
            Console.WriteLine("=              Relatório de Vendas (Completo)                 =");
            Console.WriteLine("===============================================================");
        }

        public void ExibirInformacoes(Venda venda)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("===============================================================");
            Console.WriteLine($"Produto: {venda.Produto}");
            Console.WriteLine($"Tipo de Produto: {venda.TipoProduto}");
            Console.WriteLine($"Preço: {venda.Preco.ToString("C", CultureInfo.GetCultureInfo("pt-BR"))}");
            Console.WriteLine($"Quantidade Vendida: {venda.QuantidadeVendida}");
            Console.WriteLine($"Total da Venda: {venda.TotalVenda.ToString("C", CultureInfo.GetCultureInfo("pt-BR"))}");
            Console.WriteLine($"Data da Venda: {venda.DataVenda:dd/MM/yyyy}");
            Console.WriteLine($"Hora da Venda: {venda.HoraVenda:hh\\:mm\\:ss}");
            Console.WriteLine($"Id do Produto: {venda.IdProduto}");
            Console.WriteLine("===============================================================");
        }

        public void MensagemOpcaoInvalida()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Opção inválida. Pressione qualquer tecla para continuar...");
            Console.ReadKey();
        }

        public void EstoqueInsuficiente()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Estoque insuficiente.");
        }

        public void ErroVenda()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Erro ao realizar venda.");
            Console.ReadLine();
        }

        public void MensagemSairMenu()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Pressione qualquer tecla pra sair...");
            Console.ReadKey();
        }

        public void MensagemProdutoNaoEncontrado()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Produto não encontrado. Pressione qualquer tecla para continuar...");
            Console.ReadKey();
        }
        
        public void ErroAoCadastrarProduto()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("===============================================================");
            Console.WriteLine("= Erro ao cadastrar produto.                                  =");
            Console.WriteLine("===============================================================");
        }

        public void IdNaoEncontrado()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("===============================================================");
            Console.WriteLine("= Não há produto com esse id                                  =");
            Console.WriteLine("===============================================================");
            Console.ResetColor();
            MensagemSairMenu();
        }
    }
}
