using Grpc.Core;

namespace WereHouse.Server.Services
{
    public class WerehouseService: WereHouse.WereHouseBase
    {
        private static Dictionary<string, Product> _products = new Dictionary<string, Product>();
        public override Task<ProductID> AddProducttById(Product request, ServerCallContext context)
        {
            _products[request.Id] = request;
            return Task.FromResult(new ProductID { Id = request.Id});
        }

        public override Task<Product> UpdateProducttById(Product request, ServerCallContext context)
        {
            if (_products.ContainsKey(request.Id))
            {
                _products[request.Id] = request;
                return Task.FromResult(request);
            }
            else 
            {
                var errorResponse = new ErrorResponse
                {
                    Razon = "Productos no encontrado.",
                    Detalle = { $"Producto con el ID: {request.Id} no existe." }
                };
                context.Status = new Status(StatusCode.NotFound, $"{errorResponse.Razon}. Detalle: {errorResponse.Detalle}");
                return Task.FromResult(new Product());
            }
        }

        public override Task<Product> GetProducttById(ProductID request, ServerCallContext context)
        {
            if (_products.TryGetValue(request.Id, out Product product) && product != null)
            {
                return Task.FromResult(product);
            }
            else
            {
                var errorResponse = new ErrorResponse
                {
                    Razon = "Productos no encontrado.",
                    Detalle = { $"Producto con el ID: {request.Id} no existe." }
                };
                context.Status = new Status(StatusCode.NotFound, $"{errorResponse.Razon}. Detalle: {errorResponse.Detalle}");
                return Task.FromResult(new Product());
            }
        }

        public override Task<Product> GetProducttByName(ProductName request, ServerCallContext context)
        {
            Product matchingProduct= null;
            foreach (var product in _products.Values) {
                if (product.Name == request.Name)
                {
                    matchingProduct = product;
                    break;
                }
            }
            if (matchingProduct != null)
            { 
                return Task.FromResult(matchingProduct);
            }
            else
            {
                var errorResponse = new ErrorResponse
                {
                    Razon = "Productos no encontrado.",
                    Detalle = { $"Producto con el ID: {request.Name} no existe." }
                };
                context.Status = new Status(StatusCode.NotFound, $"{errorResponse.Razon}. Detalle: {errorResponse.Detalle}");
                return Task.FromResult(new Product());
            }
        }

    }
}
