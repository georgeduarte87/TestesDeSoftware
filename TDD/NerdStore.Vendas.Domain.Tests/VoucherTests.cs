using System;
using System.Linq;
using Xunit;

namespace NerdStore.Vendas.Domain.Tests
{
    public class VoucherTests
    {
        [Fact(DisplayName = "Validar Voucher Tipo Valor Valido")]
        [Trait("Categoria","Vendas - Voucher")]
        public void Voucher_ValidarVoucherTipoValor_DeveEstarValido()
        {
            /*
            var voucher = new Voucher
            {
                Codigo = "PROMO-15REAIS",
                ValorDesconto = 15,
                PercentualDesconto = null,
                Quantidade = 1,
                DataValidade = DateTime.Now,
                Ativo = true,
                Utilizado = false
            };*/

            var voucher = new Voucher("PROMO-15REAIS", null, 15, 1, TipoDescontoVoucher.Valor, DateTime.Now.AddDays(15), true, false);

            // Act
            var result = voucher.ValidarSeAplicavel();

            // Assert
            //Assert.True(result);
            Assert.True(result.IsValid);
        }

        [Fact(DisplayName = "Validar Vocuher Tipo Valor Inválido")]
        [Trait("Categoria", "Vendas - Voucher")]
        public void Voucher_ValidarVoucherTipoValor_DeveEstarInvalido()
        {
            // Arrange
            var voucher = new Voucher("", null, null, 0, TipoDescontoVoucher.Valor, DateTime.Now.AddDays(-1), false, true);

            // Act
            var result = voucher.ValidarSeAplicavel();

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(6, result.Errors.Count);
            Assert.Contains(VoucherAplicavelValidation.AtivoErroMsg, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(VoucherAplicavelValidation.CodigoErroMsg, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(VoucherAplicavelValidation.DataValidadeErroMsg, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(VoucherAplicavelValidation.QuantidadeErroMsg, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(VoucherAplicavelValidation.UtilizadoErroMsg, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(VoucherAplicavelValidation.ValorDescontoErroMsg, result.Errors.Select(c => c.ErrorMessage));
        }

        [Fact(DisplayName = "Validar Voucher Percentagem Válido")]
        [Trait("Categoria", "Vendas - Voucher")]
        public void Voucher_ValidarVoucherPorcentagem_DeveEstarValido()
        {
            // Arrange
            var voucher = new Voucher("PROMO-15OFF", 15, null, 1, TipoDescontoVoucher.Porcentagem, DateTime.Now.AddDays(15), true, false);

            // Act
            var result = voucher.ValidarSeAplicavel();

            // Assert
            Assert.True(result.IsValid);
        }

        [Fact(DisplayName = "Validar Voucher Percentagem Inválido")]
        [Trait("Categoria", "Vendas - Voucher")]
        public void Voucher_ValidarVoucherPorcentagem_DeveEstarInalido()
        {
            // Arrange
            var voucher = new Voucher("", null, null, 0, TipoDescontoVoucher.Porcentagem, DateTime.Now.AddDays(-1), false, true);

            // Act
            var result = voucher.ValidarSeAplicavel();

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(6, result.Errors.Count);
            Assert.Contains(VoucherAplicavelValidation.AtivoErroMsg, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(VoucherAplicavelValidation.CodigoErroMsg, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(VoucherAplicavelValidation.DataValidadeErroMsg, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(VoucherAplicavelValidation.QuantidadeErroMsg, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(VoucherAplicavelValidation.UtilizadoErroMsg, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(VoucherAplicavelValidation.PercentualDescontoErroMsg, result.Errors.Select(c => c.ErrorMessage));
        }
    }
}
