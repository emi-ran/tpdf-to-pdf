# Base64 Görseli PDF'ye Dönüştürücü

Bu proje, `.tpdf` uzantılı dosyalardaki Base64 kodlu PNG görsellerini çıkartıp, bunları bir PDF dosyasına dönüştüren bir C# uygulamasıdır. Uygulama, belirtilen bir giriş klasöründeki tüm `.tpdf` dosyalarını işler ve çıkartılan görselleri içeren PDF dosyalarını belirlenen bir çıkış klasörüne kaydeder.

## Özellikler

- `.tpdf` dosyalarındaki Base64 kodlu PNG görsellerini tanıma ve çıkartma.
- Çıkarılan görselleri PDF dosyası olarak kaydetme.
- Hata yönetimi ve kullanıcı dostu hata mesajları.
- Kullanıcıdan klasör girişleri alarak dinamik çalışma.

## Gereksinimler

- .NET Framework 4.5 veya üzeri
- [iTextSharp](https://github.com/itext/itextsharp) kütüphanesi (PDF oluşturmak için)
- System.Drawing kütüphanesi (Görsel işleme için)

## Kullanım

1. **Gerekli Klasörleri Oluşturma**:

   - Proje dizininde `tpdfs` adında bir klasör oluşturun.
   - Dönüştürmek istediğiniz `.tpdf` dosyalarını bu klasöre ekleyin.

2. **Uygulamayı Çalıştırma**:

   - Uygulamayı çalıştırın. PDF dosyaları, `pdfs` adında bir klasöre kaydedilecektir. Eğer `pdfs` klasörü mevcut değilse, uygulama bunu otomatik olarak oluşturacaktır.

3. **Hata Yönetimi**:
   - Eğer `tpdfs` klasörü yoksa veya içerisinde `.tpdf` dosyası yoksa, uygulama hata mesajı verecektir.

## Notlar

- `.tpdf` dosyalarını, [Chrome eklentim](https://github.com/emi-ran/Restricted-PDF-Downloader) ile elde edebilirsiniz.

## İletişim

Herhangi bir sorun veya öneri için benimle iletişime geçebilirsiniz. Geri bildirimlerinizi bekliyorum!

## Teşekkürler

Bu projeyi fikrini oluşturmama ilham kaynağı olan "Sailor Vikas" kanalındaki "Download protected files from Classroom app" başlıklı videosuna özel teşekkürlerimi sunarım.
