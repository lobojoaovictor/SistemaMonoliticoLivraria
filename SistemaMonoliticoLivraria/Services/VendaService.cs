using SistemaMonoliticoLivraria.Data;
using SistemaMonoliticoLivraria.Models;
using SistemaMonoliticoLivraria.UI;
using System;
using System.Collections.Generic;

namespace SistemaMonoliticoLivraria.Services
{
    internal class VendaService
    {
        private readonly Database _database;
        private readonly VendaRepository _vendaRepository;
        private readonly FilmeRepository _filmeRepository;
        private readonly LivroRepository _livroRepository;
        private readonly Menu _menu;
        public VendaService(Database database)
        {
            _database = database;
            _vendaRepository = new VendaRepository(_database);
            _filmeRepository = new FilmeRepository(_database);
            _livroRepository = new LivroRepository(_database);
            _menu = new Menu();
        }

        public bool VenderProduto(Venda venda)
        {
            ValidateVenda(venda);
            if (!RemoveStock(venda))
            {
                throw new ArgumentException("Quantidade em estoque insuficiente.");
            }

            try
            {
                VerifyInsertion(venda);
                return true;
            }
            catch
            {
                RestoreStock(venda);
                throw;
            }
        }

        public bool VenderPorMenu(List<string> entradas, string tipoProduto)
        {
            if (entradas == null || entradas.Count != 2)
            {
                return false;
            }

            string titulo = entradas[0].Trim();
            if (string.IsNullOrWhiteSpace(titulo))
            {
                throw new ArgumentException("O título do produto não pode ser vazio.", nameof(entradas));
            }

            if (!int.TryParse(entradas[1].Trim(), out int quantidade))
            {
                throw new ArgumentException("A quantidade informada é inválida.", nameof(entradas));
            }

            DateTime agora = DateTime.Now;
            Venda venda;
            if (string.Equals(tipoProduto, "Livro", StringComparison.OrdinalIgnoreCase))
            {
                Livro livro = _livroRepository.GetByTitle(titulo);
                if (livro == null)
                {
                    throw new ArgumentException("Livro não encontrado.", nameof(titulo));
                }
                venda = new Venda(livro.Titulo, "Livro", livro.Preco, quantidade, agora.Date, agora.TimeOfDay, livro.Id);
            }
            else if (string.Equals(tipoProduto, "Filme", StringComparison.OrdinalIgnoreCase))
            {
                Filme filme = _filmeRepository.GetByTitle(titulo);
                if (filme == null)
                {
                    throw new ArgumentException("Filme não encontrado.", nameof(titulo));
                }
                venda = new Venda(filme.Titulo, "Filme", filme.Preco, quantidade, agora.Date, agora.TimeOfDay, filme.Id);
            }
            else
            {
                throw new ArgumentException("Tipo de produto inválido.", nameof(tipoProduto));
            }

            return VenderProduto(venda);
        }
        private void VerifyInsertion(Venda venda)
        {
            if (!_vendaRepository.Insert(venda))
            {
                throw new InvalidOperationException("Não foi possível realizar a venda.");
            }
        }
        private bool RemoveStock(Venda venda)
        {
            if (venda.TipoProduto == "Livro")
            {
                return _livroRepository.DecStock(_livroRepository.GetById(venda.IdProduto), venda.QuantidadeVendida);
            }
            if (venda.TipoProduto == "Filme")
            {
                return _filmeRepository.DecStock(_filmeRepository.GetById(venda.IdProduto), venda.QuantidadeVendida);
            }
            return false;
        }

        private void RestoreStock(Venda venda)
        {
            if (venda.TipoProduto == "Livro")
            {
                _livroRepository.AddStock(_livroRepository.GetById(venda.IdProduto), venda.QuantidadeVendida);
            }
            else if (venda.TipoProduto == "Filme")
            {
                _filmeRepository.AddStock(_filmeRepository.GetById(venda.IdProduto), venda.QuantidadeVendida);
            }
        }

        private void ValidateVenda(Venda venda)
        {
            if (venda == null)
            {
                throw new ArgumentNullException(nameof(venda));
            }
            if (venda.QuantidadeVendida <= 0)
            {
                throw new ArgumentException("A quantidade vendida deve ser maior que zero.");
            }
            if (venda.Preco <= 0)
            {
                throw new ArgumentException("O preço do produto deve ser maior que zero.");
            }
        }

        public void ExibirVendasFilmes()
        {
            List<Venda> listaDeVendasFilmes = _vendaRepository.GetAllFilmes();

            foreach (var venda in listaDeVendasFilmes)
            {
                _menu.ExibirInformacoes(venda);
            }
        }

        public void ExibirVendasLivros()
        {
            List<Venda> listaDeVendasLivros = _vendaRepository.GetAllLivros();
            foreach (var venda in listaDeVendasLivros)
            {
                _menu.ExibirInformacoes(venda);
            }
        }
        public void ExibirRelatorioCompleto()
        {
            List<Venda> listaDeVendas = _vendaRepository.GetAllComJoin();
        
            foreach (var venda in listaDeVendas)
            {
                _menu.ExibirInformacoes(venda);
            }
        }
    }
}
