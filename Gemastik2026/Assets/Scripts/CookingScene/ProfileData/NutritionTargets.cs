public static class NutritionTargets
{
    public static NutritionTarget GetTarget(NutritionProblem problem)
    {
        switch (problem)
        {
            case NutritionProblem.Healthy:
                return new NutritionTarget(50, 30, 20);

            case NutritionProblem.OverweightMalnutrition:
                return new NutritionTarget(30, 45, 25);

            case NutritionProblem.ProteinMalnutrition:
                return new NutritionTarget(30, 45, 25);

            case NutritionProblem.FatMalnutrition:
                return new NutritionTarget(30, 25, 45);

            default:
                return new NutritionTarget(50, 30, 20);
        }
    }
}