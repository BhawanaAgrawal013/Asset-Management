using NuGet.ContentModel;
namespace AssetManagementSystemUI
{
    public class UserService
    {
        private readonly HelperAPI _api = new HelperAPI();
        private string _token { get; set; }
        private string _email { get; set; }

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AssetService _assetService;
        private readonly ISession _session;
        public UserService(IHttpContextAccessor httpContextAccessor, AssetService assetService)
        {
            _httpContextAccessor = httpContextAccessor;
            _session = _httpContextAccessor.HttpContext.Session;
            _assetService = assetService;
            _token = _session.GetString("_token");
            _email = _session.GetString("_userEmail");
        }

        public async Task<List<UserViewModel>> GetUser()
        {
            List<UserViewModel> users = new List<UserViewModel>();
            using (HttpClient client = _api.Initial())
            {
                HttpResponseMessage res = await client.GetAsync("api/User/Display");

                if (res.IsSuccessStatusCode)
                {
                    var result = res.Content.ReadAsStringAsync().Result;
                    users = JsonConvert.DeserializeObject<List<UserViewModel>>(result);
                }
                return users;
            }
        }

        public async Task<UserViewModel> EditProfile()
        {
            long id = GetUser().Result.Where(i => i.Email == _email).Select(x => x.Id).FirstOrDefault();
            UserViewModel user = new UserViewModel();
            using (HttpClient client = _api.Initial())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer", parameter: _token);

                HttpResponseMessage res = await client.GetAsync($"api/User/Search/{id}");

                if (res.IsSuccessStatusCode)
                {
                    var result = res.Content.ReadAsStringAsync().Result;
                    user = JsonConvert.DeserializeObject<UserViewModel>(result);

                }
                return (user);
            }
        }

        public string RequestAsset(AssetViewModel assetViewModel)
        {
            AssignedAsset asset = new AssignedAsset();
            asset.UserId = GetUser().Result.Where(x => x.Email == _email).Select(x => x.Id).FirstOrDefault();
            asset.AssetId = Convert.ToInt64(assetViewModel.AssetName);
            asset.Status = Status.Request.ToString();
            asset.DateOfReturn = assetViewModel.DateOfReturn;

            using (HttpClient client = _api.Initial())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer", parameter: _token);

                var postTask = client.PostAsJsonAsync<AssignedAsset>("api/Asset/Assign", asset);
                postTask.Wait();

                var result = postTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    return "Asset Requested Successfully";
                }

                else
                {
                    return "connection problem";
                }
            }
        }

        public async Task<List<AssetViewModel>> DisplayRequest()
        {
            long id = GetUser().Result.Where(x => x.Email == _email).Select(x => x.Id).FirstOrDefault();
            List<AssetViewModel> assets = new List<AssetViewModel>();
            using (HttpClient client = _api.Initial())
            {

                HttpResponseMessage res = await client.GetAsync($"api/Asset/Search/{id}");

                if (res.IsSuccessStatusCode)
                {
                    var result = res.Content.ReadAsStringAsync().Result;
                    assets = JsonConvert.DeserializeObject<List<AssetViewModel>>(result);

                }
                foreach (var item in assets)
                {
                    item.AssignedAsset = _assetService.GetDetails().Result.Where(i => i.Id == item.AssetId).Select(i => i.Name).FirstOrDefault();
                    var categoryid = _assetService.GetDetails().Result.Where(i => i.Id == item.AssetId).Select(i => i.CategoryId).FirstOrDefault();
                    item.AssetName = _assetService.GetCategories().Result.Where(i => i.Id == categoryid).Select(i => i.Name).FirstOrDefault();
                }
                return assets;
            }
        }

        public string ReturnAsset(int id)
        {
            AssetViewModel assetViewModel = _assetService.GetAssetViews().Result.Where(i => i.Id == id).FirstOrDefault();

            AssignedAsset asset = new AssignedAsset();
            asset.id = assetViewModel.Id;
            asset.UserId = assetViewModel.UserId;
            asset.AssetId = assetViewModel.AssetId;
            asset.Status = Status.Return.ToString();
            asset.DateOfReturn = assetViewModel.DateOfReturn;
            asset.Reason = assetViewModel.Reason;

            using (HttpClient client = _api.Initial())
            {
                var postTask = client.PutAsJsonAsync<AssignedAsset>($"api/Asset/Unassign/{asset.id}", asset);
                postTask.Wait();

                var result = postTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    return "Asset Returned Successfully";
                }

                else
                {
                    return "connection problem";
                }
            }
        }

        public List<DataPoint> PieData()
        {
			List<DataPoint> dataPoints = new List<DataPoint>();

            var assets = _assetService.GetDetails().Result.GroupBy(i => i.CategoryId);

            foreach(var group in assets)
            {
                var name = _assetService.GetCategories().Result.Where(i => i.Id == group.Key).Select(i => i.Name).FirstOrDefault();
                dataPoints.Add(new DataPoint(name, group.Count()));
            }

            return dataPoints;
		}
    }
}
