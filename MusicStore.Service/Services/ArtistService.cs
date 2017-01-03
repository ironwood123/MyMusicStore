using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MusicStore.Repository;
using MusicStore.Models;
namespace MusicStore.Service
{
    public class ArtistService : IArtistService
    {
        private IUnitOfWork _unitOfWork;
        public ArtistService(IUnitOfWork unitOfWork)
        {
            // _modelState = modelState;
            _unitOfWork = unitOfWork;
        }

        //protected bool ValidateProduct(Product productToValidate)
        //{
        //    if (productToValidate.Name.Trim().Length == 0)
        //        _modelState.AddModelError("Name", "Name is required.");
        //    if (productToValidate.Description.Trim().Length == 0)
        //        _modelState.AddModelError("Description", "Description is required.");
        //    if (productToValidate.UnitsInStock < 0)
        //        _modelState.AddModelError("UnitsInStock", "Units in stock cannot be less than zero.");
        //    return _modelState.IsValid;
        //}
        public IEnumerable<Artist> ListArtist()
        {
            return _unitOfWork.ArtistRepository.ListArtist();
        }

        public Artist GetArtistById(int Id)
        {
            return _unitOfWork.ArtistRepository.GetArtistById(Id);
        }
    }
}