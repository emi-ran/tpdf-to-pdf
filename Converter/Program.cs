using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Diagnostics;
using iTextSharp.text;
using iTextSharp.text.pdf;

class Base64ImageToPdfConverter
{
    static void Main(string[] args)
    {
        string inputDir = "tpdfs";
        string outputDir = "pdfs";

        try
        {
            // Giriş dizininin var olup olmadığını kontrol et
            if (!Directory.Exists(inputDir))
            {
                Console.WriteLine($"Hata: '{inputDir}' klasörü bulunamadı.");
                Console.WriteLine("Lütfen 'tpdfs' klasörünü oluşturun ve içine dosyaları ekleyin.");
                WaitForKeyPress();
                return;
            }

            // Giriş dizinindeki .tpdf dosyalarını al
            string[] inputFiles = Directory.GetFiles(inputDir, "*.tpdf");

            // Dosya sayısını kontrol et
            if (inputFiles.Length == 0)
            {
                Console.WriteLine($"Hata: '{inputDir}' klasöründe hiç .tpdf dosyası bulunamadı.");
                Console.WriteLine("Lütfen klasöre .tpdf uzantılı dosyalar ekleyin.");
                WaitForKeyPress();
                return;
            }

            // Çıktı dizinini oluştur
            Directory.CreateDirectory(outputDir);

            bool anyProcessed = false;

            // Tüm .tpdf dosyalarını işle
            foreach (var inputFile in inputFiles.OrderBy(f => f))
            {
                try
                {
                    string filename = Path.GetFileNameWithoutExtension(inputFile);
                    string imageOutputDir = Path.Combine(outputDir, filename);

                    // Görsel klasörünü oluştur
                    Directory.CreateDirectory(imageOutputDir);

                    int totalImages = ExtractAndProcessImages(inputFile, imageOutputDir);

                    if (totalImages > 0)
                    {
                        string pdfOutputPath = Path.Combine(outputDir, $"{filename}.pdf");
                        CreatePdf(imageOutputDir, pdfOutputPath);

                        // Geçici görsel klasörünü sil
                        Directory.Delete(imageOutputDir, true);
                        Console.WriteLine($"{filename}.pdf başarıyla oluşturuldu.");
                        anyProcessed = true;
                    }
                    else
                    {
                        Console.WriteLine($"{filename} dosyasından görsel çıkarılamadı.");
                        // Boş klasörü sil
                        Directory.Delete(imageOutputDir, true);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Dosya işlenirken hata oluştu: {Path.GetFileName(inputFile)}");
                    Console.WriteLine($"Hata detayı: {ex.Message}");
                }
            }

            if (!anyProcessed)
            {
                Console.WriteLine("Hiçbir dosya işlenemedi.");
            }
            else
            {
                Console.WriteLine("Tüm dosyalar işlendi.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Beklenmedik bir hata oluştu: {ex.Message}");
        }
        finally
        {
            // Eğer debugger açıksa
            if (Debugger.IsAttached)
            {
                Console.WriteLine("\nDebugger modunda çalışıyor. Çıkmak için bir tuşa basın...");
                Console.ReadKey();
            }
        }
    }

    // Kullanıcının konsol penceresini kapatmasını önlemek için
    static void WaitForKeyPress()
    {
        // Yalnızca debugger bağlı değilse bekle
        if (!Debugger.IsAttached)
        {
            Console.WriteLine("\nDevam etmek için bir tuşa basın...");
            Console.ReadKey();
        }
    }

    static int ExtractAndProcessImages(string inputFile, string outputDir)
    {
        int totalImages = 0;
        string[] lines = File.ReadAllLines(inputFile, Encoding.UTF8);

        foreach (var line in lines)
        {
            if (line.StartsWith("data:image/png;base64,"))
            {
                totalImages++;
                string base64Data = line.Replace("data:image/png;base64,", "").Trim();
                string imagePath = Path.Combine(outputDir, $"image_{totalImages}.png");

                DecodeAndSaveImage(base64Data, imagePath);
            }
        }

        return totalImages;
    }

    static void DecodeAndSaveImage(string base64Data, string imagePath)
    {
        byte[] imageBytes = Convert.FromBase64String(base64Data);

        using (MemoryStream ms = new MemoryStream(imageBytes))
        {
            using (System.Drawing.Image systemImage = System.Drawing.Image.FromStream(ms))
            {
                systemImage.Save(imagePath, ImageFormat.Png);
            }
        }
    }

    static void CreatePdf(string imageDir, string outputPdf)
    {
        using (var document = new Document())
        {
            PdfWriter.GetInstance(document, new FileStream(outputPdf, FileMode.Create));
            document.Open();

            // Sıralı şekilde görselleri ekle
            foreach (var imagePath in Directory.GetFiles(imageDir).OrderBy(f => f))
            {
                var itextImage = iTextSharp.text.Image.GetInstance(imagePath);

                // Görüntüyü sayfaya sığdır
                itextImage.ScaleToFit(document.PageSize.Width - 50, document.PageSize.Height - 50);
                itextImage.Alignment = Element.ALIGN_CENTER;
                document.Add(itextImage);
                document.NewPage();
            }

            document.Close();
        }
    }
}