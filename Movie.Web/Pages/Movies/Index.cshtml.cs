using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Movie.Application.Interfaces;
using Movie.Domain.DTOs;

namespace Movie.Web.Pages.Movies
{
    public class IndexModel : PageModel
    {
        private readonly IMovieService _movieService;

        public IndexModel(IMovieService movieService)
        {
            _movieService = movieService;
        }

        public ICollection<MovieDTO> Movies { get; set; } = new List<MovieDTO>();

        // ---------- search filter fields ----------
        [BindProperty]
        public int? Year { get; set; }

        [BindProperty]
        public string? StudioName { get; set; }

        [BindProperty]
        public int? MinimumActorCount { get; set; }

        public bool IsFiltered { get; set; }

        public string? SearchError { get; set; }

        // Normal page load (and also what "Reset Filter" does: the link
        // goes to the page without any filter values, so everything is shown again)
        public async Task OnGetAsync()
        {
            await LoadAllMoviesAsync();
        }

        public async Task OnPostSearchAsync()
        {
            if (string.IsNullOrWhiteSpace(StudioName))
            {
                SearchError = "Please enter a studio name to search.";
                await LoadAllMoviesAsync();
                return;
            }

            var results = await _movieService.SearchMoviesByStudioAsync(
                Year ?? 0,
                StudioName.Trim(),
                MinimumActorCount ?? 0);

            Movies = Sort(results);
            IsFiltered = true;
        }

        private async Task LoadAllMoviesAsync()
        {
            var movies = await _movieService.GetAllMoviesAsync();
            Movies = Sort(movies);
        }

        // newest first, then alphabetical
        private static List<MovieDTO> Sort(IEnumerable<MovieDTO> movies)
        {
            return movies
                .OrderByDescending(m => m.ReleaseYear)
                .ThenBy(m => m.Title)
                .ToList();
        }
    }
}