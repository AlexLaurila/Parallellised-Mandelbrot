using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amplifier.OpenCL;

namespace MandelWindow
{
    class Kernels : OpenCLFunctions
    {
        [OpenCLKernel]
        void IterCount([Global] double[] cx, [Global] double[] cy, [Global] int[] result, int mandelDepth)
        {
            int i = get_global_id(0);
            double x = 0.0f;
            double y = 0.0f;
            double xx = 0.0f, yy = 0.0;
            while (xx + yy <= 4.0 && result[i] < mandelDepth) // are we out of control disk?
            {
                xx = x * x;
                yy = y * y;
                double xtmp = xx - yy + cx[i];
                y = 2.0f * x * y + cy[i]; // computes z^2 + c
                x = xtmp;
                result[i] = result[i] + 1;
            }
        }
    }
}
