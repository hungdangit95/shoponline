using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ShopOnlineApp.Data.Enums;
using ShopOnlineApp.Data.Interfaces;
using ShopOnlineApp.Infrastructure.SharedKernel;

namespace ShopOnlineApp.Data.Entities
{
    [Table("Blogs")]
    public class Blog : DomainEntity<int>, ISwitchable, IDateTracking, IHasSeoMetaData
    {
        public Blog() { }
        public Blog(string name,string thumbnailImage,
           string description, string content, bool? homeFlag, bool? hotFlag,
           string tags, Status status, string seoPageTitle,
           string seoAlias, string seoMetaKeyword,
           string seoMetaDescription)
        {
            Name = name;
            Image = thumbnailImage;
            Description = description;
            Content = content;
            HomeFlag = homeFlag;
            HotFlag = hotFlag;
            Tags = tags;
            Status = status;
            SeoPageTitle = seoPageTitle;
            SeoAlias = seoAlias;
            SeoKeywords = seoMetaKeyword;
            SeoDescription = seoMetaDescription;
        }

        public Blog(int id, string name,string thumbnailImage,
             string description, string content, bool? homeFlag, bool? hotFlag,
             string tags, Status status, string seoPageTitle,
             string seoAlias, string seoMetaKeyword,
             string seoMetaDescription)
        {
            Id = id;
            Name = name;
            Image = thumbnailImage;
            Description = description;
            Content = content;
            HomeFlag = homeFlag;
            HotFlag = hotFlag;
            Tags = tags;
            Status = status;
            SeoPageTitle = seoPageTitle;
            SeoAlias = seoAlias;
            SeoKeywords = seoMetaKeyword;
            SeoDescription = seoMetaDescription;
        }
        [Required]
        [MaxLength(256)]
        public string Name { set; get; }


        [MaxLength(256)]
        public string Image { set; get; }

        [MaxLength(500)]
        public string Description { set; get; }
        public string Content { set; get; }
       public bool? HomeFlag { set; get; }
        public bool? HotFlag { set; get; }
        public int? ViewCount { set; get; }
        public string Tags { get; set; }
        public virtual ICollection<BlogTag> BlogTags { set; get; }
        public DateTime DateCreated { set; get; }
        public DateTime DateModified { set; get; }
        public Status Status { set; get; }
        [Required]
        public int BlogCategoryId { get; set; }

        [MaxLength(256)]
        public string SeoPageTitle { set; get; }

        [MaxLength(256)]
        public string SeoAlias { set; get; }

        [MaxLength(256)]
        public string SeoKeywords { set; get; }

        [MaxLength(256)]
        public string SeoDescription { set; get; }

        [ForeignKey("BlogCategoryId")]
        public virtual BlogCategory BlogCategory { set; get; }

        public virtual ICollection<BlogComment> BlogComments { set; get; }
        //public virtual ICollection<BlogCategory> BlogCategories { set; get; }
    }
}