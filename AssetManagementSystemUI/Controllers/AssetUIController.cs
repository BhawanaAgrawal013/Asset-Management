namespace AssetManagementSystemUI
{
    [Route("Asset/[action]")]
    public class AssetUIController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly AssetService assetService;

        public AssetUIController(AssetService _assetService, IHttpContextAccessor httpContextAccessor)
        {
            this.assetService = _assetService;
            this._httpContextAccessor = httpContextAccessor;
        }

        public ActionResult<AssetViewModel> Display()
        {
            try
            {
                var asset = assetService.GetAssetViews().Result.Where(i => i.Status == Status.Assigned.ToString() || i.Status == Status.Unassigned.ToString()).ToList();
                if(asset == null)
                {
                    return View("UnAuthorized");
                }
                return View(asset);
            }
            catch
            {
                return RedirectToAction("Services", "Admin");
            }
        }

        [HttpPost]
        public ActionResult<AssetViewModel> Display(string all)
        {
            try
            {
                return View(assetService.GetFilteredAsset(all).Where(i => i.Status == Status.Assigned.ToString() || i.Status == Status.Unassigned.ToString()).ToList());
            }
            catch
            {
                return RedirectToAction("Services", "Admin");
            }
        }

        [HttpPost]
        public ActionResult GetAssets(string filter, string genre)
        {
            return Json(assetService.GetAssetDetails(filter, genre));
        }

        [HttpPost]
        public ActionResult GetAssetType(string filter) 
        {
            return Json(assetService.GetAssetType(filter));
        }

        public ActionResult<AssetViewModel> DisplayRequest()
        {
            try
            {
                return View(assetService.GetAssetViews().Result.Where(i => i.Status == Status.Request.ToString()));
            }
            catch
            {
                return View(null);
            }
        }

        [HttpPost]
        public ActionResult<AssetViewModel> DisplayRequest(string all)
        {
            try
            {
                return View(assetService.GetFilteredAsset(all).Where(i => i.Status == Status.Request.ToString() || i.Status == Status.Decline.ToString()));

            }
            catch
            {
                return RedirectToAction("Services", "Admin");
            }
        }

        public ActionResult<AssetViewModel> History()
        {
            try
            {
                var asset = assetService.GetAssetViews().Result;
                if(asset == null)
                {
                    return View("UnAuthorized");
                }
                return View(asset);
            }
            catch 
            { 
                return View(null); 
            }
        }

        [HttpPost]
        public ActionResult<AssetViewModel> History(string all)
        {
            return View(assetService.GetFilteredAsset(all));
        }
    }
}
