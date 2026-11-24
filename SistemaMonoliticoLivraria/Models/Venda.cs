using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaMonoliticoLivraria.Models
{
    internal class Venda
    {

        public int Id { get; private set; }
        public string Produto { get; set; }
        public string TipoProduto { get; set; }
        public decimal Preco { get; set; }
        public int QuantidadeVendida { get; set; }
        public decimal TotalVenda { get; set; }
        public DateTime DataVenda { get; set; }
        public TimeSpan HoraVenda { get; set; }
        public int IdProduto { get; set; }

        public Venda(int id, string produto, string tipoProduto, decimal preco, int quantidadeVendida, decimal totalVenda, DateTime dataVenda, TimeSpan horaVenda, int idProduto)
        {
            Id = id;
            Produto = produto;
            TipoProduto = tipoProduto;
            Preco = preco;
            QuantidadeVendida = quantidadeVendida;
            TotalVenda = QuantidadeVendida * Preco;
            DataVenda = dataVenda;
            HoraVenda = horaVenda;
            IdProduto = idProduto;
        }

        public Venda(string produto, string tipoProduto, decimal preco, int quantidadeVendida, DateTime dataVenda, TimeSpan horaVenda, int idProduto)
        {
            IdProduto = idProduto;
            Produto = produto;
            TipoProduto = tipoProduto;
            Preco = preco;
            QuantidadeVendida = quantidadeVendida;
            TotalVenda = QuantidadeVendida * Preco;
            DataVenda = dataVenda;
            HoraVenda = horaVenda;
        }       
    }
}
