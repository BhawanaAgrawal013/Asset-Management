using Microsoft.AspNetCore.Authorization;

namespace AssetManagementWebApi
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AssetController : ControllerBase
    {
        private readonly IAssetRepository _assetRepository;
        private readonly ILog logger;
        public AssetController( IAssetRepository asset, ILog logger)
        {
            _assetRepository = asset;
            this.logger = logger;
        }


        [HttpGet]
        public ActionResult<Assets> Display()
        {
            try
            {
                logger.Information("Asset list fetched successfully");
                return Ok(_assetRepository.Display());
            }
            catch
            {
                logger.Error("Asset list could not be found");
                return NotFound();
            }
        }

        [HttpGet("{UserId}")]
        public ActionResult<Assets> Search(int UserId)
        {
            try
            {
                logger.Information($"Asset {UserId} details fetched sucessfully");
                return Ok(_assetRepository.Get(UserId));
            }
            catch
            {
                logger.Error($"Asset {UserId} details could not be found");
                return NotFound("Id not found");
            }
        }

        [HttpPost]
        public ActionResult<string> Assign( Assets asset)
        {
            try
            {
                logger.Information($"Asset assigned successfully");
                return Ok(_assetRepository.Assign(asset));
            }
            catch
            {
                logger.Error("Asset cannot be assigned");
                return NotFound();
            }
        }

        [HttpPut("{id}")]
        public ActionResult<Assets> UnAssign(int id, Assets asset)
        {
            try
            {
                logger.Information($"Asset {asset.AssetId} {asset.Status} to user {asset.UserId}");
                return Ok(_assetRepository.UnAssign(asset));
            }
            catch
            {
                logger.Error($"Asset {asset.AssetId} cannot be {asset.Status} to user {asset.UserId}");
                return NotFound("Id not found");
            }
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                logger.Information($"Asset removed successfully");
                return Ok(_assetRepository.Remove(id));
            }
            catch
            {
                logger.Error($"Asset cannot be removed successfully");
                return NotFound("Id not found");
            }
        }
    }
}
