using Microsoft.AspNetCore.Mvc;
using pressF.API.Model;
using pressF.API.Repository.Interfaces;
using pressF.API.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace pressF.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _uow;

        public ProductController(IProductRepository productRepository, IUnitOfWork uow)
        {
            _productRepository = productRepository;
            _uow = uow;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> Get()
        {
            var products = await _productRepository.GetAll();

            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> Get(string id)
        {
            var product = await _productRepository.GetById(id);

            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> Post([FromBody] ProductViewModel value)
        {
            var product = new Product(value);
            _productRepository.Add(product);

            //// it will be null
            var testProduct = await _productRepository.GetById(product.Id);

            // If everything is ok then:
            await _uow.Commit();

            // The product will be added only after commit
            testProduct = await _productRepository.GetById(product.Id);

            return Ok(testProduct);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Product>> Put(string id, [FromBody] ProductViewModel value)
        {
            var product = new Product(value);
            product.Id = id;

            _productRepository.Update(product);

            await _uow.Commit();

            return Ok(await _productRepository.GetById(id));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            _productRepository.Remove(id);

            // it won't be null
            var testProduct = await _productRepository.GetById(id);

            // If everything is ok then:
            await _uow.Commit();

            // not it must by null
            testProduct = await _productRepository.GetById(id);

            return Ok();
        }
    }
}