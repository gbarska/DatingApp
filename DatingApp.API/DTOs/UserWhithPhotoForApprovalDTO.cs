using System.Collections.Generic;

namespace DatingApp.API.DTOs
{
    public class UserWhithPhotoForApprovalDTO
    {
          public int Id { get; set; }
          public ICollection<PhotosForDetailedDTO> Photos { get; set; }
           public string KnownAs { get; set; }

    }   
}