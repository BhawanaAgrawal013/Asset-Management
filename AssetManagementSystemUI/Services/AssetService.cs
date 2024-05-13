namespace AssetManagementSystemUI
{
    public partial class AssetService
    {
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

        private bool UserExists(string email)
        {
            if(GetUser().Result.Where(x => x.Email == email).Any())
            {
                return true;
            }
            return false;
        }

        public string Assign(AssetViewModel assetViewModel)
        {
            if (UserExists(assetViewModel.UserEmail))
            {
                AssignedAsset asset = new AssignedAsset();
                asset.UserId = GetUser().Result.Where(x => x.Email.ToString() == assetViewModel.UserEmail.ToString()).Select(x => x.Id).First();
                asset.AssetId = GetDetails().Result.Where(x => x.Id.ToString() == assetViewModel.AssetName).Select(x => x.Id).First();
                asset.Status = Status.Assigned.ToString();
                asset.DateOfReturn = assetViewModel.DateOfReturn;

                using (HttpClient client = _api.Initial())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer", parameter: _token);

                    var postTask = client.PostAsJsonAsync<AssignedAsset>("api/Asset/Assign", asset);
                    postTask.Wait();

                    var result = postTask.Result;

                    if (result.IsSuccessStatusCode)
                    {
                        return "Asset Assigned Successfully";
                    }

                    else
                    {
                        return "connection problem";
                    }
                }
            }
            return "User does not exist";
        }

        public string Unassign(int id)
        {
            AssetViewModel assetViewModel = GetAssetViews().Result.Where(i => i.Id == id).FirstOrDefault();

            AssignedAsset asset = new AssignedAsset();
            asset.id = assetViewModel.Id;
            asset.UserId = assetViewModel.UserId;
            asset.AssetId = assetViewModel.AssetId;
            asset.Status = Status.Unassigned.ToString();
            asset.DateOfReturn = assetViewModel.DateOfReturn;
            asset.Reason = assetViewModel.Reason;

            using (HttpClient client = _api.Initial())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer", parameter: _token);

                var postTask = client.PutAsJsonAsync<AssignedAsset>($"api/Asset/Unassign/{asset.id}", asset);
                postTask.Wait();

                var result = postTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    return "Asset Unassigned Successfully";
                }

                else
                {
                    return "connection problem";
                }
            }
        }

        public string AcceptRequest(int id)
        {
            AssetViewModel assetViewModel = GetAssetViews().Result.Where(i => i.Id == id).FirstOrDefault();

            AssignedAsset asset = new AssignedAsset();
            asset.id = assetViewModel.Id;
            asset.UserId = assetViewModel.UserId;
            asset.AssetId = assetViewModel.AssetId;
            asset.Status = Status.Assigned.ToString();
            asset.DateOfReturn = assetViewModel.DateOfReturn;
            asset.Reason = assetViewModel.Reason;

            using (HttpClient client = _api.Initial())
            {
                var postTask = client.PutAsJsonAsync<AssignedAsset>($"api/Asset/Unassign/{asset.id}", asset);
                postTask.Wait();

                var result = postTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    return "Asset Assogned Successfully";
                }

                else
                {
                    return "connection problem";
                }
            }
        }

        public string DeclineRequest(int id, string reason)
        {
            AssetViewModel assetViewModel = GetAssetViews().Result.Where(i => i.Id == id).FirstOrDefault();

            AssignedAsset asset = new AssignedAsset();
            asset.id = assetViewModel.Id;
            asset.UserId = assetViewModel.UserId;
            asset.AssetId = assetViewModel.AssetId;
            asset.Status = Status.Decline.ToString();
            asset.DateOfReturn = assetViewModel.DateOfReturn;
            asset.Reason = reason;

            using (HttpClient client = _api.Initial())
            {
                var postTask = client.PutAsJsonAsync<AssignedAsset>($"api/Asset/Unassign/{asset.id}", asset);
                postTask.Wait();

                var result = postTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    return "Asset Request Declined Successfully";
                }

                else
                {
                    return "connection problem";
                }
            }
        }
    }
}
