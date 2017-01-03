using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MusicStore.Models;
namespace MusicStore.Repository
{
    public class GenreRepository : GenericRepository<Genre>, IGenreRepository
    {
        public GenreRepository(ApplicationDbContext context):base(context)
        { }

        public IEnumerable<Genre> ListGenre()
        {
            return base.Get();
        }
        public Genre GetGenreByID(int genreID)
        {
            return dbSet.Where(g => g.GenreId == genreID).FirstOrDefault();
        }
    }
}