namespace Utils.Services
{
    interface IFuzzyMatchingService
    {
        double Distance(string first, string second);
        double Proximity(string first, string second);
    }
}
