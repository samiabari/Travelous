﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Travel.Models.DB
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Validation;
    using System.Linq;

    public partial class TourDBEntities : DbContext
    {
        public TourDBEntities()
            : base("name=TourDBEntities")
        {
        }
        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                string errorMessages = string.Join("; ", ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Select(x => x.ErrorMessage));
                throw new DbEntityValidationException(errorMessages);
            }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AlbumType> AlbumTypes { get; set; }
        public virtual DbSet<CheckIn> CheckIns { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Host> Hosts { get; set; }
        public virtual DbSet<HostAlbum> HostAlbums { get; set; }
        public virtual DbSet<HostPicture> HostPictures { get; set; }
        public virtual DbSet<HostType> HostTypes { get; set; }
        public virtual DbSet<Offer> Offers { get; set; }
        public virtual DbSet<OfferAlbum> OfferAlbums { get; set; }
        public virtual DbSet<OfferPicture> OfferPictures { get; set; }
        public virtual DbSet<OfferType> OfferTypes { get; set; }
        public virtual DbSet<Place> Places { get; set; }
        public virtual DbSet<PlaceAlbum> PlaceAlbums { get; set; }
        public virtual DbSet<PlacePicture> PlacePictures { get; set; }
        public virtual DbSet<PlaceType> PlaceTypes { get; set; }
        public virtual DbSet<Subscriber> Subscribers { get; set; }
        public virtual DbSet<Tourist> Tourists { get; set; }
        public virtual DbSet<TouristAlbum> TouristAlbums { get; set; }
        public virtual DbSet<TouristPicture> TouristPictures { get; set; }
        public virtual DbSet<Vendor> Vendors { get; set; }
        public virtual DbSet<VendorAlbum> VendorAlbums { get; set; }
        public virtual DbSet<VendorPicture> VendorPictures { get; set; }
        public virtual DbSet<VendorType> VendorTypes { get; set; }
        public virtual DbSet<WishList> WishLists { get; set; }
    }
}
