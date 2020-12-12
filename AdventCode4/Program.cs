using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventCode4
{
    class Program
    {
        private static Regex rgxHcl = new Regex("^#[0-9a-f]{6}$");
        private static Regex rgxEcl = new Regex("^(amb)|(blu)|(brn)|(gry)|(grn)|(hzl)|(oth)$");

        static void Main(string[] args)
        {
            var passport = new Dictionary<string, string>();


            var requiredInfo = new string[] { "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid" };
            var correctPassports = 0;
            string line;
            var file = new System.IO.StreamReader(@"C:\Repos\temp\AdventCode\AdventCode4\data.txt");

            while ((line = file.ReadLine()) != null)
            {
                if (line == string.Empty)
                {
                    var correct = true;
                    if (requiredInfo.Any(i => !passport.ContainsKey(i) || !Valid(i, passport[i])))
                        correct = false;

                    if (correct)
                        correctPassports++;

                    passport = new Dictionary<string, string>();
                    continue;
                }


                var pairs = line.Split(" ");
                for (int i = 0; i < pairs.Length; i++)
                {
                    var pair = pairs[i].Split(":");
                    passport.Add(pair[0], pair[1]);
                }
            }



            if (!requiredInfo.Any(i => !passport.ContainsKey(i) || !Valid(i, passport[i])))
                correctPassports++;
            
            
            
            file.Close();
            Console.WriteLine($"Found {correctPassports} valid passports");
        }


        private static bool Valid(string field, string value) =>
            field switch
            {
                "byr" => int.TryParse(value, out int r) && r >= 1920 && r <= 2002,
                "iyr" => int.TryParse(value, out int r) && r >= 2010 && r <= 2020,
                "eyr" => int.TryParse(value, out int r) && r >= 2020 && r <= 2030,
                "hgt" => value.EndsWith("cm")
                                ? int.TryParse(value.Substring(0, value.IndexOf("cm")), out int r) && r >= 150 && r <= 193
                                : value.EndsWith("in")
                                    ? int.TryParse(value.Substring(0, value.IndexOf("in")), out int re) && re >= 59 && re <= 76
                                    : false,
                "hcl" => rgxHcl.IsMatch(value),
                "ecl" => rgxEcl.IsMatch(value),
                "pid" => int.TryParse(value, out _) && value.Length == 9,
                _ => true
            };
    }
}
