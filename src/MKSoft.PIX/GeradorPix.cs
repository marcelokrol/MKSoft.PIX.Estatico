//using MessagingToolkit.QRCode.Codec;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using QRCoder;


namespace MKSoft.PIX
{
   public class GeradorPix : IDisposable
   {
      /// <summary>Chave PIX cadastrada no BACEN</summary>
      public string ChavePix { get; set; }

      /// <summary>Descrição para o pagamento. Ex.: "Venda no mercado da esquina" </summary>
      public string DescricaoPagamento { get; set; }

      /// <summary>Valor do PIX </summary>
      public decimal ValorPix { get; set; }

      /// <summary>Identificador para o PIX. Pode ser o número da venda, ex.: 123456</summary>
      public string IdentificadorPix { get; set; }

      /// <summary>Decritivo com o nome da loja. Melhor usar o nome fantasia. </summary>
      public string NomeLoja { get; set; }

      /// <summary>Cidade da Loja </summary>
      public string CidadeLoja { get; set; }

      // <summary>Gera um arquivo PIX.PNG na pasta atual, e retorna o caminho completo do arquivo gerado. </summary>
      //public string Gerar_QrCode_Pix()
      //{



      //var cobranca = new Cobranca(ChavePix) { SolicitacaoPagador = LimparTexto(DescricaoPagamento) };


      //var payload = cobranca.ToPayload(IdentificadorPix, new Merchant(LimparTexto(NomeLoja), LimparTexto(CidadeLoja)));
      //var stringToQrCode = payload.GenerateStringToQrCode();

      //using var qrGenerator = new QRCodeGenerator();

      ////var stringQrCode = stringToQrCode;
      //var qrCodeData = qrGenerator.CreateQrCode(stringToQrCode, QRCodeGenerator.ECCLevel.L);
      //using var qrCode = new QRCode(qrCodeData);

      //var qrCodeImage = qrCode.GetGraphic(20);

      //File.WriteAllBytes(CaminhoArquivoImagemQrCode, ImageToByte(qrCodeImage));

      //return CaminhoArquivoImagemQrCode;


      //PictureBox1.Image = qrCodeImage;


      //// ----------------- QRCODE ------------------
      //QRCodeEncoder qrencod = new QRCodeEncoder();
      //Bitmap qrcode_2 = qrencod.Encode(stringToQrCode);
      //PictureBox picQRCode = new PictureBox();
      //// picQRCode.Image = TryCast(qrcode_2, Image)
      //// PictureBox2.Image = TryCast(qrcode_2, Image)
      //PictureBox2.Image = qrcode_2;
      //}

      /// <summary>Cria um arquivo de imagem, no formato BMP com o QR-Code do PIX.</summary>
      /// /// <param name="ondeSalvar">Local e nome do arquivo que será salvo</param>
      public void GerarImagemQrCode(string ondeSalvar)
      {
         try
         {
            var conteudoQrCode = GerarChaveQrCode();

            var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(conteudoQrCode, QRCodeGenerator.ECCLevel.L);
            var qrCode = new QRCode(qrCodeData);
            var qrCodeImage = qrCode.GetGraphic(20);

            var ondeSalvarTemp = $"{ondeSalvar}.tmp";

            if (File.Exists(ondeSalvar)) File.Delete(ondeSalvar);
            if (File.Exists(ondeSalvarTemp)) File.Delete(ondeSalvarTemp);
            
            File.WriteAllBytes(ondeSalvarTemp, ImageToByte(qrCodeImage));

            var Dummy = Image.FromFile(ondeSalvarTemp);
            Dummy.Save(ondeSalvar, ImageFormat.Bmp);
            Dummy.Dispose();

            if (File.Exists(ondeSalvarTemp)) File.Delete(ondeSalvarTemp);
         }
         catch (Exception e)
         {
            Console.WriteLine(e);
            throw;
         }




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

      /// <summary>Gera o código 'Copia e Cola' do pix, ou seja, o conteudo do QR-Code.</summary>
      /// <returns>Conteudo do código QR-Code</returns>
      public string GerarChaveQrCode()
      {
         DescricaoPagamento = LimparTexto(DescricaoPagamento);
         NomeLoja = LimparTexto(NomeLoja);
         CidadeLoja = LimparTexto(CidadeLoja);
         IdentificadorPix = LimparTexto(IdentificadorPix);

         if (DescricaoPagamento.Length > 25)
            DescricaoPagamento = DescricaoPagamento.Substring(0,25);

         if (NomeLoja.Length > 25)
            NomeLoja = NomeLoja.Substring(0, 25);

         if (CidadeLoja.Length > 25)
            CidadeLoja = CidadeLoja.Substring(0, 25);

         if (IdentificadorPix.Length > 25)
            IdentificadorPix = IdentificadorPix.Substring(0, 25);

         try
         {
            var codigo = new StringBuilder();

            //00 - Payload Format Indicator, tamanho 02
            codigo.Append("00" + "02" + "01");

            //01 - Point of Initiation Method
            //codigo.Append("01" + "02");
            //codigo.Append(pPagamentoUnico ? "12" : "11"); //“11” (QR reutilizável) ou “12” (QR utilizável apenas uma vez)

            //26 - Merchant Account Information
            codigo.Append("26");

            //arranjo GUI banco central
            const string tValueArranjo25 = "BR.GOV.BCB.PIX";
            var tPix2600 = ("00" + (tValueArranjo25.Length.ToString().PadLeft(2, (char)48) + tValueArranjo25));
            var tPix2601 = ("01" + (ChavePix.Length.ToString().PadLeft(2, (char)48) + ChavePix));
            codigo.Append((tPix2600.Length + tPix2601.Length).ToString().PadLeft(2, (char)48));
            codigo.Append(tPix2600);
            codigo.Append(tPix2601);

            //52 - Merchant Category Code (“0000” ou MCC ISO18245)
            codigo.Append("52" + "04" + "0000");

            //53 - Transaction Currency (“986” – BRL: real brasileiro - ISO4217)
            codigo.Append("53" + "03" + "986");

            //54 - Transaction Amount
            codigo.Append("54");
            var valor = ValorPix.ToString("F2").Replace(".", "").Replace(",", ".");
            codigo.Append(valor.Length.ToString().PadLeft(2, (char)48));
            codigo.Append(valor);

            //58 - Country Code
            codigo.Append("58" + "02" + "BR"); //“BR” – código de país ISO3166-1 alpha 2

            //59 - Merchant Name - nome do beneficiário/recebedor
            codigo.Append("59");
            codigo.Append(NomeLoja.Length.ToString().PadLeft(2, (char)48));
            codigo.Append(NomeLoja);

            //60 - Merchant City
            codigo.Append("60");
            codigo.Append(CidadeLoja.Length.ToString().PadLeft(2, (char)48));
            codigo.Append(CidadeLoja);

            //61 - Postal Code (opcional)

            //62 - Additional Data Field
            codigo.Append("62");
            if (IdentificadorPix == "")
               IdentificadorPix = "***";

            var tPix6205 = "05" + (IdentificadorPix.Length.ToString().PadLeft(2, (char)48) + IdentificadorPix);
            codigo.Append(tPix6205.Length.ToString().PadLeft(2, (char)48));

            codigo.Append(tPix6205);

            //63 - CRC16
            //var crc = CRC16(codigo.ToString());
            codigo.Append("63");
            codigo.Append("04"); //tamanho
            codigo.Append(CRC16(codigo.ToString()));

            return codigo.ToString();
         }
         catch (Exception ex)
         {
            Console.WriteLine(ex.ToString());
            return "";
         }
      }

      private static string CRC16(string s)
      {
         var crc = 0xFFFF;
         const int polynomial = 0x1021;
         var bytes = Encoding.ASCII.GetBytes(s);
         foreach (var b in bytes)
         {
            for (var i = 0; i <= 8 - 1; i++)
            {
               bool bit = ((b >> (7 - i) & 1) == 1);
               bool c15 = ((crc >> 15 & 1) == 1);
               crc <<= 1;
               if (c15 ^ bit)
                  crc = crc ^ polynomial;
            }
         }
         crc = crc & 0xFFFF;
         return crc.ToString("X").PadLeft(4, (char)48);
      }

      private byte[] ImageToByte(Bitmap img)
      {
         var converter = new ImageConverter();
         return (byte[])converter.ConvertTo(img, typeof(byte[]));
      }

      private string LimparTexto(string strTexto)
      {
         strTexto = Regex.Replace(strTexto, "[ÁÀÂÃ]", "A");
         strTexto = Regex.Replace(strTexto, "[ÉÈÊ]", "E");
         strTexto = Regex.Replace(strTexto, "[Í]", "I");
         strTexto = Regex.Replace(strTexto, "[ÓÒÔÕ]", "O");
         strTexto = Regex.Replace(strTexto, "[ÚÙÛÜ]", "U");
         strTexto = Regex.Replace(strTexto, "[Ç]", "C");
         strTexto = Regex.Replace(strTexto, "[áàâã]", "a");
         strTexto = Regex.Replace(strTexto, "[éèê]", "e");
         strTexto = Regex.Replace(strTexto, "[í]", "i");
         strTexto = Regex.Replace(strTexto, "[óòôõ]", "o");
         strTexto = Regex.Replace(strTexto, "[úùûü]", "u");
         strTexto = Regex.Replace(strTexto, "[ç]", "c");
         strTexto = Regex.Replace(strTexto, "[��]", "_");
         return Regex.Replace(strTexto, @"[,%'*]", "").Trim();
      }

      public void Dispose()
      {
      }
   }
}
