using System;
using System.Linq;
using System.Collections.Generic;
using NerdStore.Core.DomainObjects;
using FluentValidation.Results;

namespace NerdStore.Vendas.Domain
{
    public class Pedido : Entity, IAggregateRoot
    {
        public static int MAX_UNIDADES_ITEM => 15;
        public static int MIN_UNIDADES_ITEM => 1;

        public Guid ClienteId { get; private set; }

        public decimal ValorTotal { get; private set; }

        public decimal Desconto { get; private set; }

        public PedidoStatus PedidoStatus { get; private set; }

        public bool VoucherUtilizado { get; private set; }

        public Voucher Voucher { get; private set; }

        private readonly List<PedidoItem> _pedidoitems;

        public IReadOnlyCollection<PedidoItem> PedidoItems => _pedidoitems;


        protected Pedido()
        {
            _pedidoitems = new List<PedidoItem>();
        }

        private void CalcularValorPedido()
        {
            ValorTotal = PedidoItems.Sum(i => i.CalcularValor());
            CalcularValorTotalDesconto();
        }

        private bool PedidoItemExistente(PedidoItem item)
        {
            return _pedidoitems.Any(p => p.ProdutoId == item.ProdutoId);
        }

        private void ValidarQuantidadeItemPermitida(PedidoItem item)
        {
            var quantidadeItens = item.Quantidade;

            if(PedidoItemExistente(item))
            {
                var itemExistente = _pedidoitems.FirstOrDefault(p => p.ProdutoId == item.ProdutoId);
                quantidadeItens += itemExistente.Quantidade;
            }

            if(quantidadeItens > MAX_UNIDADES_ITEM) throw new DomainException($"Máximo de {MAX_UNIDADES_ITEM} unidades por produto.");
        }

        private void ValidarPedidoItemInexistente(PedidoItem pedidoItem)
        {
            if (!PedidoItemExistente(pedidoItem)) throw new DomainException("O item não existe no pedido");
        }


        public ValidationResult AplicarVoucher(Voucher voucher)
        {
            var result = voucher.ValidarSeAplicavel();
            if (!result.IsValid) return result;

            Voucher = voucher;
            VoucherUtilizado = true;

            //ValorTotal -= voucher.ValorDesconto.Value;  Antes de refatorar
            CalcularValorTotalDesconto();

            return result;
        }

        public void CalcularValorTotalDesconto()
        {
            if (!VoucherUtilizado) return;

            decimal desconto = 0;

            if(Voucher.TipoDescontoVoucher == TipoDescontoVoucher.Valor)
            {
                if(Voucher.ValorDesconto.HasValue)
                {
                    desconto = Voucher.ValorDesconto.Value;
                }
            }
            else
            {
                if (Voucher.PercentualDesconto.HasValue)
                {
                    desconto = (ValorTotal * Voucher.PercentualDesconto.Value) / 100;
                }
            }

            ValorTotal = ValorTotal < desconto ? 0 : ValorTotal -= desconto;  
            Desconto = desconto;
        }

        public void AdicionarItem(PedidoItem pedidoItem)
        {
            // ValorTotal = 200; Mocando resultado apenas para passar

            // Movido pós refatoração para classe pedido item
            // if (pedidoItem.Quantidade < Pedido.MIN_UNIDADES_ITEM) throw new DomainException($"Mínimo de {Pedido.MIN_UNIDADES_ITEM} unidades por produto.");

            //if (pedidoItem.Quantidade > MAX_UNIDADES_ITEM) throw new DomainException($"Máximo de {MAX_UNIDADES_ITEM} unidades por produto.");

            ValidarQuantidadeItemPermitida(pedidoItem);

            //if (_pedidoitems.Any(p => p.ProdutoId == pedidoItem.ProdutoId))
            if (PedidoItemExistente(pedidoItem))
            {
                //var quantidadeItens = pedidoItem.Quantidade;
                var itemExistente = _pedidoitems.FirstOrDefault(p => p.ProdutoId == pedidoItem.ProdutoId);

                //if(quantidadeItens + itemExistente.Quantidade > MAX_UNIDADES_ITEM) throw new DomainException($"Máximo de {MAX_UNIDADES_ITEM} unidades por produto.");

                itemExistente.AdicionarUnidades(pedidoItem.Quantidade);
                pedidoItem = itemExistente;

                _pedidoitems.Remove(itemExistente);
            }

            _pedidoitems.Add(pedidoItem);
            //ValorTotal = PedidoItems.Sum(i => i.Quantidade * i.ValorUnitario);
            CalcularValorPedido();
        }

        public void AtualizarItem(PedidoItem pedidoItem)
        {
            ValidarPedidoItemInexistente(pedidoItem);

            ValidarQuantidadeItemPermitida(pedidoItem);

            var itemExistente = PedidoItems.FirstOrDefault(p => p.ProdutoId == pedidoItem.ProdutoId);

            _pedidoitems.Remove(itemExistente);
            _pedidoitems.Add(pedidoItem);

            CalcularValorPedido();
        }

        public void RemoverItem(PedidoItem pedidoItem)
        {
            ValidarPedidoItemInexistente(pedidoItem);

            _pedidoitems.Remove(pedidoItem);

            CalcularValorPedido();
        }

        public void TornarRascunho()
        {
            PedidoStatus = PedidoStatus.Rascunho;
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
    }
}
