using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EStore.Domain.EntityDtos
{
    public class CreateProductDto
    {
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string LongDesrciption { get; set; }
        public string Brand { get; set; }
        public IFormFile ImageFile { get; set; }  // Change this line
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
       
    }
}
