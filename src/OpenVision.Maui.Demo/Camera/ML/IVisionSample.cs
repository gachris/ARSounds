using OpenVision.ML.Maui.Demo.PrePostProcessing;

namespace OpenVision.ML.Maui.Demo.ML;

public interface IVisionSample
{
    string Name { get; }
    string ModelName { get; }
    Task InitializeAsync();
    Task UpdateExecutionProviderAsync(ExecutionProviders executionProvider);
    Task<ImageProcessingResult> ProcessImageAsync(byte[] image);
}
