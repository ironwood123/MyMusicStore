using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using MusicStore.Models;
using System.Linq.Expressions;
namespace MusicStore.Repository
{
    public class AlbumRepository : GenericRepository<Album>, IAlbumRepository
    {
        public AlbumRepository(ApplicationDbContext context):base(context)
        { }
        public Album GetAlbumByID(int albumID)
        {
            return dbSet.Where( a => a.AlbumId == albumID).FirstOrDefault();
        }
        public IEnumerable<Album> GetAlbums()
        {
            return base.Get(null,null,"genre,artist");
        }

        public IEnumerable<Album> GetCatalogAlbums(Expression<Func<Album, bool>> filter, Func<IQueryable<Album>, IOrderedQueryable<Album>> orderBy)
        {
            return base.Get(filter, orderBy, "genre,artist");
        }

        public IEnumerable<Album> GetPagedAlbums(int? page, int? pageSize, out int totalCount)
        {
            var albums = GetAlbums();
            totalCount = albums.Count();
            return base.SelectPage(out totalCount, null, null, null, page, pageSize);
        }
        public void InsertAlbum(Album album)
        {
            base.Insert(album);
        }
        public void DeleteAlbum(int albumID)
        {
            base.Delete(albumID);
        }
        public void UpdateAlum(Album album)
        {
            base.Update(album);
        }

        public Genre GetAlbumGenre(int albumID)
        {
            return dbSet.Find(albumID).Genre;
        }
        public Artist GetAlumArtist(int albumID)
        {
            return dbSet.Find(albumID).Artist;
        }

    }
}