using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaMonoliticoLivraria.Models
{
    internal class Livro
    {

        public int Id { get; private set; }
        public string Titulo { get; set; }
        public string Genero { get; set; }
        public string Autor { get; set; }
        public decimal Preco { get; set; }
        public int QuantidadeEstoque { get; set; }

        public Livro(string titulo, string genero, string autor, decimal preco, int quantidadeEstoque)
        {
            Titulo = titulo;
            Genero = genero;
            Autor = autor;
            Preco = preco;
            QuantidadeEstoque = quantidadeEstoque;
        }
        public Livro(int id, string titulo, string genero, string autor, decimal preco, int quantidadeEstoque)
        {
            Id = id;
            Titulo = titulo;
            Genero = genero;
            Autor = autor;
            Preco = preco;
            QuantidadeEstoque = quantidadeEstoque;
        }

    }
}
