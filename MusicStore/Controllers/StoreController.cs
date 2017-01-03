using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MusicStore.Models;
using PagedList;
using MusicStore.Repository;
using MusicStore.Service;
namespace MusicStore.Controllers
{
    public class StoreController : Controller
    {

        private readonly IAlbumService _albumService;
        private readonly IGenreService _genreService;

        public StoreController()
        {
            _albumService = new AlbumService(new UnitOfWork(), new ModelStateWrapper(this.ModelState));
            _genreService = new GenreService(new UnitOfWork());
        }
        public StoreController(IAlbumService storeService, IGenreService genreService)
        {
            _albumService = storeService;
            _genreService = genreService;
        }
        public ActionResult Index()
        {
            var genres = _albumService.ListAlbum();
            return View(genres);
        }
        //
        // GET: /Store/Browse
        public ActionResult Browse(string genre, int? page)
        {
            ViewBag.Genre = genre;
            int pageSize = 21;
            int pageNumber = (page ?? 1);

            var Albums = _albumService.ListAlbum().Where(a => a.Genre.Name == genre).OrderBy(a => a.Title);           
            return View(Albums.ToPagedList(pageNumber, pageSize));


        }
        //
        // GET: /Store/Details
        public ActionResult Details(int id)
        {
            var Album = _albumService.GetAlumById(id);
            return View(Album);
        }

        [ChildActionOnly]
        public ActionResult GenreMenu()
        {
            var genres = _genreService.ListGenre();
            return PartialView(genres);
        }
    }
}