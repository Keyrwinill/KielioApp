//20251115 Initial

using System.Text.Json.Serialization;

namespace ViewModel.Models.ResponseModels;

public class ImageResponseModel
{
	[JsonPropertyName("original_path")]
	public string? OriginalPath { get; set; }
	
	[JsonPropertyName("processed_path")]
	public string? ProcessedPath { get; set; }
}