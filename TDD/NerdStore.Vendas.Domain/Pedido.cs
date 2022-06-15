using System;
using System.Linq;
using System.Collections.Generic;

namespace NerdStore.Vendas.Domain
{
    public class Pedido
    {
        public Guid ClienteId { get; private set; }

        public decimal ValorTotal { get; private set; }

        public PedidoStatus PedidoStatus { get; private set; }

        private readonly List<PedidoItem> _pedidoitems;

        public IReadOnlyCollection<PedidoItem> PedidoItems => _pedidoitems;


        protected Pedido()
        {
            _pedidoitems = new List<PedidoItem>();
        }

        public void CalcularValorPedido()
        {
            ValorTotal = PedidoItems.Sum(i => i.CalcularValor());
        }


        public void AdicionarItem(PedidoItem pedidoItem)
        {
            //ValorTotal = 200; Mocando resultado apenas para passar

            if(_pedidoitems.Any(p => p.ProdutoId == pedidoItem.ProdutoId))
            {
                var itemExistente = _pedidoitems.FirstOrDefault(p => p.ProdutoId == pedidoItem.ProdutoId);
                itemExistente.AdicionarUnidades(pedidoItem.Quantidade);
                pedidoItem = itemExistente;

                _pedidoitems.Remove(itemExistente);
            }

            _pedidoitems.Add(pedidoItem);
            //ValorTotal = PedidoItems.Sum(i => i.Quantidade * i.ValorUnitario);
            CalcularValorPedido();
        }

        public static class PedidoFactory
        {
            public static Pedido NovoPedidoRascunho(Guid clienteId)
            {
                var pedido = new Pedido
                {
                    ClienteId = clienteId
                };

                pedido.TornarRascunho();
                return pedido;
            }
        }

        public void TornarRascunho()
        {
            PedidoStatus = PedidoStatus.Rascunho;
        }
    }

    public enum PedidoStatus
    {
        Rascunho = 0,
        Iniciado = 1,
        Pago = 4,
        Entregue = 5,
        Cancelado = 6
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

        internal void AdicionarUnidades(int unidades)
        {
            Quantidade += unidades;
        }

        internal decimal CalcularValor()
        {
            return Quantidade * ValorUnitario;
        }
    }
}
