using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MusicStore.Repository;
using MusicStore.Models;
using MusicStore.Models.Utilities;
using System.Linq.Expressions;

namespace MusicStore.Service
{
    public class AlbumService : IAlbumService
    {

        private IUnitOfWork _unitOfWork;
        private IValidationDictionary _validatonDictionary = null;

        public AlbumService(IUnitOfWork unitOfWork, IValidationDictionary validationDictionary)
        {
            _unitOfWork = unitOfWork;
            _validatonDictionary = validationDictionary;
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

        public PagedAlbums Search(KendoRequest kendorequest)
        {

            var albumquery = _unitOfWork.AlbumRepository.Query(); 



            int totalRecords;

            Expression<Func<Album, bool>> strwhere = null;
            Func<IQueryable<Album>, IOrderedQueryable<Album>> order = null;
            if (kendorequest != null && kendorequest.filter != null && (kendorequest.filter.filters != null && kendorequest.filter.filters.Count > 0))
            {
                strwhere = ExpressionBuilder.GetWhereExpression<Album>(kendorequest.filter);
            }

            
            if (kendorequest != null && kendorequest.sort != null && kendorequest.sort.Count > 0)
            {
                foreach (var s in kendorequest.sort)
                {
                    //s.field = (s.field == "ProductType") ? "ProductTypeId" : s.Field;
                    order = ExpressionBuilder.GetOrderBy<Album>(s.field, s.dir);
                    albumquery = _unitOfWork.AlbumRepository.Query(strwhere, order);
                }
            }
            else
            {
                order = ExpressionBuilder.GetOrderBy<Album>("AlbumId", "asc");
                albumquery = _unitOfWork.AlbumRepository.Query(strwhere, order);
            }

            totalRecords = albumquery.Count();
            if (kendorequest.page != 0 && kendorequest.pageSize != 0) {
                albumquery = albumquery.Skip((kendorequest.page - 1) * kendorequest.pageSize).Take(kendorequest.pageSize).ToList().AsQueryable();
            }

            PagedAlbums r = new PagedAlbums { Count = totalRecords, Items = albumquery };

            return r;
        }





        public IEnumerable<Album> ListAlbum()
        {
              return _unitOfWork.AlbumRepository.GetAlbums();
        }

        public Album GetAlumById(int Id)
        {
            return _unitOfWork.AlbumRepository.GetAlbumByID(Id);
        }
        public void InsertAlbum(Album album)
        {
            _unitOfWork.AlbumRepository.InsertAlbum(album);
            _unitOfWork.Save();
        }

        public void DeleteAlbum(int albumID)
        {
            _unitOfWork.AlbumRepository.DeleteAlbum(albumID);
            _unitOfWork.Save();
        }

        public void UpdateAlum(Album album)
        {
            _unitOfWork.AlbumRepository.UpdateAlum(album);
            _unitOfWork.Save();
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}