using SistemaMonoliticoLivraria.Data;
using SistemaMonoliticoLivraria.Models;
using SistemaMonoliticoLivraria.UI;
using System;
using System.Collections.Generic;

namespace SistemaMonoliticoLivraria.Services
{
    internal class ProdutoService
    {
        private readonly Database _database;
        private readonly VendaRepository _vendaRepository;
        private readonly FilmeRepository _filmeRepository;
        private readonly LivroRepository _livroRepository;
        private readonly Menu _menu;
        public ProdutoService(Database database)
        {
            _database = database;
            _vendaRepository = new VendaRepository(_database);
            _filmeRepository = new FilmeRepository(_database);
            _livroRepository = new LivroRepository(_database);
            _menu = new Menu();
        }
        public bool CadastrarFilme(Filme filme)
        {
            ValidateProduto(filme);
            bool boolCadastro = _filmeRepository.Insert(filme);

            if (boolCadastro)
            {
                _menu.CadastroFilmeSucesso();
            }
            return boolCadastro;
        }

        public bool CadastrarLivro(Livro livro)
        {
            ValidateProduto(livro);
            bool boolCadastro = _livroRepository.Insert(livro);

            if (boolCadastro)
            {
                _menu.CadastroLivroSucesso();
            }
            return boolCadastro;
        }

        public bool CadastrarFilmePorMenu(List<string> entradas)
        {
            if (entradas == null || entradas.Count == 0)
            {
                return false;
            }
            try
            {
                Filme filme = _filmeRepository.CreateFilme(entradas);
                bool boolCadastro = CadastrarFilme(filme);
                _menu.CadastroFilmeSucesso();
                return boolCadastro;
            }
            catch
            {
                _menu.ErroAoCadastrarProduto();
                return false;
            }
        }

        public bool CadastrarLivroPorMenu(List<string> entradas)
        {
            if (entradas == null || entradas.Count == 0)
            {
                return false;
            }
            try
            {
                Livro livro = _livroRepository.CreateLivro(entradas);
                bool boolCadastro = CadastrarLivro(livro);
                _menu.CadastroLivroSucesso();
                return boolCadastro;
            }
            catch 
            {
                _menu.ErroAoCadastrarProduto();
                return false;
            }
        }

        public Filme BuscarFilmePorId(int filmeId)
        {
            ValidateId(filmeId);
            return _filmeRepository.GetById(filmeId);
        }

        public Livro BuscarLivroPorId(int livroId)
        {
            ValidateId(livroId);
            return _livroRepository.GetById(livroId);
        }

        public Filme BuscarFilmePorTitulo(string titulo)
        {
            if (string.IsNullOrWhiteSpace(titulo))
            {
                throw new ArgumentException("O título não pode ser vazio.", nameof(titulo));
            }
            return _filmeRepository.GetByTitle(titulo);
        }

        public Livro BuscarLivroPorTitulo(string titulo)
        {
            if (string.IsNullOrWhiteSpace(titulo))
            {
                throw new ArgumentException("O título não pode ser vazio.", nameof(titulo));
            }
            return _livroRepository.GetByTitle(titulo);
        }

        public void ExibirTodosFilmes()
        {
            List<Filme> listaDeFilmes = _filmeRepository.GetAll();

            _menu.TituloInformacoesFilmes();
            foreach (var filme in listaDeFilmes)
            {
                _menu.ExibirInformacoes(filme);
            }
            _menu.MensagemSairMenu();
        }

        public void ExibirTodosLivros()
        {
            List<Livro> listaDeLivros = _livroRepository.GetAll();
            _menu.TituloInformacoesLivros();
            foreach (var livro in listaDeLivros)
            {
                _menu.ExibirInformacoes(livro);
            }
            _menu.MensagemSairMenu();
        }
        public void DeletarFilme(int filmeId)
        {
            if (VerifyIdFilmeExist(filmeId))
            {
                _filmeRepository.Delete(filmeId);
                _menu.ConfirmacaoExcluirFilme();
            }
            else
            {
                _menu.IdNaoEncontrado();
            }
        }

        public void DeletarLivro(int livroId)
        {
            if (VerifyIdLivroExist(livroId))
            {
                _livroRepository.Delete(livroId);
                _menu.ConfirmacaoExcluirLivro();
            }
            else
            {
                _menu.IdNaoEncontrado();
            }
        }
        public bool AtualizarFilme(Filme filme)
        {
            ValidateProduto(filme);
            return _filmeRepository.GeneralUpdate(filme);
        }
        public bool AtualizarLivro(Livro livro)
        {
            ValidateProduto(livro);
            return _livroRepository.GeneralUpdate(livro);
        }
        public bool SetStockFilme(int id, int estoqueAtual)
        {
            return _filmeRepository.SetStockFilme(id, estoqueAtual);
        }

        public bool SetStockLivro(int id, int estoqueAtual)
        {
            ValidateId(id);
            if (estoqueAtual < 0)
            {
                throw new ArgumentException("A quantidade em estoque não pode ser negativa.", nameof(estoqueAtual));
            }
            return _livroRepository.SetStockLivro(id, estoqueAtual);
        }
        public bool AddStock(Filme filme, int quantidade)
        {
            ValidateQuantidade(quantidade);
            return _filmeRepository.AddStock(filme, quantidade);
        }

        public bool AddStock(Livro livro, int quantidade)
        {
            ValidateQuantidade(quantidade);
            return _livroRepository.AddStock(livro, quantidade);
        }

        public bool RemoveStock(Filme filme, int quantidade)
        {
            ValidateQuantidade(quantidade);
            return _filmeRepository.DecStock(filme, quantidade);
        }

        public bool RemoveStock(Livro livro, int quantidade)
        {
            ValidateQuantidade(quantidade);
            return _livroRepository.DecStock(livro, quantidade);
        }

        public bool VerifyStringNotEmpity(string texto)
        {
            return ValidateStringNotEmpty(texto);
        }
        public int ConvertStringForInt(string texto)
        {
            try
            {
                if (int.TryParse(texto, out int opcao))
                {
                    return opcao;
                }
                else return -1;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ocorreu um erro ao processar a entrada: " + ex.Message);
                return -1;
            }
        }

        public bool VerifyIdFilmeExist(int id)
        {
            ValidateId(id);

            List<Filme> listaDeFilmes = _filmeRepository.GetAll();
            foreach (Filme filme in listaDeFilmes)
            {
                if (filme.Id == id)
                {
                    return true;
                }
            }
            
            return false;
        }

        public bool VerifyIdLivroExist(int id)
        {
            ValidateId(id);

            List<Livro> listaDeLivros = _livroRepository.GetAll();

            foreach (Livro livro in listaDeLivros)
            {
                if (livro.Id == id)
                {
                    return true;
                }
            }

            return false;
        }

        private bool ValidateStringNotEmpty(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
            {
                return false;
            }
            else return true;
        }
        private void ValidateProduto(Filme filme)
        {
            if (filme == null)
            {
                throw new ArgumentNullException(nameof(filme), "O filme não pode ser nulo.");
            }
            if (string.IsNullOrWhiteSpace(filme.Titulo))
            {
                throw new ArgumentException("O título do filme não pode ser vazio.", nameof(filme.Titulo));
            }
            if (filme.Preco <= 0)
            {
                throw new ArgumentException("O preço do filme deve ser maior que zero.", nameof(filme.Preco));
            }
            if (filme.QuantidadeEstoque < 0)
            {
                throw new ArgumentException("A quantidade em estoque não pode ser zero ou negativa.", nameof(filme.QuantidadeEstoque));
            }
        }

        private void ValidateProduto(Livro livro)
        {
            if (livro == null)
            {
                throw new ArgumentNullException(nameof(livro), "O livro não pode ser nulo.");
            }
            if (string.IsNullOrWhiteSpace(livro.Titulo))
            {
                throw new ArgumentException("O título do livro não pode ser vazio.", nameof(livro.Titulo));
            }
            if (livro.Preco <= 0)
            {
                throw new ArgumentException("O preço do livro deve ser maior que zero.", nameof(livro.Preco));
            }
            if (livro.QuantidadeEstoque < 0)
            {
                throw new ArgumentException("A quantidade em estoque não pode ser negativa.", nameof(livro.QuantidadeEstoque));
            }
        }

        private void ValidateId(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("O ID deve ser maior que zero.", nameof(id));
            }
        }

        private void ValidateQuantidade(int quantidade)
        {
            if (quantidade <= 0)
            {
                throw new ArgumentException("A quantidade deve ser maior que zero.", nameof(quantidade));
            }
        }

        private void ValidateList(List<string> entradas)
        {
            try
            {
                decimal.Parse(entradas[4]);
            }
            catch
            {
                throw new ArgumentException("Entrada inválida.");
            }

            try
            {
                int.Parse(entradas[5]);
            }
            catch
            {
                throw new ArgumentException("Entrada inválida.");
            }
        }
    }
}
