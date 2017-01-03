using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MusicStore.Models;
namespace MusicStore.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _context = new ApplicationDbContext();
        private AlbumRepository albumRepository;
        private GenreRepository genreRepository;
        private ArtistRepository artistRepository;
        private CartRepository cartRepository;
        private OrderRepository orderRepository;
        private OrderDetailRepository orderdetailRepository;
        public AlbumRepository AlbumRepository
        {
            get
            {
                if (this.albumRepository == null)
                {
                    this.albumRepository = new AlbumRepository(_context);
                }

                return albumRepository;
            }

        }

        public OrderRepository OrderRepository
        {
            get
            {
                if (this.orderRepository == null)
                {
                    this.orderRepository = new OrderRepository(_context);
                }
                return orderRepository;
            }
        }

        public OrderDetailRepository OrderDetailRepository
        {
            get
            {
                if (this.orderdetailRepository == null)
                {
                    this.orderdetailRepository = new OrderDetailRepository(_context);
                }
                return orderdetailRepository;
            }
        }
        public GenreRepository GenreRepository
        {
            get
            {
                if (this.genreRepository == null)
                {
                    this.genreRepository = new GenreRepository(_context);
                }

                return genreRepository;
            }
        }
        public CartRepository CartRepository
        {
            get
            {
                if (this.cartRepository == null)
                {
                    this.cartRepository = new CartRepository(_context);
                }
                return this.cartRepository;
            }
        }
        public ArtistRepository ArtistRepository
        {
            get
            {
                if (this.artistRepository == null) {
                    this.artistRepository = new ArtistRepository(_context);
                }
                return artistRepository;
            }

        }
        public void Save()
        {
            _context.SaveChanges();
        }


        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}