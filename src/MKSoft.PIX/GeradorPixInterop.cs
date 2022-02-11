using Microsoft.VisualBasic;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace MKSoft.PIX
{
   [ComVisible(true)]
   [ComClass(ClassId, InterfaceId, EventsId)]
   [ClassInterface(ClassInterfaceType.AutoDual)]
   public class GeradorPixInterop
   {

      #region "COM GUIDs"

      // These  GUIDs provide the COM identity for this class 
      // and its COM interfaces. If you change them, existing 
      // clients will no longer be able to access the class.
      public const string ClassId = "{B25FA244-A2AA-4BCB-B709-22C14E80F89D}";
      public const string InterfaceId = "{E373E76C-B7B6-47EB-B556-1DDF1B755555}";
      public const string EventsId = "{B2A0A8F3-8AB7-43C7-B27E-36EB55EEB603}";

      #endregion

      /// <summary>Chave PIX cadastrada no BACEN</summary>
      public string ChavePix { get; set; }

      /// <summary>Descrição para o pagamento. Ex.: "Venda no mercado da esquina" </summary>
      public string DescricaoPagamento { get; set; }

      /// <summary>Valor do PIX </summary>
      public double ValorPix { get; set; }

      /// <summary>Identificador para o PIX. Pode ser o número da venda, ex.: 123456</summary>
      public string IdentificadorPix { get; set; }

      /// <summary>Decritivo com o nome da loja. Melhor usar o nome fantasia. </summary>
      public string NomeLoja { get; set; }

      /// <summary>Cidade da Loja </summary>
      public string CidadeLoja { get; set; }

      /// <summary>Cria um arquivo de imagem, no formato BMP com o QR-Code do PIX.</summary>
      /// <param name="ondeSalvar">Local e nome do arquivo que será salvo</param>
      public void GerarImagemQrCode(string ondeSalvar)
      {
         using var geradorPix = new GeradorPix
         {
            ChavePix = ChavePix,
            DescricaoPagamento = DescricaoPagamento,
            ValorPix = Convert.ToDecimal(ValorPix),
            IdentificadorPix = IdentificadorPix,
            NomeLoja = NomeLoja,
            CidadeLoja = CidadeLoja
         };

         geradorPix.GerarImagemQrCode(ondeSalvar);
      }

      /// <summary>Gera o código 'Copia e Cola' do pix, ou seja, o conteudo do QR-Code.</summary>
      /// <returns>Conteudo do código QR-Code</returns>
      public string GerarChaveQrCode()
      {
         using var geradorPix = new GeradorPix
         {
            ChavePix = ChavePix,
            DescricaoPagamento = DescricaoPagamento,
            ValorPix = Convert.ToDecimal(ValorPix),
            IdentificadorPix = IdentificadorPix,
            NomeLoja = NomeLoja,
            CidadeLoja = CidadeLoja
         };

         return geradorPix.GerarChaveQrCode();

         //var arquivo = GeraArquivoImagemQrCode();

         //var fileInfo = new FileInfo(arquivo);
         //var arquivoBmp = Path.Combine(fileInfo.DirectoryName, fileInfo.Name);

         //var fileTemp = Path.GetTempFileName();

         //var Dummy = Image.FromFile(arquivo);
         //Dummy.Save(fileTemp, ImageFormat.Bmp);
         //Dummy.Dispose();

         //if (File.Exists(arquivoBmp))
         //   File.Delete(arquivoBmp);

         //File.Move(fileTemp, arquivoBmp);

         //return arquivoBmp;
      }
   }
}