using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using MusicStore.Models;
using MusicStore.Service;
using MusicStore.Repository;
namespace MusicStore.WebAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [AllowAnonymous]
    public class GenreController : ApiController
    {
       // private ApplicationDbContext db = new ApplicationDbContext();
        private readonly IGenreService _genreService;
        // GET: api/GenressService

        public GenreController( IGenreService genreService)
        {
            _genreService = genreService;
        }
        public GenreController()
        {
            _genreService = new GenreService(new UnitOfWork());
        }
        // GET: api/Genre
        public IEnumerable<Genre> GetGenres()
        {
            
            return _genreService.ListGenre().ToList();
        }

        //GET: api/Genres/5

        public IHttpActionResult GetGenre(int id)
        {
            Genre genre = _genreService.GetGenreById(id);
            if (genre == null)
            {
                return NotFound();
            }

            return Ok(genre);
        }

        //// PUT: api/Genres/5
        //[ResponseType(typeof(void))]
        //public IHttpActionResult PutGenre(int id, Genre genre)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != genre.GenreId)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(genre).State = EntityState.Modified;

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!GenreExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return StatusCode(HttpStatusCode.NoContent);
        //}

        //// POST: api/Genres
        //[ResponseType(typeof(Genre))]
        //public IHttpActionResult PostGenre(Genre genre)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.Genres.Add(genre);
        //    db.SaveChanges();

        //    return CreatedAtRoute("DefaultApi", new { id = genre.GenreId }, genre);
        //}

        //// DELETE: api/Genres/5
        //[ResponseType(typeof(Genre))]
        //public IHttpActionResult DeleteGenre(int id)
        //{
        //    Genre genre = db.Genres.Find(id);
        //    if (genre == null)
        //    {
        //        return NotFound();
        //    }

        //    db.Genres.Remove(genre);
        //    db.SaveChanges();

        //    return Ok(genre);
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

        //private bool GenreExists(int id)
        //{
        //    return db.Genres.Count(e => e.GenreId == id) > 0;
        //}
    }
}