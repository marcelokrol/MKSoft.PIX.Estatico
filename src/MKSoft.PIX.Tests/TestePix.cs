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
            ChavePix = "32249888000197",
            CidadeLoja = "Rebouças-PR",
            DescricaoPagamento = "Pagamento de Teste",
            IdentificadorPix = "123456",
            NomeLoja = "R.F. Comercio de Alimentos Eireli",
            ValorPix = Convert.ToDecimal("0,01")
         };

         File.WriteAllText(
            @"D:\dev\Pacotes_MKSoftwares\MKSoft.PIX\MKSoft.PIX.Tests\bin\Debug\ChaveQrCode.txt",
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
            ChavePix = "13751890000115",
            CidadeLoja = "Rebouças/PR",
            DescricaoPagamento = "Pagamento de Teste",
            IdentificadorPix = "123456",
            NomeLoja = "Loja de Teste",
            ValorPix = Convert.ToDouble("1.856,52")
         };

         const string aquivoQrCode = @"D:\dev\Pacotes_MKSoftwares\MKSoft.PIX\MKSoft.PIX.Tests\bin\Debug\QrCodePixInterop.bmp";
         g.GerarImagemQrCode( aquivoQrCode);

         Assert.IsTrue(File.Exists(aquivoQrCode));
      }
   }
}
