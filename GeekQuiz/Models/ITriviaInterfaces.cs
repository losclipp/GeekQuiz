using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace GeekQuiz.Models
{
	public interface ITriviaContext : IDisposable
	{
		DbSet<TriviaQuestion> TriviaQuestions { get; set; }

		DbSet<TriviaOption> TriviaOptions { get; set; }

		DbSet<TriviaAnswer> TriviaAnswers { get; set; }

		Task<int> SaveChangesAsync();
	}
}