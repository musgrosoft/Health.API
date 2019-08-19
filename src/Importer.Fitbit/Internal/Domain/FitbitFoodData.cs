using System;
using System.Collections.Generic;

namespace Importer.Fitbit.Internal.Domain
{

    public class NutritionalValues
    {
        public int biotin { get; set; }
        public int calcium { get; set; }
        public int calories { get; set; }
        public int caloriesFromFat { get; set; }
        public int cholesterol { get; set; }
        public int copper { get; set; }
        public int dietaryFiber { get; set; }
        public int folicAcid { get; set; }
        public int iodine { get; set; }
        public int iron { get; set; }
        public int magnesium { get; set; }
        public int monounsaturatedFat { get; set; }
        public int niacin { get; set; }
        public int pantothenicAcid { get; set; }
        public int phosphorus { get; set; }
        public int polyunsaturatedFat { get; set; }
        public int potassium { get; set; }
        public int protein { get; set; }
        public int riboflavin { get; set; }
        public int saturatedFat { get; set; }
        public int sodium { get; set; }
        public int sugars { get; set; }
        public int thiamin { get; set; }
        public int totalCarbohydrate { get; set; }
        public int totalFat { get; set; }
        public int transFat { get; set; }
        public int vitaminA { get; set; }
        public int vitaminB12 { get; set; }
        public int vitaminB6 { get; set; }
        public int vitaminC { get; set; }
        public int vitaminD { get; set; }
        public int vitaminE { get; set; }
        public int zinc { get; set; }
    }

    public class Cl
    {
        public double multiplier { get; set; }
        public string unitName { get; set; }
        public string unitNamePlural { get; set; }
    }

    public class Servings
    {
        public Cl cl { get; set; }
    }

    public class Food2
    {
        public string accessLevel { get; set; }
        public string brand { get; set; }
        public int calories { get; set; }
        public string creatorEncodedId { get; set; }
        public string defaultServingUnit { get; set; }
        public int foodId { get; set; }
        public bool isGeneric { get; set; }
        public bool isQuickCaloriesAdd { get; set; }
        public bool isRaw { get; set; }
        public string name { get; set; }
        public NutritionalValues nutritionalValues { get; set; }
        public Servings servings { get; set; }
    }

    public class LoggedFood
    {
        public string accessLevel { get; set; }
        public double amount { get; set; }
        public string brand { get; set; }
        public int calories { get; set; }
        public string creatorEncodedId { get; set; }
        public int foodId { get; set; }
        public string mealType { get; set; }
        public string name { get; set; }
        public string unitName { get; set; }
        public string unitNamePlural { get; set; }
    }

    public class Food
    {
        public Food2 food { get; set; }
        public bool isGeneric { get; set; }
        public bool isQuickCaloriesAdd { get; set; }
        public DateTime logDate { get; set; }
        public object logId { get; set; }
        public LoggedFood loggedFood { get; set; }
    }

    public class FoodSummary
    {
        public int calories { get; set; }
        public int carbs { get; set; }
        public int fat { get; set; }
        public int fiber { get; set; }
        public int protein { get; set; }
        public int sodium { get; set; }
        public int water { get; set; }
    }

    public class FitbitFoodData
    {
        public List<Food> foods { get; set; }
        public FoodSummary summary { get; set; }
    }
}