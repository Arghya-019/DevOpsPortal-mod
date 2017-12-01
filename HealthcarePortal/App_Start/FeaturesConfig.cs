namespace DevOpsPortal
{
    using FeatureSwitcher.Configuration;

    public class FeaturesConfig
    {
        public static void Start()
        {
            FeatureSwitcher.Configuration.Features.Are.ConfiguredBy.AppConfig();
        }
    }
}