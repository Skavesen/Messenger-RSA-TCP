using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace RSAEncryption
{
    public class RSA
    {
        //Рандомная переменная для выбора рандомного числа из списка простых чисел
        static Random random = new Random();
        //переменные для создания ключей и шифрования
        public long p;
        public long q;
        public long e;
        public long n;
        public long d;
        public long m;
        //запуск получения чисел и ключей
        public RSA()
        {
            Create();
        }
        public void Create()
        {
            //p и q простые числа
            //Генерация p
            p = Generate();
            //Генерация q
            q = Generate();
            //Получение n(Модуль произведения)
            n = p * q;
            //Получение m(Функция Эйлера)
            m = (p - 1) * (q - 1);
            //Получение числа е
            e = PutE(m);
            //Получение числа d
            d = PutD(m, e);
        }
        //Метод шифрования для передачи клиенту
        public string[] Encrypt(string text, long E, long N)
        {
            return Encode(text, E, N);
        }
        //Метод расшифровки для передачи клиенту
        public string Decrypt(string[] data)
        {
            return Decode(data, d, n);
        }
        //Метод для шифровки
        private string[] Encode(string text, long e, long n)
        {
            //Создание массива с текстом
            List<string> enText = new List<string>();
            BigInteger num;
            //Шифрование с помощью открытого ключа и запись в массив
            foreach (char symbol in text)
            {
                //Умножение на х,символ каждый раз имеет новую комбинацию чисел
                int index = symbol;
                //Шифрование по методу RSA
                num = BigInteger.ModPow(index, e, n);
                //Преобразование в строку и добавление текста в список
                enText.Add(num.ToString());
            }
            //Копируем массив
            return enText.ToArray();
        }
        //Метод ля расшифровки
        private string Decode(string[] enText, long d, long n)
        {
            try
            {
                //Изменяемая строка символов
                StringBuilder stringBuild = new StringBuilder();
                BigInteger num;
                //Расшифровка текста
                foreach (string item in enText)
                {
                    BigInteger value = new BigInteger(Convert.ToInt64(item));
                    num = BigInteger.ModPow(value, d, n);
                    //Добавление расшифрованного текста посимвольно
                    stringBuild.Append((char)num);
                }
                //Результат расшифрованного текста в строковой форме
                return stringBuild.ToString();
            }
            catch
            {
                //Результат расшифрованного текста в строковой форме
                return "Ошибка";
            }
        }
        //Функция для генерации просытх чисел
        public static long Generate()
        {
            //Массив для простых чисел
            List<long> simpleNums = new List<long>();
            //числовой диапазон для чисел
            int startPosition = 1000;
            int endPosition = 10000;
            //перебор всех чисел из диапазона
            while (startPosition++ < endPosition)
            {
                //если оно простое то добавляем его в список
                if (IsSimple(startPosition))
                    simpleNums.Add(startPosition);
            }
            //функция возвращает 1 простое число из списка
            return simpleNums[random.Next(0, simpleNums.Count)];
        }
        //Проверка простое ли число
        public static bool IsSimple(long n)
        {
            if (n == 1)
            {
                return false;
            }
            for (int d = 2; d * d <= n; d++)
            {
                if (n % d == 0)
                    return false;
            }
            return true;
        }
        //Получение числа е для открытого ключа
        private long PutE(long m)
        {
            long e = m - 1;
            while (true)
            {
                //Проверка условий для числа е
                if (IsSimple(e) && e < m && BigInteger.GreatestCommonDivisor(new BigInteger(e), new BigInteger(m)) == BigInteger.One)
                    break;
                e--;
            }
            return e;
        }
        //Получение числа d для приватного ключа
        private long PutD(long m, long e)
        {
            long d = e + 1;
            while (true)
            {
                //Проверка условий для числа d
                if ((d * e) % m == 1)
                    break;
                d++;
            }
            return d;
        }
    }
}

