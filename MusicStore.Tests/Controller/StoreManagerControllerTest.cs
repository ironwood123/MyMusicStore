using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Threading.Tasks;
using MusicStore.Controllers;
using MusicStore.Repository;
using MusicStore.Models;
using MusicStore.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
namespace MusicStore.Tests.Controller
{
    [TestClass]
    public class StoreManagerControllerTest
    {
        private Mock<IAlbumService> _albumService;
        private Mock<IArtistService> _artistService;
        private Mock<IGenreService> _genreService;

        [TestInitialize]
        public void Initialize()
        {
            _albumService = new Mock<IAlbumService>();
            _genreService = new Mock<IGenreService>();
            _artistService = new Mock<IArtistService>();
        }

        [TestMethod]
        public void TestDetailsViewData()
        {
            var album = new Album { Title = "The Best Of Men At Work", Genre = new Genre() { Name = "Rock" }, Price = 8.99M, Artist = new Artist() { Name = "Aaron Goldberg" }, AlbumArtUrl = "/Content/Images/placeholder.gif" };

            //albumService.Setup(x => x.InsertAlbum(album)).Verifiable();          
            // var controller = new StoreManagerController(_albumService.Object,null,null);

            //  var lastalbum = _albumService.SetupGet<Album>(x => x.ListAlbum().OrderByDescending(c => c.AlbumId).FirstOrDefault());

            // lastalbum.Returns(album);

            var data = (RedirectToRouteResult)new StoreManagerController(_albumService.Object, _artistService.Object, _genreService.Object).Create(album);
            Assert.IsNotNull(data);
            Assert.AreEqual("Index", data.RouteValues["action"]);
        }
    }
}