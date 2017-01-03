using System;
using System.Collections.Generic;
using MusicStore.Models;
using MusicStore.Models.Utilities;
namespace MusicStore.Service
{
   public  interface IAlbumService
    {
      //  IUnitOfWork _unitOfWork { get; }
        IEnumerable<Album> ListAlbum();
        PagedAlbums Search(KendoRequest kendorequest);
        Album GetAlumById(int Id);
        void InsertAlbum(Album album);
        void DeleteAlbum(int albumID);
        void UpdateAlum(Album album);
        void Dispose();
    }
}
