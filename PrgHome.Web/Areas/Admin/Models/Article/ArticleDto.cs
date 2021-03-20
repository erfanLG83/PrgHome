using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PrgHome.DataLayer.Models;
using PrgHome.Web.Models;

namespace PrgHome.Web.Areas.Admin.Models
{
    public class ArticleDto
    {
        public ArticleDto()
        {

        }
        public ArticleDto(Article article)
        {
            Id = article.Id;
            Title = article.Title;
            IsPublish = article.IsPublish;
            TimeToRead = article.TimeToRead;
            Tags = article.Tags;
            //FormFile = (IFormFile) new System.IO.FileStream(article.Image,System.IO.FileMode.Open);
            Content = article.Content;
            Description = article.Description;
            CategoryId = article.CategoryId;
        }
        public int Id { get; set; }
        [Required(ErrorMessage = "لطفا عنوان را وارد کنید")]
        public string Title { get; set; }
        public int? TimeToRead { get; set; }
        public bool IsPublish { get; set; }
        public string Tags { get; set; }
        public IFormFile FormFile { get; set; }
        public string Content { get; set; }
        public int? CategoryId { get; set; }
        [MaxLength(250, ErrorMessage = "لطفا توضیحات را کمتر از 250 کرکتر وارد کنید")]
        public string Description { get; set; }
        public ModelValid IsValid(ArticleDto dto,bool checkFormFile = true)
        {
            ModelValid modelValid = new ModelValid();
            if (checkFormFile)
            {
                if (dto.FormFile == null)
                {
                    modelValid.Valid = false;
                    modelValid.Errors.Add("در صورت انتشار مقاله , باید عکس مقاله را وارد کنید");
                }
            }
            if (!dto.TimeToRead.HasValue || dto.TimeToRead.Value == 0)
            {
                modelValid.Valid = false;
                modelValid.Errors.Add("در صورت انتشار مقاله , باید فیلد 'زمان مطالعه' را وارد کنید");
            }
            if (!dto.CategoryId.HasValue)
            {
                modelValid.Valid = false;
                modelValid.Errors.Add("در صورت انتشار مقاله , باید فیلد 'دسته بندی' را وارد کنید");
            }
            if (String.IsNullOrWhiteSpace(dto.Content))
            {
                modelValid.Valid = false;
                modelValid.Errors.Add("در صورت انتشار مقاله , باید فیلد 'محتوا' را وارد کنید");
            }
            if (String.IsNullOrWhiteSpace(dto.Description))
            {
                modelValid.Valid = false;
                modelValid.Errors.Add("در صورت انتشار مقاله , باید فیلد 'توضیح کوتاه' را وارد کنید");
            }
            return modelValid;
        }
    }
}
