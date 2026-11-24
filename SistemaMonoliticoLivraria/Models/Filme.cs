using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaMonoliticoLivraria.Models
{
    internal class Filme
    {

        public int Id { get; private set; }
        public string Titulo { get; set; }
        public string Genero { get; set; }
        public string Diretor { get; set; }
        public decimal Preco { get; set; }
        public int QuantidadeEstoque { get; set; }

        public Filme(string titulo, string genero, string diretor, decimal preco, int quantidadeEstoque)
        {            
            Titulo = titulo;
            Genero = genero;
            Diretor = diretor;
            Preco = preco;
            QuantidadeEstoque = quantidadeEstoque;
        }

        public Filme(int id, string titulo, string genero, string diretor, decimal preco, int quantidadeEstoque)
        {
            Id = id;
            Titulo = titulo;
            Genero = genero;
            Diretor = diretor;
            Preco = preco;
            QuantidadeEstoque = quantidadeEstoque;
        }



    }
}
