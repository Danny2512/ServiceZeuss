using Newtonsoft.Json;

namespace ServiceZeuss.Models
{ 
    public class CategoryApiResponseModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public List<ProductApiResponseModel> Products { get; set; }
    }
}
