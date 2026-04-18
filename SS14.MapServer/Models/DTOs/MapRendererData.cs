using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using SS14.MapServer.Models.Types;

namespace SS14.MapServer.Models.DTOs;

public sealed class MapRendererData
{
    public required string Id {get; set;}
    [Required, JsonProperty("Name")]
    public required string DisplayName {get; set;}
    [JsonProperty("Attributions")]
    public string? Attribution {get; set;}
    [Required]
    public List<GridData> Grids {get;} = new();
    public required List<ParallaxLayer> ParallaxLayers {get; set;}
}
