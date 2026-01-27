/*
	Update date			Description 
	--------------------------------
	20251125			Initial
*/
using DataAccessLibrary.Entities;
using DataAccessLibrary.Interfaces;
using ViewModel.Models.ResponseModels;
using WebService.Interfaces;
using ViewModel.Enums;

namespace WebService;

public class DeutschAdjektivService : IDeutschAdjektivService
{
	private readonly IDeutschAdjektivRepository _detschadj;

	public DeutschAdjektivService(IDeutschAdjektivRepository detschadj)
	{
		_detschadj = detschadj;
	}

	public List<DeutschAdjResponseViewModel> GetViewByType(string articleType)
	{
		var adjectives = _detschadj.GetAll().ToList();

		var articleTypeList = GetArticleTypeList(adjectives);

		var response = new List<DeutschAdjResponseViewModel>();

		if (string.IsNullOrEmpty(articleType))
		{
			var model = new DeutschAdjResponseViewModel
			{
				Type = articleType,
				Gender = "",
				Case = "",
				ArticleEnding = "",
				AdjectiveEnding = "",
				ArticleTypeList = articleTypeList,
			};
			response.Add(model);
		}
		else if (articleType == "All")
		{
			foreach (var adj in adjectives)
			{
				var model = new DeutschAdjResponseViewModel
				{
					Type = adj.Type,
					Gender = adj.Gender,
					Case = adj.Case,
					ArticleEnding = adj.ArticleEnding,
					AdjectiveEnding = adj.AdjectiveEnding,
					ArticleTypeList = articleTypeList,
				};
				response.Add(model);
			}
		}
		else
		{
			var adjFiltered = adjectives.Where(adj => adj.Type == articleType).ToList();
			foreach (var adj in adjFiltered)
			{
				var model = new DeutschAdjResponseViewModel
				{
					Type = adj.Type,
					Gender = adj.Gender,
					Case = adj.Case,
					ArticleEnding = adj.ArticleEnding,
					AdjectiveEnding = adj.AdjectiveEnding,
					ArticleTypeList = articleTypeList,
				};
				response.Add(model);
			}
		}

		return response;

		List<string?> GetArticleTypeList(List<DeutschAdjektiv> adjectives)
		{
			var articleTypes = adjectives.Where(adj => !string.IsNullOrEmpty(adj.Type))
										 .Select(adj => adj.Type)
										 .Distinct()
										 .ToList();
			var sortedTypes = articleTypes.OrderBy(type => (int)Enum.Parse(typeof(DeutschArticleType), type)).ToList();
			sortedTypes.Insert(0, "All");
			sortedTypes.Insert(0, "");
			return sortedTypes;
		}
	}
}
