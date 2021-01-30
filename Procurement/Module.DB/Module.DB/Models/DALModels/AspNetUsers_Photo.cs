using System.ComponentModel.DataAnnotations;
using System;

namespace Module.DB
{

    public partial class AspNetUsers_PhotosMetaData
    { }
    [MetadataType(typeof(AspNetUsers_PhotosMetaData))]
    public partial class AspNetUsers_Photo
    {
        public string GetPhotoString()
        {
            byte[] arr = Photo.ToArray();
            return Convert.ToBase64String(arr);
        }
    }
}