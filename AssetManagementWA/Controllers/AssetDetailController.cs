namespace AssetManagementWebApi
{ 
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AssetDetailController : ControllerBase
    {
        private readonly IAssetDetail _assetDetailRepo;
        private readonly ILog logger;
        public AssetDetailController(IAssetDetail assetDetailRepo, ILog logger)
        {
            this._assetDetailRepo = assetDetailRepo;
            this.logger = logger;
        }


        [HttpGet]
        public ActionResult<AssetDetail> Display()
        {
            try
            {
                logger.Information("Asset detail fetched successfully");
                return Ok(_assetDetailRepo.Display());
            }
            catch
            {
                logger.Error("Asset detail could not be fetched");
                return BadRequest("Asset could not be found");
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult<AssetDetail> Search(int id)
        {
            try
            {
                logger.Information($"Search {id} fetched succesfully");
                return (_assetDetailRepo.Get(id));
            }
            catch
            {
                logger.Error($"Search {id} could not be fetched");
                return NotFound("Id not found");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult<string> Add(AssetDetail asset)
        {
            try
            {
                logger.Information($"{asset.CategoryId} added successfully");
                return _assetDetailRepo.Add(asset);
            }
            catch
            {
                logger.Error($"asset could not be added");
                return "Asset could not be Added";
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult<AssetDetail> Update(int id, AssetDetail asset)
        {
            try
            {
                logger.Information($"{asset.Id} updated sucessfully");
                return Ok(_assetDetailRepo.Update(id, asset));
            }
            catch
            {
                logger.Error("information could not be updated");
                return NotFound("Id not found");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            try
            {
                logger.Information($"{id} deleted successfully");
                return Ok(_assetDetailRepo.Remove(id));
            }
            catch
            {
                logger.Error("asset could not be deleted");
                return NotFound("Id not found");
            }
        }

        [HttpGet]
        public ActionResult<AssetCategory> DisplayCategory()
        {
            try
            {
                logger.Information("categories fetched successfully");
                return Ok(_assetDetailRepo.DisplayCategory());
            }
            catch
            {
                logger.Error("categories could not be fetched");
                return BadRequest("Asset could not be found");
            }
        }
    }
}
