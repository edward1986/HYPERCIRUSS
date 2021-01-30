using DPUruNet;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UareU.Common
{
    public interface ParentForm
    {
        Reader CurrentReader { get; set; }
        Bitmap CreateBitmap(byte[] bytes, int width, int height);
    }

    public interface IForm
    {
        bool IsOnline { get; set; }
    }
}
