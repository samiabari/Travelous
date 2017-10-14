//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class Place
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Place()
        {
            this.CheckIns = new HashSet<CheckIn>();
            this.HostAlbums = new HashSet<HostAlbum>();
            this.HostPictures = new HashSet<HostPicture>();
            this.OfferAlbums = new HashSet<OfferAlbum>();
            this.OfferPictures = new HashSet<OfferPicture>();
            this.PlaceAlbums = new HashSet<PlaceAlbum>();
            this.PlacePictures = new HashSet<PlacePicture>();
            this.TouristAlbums = new HashSet<TouristAlbum>();
            this.VendorAlbums = new HashSet<VendorAlbum>();
            this.VendorPictures = new HashSet<VendorPicture>();
            this.WishLists = new HashSet<WishList>();
        }
    
        public int PlaceId { get; set; }
        public string PlaceName { get; set; }
        public Nullable<int> CountryId { get; set; }
        public Nullable<int> CityId { get; set; }
        public Nullable<int> PlaceTypeId { get; set; }
        public string PlaceAddress { get; set; }
        public string PlaceDetail { get; set; }
        public Nullable<bool> PlaceAdminsPermit { get; set; }
        public Nullable<System.DateTime> PlaceDateOfAccountCreation { get; set; }
        public Nullable<int> PlaceFavourite { get; set; }
        public Nullable<int> PlaceProfilePicId { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CheckIn> CheckIns { get; set; }
        public virtual City City { get; set; }
        public virtual Country Country { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HostAlbum> HostAlbums { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HostPicture> HostPictures { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OfferAlbum> OfferAlbums { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OfferPicture> OfferPictures { get; set; }
        public virtual PlacePicture PlacePicture { get; set; }
        public virtual PlaceType PlaceType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PlaceAlbum> PlaceAlbums { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PlacePicture> PlacePictures { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TouristAlbum> TouristAlbums { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VendorAlbum> VendorAlbums { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VendorPicture> VendorPictures { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WishList> WishLists { get; set; }
    }
}
