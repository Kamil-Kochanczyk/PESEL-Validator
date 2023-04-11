/*
 * PESEL:     0 2 2 5 0 4 0 9 1 9 4
 * Meaning:   Y Y M M D D S S S S C
 * Weight:    1 3 7 9 1 3 7 9 1 3 -
 * 
 * Y - Year (2 last digits of a birth year)
 * M - Month (2 digits representing a birth month, based on a pre-defined table)
 * D - Day (birth day)
 * S - Sex (last S digit indicates sex; male - 1, 3, 5, 7, 9; female - 0, 2, 4, 6, 8)
 * C - Control number
 * 
 * PESEL is correct when it passes 3 conditions:
 * 1. Length of PESEL is equal to 11.
 * 2. Every character in PESEL is a natural number.
 * 3. Control number is calculated correctly.
 * 
 * Algorithm for calculating control number:
 * 1. Multiply PESEL characters (numbers) and their corresponding weights.
 *    If any product is a two-digit number, take only the last digit.
 *    0 * 1 = 0
 *    2 * 3 = 6
 *    2 * 7 = 14 = 4
 *    5 * 9 = 45 = 5
 *    0 * 1 = 0
 *    4 * 3 = 12 = 2
 *    0 * 7 = 0
 *    9 * 9 = 81 = 1
 *    1 * 1 = 1
 *    9 * 3 = 27 = 7
 * 2. Add all products.
 *    If sum is a two-digit number, take only the last digit.
 *    0 + 6 + 4 + 5 + 0 + 2 + 0 + 1 + 1 + 7 = 26 = 6
 * 3. Subtract sum from 10
 *    10 - 6 = 4 = C
*/

namespace PESEL_Validator
{
    public class PESEL
    {
        public static string[,] MonthsAndYears = new string[12, 5]
        {
            //1800  1900  2000  2100  2200

            { "81", "01", "21", "41", "61" },   // January
            { "82", "02", "22", "42", "62" },   // February
            { "83", "03", "23", "43", "63" },   // March
            { "84", "04", "24", "44", "64" },   // April
            { "85", "05", "25", "45", "65" },   // May
            { "86", "06", "26", "46", "66" },   // June
            { "87", "07", "27", "47", "67" },   // July
            { "88", "08", "28", "48", "68" },   // August
            { "89", "09", "29", "49", "69" },   // September
            { "90", "10", "30", "50", "70" },   // October
            { "91", "11", "31", "51", "71" },   // November
            { "92", "12", "32", "52", "72" }    // December
        };

        private string _pesel, _sex, _day, _month, _year;

        public string Pesel => _pesel;
        public string Sex => _sex;
        public string Day => _day;
        public string Month => _month;
        public string Year => _year;

        public PESEL(string pesel)
        {
            this._pesel = pesel;

            string sex, day, month, year;
            ValidatePESEL(pesel, out sex, out day, out month, out year);

            this._sex = sex;
            this._day = day;
            this._month = month;
            this._year = year;
        }

        private static void ValidatePESEL(string pesel, out string sex, out string day, out string month, out string year)
        {
            bool peselCorrect = (LengthCorrect(pesel) && CharactersCorrect(pesel)) && ControlNumberCorrect(pesel);

            if (peselCorrect)
            {
                sex = (int.Parse(pesel[9].ToString()) % 2 == 0) ? "Female" : "Male";

                day = pesel.Substring(4, 2);

                month = pesel.Substring(2, 2);

                year = pesel.Substring(0, 2);

                int row = 0, column = 0;
                bool found = false;
                for (row = 0; row < MonthsAndYears.GetLength(0); row++)
                {
                    for (column = 0; column < MonthsAndYears.GetLength(1); column++)
                    {
                        if (MonthsAndYears[row, column] == month)
                        {
                            found = true;
                            break;
                        }
                    }

                    if (found)
                    {
                        break;
                    }
                }

                switch (row)
                {
                    case 0:
                        month = "January";
                        break;
                    case 1:
                        month = "February";
                        break;
                    case 2:
                        month = "March";
                        break;
                    case 3:
                        month = "April";
                        break;
                    case 4:
                        month = "May";
                        break;
                    case 5:
                        month = "June";
                        break;
                    case 6:
                        month = "July";
                        break;
                    case 7:
                        month = "August";
                        break;
                    case 8:
                        month = "September";
                        break;
                    case 9:
                        month = "October";
                        break;
                    case 10:
                        month = "November";
                        break;
                    case 11:
                        month = "December";
                        break;
                }

                switch (column)
                {
                    case 0:
                        year = "18" + year;
                        break;
                    case 1:
                        year = "19" + year;
                        break;
                    case 2:
                        year = "20" + year;
                        break;
                    case 3:
                        year = "21" + year;
                        break;
                    case 4:
                        year = "22" + year;
                        break;
                }
            }
            else
            {
                sex = "Unknown";
                day = "Unknown";
                month = "Unknown";
                year = "Unknown";
            }
        }

        private static bool LengthCorrect(string pesel)
        {
            return pesel.Length == 11;
        }

        private static bool CharactersCorrect(string pesel)
        {
            bool result = true;

            foreach (char character in pesel)
            {
                if (!char.IsNumber(character))
                {
                    result = false;
                    break;
                }
            }

            return result;
        }

        private static bool ControlNumberCorrect(string pesel)
        {
            int controlNumber, lastNumber = int.Parse(pesel[10].ToString());

            int weight = 1, sum = 0;

            for (int i = 0; i < pesel.Length - 1; i++)
            {
                char character = pesel[i];

                int product = int.Parse(character.ToString()) * weight;

                if (product >= 10)
                {
                    product = int.Parse(product.ToString()[1].ToString());
                }

                sum += product;

                switch (weight)
                {
                    case 1:
                        weight = 3;
                        break;
                    case 3:
                        weight = 7;
                        break;
                    case 7:
                        weight = 9;
                        break;
                    case 9:
                        weight = 1;
                        break;
                }
            }

            if (sum >= 10)
            {
                sum = int.Parse(sum.ToString()[1].ToString());
            }

            controlNumber = 10 - sum;

            return controlNumber == lastNumber;
        }
    }
}