using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bsuir.Misoi.Core.Images.Filtering.Implementation
{
    public class MedianFilter : IFilter
    {
        public string Name => "Median filter";

        public void Filter(IImage image)
        {
           

            int w_b = image.Width;
            int h_b = image.Height;

            int rad = 7;

            for (int x = rad + 1; x < w_b - rad; x++)
            {
                for (int y = rad + 1; y < h_b - rad; y++)
                {
                    median_filter(image, x, y); 

                }
            }
        }

   
        private int partition(int[] a, int p, int r)
        {
            int x = a[r];
            int i = p - 1;
            int tmp;
            for (int j = p; j < r; j++)
            {

                if (a[j] <= x)
                {
                    i++;
                    tmp = a[i];
                    a[i] = a[j];
                    a[j] = tmp;

                }
            }
            tmp = a[r];
            a[r] = a[i + 1];
            a[i + 1] = tmp;
            return (i + 1);

        }


        private void quicksort(int[] a, int p, int r)
        {
            if (p < r)
            {
                int q = partition(a, p, r);
                quicksort(a, p, q - 1);
                quicksort(a, q + 1, r);
            }
        }


        int rad;
        //МЕДИАННЫЙ ФИЛЬТР!!!!!!!!!!
        private void median_filter(IImage image, int x, int y)//Bitmap my_bitmap 
        {
            int n;//элемнты в массиве
            int cR_, cB_, cG_;//искомые медианные значения
            int k = 1;

            n = (2 * rad + 1) * (2 * rad + 1); //?

            //    toolStripProgressBar2.Step = 1;
            //  toolStripProgressBar2.Maximum = n;

            // обнуляем массив ---------------------------
            int[] cR = new int[n + 1];
            int[] cB = new int[n + 1];
            int[] cG = new int[n + 1];

            for (int i = 0; i < n + 1; i++)
            {
                cR[i] = 0;
                cG[i] = 0;
                cB[i] = 0;
            }
            //--------------------------------------------
            int w_b = image.Width;
            int h_b = image.Height;

            for (int i = x - rad; i < x + rad + 1; i++)
            {
                for (int j = y - rad; j < y + rad + 1; j++)
                {

                    IImage pixel = image.GetPixel(i, j);
                    cR[k] = Convert.ToInt32(pixel.R);
                    cG[k] = Convert.ToInt32(pixel.G);
                    cB[k] = Convert.ToInt32(pixel.B);
                    k++;
     
                }
            }
            
            quicksort(cR, 0, n - 1);//сортируем массивы
            quicksort(cG, 0, n - 1);
            quicksort(cB, 0, n - 1);

            int n_ = (int)(n / 2) + 1;

            cR_ = cR[n_];
            cG_ = cG[n_];
            cB_ = cB[n_];

            image.SetPixel(x, y, new IImage { R = (byte)cR_, G = (byte)cG_, B = (byte)cB_ });
        }




    }

}

