using ThreadingLogic.Map;

namespace BlazorApp.Services;

public record ThreadingLogicStatus(
    int WaitingClientsCount, 
    IReadOnlyList<string> GoCartsBufferPreview,
    IReadOnlyList<Section> Map
);