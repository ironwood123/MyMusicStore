﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MusicStore.Models;
using MusicStore.Repository;

namespace MusicStore.Service
{
    public class GenreService : IGenreService
    {
        protected IUnitOfWork _unitOfWork;
        public GenreService(IUnitOfWork unitOfWork)
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
        public IEnumerable<Genre> ListGenre()
        {
            return _unitOfWork.GenreRepository.ListGenre();
        }

        public Genre GetGenreById(int Id)
        {
            return _unitOfWork.GenreRepository.GetGenreByID(Id);
        }

    }
}