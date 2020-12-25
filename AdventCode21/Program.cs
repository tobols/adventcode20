using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventCode21
{
    class Program
    {
        static void Main(string[] args)
        {
            var recepies = new List<Recepie>();

            foreach (var line in ReadFile("data"))
            {
                var s = line.Split("(contains");
                var i = s[0].Trim().Split(" ").ToList();
                var a = s[1].Remove(s[1].Length - 1).Split(',').Select(s => s.Trim()).ToList();

                recepies.Add(new Recepie { ingredients = i, allergens = a });
            }

            var allIngredients = recepies.SelectMany(r => r.ingredients).Distinct().ToList();
            var allAllergens = recepies.SelectMany(r => r.allergens).Distinct().ToList();
            var allergenIngredients = new Dictionary<string, List<string>>();

            foreach (var allergen in allAllergens)
            {
                var allergenRecepies = recepies.Where(r => r.allergens.Contains(allergen)).ToList();
                allergenIngredients.Add(allergen, allergenRecepies.SelectMany(r => r.ingredients.Where(i => allergenRecepies.All(x => x.ingredients.Contains(i)))).Distinct().ToList());
            }

            var nonAllergens = allIngredients.Where(i => !allergenIngredients.Values.Any(v => v.Contains(i))).ToList();
            var sum = recepies.SelectMany(r => r.ingredients.Intersect(nonAllergens)).Count();

            Console.WriteLine(sum);


            while(allergenIngredients.Any(ai => ai.Value.Count() > 1))
                foreach(var ai in allergenIngredients.Values.Where(a => a.Count() == 1))
                {
                    var ingredient = ai.FirstOrDefault();
                    foreach (var x in allergenIngredients.Values.Where(v => v.Count() > 1 && v.Contains(ingredient)))
                        x.Remove(ingredient);
                }

            var ordered = allergenIngredients.OrderBy(a => a.Key).ToList();
            var str = ordered.Aggregate("", (a, b) => a + "," + b.Value.FirstOrDefault()).Substring(1);

            Console.WriteLine(str);
        }

        private static IEnumerable<string> ReadFile(string name)
        {
            var file = new StreamReader(Path.Combine(Environment.CurrentDirectory, $"{name}.txt"));
            string line;
            while ((line = file.ReadLine()) != null)
                yield return line;
            file.Close();
        }


        private class Recepie
        {
            public List<string> ingredients;
            public List<string> allergens;
        }
    }
}
