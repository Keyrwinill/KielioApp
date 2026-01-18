/*
	Update date			Description 
	--------------------------------
	20251115			Initial
*/
using ViewModel.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http.Headers;
using System.Text.Json;

namespace KielioApp.Controllers;

public class ImageController : Controller
{
	private readonly IWebHostEnvironment _env;
	private readonly HttpClient _httpClient;

	public ImageController(IWebHostEnvironment env)
	{
		_env = env;
		_httpClient = new HttpClient();
	}

	[HttpGet]
	public IActionResult Blur()
	{
		return View("Index");
	}

	[HttpPost]
	public async Task<IActionResult> Blur(IFormFile imageFile)
	{
		if (imageFile == null || imageFile.Length == 0)
		{
			ViewBag.Error = "Please choose an image.";
			return View("Index");
		}

		// Save original image to /wwwroot/uploads
		var uplaodsPath = Path.Combine(_env.WebRootPath, "uploads");
		Directory.CreateDirectory(uplaodsPath);
		var originalFilePath = Path.Combine(uplaodsPath, imageFile.Name);
		using (var stream = new FileStream(originalFilePath, FileMode.Create))
		{
			await imageFile.CopyToAsync(stream);
		}

		// Send to Python API
		using var content = new MultipartFormDataContent();
		using var fileStream = new FileStream(originalFilePath, FileMode.Open, FileAccess.Read);
		var fileContent = new StreamContent(fileStream);
		fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(imageFile.ContentType);
		content.Add(fileContent, "file", imageFile.FileName);

		var response = await _httpClient.PostAsync("http://127.0.0.1:8000/Image/Blur", content);

		if (!response.IsSuccessStatusCode)
		{
			ViewBag.Error = $"{response.StatusCode} - {await response.Content.ReadAsStringAsync()}";
			return View("Index");
		}

		// Save processed image to /wwwroot/processed
		var processedPath = Path.Combine(_env.WebRootPath, "processed");
		Directory.CreateDirectory(processedPath);
		var processedFilePath = Path.Combine(processedPath, "processed_" + imageFile.FileName);
		var processedBytes = await response.Content.ReadAsByteArrayAsync();

		System.IO.File.WriteAllBytes(processedFilePath, processedBytes);

		// Pass paths to Razor
		ViewBag.OriginalImage = "/uploads/" + imageFile.FileName;
		ViewBag.ProcessedImage = "/processed/" + "processed_" + imageFile.FileName;

		return View("Index");
	}
}