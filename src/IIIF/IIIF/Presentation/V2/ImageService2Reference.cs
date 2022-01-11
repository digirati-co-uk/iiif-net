using IIIF.ImageApi.Service;
using System;

namespace IIIF.Presentation.V2
{
    public class ImageService2Reference : ResourceBase, IService
    {
        public override string Type 
        { 
            get => nameof(ImageService2);
            set => throw new NotImplementedException(); 
        }
    }
}
