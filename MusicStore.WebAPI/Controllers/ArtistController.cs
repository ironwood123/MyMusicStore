using System;
using System.Collections.Generic;
using System.Web.Http.Cors;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using MusicStore.Models;
using MusicStore.Service;
using MusicStore.Repository;
namespace MusicStore.WebAPI.Controllers
{
    [EnableCors(origins: "http://localhost:6225", headers: "*", methods: "*")]
    [AllowAnonymous]
    public class ArtistController : ApiController
    {
        private readonly IArtistService _artistService;
        public ArtistController(IArtistService artistService)
        {
            _artistService = artistService;
        }
        public ArtistController()
        {
            _artistService = new ArtistService(new UnitOfWork());
        }
        // GET: api/Artists
        public IEnumerable<Artist> GetArtists()
        {
            return _artistService.ListArtist().ToList();
        }

        // GET: api/Artists/5
        [ResponseType(typeof(Artist))]
        public IHttpActionResult GetArtist(int id)
        {
            Artist artist = _artistService.GetArtistById(id);
            if (artist == null)
            {
                return NotFound();
            }

            return Ok(artist);
        }

        // PUT: api/Artists/5
        //[ResponseType(typeof(Artist))]
        //public IHttpActionResult PutArtist(int id, Artist artist)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != artist.ArtistId)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(artist).State = EntityState.Modified;

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!ArtistExists(id))
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

        //// POST: api/Artists
        //[ResponseType(typeof(Artist))]
        //public IHttpActionResult PostArtist(Artist artist)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.Artists.Add(artist);
        //    db.SaveChanges();

        //    return CreatedAtRoute("DefaultApi", new { id = artist.ArtistId }, artist);
        //}

        //// DELETE: api/Artists/5
        //[ResponseType(typeof(Artist))]
        //public IHttpActionResult DeleteArtist(int id)
        //{
        //    Artist artist = db.Artists.Find(id);
        //    if (artist == null)
        //    {
        //        return NotFound();
        //    }

        //    db.Artists.Remove(artist);
        //    db.SaveChanges();

        //    return Ok(artist);
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

        //private bool ArtistExists(int id)
        //{
        //    return db.Artists.Count(e => e.ArtistId == id) > 0;
        //}
    }
}