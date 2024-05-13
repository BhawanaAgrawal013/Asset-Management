namespace AssetManagementSystemUI
{
    public partial class AssetService
    {
        private readonly HelperAPI _api = new HelperAPI();
        private string _token { get; set; }

        private readonly IHttpContextAccessor _httpContextAccessor; 
        private readonly ISession _session; 
        public AssetService(IHttpContextAccessor httpContextAccessor) 
        { 
            _httpContextAccessor = httpContextAccessor; 
            _session = _httpContextAccessor.HttpContext.Session;
            _token = _session.GetString("_admintoken");
        }

        public async Task<List<AssetViewModel>> GetAssetViews()
        {
            List<AssetViewModel> asset = new List<AssetViewModel>();

            using (HttpClient client = _api.Initial())
            {

                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer", parameter: _token);

                HttpResponseMessage res = await client.GetAsync("api/Asset/Display");

                if (res.IsSuccessStatusCode)
                {
                    var result = res.Content.ReadAsStringAsync().Result;
                    asset = JsonConvert.DeserializeObject<List<AssetViewModel>>(result);
                }
                else if (res.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return null;
                }
                foreach (var item in asset)
                {
                    item.AssignedAsset = GetDetails().Result.Where(i => i.Id == item.AssetId).Select(i => i.Name).FirstOrDefault();
                    var categoryid = GetDetails().Result.Where(i => i.Id == item.AssetId).Select(i => i.CategoryId).FirstOrDefault();
                    item.AssetName = GetCategories().Result.Where(i => i.Id == categoryid).Select(i => i.Name).FirstOrDefault();
                    item.UserEmail = GetUser().Result.Where(i => i.Id == item.UserId).Select(i => i.Email).FirstOrDefault();
                }

                return asset;
            }
        }

        public async Task<List<AssetCategory>> GetCategories()
        {
            List<AssetCategory> assets = new List<AssetCategory>();

            using (HttpClient client = _api.Initial())
            {
                HttpResponseMessage res = await client.GetAsync("api/AssetDetail/DisplayCategory");

                if (res.IsSuccessStatusCode)
                {
                    var result = res.Content.ReadAsStringAsync().Result;
                    assets = JsonConvert.DeserializeObject<List<AssetCategory>>(result);
                }
            }
            return assets;
        }

        public List<AssetViewModel> GetFilteredAsset(string assetName)
        {
            List<AssetViewModel> assets = GetAssetViews().Result;

            if (Convert.ToInt32(assetName) == (int)Assets.Book)
            {
                return assets.Where(i => i.AssetName == Assets.Book.ToString()).ToList();
            }
            else if (Convert.ToInt32(assetName) == (int)Assets.Software)
            {
                return assets.Where(i => i.AssetName == Assets.Software.ToString()).ToList();
            }
            else if (Convert.ToInt32(assetName) == (int)Assets.Hardware)
            {
                return assets.Where(i => i.AssetName == Assets.Hardware.ToString()).ToList();
            }
            else
            {
                return assets.ToList();
            }
        }

        public async Task<List<AssetDetailViewModel>> GetDetails()
        {
            List<AssetDetailViewModel> assets = new List<AssetDetailViewModel>();

            using (HttpClient client = _api.Initial())
            {

                HttpResponseMessage res = await client.GetAsync("api/AssetDetail/Display");

                if (res.IsSuccessStatusCode)
                {
                    var result = res.Content.ReadAsStringAsync().Result;
                    assets = JsonConvert.DeserializeObject<List<AssetDetailViewModel>>(result);
                }
            }
            return assets;
        }

        public List<string> GetAssetType(string filter)
        {
            if (filter != null)
            {
                return GetDetails().Result.Where(i => i.CategoryId == Convert.ToInt32(filter)).Select(i => i.Type).Distinct().ToList();
            }
            else
            {
                return (null);
            }
        }

        public List<AssetDetailViewModel> GetAssetDetails(string filter, string genre)
        {
            if(filter != null)
            {
                return GetDetails().Result.Where(i => i.Type == genre && i.CategoryId == Convert.ToInt32(filter) && i.Quantity > 0).ToList();
            }
            else
            {
                return (null);
            }
        }

		/*private List<AssetViewModel> PieData()
		{


			var assets = GetAssetViews().Result.GroupBy(i => i.AssetName).ToList();

			*//*foreach (var group in assets)
			{
				dataPoints.Add(new DataPoint("Assigned " + group.Key, group.Count()));
                dataPoints.Add(new DataPoint("Unassigned " + group.Key, 100-group.Count()));
			}*//*

			return assets;
		}*/

        public List<DataPoint> BookPieData()
        {
			List<DataPoint> dataPoints = new List<DataPoint>();

            //int total = GetDetails().Result.Where(i => i.CategoryId == ((int)Assets.Book)).Count();

			var count = GetAssetViews().Result.Where(i => i.AssetName == Assets.Book.ToString()).Count();

			dataPoints.Add(new DataPoint("Assigned " + Assets.Book.ToString(), count));
			dataPoints.Add(new DataPoint("Unassigned " + Assets.Book.ToString(), 100 - count));

            return dataPoints;
		}

		public List<DataPoint> SoftwarePieData()
		{
			List<DataPoint> dataPoints = new List<DataPoint>();

			//int total = GetDetails().Result.Where(i => i.CategoryId == ((int)Assets.Book)).Count();

			var count = GetAssetViews().Result.Where(i => i.AssetName == Assets.Software.ToString()).Count();

			dataPoints.Add(new DataPoint("Assigned " + Assets.Software.ToString(), count));
			dataPoints.Add(new DataPoint("Unassigned " + Assets.Software.ToString(), 100 - count));

			return dataPoints;
		}

		public List<DataPoint> HardwarePieData()
		{
			List<DataPoint> dataPoints = new List<DataPoint>();

			//int total = GetDetails().Result.Where(i => i.CategoryId == ((int)Assets.Book)).Count();

			var count = GetAssetViews().Result.Where(i => i.AssetName == Assets.Hardware.ToString()).Count();

			dataPoints.Add(new DataPoint("Assigned " + Assets.Hardware.ToString(), count));
			dataPoints.Add(new DataPoint("Unassigned " + Assets.Hardware.ToString(), 100 - count));

			return dataPoints;
		}
	}
}
