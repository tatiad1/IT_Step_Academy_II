using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Movie.Application.Interfaces;
using Movie.Domain.DTOs;

namespace Movie.Web.Pages.Movies
{
    public class DetailsModel : PageModel
    {
        private readonly IMovieService _movieService;

        public DetailsModel(IMovieService movieService)
        {
            _movieService = movieService;
        }

        public MovieDTO MovieDetails { get; set; } = null!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var movie = await _movieService.GetMovieByIDAsync(id);

            if (movie == null)
            {
                return NotFound();
            }

            MovieDetails = movie;
            return Page();
        }
    }
}