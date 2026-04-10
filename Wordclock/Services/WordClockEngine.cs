using System;
using System.Collections.Generic;

namespace Wordclock
{
    /// <summary>
    /// German word clock engine that maps time to active grid character indices
    /// </summary>
    public class WordClockEngine : IWordClockEngine
    {
        // Grid layout (11 columns x 10 rows):
        //  0: E S K I S T L F Ü N F
        // 11: Z E H N Z W A N Z I G
        // 22: D R E I V I E R T E L
        // 33: T G N A C H V O R J M
        // 44: H A L B Q Z W Ö L F P
        // 55: Z W E I N S I E B E N
        // 66: K D R E I R H F Ü N F
        // 77: E L F N E U N V I E R
        // 88: W A C H T Z E H N R S
        // 99: B S E C H S F M U H R

        public string[] ClockChars { get; } =
        {
            "E","S","K","I","S","T","L","F","Ü","N","F",
            "Z","E","H","N","Z","W","A","N","Z","I","G",
            "D","R","E","I","V","I","E","R","T","E","L",
            "T","G","N","A","C","H","V","O","R","J","M",
            "H","A","L","B","Q","Z","W","Ö","L","F","P",
            "Z","W","E","I","N","S","I","E","B","E","N",
            "K","D","R","E","I","R","H","F","Ü","N","F",
            "E","L","F","N","E","U","N","V","I","E","R",
            "W","A","C","H","T","Z","E","H","N","R","S",
            "B","S","E","C","H","S","F","M","U","H","R",
        };

        // Word index ranges
        private const int EsStart = 0, EsEnd = 1;           // ES
        private const int IstStart = 3, IstEnd = 5;         // IST
        private const int FuenfMStart = 7, FuenfMEnd = 10;  // FÜNF (minutes)
        private const int ZehnMStart = 11, ZehnMEnd = 14;   // ZEHN (minutes)
        private const int ZwanzigStart = 15, ZwanzigEnd = 21;// ZWANZIG
        private const int ViertelStart = 26, ViertelEnd = 32;// VIERTEL
        private const int NachStart = 35, NachEnd = 38;     // NACH
        private const int VorStart = 39, VorEnd = 41;       // VOR
        private const int HalbStart = 44, HalbEnd = 47;     // HALB
        private const int ZwoelfStart = 49, ZwoelfEnd = 53; // ZWÖLF
        private const int ZweiStart = 55, ZweiEnd = 58;     // ZWEI
        private const int EinStart = 57, EinEnd = 59;       // EIN
        private const int EinsStart = 57, EinsEnd = 60;     // EINS
        private const int SiebenStart = 60, SiebenEnd = 65; // SIEBEN
        private const int DreiStart = 67, DreiEnd = 70;     // DREI
        private const int FuenfHStart = 73, FuenfHEnd = 76; // FÜNF (hours)
        private const int ElfStart = 77, ElfEnd = 79;       // ELF
        private const int NeunStart = 80, NeunEnd = 83;     // NEUN
        private const int VierStart = 84, VierEnd = 87;     // VIER
        private const int AchtStart = 89, AchtEnd = 92;     // ACHT
        private const int ZehnHStart = 93, ZehnHEnd = 96;   // ZEHN (hours)
        private const int SechsStart = 100, SechsEnd = 104; // SECHS
        private const int UhrStart = 107, UhrEnd = 109;     // UHR

        public HashSet<int> GetActiveIndices(DateTime time)
        {
            var active = new HashSet<int>();
            var dt = time;

            if (dt.Minute >= 25)
                dt = dt.AddHours(1);

            // ES IST
            AddRange(active, EsStart, EsEnd);
            AddRange(active, IstStart, IstEnd);

            // Coarse minute evaluation
            if (dt.Minute >= 25 && dt.Minute <= 35)
                AddRange(active, HalbStart, HalbEnd);

            if ((dt.Minute > 35 && dt.Minute <= 55) || dt.Minute == 25)
                AddRange(active, VorStart, VorEnd);
            else if ((dt.Minute > 0 && dt.Minute < 25) || dt.Minute == 35)
                AddRange(active, NachStart, NachEnd);
            else if (dt.Minute == 0)
                AddRange(active, UhrStart, UhrEnd);

            // Fine minute evaluation
            if (dt.Minute == 5 || dt.Minute == 55 || dt.Minute == 25 || dt.Minute == 35)
                AddRange(active, FuenfMStart, FuenfMEnd);
            else if (dt.Minute == 10 || dt.Minute == 50)
                AddRange(active, ZehnMStart, ZehnMEnd);
            else if (dt.Minute == 15 || dt.Minute == 45)
                AddRange(active, ViertelStart, ViertelEnd);
            else if (dt.Minute == 20 || dt.Minute == 40)
                AddRange(active, ZwanzigStart, ZwanzigEnd);

            // Hour evaluation
            switch (dt.Hour % 12)
            {
                case 1:
                    if (dt.Minute == 0)
                        AddRange(active, EinStart, EinEnd);
                    else
                        AddRange(active, EinsStart, EinsEnd);
                    break;
                case 2:  AddRange(active, ZweiStart, ZweiEnd); break;
                case 3:  AddRange(active, DreiStart, DreiEnd); break;
                case 4:  AddRange(active, VierStart, VierEnd); break;
                case 5:  AddRange(active, FuenfHStart, FuenfHEnd); break;
                case 6:  AddRange(active, SechsStart, SechsEnd); break;
                case 7:  AddRange(active, SiebenStart, SiebenEnd); break;
                case 8:  AddRange(active, AchtStart, AchtEnd); break;
                case 9:  AddRange(active, NeunStart, NeunEnd); break;
                case 10: AddRange(active, ZehnHStart, ZehnHEnd); break;
                case 11: AddRange(active, ElfStart, ElfEnd); break;
                case 0:  AddRange(active, ZwoelfStart, ZwoelfEnd); break;
            }

            return active;
        }

        private static void AddRange(HashSet<int> set, int start, int end)
        {
            for (int i = start; i <= end; i++)
                set.Add(i);
        }
    }
}
