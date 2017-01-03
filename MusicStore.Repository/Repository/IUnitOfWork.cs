using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicStore.Models;

namespace MusicStore.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        AlbumRepository AlbumRepository { get; } 
        GenreRepository GenreRepository { get; }
        ArtistRepository ArtistRepository { get; }
        CartRepository CartRepository { get; }
        OrderRepository OrderRepository { get; }
        OrderDetailRepository OrderDetailRepository { get; }
        void Save();
    }
}
