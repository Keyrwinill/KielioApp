/*
	Update date			Description 
	----------------------------------------------------------------------------------
	20260418			Created ResponseModel as a base class for all response models.
*/

namespace ViewModel.Models.ResponseModels;

public class ResponseModel<T>
{
	public bool Success { get; set; }
	public string? ErrorCode { get; set; }
	public string? Message { get; set; }
	public T? Data { get; set; }
}