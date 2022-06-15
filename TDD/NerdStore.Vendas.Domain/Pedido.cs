using System;
using System.Linq;
using System.Collections.Generic;

namespace NerdStore.Vendas.Domain
{
    public class Pedido
    {
        public decimal ValorTotal { get; private set; }

        private readonly List<PedidoItem> _pedidoitems;

        public IReadOnlyCollection<PedidoItem> PedidoItems => _pedidoitems;


        public Pedido()
        {
            _pedidoitems = new List<PedidoItem>();
        }


        public void AdicionarItem(PedidoItem pedidoItem)
        {
            //ValorTotal = 200; Mocando resultado apenas para passar

            _pedidoitems.Add(pedidoItem);
            ValorTotal = PedidoItems.Sum(i => i.Quantidade * i.ValorUnitario);
        }
    }

    public class PedidoItem
    {
        public Guid ProdutoId { get; private set; }
        public string ProdutoNome { get; private set; }
        public int Quantidade { get; private set; }
        public decimal ValorUnitario { get; private set; }

        public PedidoItem(Guid produtoId, string produtoNome, int quantidade, decimal valorUnitario)
        {
            ProdutoId = produtoId;
            ProdutoNome = produtoNome;
            Quantidade = quantidade;
            ValorUnitario = valorUnitario;
        }
    }
}
