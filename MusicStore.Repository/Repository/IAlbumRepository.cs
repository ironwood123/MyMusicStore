using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicStore.Models;
using System.Linq.Expressions;
namespace MusicStore.Repository
{
    public interface IAlbumRepository: IGenericRepository<Album>
    {
        Genre GetAlbumGenre(int albumID);
        Artist GetAlumArtist(int albumID);
        IEnumerable<Album> GetCatalogAlbums(Expression<Func<Album, bool>> filter, Func<IQueryable<Album>, IOrderedQueryable<Album>> orderBy);
        IEnumerable<Album> GetPagedAlbums(int? page, int? pageSize, out int totalCount);
    }
}
