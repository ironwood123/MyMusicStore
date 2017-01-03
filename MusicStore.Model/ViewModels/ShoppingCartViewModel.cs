using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MusicStore.Models;
namespace MusicStore.Models.ViewModels
{
    public class ShoppingCartViewModel
    {
        [System.ComponentModel.DataAnnotations.Key]
        public int Id { get; set; }
        public List<Cart> CartItems { get; set; }
        public decimal CartTotal { get; set; }
    }
}