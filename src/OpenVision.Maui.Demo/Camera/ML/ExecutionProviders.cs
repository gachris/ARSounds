namespace OpenVision.ML.Maui.Demo.ML;

public enum ExecutionProviders
{
    CPU,   // CPU execution provider is always available by default
    NNAPI, // NNAPI is available on Android
    CoreML // CoreML is available on iOS/macOS
}