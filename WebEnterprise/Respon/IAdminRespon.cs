using WebEnterprise.Models.DTO;

namespace WebEnterprise.Respon
{
    public interface IAdminRespon
    {
        public List<CustomUserDTO> GetAllAdmin();
        public List<CustomUserDTO> GetAllStaff();
  
        public CustomUserDTO GetEditStaff(string id);
        public Task<CustomUserDTO> PostEditStaff(CustomUserDTO staff , List<IFormFile> postedFile );
        public Task<CustomUserDTO> PostCreateStaff(CustomUserDTO staff, IFormFile postedFile);
        public Task<CustomUserDTO> PostCreateManager(CustomUserDTO manager, List<IFormFile> postedFile);
        public Task<CustomUserDTO> PostCreateCoor(CustomUserDTO coor, List<IFormFile> postedFile);
        public List<CustomUserDTO> GetAllCoor();
        public bool DeleteCoor(string id);
        public bool DeleteStaff(string id);
        public CustomUserDTO GetEditCoor(string id);
        public  Task<CustomUserDTO> PostEditCoor(CustomUserDTO coor, List<IFormFile> postedFile);
        public Task<CustomUserDTO> PostEditManager(CustomUserDTO manager, List<IFormFile> postedFile);
        public List<CustomUserDTO> GetAllManager();
        public bool DeleteManager(string id);
        public CustomUserDTO GetEditManager(string id);
    

    }
}
