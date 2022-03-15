using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Windows.Forms;
using MKSoft.PIX;

namespace MKSoft.PIX.Tests
{
   [TestClass]
   public class TestePix
   {
      [TestMethod]
      public void GeracaoDoArquivo()
      {
         GeradorPix g = new GeradorPix
         {
            ChavePix = "49002221000198",
            CidadeLoja = "Rebouças-PR",
            DescricaoPagamento = "Pagamento de Teste",
            IdentificadorPix = "123456",
            NomeLoja = "Empresa de teste",
            ValorPix = Convert.ToDecimal("0,01")
         };

         File.WriteAllText(
            @"D:\dev\MKSoft.PIX\MKSoft.PIX.Tests\bin\Debug\ChaveQrCode.txt",
            g.GerarChaveQrCode());

         const string aquivoQrCode = @"D:\dev\Pacotes_MKSoftwares\MKSoft.PIX\MKSoft.PIX.Tests\bin\Debug\QrCodePix.bmp";
         g.GerarImagemQrCode(aquivoQrCode);

         Assert.IsTrue(File.Exists(aquivoQrCode));
      }

      [TestMethod]
      public void GeracaoDoArquivoInterop()
      {
         GeradorPixInterop g = new GeradorPixInterop
         {
            ChavePix = "49002221000198",
            CidadeLoja = "Rebouças/PR",
            DescricaoPagamento = "Pagamento de Teste",
            IdentificadorPix = "123456",
            NomeLoja = "Loja de Teste",
            ValorPix = Convert.ToDouble("1.856,52")
         };

         const string aquivoQrCode = @"D:\dev\MKSoft.PIX\MKSoft.PIX.Tests\bin\Debug\QrCodePixInterop.bmp";
         g.GerarImagemQrCode( aquivoQrCode);

         Assert.IsTrue(File.Exists(aquivoQrCode));
      }
   }
}
