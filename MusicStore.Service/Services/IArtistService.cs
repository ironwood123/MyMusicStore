using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicStore.Models;
namespace MusicStore.Service
{
   public interface IArtistService
    {
        IEnumerable<Artist> ListArtist();
        Artist GetArtistById(int Id);
    }
}
