using System;
using System.Collections.Generic;
//using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using MusicStore.Models;
using MusicStore.Repository;
using MusicStore.Service;
using PagedList;
namespace MusicStore.Controllers
{
    public class StoreManagerController : Controller
    {
        private readonly IAlbumService _albumService;
        private readonly IGenreService _genreService;
        private readonly IArtistService _artistService;

        public StoreManagerController()
        {
            _albumService = new AlbumService(new UnitOfWork(), new ModelStateWrapper(this.ModelState));
            _artistService = new ArtistService(new UnitOfWork());
            _genreService = new GenreService(new UnitOfWork());
        }
        public StoreManagerController(IAlbumService storeService, IArtistService artistService, IGenreService genreService)
        {
            _albumService = storeService;
            _artistService = artistService;
            _genreService = genreService;
        }
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.GenreSortParm = string.IsNullOrEmpty(sortOrder) ? "Genre_desc" : "";
            ViewBag.PriceSortParm = sortOrder == "Price" ? "Price_asc" : "";

            if (searchString != null)
            {
                page = 1;
            }
            else {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var Albums = _albumService.ListAlbum();

            if (!string.IsNullOrEmpty(searchString)) {
                Albums = Albums.Where(a => a.Title.ToUpper().Contains(searchString.ToUpper()));
            }

            switch (sortOrder) {
                case "Genre_desc":
                    Albums = Albums.OrderByDescending(a => a.Genre.Name);
                    break;
                case "Price_asc":
                    Albums = Albums.OrderBy(a => a.Price);
                    break;
                default:
                    Albums = Albums.OrderBy(a => a.Title);
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(Albums.ToPagedList(pageNumber, pageSize));
        }

        // GET: StoreManger/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Album Album =_albumService.GetAlumById((int)id);
            
            if (Album == null)
            {
                return HttpNotFound();
            }
            return View(Album);
        }

        // GET: StoreManger/Create
        public ActionResult Create()
        {
            ViewBag.ArtistId = new SelectList(_artistService.ListArtist(), "ArtistId", "Name");
            ViewBag.GenreId = new SelectList(_genreService.ListGenre(), "GenreId", "Name");
            return View();
        }

        // POST: StoreManger/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AlbumId,GenreId,ArtistId,Price,Title,AlbumArtUrl")] Album Album)
        {
            if (ModelState.IsValid)
            {
                _albumService.InsertAlbum(Album);
                return RedirectToAction("Index");
            }

            ViewBag.ArtistId = new SelectList(_artistService.ListArtist(), "ArtistId", "Name", Album.ArtistId);
            ViewBag.GenreId = new SelectList(_genreService.ListGenre(), "GenreId", "Name", Album.GenreId);
            return View(Album);
        }

        // GET: StoreManger/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Album Album = _albumService.GetAlumById((int)id);
            if (Album == null)
            {
                return HttpNotFound();
            }

            ViewBag.ArtistId = new SelectList(_artistService.ListArtist(), "ArtistId", "Name", Album.ArtistId);
            ViewBag.GenreId = new SelectList(_genreService.ListGenre(), "GenreId", "Name", Album.GenreId);
            return View(Album);
        }

        // POST: StoreManger/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
       // public ActionResult Edit([Bind(Include = "AlbumId,GenreId,ArtistId,Price,Title,AlbumArtUrl")] Album Album)
        public ActionResult Edit( Album Album)
        {
            if (ModelState.IsValid)
            {
                _albumService.UpdateAlum(Album);
                return RedirectToAction("Index");
            }
            ViewBag.ArtistId = new SelectList(_artistService.ListArtist(), "ArtistId", "Name", Album.ArtistId);
            ViewBag.GenreId = new SelectList(_genreService.ListGenre(), "GenreId", "Name", Album.GenreId);
            return View(Album);
        }

        // GET: StoreManger/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Album Album =_albumService.GetAlumById((int)id);
            if (Album == null)
            {
                return HttpNotFound();
            }
            return View(Album);
        }

        // POST: StoreManger/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Album Album = _albumService.GetAlumById((int)id);
            _albumService.DeleteAlbum(id);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
              
            }
            base.Dispose(disposing);
        }
    }
}
