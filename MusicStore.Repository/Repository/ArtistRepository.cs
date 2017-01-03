using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MusicStore.Models;
namespace MusicStore.Repository
{
    public class ArtistRepository : GenericRepository<Artist>, IArtistRepository
    {
        public ArtistRepository(ApplicationDbContext context):base(context)
        { }

        public IEnumerable<Artist> ListArtist()
        {
            return base.Get();
        }

        public Artist GetArtistById(int artistId)
        {
            return dbSet.Where(a => a.ArtistId == artistId).FirstOrDefault();
        }
    }
}