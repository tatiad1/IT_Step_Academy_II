using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Movie.Application.Interfaces;
using Movie.Domain.DTOs;

namespace Movie.Web.Pages.Movies
{
    public class EditModel : PageModel
    {
        private readonly IMovieService _movieService;

        public EditModel(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [BindProperty]
        public UpdateMovieDTO Input { get; set; } = new();

        public string CurrentStudioName { get; set; } = string.Empty;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var movie = await _movieService.GetMovieByIDAsync(id);

            if (movie == null)
            {
                return NotFound();
            }

            Input = new UpdateMovieDTO
            {
                Id = movie.Id,
                Title = movie.Title,
                ReleaseYear = movie.ReleaseYear,
                StudioId = movie.StudioId
            };
            CurrentStudioName = movie.StudioName;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var existing = await _movieService.GetMovieByIDAsync(id);

            if (existing == null)
            {
                return NotFound();
            }

            CurrentStudioName = existing.StudioName;
            Input.Id = id; // always trust the URL, not the form

            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var updated = await _movieService.UpdateMovieAsync(Input);

                if (!updated)
                {
                    return NotFound();
                }

                TempData["Success"] = $"Movie \"{Input.Title}\" was updated successfully.";
                return RedirectToPage("/Movies/Index");
            }
            catch (Exception)
            {
                // most likely cause: a Studio ID that doesn't exist (foreign key error)
                ModelState.AddModelError(string.Empty,
                    "Could not update the movie. Please make sure the Studio ID exists.");
                return Page();
            }
        }
    }
}