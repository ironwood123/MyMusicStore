using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using MusicStore.Models;
using MusicStore.Models.Utilities;
using MusicStore.Service;
using MusicStore.Repository;
using System.Web.Http.Cors;
using System.Threading.Tasks;
namespace MusicStore.WebAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [AllowAnonymous]
    public class AlbumController : ApiController
    {
        private readonly IAlbumService _albumService;
        private readonly IGenreService _genreService;
        // GET: api/AlbumsService

        public AlbumController(IAlbumService albumService, IGenreService genreService)
        {
            _albumService = albumService;
            _genreService = genreService;
        }
        public AlbumController()
        {
            _albumService = new AlbumService(new UnitOfWork(), null);
            _genreService = new GenreService(new UnitOfWork());
        }
        [HttpGet]
        [Route("api/Album/albums")]
        public IEnumerable<Album> GetAlbums()
        {
            return _albumService.ListAlbum().ToList();
        }
        [HttpPost]
        [Route("api/Album/catalogalbums")]
        public IHttpActionResult GetPagedAlbum(KendoRequest kendorequest)
        {
            var albums = _albumService.Search(kendorequest);
            return Ok(albums);
        }

        [HttpGet]
        [Route("api/AlbumByGenre/{genreID}")]
        public IEnumerable<Album> Browse(int genreID)
        {
            var Albums = _albumService.ListAlbum().Where(a => a.Genre.GenreId == genreID).OrderBy(a => a.Title);
            return Albums.ToList();
        }

        [HttpGet]
        [ResponseType(typeof(Album))]
        [Route("api/Album/{id}")]
        public IHttpActionResult GetAlbum(int id)
        {
            Album album = _albumService.GetAlumById(id);
            if (album == null)
            {
                return NotFound();
            }
         
            return Ok(album);
        }

        // PUT: api/Albume/5
        [HttpPut]
        [ResponseType(typeof(void))]
        [Route("api/Album/{id}")]
        public IHttpActionResult PutAlbum(int id, Album album)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != album.AlbumId)
            {
                return BadRequest();
            }

            try
            {
                _albumService.UpdateAlum(album);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AlbumExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Album
        [HttpPost]
        [Route("api/Album")]
        public IHttpActionResult PostAlbum(Album album)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _albumService.InsertAlbum(album);

            return CreatedAtRoute("DefaultApi", new { id = album.AlbumId }, album);
        }

        // DELETE: api/Album/5
        [HttpDelete]
        [Route("api/Album/{id}")]
        public IHttpActionResult DeleteAlbum(int id)
        {
            Album album = _albumService.GetAlumById(id);
            if (album == null)
            {
                return NotFound();
            }

            _albumService.DeleteAlbum(id);

            return Ok(album);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _albumService.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AlbumExists(int id)
        {
            return _albumService.ListAlbum().Count(e => e.AlbumId == id) > 0;
        }
    }
}