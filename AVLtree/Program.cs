using System;

namespace AVLAgaci
{
    // AVL ağacının her bir düğümünü temsil eden sınıf
    class Dugum
    {
        public int Deger; // Düğümdeki değeri tutar
        public Dugum Sol; // Sol alt ağacı
        public Dugum Sag; // Sağ alt ağacı
        public int Yukseklik; // Ağacın yüksekliği

        // Düğüm constructor'ı, yeni düğüm oluşturur
        public Dugum(int deger)
        {
            Deger = deger; // Değer atanır
            Sol = null; // Sol çocuk başlangıçta null
            Sag = null; // Sağ çocuk başlangıçta null
            Yukseklik = 1; // Yükseklik başta 1 olarak atanır
        }
    }

    // AVL ağacını temsil eden sınıf
    class AVLAgaci
    {
        private Dugum kok; // Ağacın kökünü tutar

        // Yüksekliği hesaplayan metod
        private int Yukseklik(Dugum dugum) => dugum == null ? 0 : dugum.Yukseklik;

        // Denge faktörünü hesaplayan metod
        private int DengeFaktoru(Dugum dugum) => dugum == null ? 0 : Yukseklik(dugum.Sol) - Yukseklik(dugum.Sag);

        // Sağ dönüş yapan metod
        private Dugum SagaDondur(Dugum y)
        {
            Dugum x = y.Sol;
            Dugum T = x.Sag;

            x.Sag = y;
            y.Sol = T;

            // Yükseklikleri günceller
            y.Yukseklik = Math.Max(Yukseklik(y.Sol), Yukseklik(y.Sag)) + 1;
            x.Yukseklik = Math.Max(Yukseklik(x.Sol), Yukseklik(x.Sag)) + 1;

            return x; // Yeni kökü döndürür
        }

        // Sol dönüş yapan metod
        private Dugum SolaDondur(Dugum x)
        {
            Dugum y = x.Sag;
            Dugum T = y.Sol;

            y.Sol = x;
            x.Sag = T;

            // Yükseklikleri günceller
            x.Yukseklik = Math.Max(Yukseklik(x.Sol), Yukseklik(x.Sag)) + 1;
            y.Yukseklik = Math.Max(Yukseklik(y.Sol), Yukseklik(y.Sag)) + 1;

            return y; // Yeni kökü döndürür
        }

        // Yeni bir eleman ekler
        public void Ekle(int deger)
        {
            kok = Ekle(kok, deger); // Kökten başlayarak eleman ekler
            Goster(); // Ağacı gösterir
        }

        private Dugum Ekle(Dugum dugum, int deger)
        {
            if (dugum == null) // Eğer düğüm boşsa yeni bir düğüm oluşturur
                return new Dugum(deger);

            if (deger < dugum.Deger) // Eğer değer küçükse sol alt ağaca ekler
                dugum.Sol = Ekle(dugum.Sol, deger);
            else if (deger > dugum.Deger) // Eğer değer büyükse sağ alt ağaca ekler
                dugum.Sag = Ekle(dugum.Sag, deger);
            else
                return dugum; // Aynı değer varsa eklemez

            // Yükseklik güncellenir
            dugum.Yukseklik = 1 + Math.Max(Yukseklik(dugum.Sol), Yukseklik(dugum.Sag));

            int denge = DengeFaktoru(dugum); // Denge faktörü hesaplanır

            // AVL ağacını dengelemek için dönüşler yapılır
            if (denge > 1 && deger < dugum.Sol.Deger)
                return SagaDondur(dugum);

            if (denge < -1 && deger > dugum.Sag.Deger)
                return SolaDondur(dugum);

            if (denge > 1 && deger > dugum.Sol.Deger)
            {
                dugum.Sol = SolaDondur(dugum.Sol);
                return SagaDondur(dugum);
            }

            if (denge < -1 && deger < dugum.Sag.Deger)
            {
                dugum.Sag = SagaDondur(dugum.Sag);
                return SolaDondur(dugum);
            }

            return dugum; // Düğüm geri döndürülür
        }

        // Eleman silme fonksiyonu
        public void Sil(int deger)
        {
            kok = Sil(kok, deger); // Kökten başlayarak silme işlemi yapılır
            Goster(); // Ağacı tekrar gösterir
        }

        private Dugum Sil(Dugum dugum, int deger)
        {
            if (dugum == null)
                return dugum; // Eğer düğüm null ise zaten yapılacak bir şey yok

            if (deger < dugum.Deger) // Silinecek değer sol alt ağaçta
                dugum.Sol = Sil(dugum.Sol, deger);
            else if (deger > dugum.Deger) // Sağ alt ağaçta
                dugum.Sag = Sil(dugum.Sag, deger);
            else // Düğüm bulundu
            {
                if (dugum.Sol == null || dugum.Sag == null) // Bir çocuğu yoksa, direkt diğer çocukla değiştirir
                    dugum = (dugum.Sol != null) ? dugum.Sol : dugum.Sag;
                else
                {
                    Dugum temp = EnKucukDegerDugumu(dugum.Sag); // Sağ alt ağacın en küçük değeri ile değiştirir
                    dugum.Deger = temp.Deger;
                    dugum.Sag = Sil(dugum.Sag, temp.Deger); // Bu değeri siler
                }
            }

            if (dugum == null)
                return dugum;

            // Yükseklikleri günceller
            dugum.Yukseklik = Math.Max(Yukseklik(dugum.Sol), Yukseklik(dugum.Sag)) + 1;

            int denge = DengeFaktoru(dugum); // Denge faktörü hesaplanır

            // AVL ağacını dengelemek için dönüşler yapılır
            if (denge > 1 && DengeFaktoru(dugum.Sol) >= 0)
                return SagaDondur(dugum);

            if (denge > 1 && DengeFaktoru(dugum.Sol) < 0)
            {
                dugum.Sol = SolaDondur(dugum.Sol);
                return SagaDondur(dugum);
            }

            if (denge < -1 && DengeFaktoru(dugum.Sag) <= 0)
                return SolaDondur(dugum);

            if (denge < -1 && DengeFaktoru(dugum.Sag) > 0)
            {
                dugum.Sag = SagaDondur(dugum.Sag);
                return SolaDondur(dugum);
            }

            return dugum; // Düğüm geri döndürülür
        }

        // Sağ alt ağacın en küçük değerini bulur
        private Dugum EnKucukDegerDugumu(Dugum dugum)
        {
            Dugum temp = dugum;
            while (temp.Sol != null) // Sol alt ağaca gider
                temp = temp.Sol;
            return temp; // En küçük değer döndürülür
        }

        // Eleman arama fonksiyonu
        public void Ara(int deger)
        {
            int adimlar = 0;
            bool bulundu = Ara(kok, deger, ref adimlar);

            if (bulundu)
                Console.WriteLine($"Eleman {adimlar} adımda bulundu.");
            else
                Console.WriteLine("Eleman bulunamadı.");
        }

        private bool Ara(Dugum dugum, int deger, ref int adimlar)
        {
            adimlar++; // Her adımda artırılır
            if (dugum == null)
                return false; // Eğer düğüm null ise eleman bulunamaz

            if (deger == dugum.Deger)
                return true; // Eğer değer bulunduysa true döndürülür

            if (deger < dugum.Deger)
                return Ara(dugum.Sol, deger, ref adimlar); // Sol alt ağaçta arama yapılır

            return Ara(dugum.Sag, deger, ref adimlar); // Sağ alt ağaçta arama yapılır
        }

        // Ağacı gösteren fonksiyon
        public void Goster()
        {
            if (kok == null)
            {
                Console.WriteLine("Kök: Ağacınız boş.");
                return;
            }

            Console.WriteLine("Kök: " + kok.Deger);
            Goster(kok, "", true); // Ağacı yazdıran yardımcı fonksiyon
        }

        private void Goster(Dugum dugum, string bosluk, bool son)
        {
            if (dugum != null)
            {
                Console.WriteLine(bosluk + (son ? "└── " : "├── ") + dugum.Deger); // Ağacın dalını yazdırır
                bosluk += son ? "    " : "│   "; // Dallara göre boşluk ekler

                Goster(dugum.Sol, bosluk, false); // Sol alt ağacı gösterir
                Goster(dugum.Sag, bosluk, true); // Sağ alt ağacı gösterir
            }
        }
    }

    // Program sınıfı, ana uygulama
    class Program
    {
        static void Main(string[] args)
        {
            AVLAgaci agac = new AVLAgaci(); // Yeni bir AVL ağacı oluşturur

            while (true) // Sonsuz döngü, kullanıcıdan giriş alır
            {
                Console.WriteLine("\n1. Ekle\n2. Sil\n3. Ara\n4. Göster\n0. Çıkış");
                Console.Write("Seçim yapınız: ");
                if (!int.TryParse(Console.ReadLine(), out int secim))
                {
                    Console.WriteLine("Geçersiz giriş, tekrar deneyin.");
                    continue; // Eğer seçim geçersizse tekrar sorar
                }

                // Kullanıcının seçimine göre işlem yapılır
                switch (secim)
                {
                    case 1:
                        Console.Write("Eklenecek eleman: ");
                        if (int.TryParse(Console.ReadLine(), out int ekle))
                        {
                            agac.Ekle(ekle);
                            Console.WriteLine("Eleman eklendi.");
                        }
                        else
                        {
                            Console.WriteLine("Geçersiz eleman değeri.");
                        }
                        break;
                    case 2:
                        Console.Write("Silinecek eleman: ");
                        if (int.TryParse(Console.ReadLine(), out int sil))
                        {
                            agac.Sil(sil);
                            Console.WriteLine("Eleman silindi.");
                        }
                        else
                        {
                            Console.WriteLine("Geçersiz eleman değeri.");
                        }
                        break;
                    case 3:
                        Console.Write("Aranacak eleman: ");
                        if (int.TryParse(Console.ReadLine(), out int ara))
                        {
                            agac.Ara(ara);
                        }
                        else
                        {
                            Console.WriteLine("Geçersiz eleman değeri.");
                        }
                        break;
                    case 4:
                        agac.Goster(); // Ağacı gösterir
                        break;
                    case 0:
                        return; // Çıkış yapar
                    default:
                        Console.WriteLine("Geçersiz seçim!"); // Yanlış seçim yapılırsa uyarı verir
                        break;
                }
            }
        }
    }
}
