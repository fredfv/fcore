using pressF.API.ViewModel;

namespace pressF.API.Model
{
    public class Product : BaseDocument
    {
        public string Description { get; private set; }

        public Product()
        {
        }

        public Product(ProductViewModel vm)
        {
            Description = vm.Description;
        }
    }
}