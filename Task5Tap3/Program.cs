using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        Console.Write("Analiz ediləcək mətn faylının tam yolunu (və ya adını) daxil edin: ");
        string faylYolu = Console.ReadLine();

        if (!File.Exists(faylYolu))
        {
            Console.WriteLine("XƏTA: Göstərilən fayl tapılmadı!");
            Console.ReadLine();
            return;
        }

        try
        {
            int setirSayi = 0;
            int sozSayi = 0;
            int simvolSayi = 0;
            string enUzunSoz = "";
            Dictionary<string, int> sozTezliyi = new Dictionary<string, int>();

            char[] ayiricilar = new char[] { ' ', '\t', ',', '.', '!', '?', ';', ':', '-', '"', '(', ')' };

            using (StreamReader sr = new StreamReader(faylYolu, Encoding.UTF8))
            {
                string setir;
                while ((setir = sr.ReadLine()) != null)
                {
                    setirSayi++;
                    simvolSayi += setir.Length;

                    string[] sozler = setir.Split(ayiricilar, StringSplitOptions.RemoveEmptyEntries);
                    sozSayi += sozler.Length;

                    foreach (string soz in sozler)
                    {
                        if (soz.Length > enUzunSoz.Length)
                        {
                            enUzunSoz = soz;
                        }

                        string kicikSoz = soz.ToLower();
                        if (sozTezliyi.ContainsKey(kicikSoz))
                        {
                            sozTezliyi[kicikSoz]++;
                        }
                        else
                        {
                            sozTezliyi[kicikSoz] = 1;
                        }
                    }
                }
            }

            var enCoxTekrarlananlar = sozTezliyi.OrderByDescending(x => x.Value).Take(3).ToList();

            string hesabatFayli = "hesabat.txt";
            using (StreamWriter sw = new StreamWriter(hesabatFayli, append: false, Encoding.UTF8))
            {
                sw.WriteLine("--- MƏTN ANALİZİ HESABATI ---");
                sw.WriteLine($"Sətir sayı: {setirSayi}");
                sw.WriteLine($"Söz sayı: {sozSayi}");
                sw.WriteLine($"Simvol sayı: {simvolSayi}");
                sw.WriteLine($"Ən uzun söz: {enUzunSoz}");
                sw.WriteLine("\nƏn çox təkrarlanan 3 söz:");

                int sira = 1;
                foreach (var item in enCoxTekrarlananlar)
                {
                    sw.WriteLine($"{sira}. \"{item.Key}\" - {item.Value} dəfə");
                    sira++;
                }
            }

            Console.WriteLine("\nAnaliz tamamlandı! Nəticələr 'hesabat.txt' faylına yazıldı:");
            Console.WriteLine(File.ReadAllText(hesabatFayli, Encoding.UTF8));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Xəta baş verdi: {ex.Message}");
        }

        Console.ReadLine();
    }
}