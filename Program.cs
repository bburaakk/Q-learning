using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        // Durumlar ve eylemler
        string[] states = new string[]
        {
            "state00", "state01", "state02", "state03", "state04",
            "state10", "state11", "state12", "state13", "state14",
            "state20", "state21", "state22", "state23", "state24",
            "state30", "state31", "state32", "state33", "state34",
            "state40", "state41", "state42", "state43", "state44"
        };
        string[] actions = new string[] { "yukari", "asagi", "sol", "sag" };
        // Q-tablosu
        Dictionary<string, Dictionary<string, double>> QTable = new Dictionary<string, Dictionary<string, double>>();

        // Q-tablosunu başlat
        foreach (string state in states)
        {
            QTable[state] = new Dictionary<string, double>();
            foreach (string action in actions)
            {
                QTable[state][action] = 0;
            }
        }

        // Pekiştirmeli öğrenme döngüsü
        for (int episode = 0; episode < 10; episode++)
        {
            // Başlangıç durumunu seç
            string state = "state00";
            int stepCount = 0;
            while (true)
            {
                stepCount++;
                // Eylemi ε-greedy politikasına göre seç
                string action;
                if (new Random().NextDouble() < 0.1)  // %10 olasılıkla rastgele bir eylem seç
                {
                    action = actions[new Random().Next(actions.Length)];
                }
                else  // %90 olasılıkla en yüksek Q-değerine sahip eylemi seç
                {
                    action = MaxQAction(QTable[state]);
                }

                Console.WriteLine("Adim " + stepCount + ": " + state + ", " + action);

                // Eylemi gerçekleştir ve ödülü al
                string nextState = PerformAction(state, action);
                double reward = Reward(nextState);

                // Q-değerini güncelle
                double oldQ = QTable[state][action];
                double maxNextQ = MaxQValue(QTable[nextState]);
                double newQ = oldQ + 0.5 * (reward + 0.9 * maxNextQ - oldQ);
                QTable[state][action] = newQ;

                // Yeni duruma geç
                state = nextState;

                // Eğer oyun bitti ise döngüyü kır
                if (state == "state44")
                {
                    Console.WriteLine("Hedefe " + stepCount + " adimda ulasildi.");
                    break;
                }
            }
        }
    }

    static string PerformAction(string state, string action)
    {
        // Durum ve eylemi ızgara koordinatlarına dönüştür
        int x = int.Parse(state.Substring(5, 1));
        int y = int.Parse(state.Substring(6, 1));

        switch (action)
        {
            case "yukari":
                x = Math.Max(0, x - 1);
                break;
            case "asagi":
                x = Math.Min(4, x + 1);
                break;
            case "sol":
                y = Math.Max(0, y - 1);
                break;
            case "sag":
                y = Math.Min(4, y + 1);
                break;
        }
        // Yeni durumu belirle
        string newState = "state" + x.ToString() + y.ToString();

        // Eğer ajan bir çukura düştüyse veya hedefe ulaştıysa, oyunu baştan başlat
        if (newState == "state04" || newState == "state23" || newState == "state21" || newState == "state22" || newState == "state40" || newState == "state41" || newState == "state42" || newState == "state43" || newState == "state34")
        {
            newState = "state00";  // Başlangıç durumuna dön
        }

        return newState;
    }

    static double Reward(string state)
    {
        // Bu fonksiyon, belirli bir durum için ödülü hesaplar.
        // Eğer ajan hedef duruma ulaştıysa büyük ödül
        if (state == "state44")
        {
            return 10;
        }
        // Eğer ajan bir çukura düştüyse büyük ceza
        else if (state == "state04" || state == "state21" || state == "state22" || state == "state23" || state == "state40")
        {
            return -1;
        }
        // Diğer durumlar için küçük ceza (hamle sayısını minimize etmek için)
        else
        {
            return -0.1;
        }
    }

    static string MaxQAction(Dictionary<string, double> actionValues)
    {
        // Bu fonksiyon, belirli bir durum için en yüksek Q-değerine sahip eylemi döndürür.
        string maxQAction = null;
        double maxQValue = double.NegativeInfinity;
        foreach (KeyValuePair<string, double> entry in actionValues)
        {
            if (entry.Value > maxQValue)
            {
                maxQValue = entry.Value;
                maxQAction = entry.Key;
            }
        }

        return maxQAction;
    }

    static double MaxQValue(Dictionary<string, double> actionValues)
    {
        // Bu fonksiyon, belirli bir durum için en yüksek Q-değerini döndürür.
        double maxQValue = double.NegativeInfinity;
        foreach (KeyValuePair<string, double> entry in actionValues)
        {
            if (entry.Value > maxQValue)
            {
                maxQValue = entry.Value;
            }
        }

        return maxQValue;
    }
}
