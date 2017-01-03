using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Models.Utilities
{
    public class PagedAlbums
    {
        public int Count;

        public PagedAlbums() {
        }

        public IEnumerable<Album> Items { get; set; }
    }
}
