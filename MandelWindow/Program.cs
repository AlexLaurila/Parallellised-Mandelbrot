using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using Amplifier;
using Amplifier.OpenCL;

namespace MandelWindow
{
    class Program
    {
        static WriteableBitmap bitmap;
        static Window windows;
        static Image image;

        // Experiment Handlers
        static DataHandler DataHandler { get; set; }
        static FileHandler FileHandler { get; set; }
        static ExperimentHandler ExperimentHandler { get; set; }
        static OpenCLCompiler compiler = new OpenCLCompiler();
        static int totalPixels;
        static double[] cx;
        static double[] cy;
        static int[] result;


        [STAThread]
        static void Main(string[] args)
        {
            image = new Image();
            RenderOptions.SetBitmapScalingMode(image, BitmapScalingMode.NearestNeighbor);
            RenderOptions.SetEdgeMode(image, EdgeMode.Aliased);

            windows = new Window();
            windows.Content = image;
            windows.Show();

            bitmap = new WriteableBitmap(
                (int)windows.ActualWidth,
                (int)windows.ActualHeight,
                96,
                96,
                PixelFormats.Bgr32,
                null);

            image.Source = bitmap;

            image.Stretch = Stretch.None;
            image.HorizontalAlignment = HorizontalAlignment.Left;
            image.VerticalAlignment = VerticalAlignment.Top;

            image.MouseLeftButtonDown +=
                new MouseButtonEventHandler(image_MouseLeftButtonDown);
            image.MouseRightButtonDown +=
                new MouseButtonEventHandler(image_MouseRightButtonDown);
            image.MouseMove +=
                new MouseEventHandler(image_MouseMove);

            windows.MouseWheel += new MouseWheelEventHandler(window_MouseWheel);

            compiler.UseDevice(0);
            compiler.CompileKernel(typeof(Kernels));
            totalPixels = bitmap.PixelHeight * bitmap.PixelWidth;
            cx = new double[totalPixels];
            cy = new double[totalPixels];
            result = new int[totalPixels];


            // Initialize Experiment
            DataHandler = new DataHandler();
            FileHandler = new FileHandler();
            ExperimentHandler = new ExperimentHandler();


            #region Experiment 1

            for (int i = 1; i < 2; i++) // TODO: Change loop to fit and work with all 3 experiments.
            {
	            #region Sequential

	            Console.WriteLine($"Running Experiment {i} sequentially");
	            foreach (var parameter in ExperimentHandler.Experiment1Parameters)
	            {
		            mandelDepth = parameter;
		            var stopwatch = Stopwatch.StartNew();
		            ExperimentHandler.RunExperiment(UpdateMandel);
		            stopwatch.Stop();
		            DataHandler.SaveData(parameter, stopwatch.Elapsed.TotalSeconds);
		            Console.Write(".");
	            }

	            Console.WriteLine($" Experiment {i} Complete");

	            // Save results to file
	            var header = $"Experiment {i} Sequential\t";
	            FileHandler.SaveToFile(DataHandler.ResultData, header);

	            // Reset ResultData before next
	            DataHandler.ResultData.Clear();

	            #endregion

	            // Run UpdateMandel with GPU first time outside the test.
	            // ------------------------------------------------------
	            mandelDepth = 100;
	            ExperimentHandler.RunExperimentParallel(UpdateMandel);
	            // ------------------------------------------------------

	            #region Parallel

	            Console.WriteLine($"Running Experiment {i} Parallel");
	            foreach (var parameter in ExperimentHandler.Experiment1Parameters)
	            {
		            mandelDepth = parameter;
		            var stopwatch = Stopwatch.StartNew();
		            ExperimentHandler.RunExperimentParallel(UpdateMandel);
		            stopwatch.Stop();
		            DataHandler.SaveData(parameter, stopwatch.Elapsed.TotalSeconds);
		            Console.Write(".");
	            }

	            Console.WriteLine($" Experiment {i} Complete");

	            // Save results to file
	            header = $"Experiment {i} Parallel\t";
	            FileHandler.SaveToFile(DataHandler.ResultData, header);

	            // Reset ResultData before next
	            DataHandler.ResultData.Clear();

	            #endregion
            }

            #endregion


            #region Experiment 2

            

            #endregion


            #region Experiment 3

            for (int i = 3; i < 4; i++) // TODO: Change loop to fit and work with all 3 experiments.
            {
	            #region Sequential

	            Console.WriteLine($"Running Experiment {i} sequentially");
	            foreach (var parameter in ExperimentHandler.Experiment3Parameters)
	            {
		            bitmap = new WriteableBitmap(
			            (int)parameter.Item1,
			            (int)parameter.Item2,
			            96,
			            96,
			            PixelFormats.Bgr32,
			            null);

		            image.Source = bitmap;

		            var stopwatch = Stopwatch.StartNew();
		            ExperimentHandler.RunExperiment(UpdateMandel);
		            stopwatch.Stop();
		            DataHandler.SaveData(parameter, stopwatch.Elapsed.TotalSeconds);
		            Console.Write(".");
	            }

	            Console.WriteLine($" Experiment {i} Complete");

	            // Save results to file
	            var header = $"Experiment {i} Sequential\t";
	            FileHandler.SaveToFile(DataHandler.ResultData, header);

	            // Reset ResultData before next
	            DataHandler.ResultData.Clear();

	            #endregion

	            // Run UpdateMandel with GPU first time outside the test.
	            // ------------------------------------------------------
	            mandelDepth = 100;
	            ExperimentHandler.RunExperimentParallel(UpdateMandel);
	            // ------------------------------------------------------

	            #region Parallel

	            Console.WriteLine($"Running Experiment {i} Parallel");
	            foreach (var parameter in ExperimentHandler.Experiment3Parameters)
	            {
		            bitmap = new WriteableBitmap(
			            (int)parameter.Item1,
			            (int)parameter.Item2,
			            96,
			            96,
			            PixelFormats.Bgr32,
			            null);

		            image.Source = bitmap;

		            var stopwatch = Stopwatch.StartNew();
		            ExperimentHandler.RunExperimentParallel(UpdateMandel);
		            stopwatch.Stop();
		            DataHandler.SaveData(parameter, stopwatch.Elapsed.TotalSeconds);
		            Console.Write(".");
	            }

	            Console.WriteLine($" Experiment {i} Complete");

	            // Save results to file
	            header = $"Experiment {i} Parallel\t";
	            FileHandler.SaveToFile(DataHandler.ResultData, header);

	            // Reset ResultData before next
	            DataHandler.ResultData.Clear();

	            #endregion
            }

            #endregion


            Application app = new Application();
            app.Run();
        }

        //Zoom Out
        static void image_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            int column = (int)e.GetPosition(image).X;
            int row = (int)e.GetPosition(image).Y;

            mandelCenterX = mandelCenterX - mandelWidth + column * ((mandelWidth * 2.0) / bitmap.PixelWidth);
            mandelCenterY = mandelCenterY - mandelHeight + row * ((mandelHeight * 2.0) / bitmap.PixelHeight);
            mandelWidth *= 2.0;
            mandelHeight *= 2.0;

            UpdateMandel();
        }

        //Zoom In
        static void image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            int column = (int)e.GetPosition(image).X;
            int row = (int)e.GetPosition(image).Y;

            mandelCenterX = mandelCenterX - mandelWidth + column * ((mandelWidth * 2.0) / bitmap.PixelWidth);
            mandelCenterY = mandelCenterY - mandelHeight + row * ((mandelHeight * 2.0) / bitmap.PixelHeight);
            mandelWidth /= 2.0;
            mandelHeight /= 2.0;

            UpdateMandel();
        }

        //Zoom In/Out
        static void window_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            int column = (int)e.GetPosition(image).X;
            int row = (int)e.GetPosition(image).Y;

            if (e.Delta > 0)
            {
                mandelCenterX = mandelCenterX - mandelWidth + column * ((mandelWidth * 2.0) / bitmap.PixelWidth);
                mandelCenterY = mandelCenterY - mandelHeight + row * ((mandelHeight * 2.0) / bitmap.PixelHeight);
                mandelWidth /= 2.0;
                mandelHeight /= 2.0;
            }
            else
            {
                mandelCenterX = mandelCenterX - mandelWidth + column * ((mandelWidth * 2.0) / bitmap.PixelWidth);
                mandelCenterY = mandelCenterY - mandelHeight + row * ((mandelHeight * 2.0) / bitmap.PixelHeight);
                mandelWidth *= 2.0;
                mandelHeight *= 2.0;
            }

            UpdateMandel();
        }

        //Track mousePosition
        static void image_MouseMove(object sender, MouseEventArgs e)
        {
            int column = (int)e.GetPosition(image).X;
            int row = (int)e.GetPosition(image).Y;

            double mouseCenterX = mandelCenterX - mandelWidth + column * ((mandelWidth * 2.0) / bitmap.PixelWidth);
            double mouseCenterY = mandelCenterY - mandelHeight + row * ((mandelHeight * 2.0) / bitmap.PixelHeight);

            windows.Title = $"Mandelbrot center X:{mouseCenterX} Y:{mouseCenterY}";
        }

        //Mandel dimensions
        static double mandelCenterX = 0.0;
        static double mandelCenterY = 0.0;

        // Parameter for Experiment 2
        static double mandelWidth = 2.0;
        static double mandelHeight = 2.0;

        // Parameter for Experiment 1
        public static int mandelDepth = 360;

        public static void UpdateMandel(bool parallel = true)
        {
            try
            {
                // Reserve the back buffer for updates.
                bitmap.Lock();

                unsafe
                {
                    if (parallel)
                        IterCountParallel();

                    int i = 0;
                    for (int row = 0; row < bitmap.PixelHeight; row++)
                    {
                        for (int column = 0; column < bitmap.PixelWidth; column++)
                        {
                            // Get a pointer to the back buffer.
                            IntPtr pBackBuffer = bitmap.BackBuffer;

                            // Find the address of the pixel to draw.
                            pBackBuffer += row * bitmap.BackBufferStride;
                            pBackBuffer += column * 4;

                            int R, G, B;
                            if (parallel)
                            {
                                HsvToRgb(result[i], 1.0, result[i] < mandelDepth ? 1.0 : 0.0, out R, out G, out B);
                                i++;
                            }
                            else
                            {
                                int light = IterCount(mandelCenterX - mandelWidth + column * ((mandelWidth * 2.0) / bitmap.PixelWidth), mandelCenterY - mandelHeight + row * ((mandelHeight * 2.0) / bitmap.PixelHeight));
                                HsvToRgb(light, 1.0, light < mandelDepth ? 1.0 : 0.0, out R, out G, out B);
                            }

                            // Compute the pixel's color.
                            int color_data = R << 16; // R
                            color_data |= G << 8;   // G
                            color_data |= B << 0;   // B

                            // Assign the color data to the pixel.
                            *((int*)pBackBuffer) = color_data;
                        }
                    }
                }

                // Specify the area of the bitmap that changed.
                bitmap.AddDirtyRect(new Int32Rect(0, 0, bitmap.PixelWidth, bitmap.PixelHeight));
            }
            finally
            {
                // Release the back buffer and make it available for display.
                bitmap.Unlock();
            }
        }

        private static void IterCountParallel()
        {
            int index = 0;
            for (int row = 0; row < bitmap.PixelHeight; row++)
            {
                for (int column = 0; column < bitmap.PixelWidth; column++)
                {
                    cy[index] = mandelCenterY - mandelHeight + row * ((mandelHeight * 2.0) / bitmap.PixelHeight);
                    cx[index] = mandelCenterX - mandelWidth + column * ((mandelWidth * 2.0) / bitmap.PixelWidth);
                    result[index] = 0;
                    index++;
                }
            }

            var exec = compiler.GetExec();
            exec.IterCount(cx, cy, result, mandelDepth);
        }

        public static int IterCount(double cx, double cy)
        {
            int result = 0;
            double x = 0.0f;
            double y = 0.0f;
            double xx = 0.0f, yy = 0.0;
            while (xx + yy <= 4.0 && result < mandelDepth) // are we out of control disk?
            {
                xx = x * x;
                yy = y * y;
                double xtmp = xx - yy + cx;
                y = 2.0f * x * y + cy; // computes z^2 + c
                x = xtmp;
                result++;
            }
            return result;
        }

        static void HsvToRgb(double h, double S, double V, out int r, out int g, out int b)
        {
            double H = h;
            while (H < 0) { H += 360; };
            while (H >= 360) { H -= 360; };
            double R, G, B;
            if (V <= 0)
            { R = G = B = 0; }
            else if (S <= 0)
            {
                R = G = B = V;
            }
            else
            {
                double hf = H / 60.0;
                int i = (int)Math.Floor(hf);
                double f = hf - i;
                double pv = V * (1 - S);
                double qv = V * (1 - S * f);
                double tv = V * (1 - S * (1 - f));
                switch (i)
                {

                    // Red is the dominant color

                    case 0:
                        R = V;
                        G = tv;
                        B = pv;
                        break;

                    // Green is the dominant color

                    case 1:
                        R = qv;
                        G = V;
                        B = pv;
                        break;
                    case 2:
                        R = pv;
                        G = V;
                        B = tv;
                        break;

                    // Blue is the dominant color

                    case 3:
                        R = pv;
                        G = qv;
                        B = V;
                        break;
                    case 4:
                        R = tv;
                        G = pv;
                        B = V;
                        break;

                    // Red is the dominant color

                    case 5:
                        R = V;
                        G = pv;
                        B = qv;
                        break;

                    // Just in case we overshoot on our math by a little, we put these here. Since its a switch it won't slow us down at all to put these here.

                    case 6:
                        R = V;
                        G = tv;
                        B = pv;
                        break;
                    case -1:
                        R = V;
                        G = pv;
                        B = qv;
                        break;

                    // The color is not defined, we should throw an error.

                    default:
                        //LFATAL("i Value error in Pixel conversion, Value is %d", i);
                        R = G = B = V; // Just pretend its black/white
                        break;
                }
            }
            r = Clamp((int)(R * 255.0));
            g = Clamp((int)(G * 255.0));
            b = Clamp((int)(B * 255.0));
        }

        /// <summary>
        /// Clamp a value to 0-255
        /// </summary>
        static int Clamp(int i)
        {
            if (i < 0) return 0;
            if (i > 255) return 255;
            return i;
        }
    }
}
