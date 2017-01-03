using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MusicStore.Repository;
using MusicStore.Models;
using MusicStore.Service;

namespace MusicStore.Tests.Fakes
{
    public class FakeAlbumService : IAlbumService
    {
        void IAlbumService.DeleteAlbum(int albumID)
        {
            throw new NotImplementedException();
        }

        Album IAlbumService.GetAlumById(int Id)
        {
            throw new NotImplementedException();
        }

        void IAlbumService.InsertAlbum(Album album)
        {
            throw new NotImplementedException();
        }

        IEnumerable<Album> IAlbumService.ListAlbum()
        {
            throw new NotImplementedException();
        }

        void IAlbumService.UpdateAlum(Album album)
        {
            throw new NotImplementedException();
        }
    }
}
