public class Config
{
    //Settings
    public static float Sound { get; set; } = 1.0f;
    public static float DefaultSound { get; private set; } = 1.0f;
    public static float Music { get; set; } = 1.0f;
    public static float DefaultMusic { get; private set; } = 1.0f;
    
    public static float ShakePower { get; set; } = 1.0f;
    public static float DefaultShakePower { get; private set; } = 1.0f;

    public static float PostProcessing { get; set; } = 1.0f;
    public static float DefaultPostProcessing { get; private set; } = 1.0f;
}
